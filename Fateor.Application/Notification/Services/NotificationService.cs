using Fateor.Application.Location.Entities;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification.Entities;
using Fateor.Application.Organization.Entities;
using Fateor.Application.UserAccess.Entities;
using Fateor.Application.UserAccess.Services;
using Microsoft.Extensions.Logging;
using oSoft.Mail.Entities;
using oSoft.Mail.Services;
using System.Text;

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

        public (bool ok, IList<string> messages) NotifyNewMember(Member member)
        {
            _logger.LogDebug("NotifyNewMember");
            Country? country = null;
            if (member.CountryId is not null)
                country = _locationService.GetCountryById(member.CountryId.Value);

            var users = _userService.GetActiveUsers().Where(u => !string.IsNullOrEmpty(u.Email));
            try
            {
                (var ok, var msg) = NotifyNewMember(member, country, users);
                return (ok, msg);
            }
            catch (Exception ex)
            {
                return (false, new List<string>() { "No se pudo enviar los correos de notificación a los administradores.", ex.Message });
            }
        }

        public (bool ok, IList<string> messages) NotifyNewMember(Member member, Country? country, IEnumerable<User> users)
        {
            _logger.LogDebug("NotifyNewMember");
            if (_emailConfiguration.DisableAllEmails)
            {
                _logger.LogDebug("Emails are disabled, will not sent");
                return (true, new List<string>());
            }

            string url = GetUrl($"member/{member.MemberId}");

            var template = _emailConfiguration.Templates.FirstOrDefault(t => t.TemplateName == "notifyNewMember");
            if (template == null)
                return (false, new List<string>() { "No se ha encontrado la plantilla de correo para el cambio de contraseña. Verifique el archivo de configuración." });

            var html = template.Content;

            bool ok = true;
            var msg = new List<string>();

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

                (var ok1, var msg1) = _emailService.SendMail(sender, recipient, template.Subject, htmlInstance);

                ok &= ok1;
                msg.AddRange(msg1);
            }

            return (ok, msg);
        }

        public (bool ok, IList<string> messages) SendWelcomeEmail(Member member, string token)
        {
            _logger.LogDebug("SendWelcomeEmail");
            if (_emailConfiguration.DisableAllEmails)
            {
                _logger.LogDebug("Emails are disabled, will not sent");
                return (true, new List<string>());
            }

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
                var sender = _emailConfiguration.DefaultSender ?? throw new Exception("Default sender not configured");
                var result = _emailService.SendMail(sender, recipient, template.Subject, html);
                _logger.LogDebug("Welcome email sent");
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending welcome email");
                return (false, new List<string>() { "No se pudo enviar el correo de bienvenida", ex.Message });
            }
        }

        public (bool ok, IList<string> messages) NotifyPasswordChangeRequest(User user)
        {
            if (_emailConfiguration.DisableAllEmails)
            {
                _logger.LogDebug("Emails are disabled, will not sent");
                return (true, new List<string>());
            }

            if (user?.UserId is null)
                throw new ArgumentNullException(nameof(user));

            var token = _userService.GetPasswordResetToken(user.UserId.Value);
            string url = GetUrl($"user/resetpassword/{token}");

            var template = _emailConfiguration?.Templates?.FirstOrDefault(t => t.TemplateName == "passwordChange");
            if (template == null)
                return (false, new List<string>() { "No se ha encontrado la plantilla de correo para el cambio de contraseña. Verifique el archivo de configuración." });

            var variables = new Dictionary<string, string>()
            {
                {"user.Login", user.Login },
                {"url" , url}
            };

            var html = SetVariables(variables, template.Content);
            var recipient = new EmailAddress(user.Email, user.Login);
            var sender = _emailConfiguration?.DefaultSender ?? throw new Exception("Default sender not configured");
            var result = _emailService.SendMail(sender, recipient, template.Subject, html);
            return result;
        }

        public (bool ok, IList<string> messages) SendLogin(string email, string token)
        {
            _logger.LogDebug("SendLogin");
            if (_emailConfiguration.DisableAllEmails)
            {
                _logger.LogDebug("Emails are disabled, will not sent");
                return (true, new List<string>());
            }

            string url = GetUrl($"member/login/{token}");
            var template = _emailConfiguration?.Templates?.FirstOrDefault(t => t.TemplateName == "memberLogin");
            if (template == null)
                return (false, new List<string>() { "No se ha encontrado la plantilla de correo para el login de miembros" });

            var variables = new Dictionary<string, string>()
            {
                {"url" , url}
            };

            var html = SetVariables(variables, template.Content);
            var recipient = new EmailAddress(email);
            var sender = _emailConfiguration?.DefaultSender ?? throw new Exception("Default sender not configured");
            var result = _emailService.SendMail(sender, recipient, template.Subject, html);
            return result;
        }
    }

}
