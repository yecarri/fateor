using Fateor.Application.Location.Entities;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification.Entities;
using Fateor.Application.Organization.Entities;
using Fateor.Application.UserAccess.Entities;
using Fateor.Application.UserAccess.Services;
using Fateor.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using oSoft.Mail.Entities;
using oSoft.Mail.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Notification.Services
{
    public sealed class NotificationService : INotificationService
    {
        private readonly IUserService _userService;
        private readonly ILocationService _locationService;
        private readonly IEmailService _emailService;
        private readonly EmailConfiguration _emailConfiguration;
        private readonly ILogger _logger;
        private readonly string _baseUrl;
        private readonly string _templatesPath;

        public NotificationService(IUserService userService,
                                   ILocationService locationService,
                                   IEmailService emailService,
                                   ILogger logger,
                                   EmailConfiguration emailConfiguration,
                                   string url,
                                   string templatesPath)
        {
            _userService = userService;
            _locationService = locationService;
            _emailService = emailService;
            _baseUrl = url;
            _emailConfiguration = emailConfiguration;
            _logger = logger;
            _templatesPath = templatesPath;
        }
        private static string SetVariables(Dictionary<string, string> variables, string html)
        {
            var sb = new StringBuilder(html);
            foreach (var v in variables)
            {
                sb.Replace("{" + v.Key + "}", v.Value);
            }
            return sb.ToString();
        }

        private string GetUrl(string relativePath)
        {
            return new Uri(new Uri(_baseUrl), relativePath).ToString();
        }

        public (bool ok, List<string> messages) NotifyNewMember(Member member)
        {

            Country? country = null;
            if (member.CountryId is not null)
                country = _locationService.GetCountryById(member.CountryId.Value);

            var users = _userService.GetAllUsers().Where(u => !string.IsNullOrEmpty(u.Email));
            try
            {
                NotifyNewMember(member, country, users);
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return (false, new List<string>() { "No se pudo enviar los correos de notificación a los administradores.", ex.Message });
            }
        }

        public (bool ok, IList<string> messages) SendWelcomeEmail(Member member, string token)
        {
            if (_emailConfiguration.DisableAllEmails)
                return (true, new List<string>());

            try
            {
                string url = GetUrl($"response/token/{token}");
                var template = _emailConfiguration?.Templates?.First(t => t.TemplateName == "welcome") ?? throw new Exception("Missing Configuration: Template [welcome]");
                var html = template.Content;
                var variables = new Dictionary<string, string>()
                {
                    {"member.GivenName", member.GivenName},
                    {"url" , url}
                };

                html = SetVariables(variables, html);
                var recipient = new EmailAddress(member.Email, member.FullName);
                var sender = _emailConfiguration.DefaultSender;
                return _emailService.SendMail(sender, recipient, template.Subject, html);

            }
            catch (Exception ex)
            {
                return (false, new List<string>() { "No se pudo enviar el correo de bienvenida", ex.Message });
            }
        }

        public (bool ok, IList<string> messages) NotifyPasswordChangeRequest(User user)
        {
            if (_emailConfiguration.DisableAllEmails)
                return (true, new List<string>());

            if (user?.UserId is null)
                throw new ArgumentNullException(nameof(user));

            var token = _userService.GetPasswordResetToken(user.UserId.Value);
            string url = GetUrl($"user/passwordreset/{token}");

            var template = _emailConfiguration.Templates.FirstOrDefault(t => t.TemplateName == "passwordChange");
            if (template == null)
                return (false, new List<string>() { "No se ha encontrado la plantilla de correo para el cambio de contraseña. Verifique el archivo de configuración." });

            var variables = new Dictionary<string, string>()
            {
                {"user.Login", user.Login },
                {"url" , url}
            };

            var html = SetVariables(variables, template.Content);
            var recipient = new EmailAddress(user.Email, user.Login);
            var sender = _emailConfiguration.DefaultSender;
            return _emailService.SendMail(sender, recipient, template.Subject, html);
        }

        public (bool ok, IList<string> messages) NotifyNewMember(Member member, Country? country, IEnumerable<User> users)
        {
            if (_emailConfiguration.DisableAllEmails)
                return (true, new List<string>());

            string url = GetUrl($"member/{member.MemberId}");

            var template = _emailConfiguration.Templates.FirstOrDefault(t => t.TemplateName == "passwordChange");
            if (template == null)
                return (false, new List<string>() { "No se ha encontrado la plantilla de correo para el cambio de contraseña. Verifique el archivo de configuración." });

            var html = template.Content;

            foreach (var user in users)
            {
                var variables = new Dictionary<string, string>()
                {
                    {"user.Login", user.Login },
                    {"member.FullName", member.FullName },
                    {"member.City", member.City },
                    {"country.CountryName", country?.CountryName ?? "(SIN PAÍS)"},
                    {"url" , url}
                };

                var htmlInstance = SetVariables(variables, html);

                var recipient = new EmailAddress(user.Email, user.Login);
                var sender = _emailConfiguration.DefaultSender;

                _emailService.SendMail(sender, recipient, template.Subject, html);
            }

            return (true, new List<string>());
        }

    }

}
