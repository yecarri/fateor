using Fateor.Application.External;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification.Services;
using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Fateor.Controllers
{    
    public class MemberRegistration : MemberControllerBase
    {
        private readonly IIdentificationNumberValidator _identifierNumberValidator;
        private readonly IExternalMemberService _externalMemberService;
        private readonly INotificationService _notificationService;
        private readonly IHostEnvironment _hostEnvironment;

        public MemberRegistration(
            IMemberService memberService,
            IIdentityDocumentService identityDocumentService,
            IQuestionnaireService questionaireService,
            ILogger<MemberRegistration> logger,
            IMapper mapper,
            IEncryptionService encryptionService,
            Settings settings,
            ILocationService locationService,
            IIdentificationNumberValidator identifierNumberValidator,
            IExternalMemberService externalMemberService,
            INotificationService notificationService,
            IHostEnvironment hostEnvironment) : base(
                memberService: memberService,
                identityDocumentService: identityDocumentService,
                locationService: locationService,
                encryptionService: encryptionService,
                questionaireService: questionaireService,
                logger: logger,
                mapper: mapper,
                settings: settings)
        {
            _identifierNumberValidator=identifierNumberValidator;
            _externalMemberService=externalMemberService;
            _notificationService=notificationService;
            _hostEnvironment=hostEnvironment;
        }

        [HttpGet]
        [Route("/")]
        [Route("/Registration")]
        public ActionResult Registration()
        {
            var model = new RegistrationViewModel();
            model.CreateExternalUser = _settings.ForumSystem_CreateUser;
            if (!_settings.ForumSystem_CreateUser)
            {
                model.AlreadyInIntranet = false;
                model.Login = "dummy";
                model.Password = "dummy";
                model.Password2 = "dummy";
            }
            LoadModel(model);
            LoadNotifications(model);
            model.New = true;
            return View(model);
        }

        [HttpPost]
        [Route("/")]
        [Route("Registration")]
        public ActionResult Registration(RegistrationViewModel model)
        {
            bool ok = false;
            IList<string> messages;

            if (ModelState.IsValid)
            {                
                var selectedDocType = (model
                    .DocumentTypes
                    ?.FirstOrDefault(t => t.IdentityDocumentTypeId==model.IdentityDocumentTypeId)
                    ?.IdentityDocument) ?? "";

                if (_identifierNumberValidator.IsValid(selectedDocType, model.IdentityDocumentNumber) ==
                    IIdentificationNumberValidator.IdentificationNumberValidity.Invalid)
                {
                    ModelState.AddModelError("", $"Por favor, ingrese un {selectedDocType} válido");
                }
                else
                {
                    var member = _mapper.Map<RegistrationViewModel, Member>(model);
                    (ok, messages) = _memberService.CreateMember(member, _settings.Default_UserId);
                    if (ok && _settings.ForumSystem_CreateUser)
                    {
                        (ok, messages) = _externalMemberService.CreateMember(member);
                    }
                    if (ok)
                    {
                        (ok, messages) = _notificationService.NotifyNewMember(member);
                        if (!ok)
                        {
                            _logger.LogError("Notification Email not sent: {0}", string.Join("\n", messages));
                        }
                        string? token = InitialQuestionaireToken(member);

                        (ok, messages) = _notificationService.SendWelcomeEmail(member, token ?? "");

                        if (!ok)
                        {
                            _logger.LogError("Notification Email not sent: {0}", string.Join("\n", messages));
                        }

                        if (token != null)
                        {
                            _logger.LogDebug("Redirecting to initial questionaire");
                            return Redirect(Url.Content($"~/response/{token}"));
                        }
                        else
                        {
                            return Redirect(Url.Content($"~/RegistrationOK"));
                        }
                    }
                    LoadModel(model);
                    LoadNotifications(model);
                    model.Messages = messages;
                }
            }
            else
            {
                model.Command = string.Empty;
                LoadModel(model);                
                LoadNotifications(model);

            }
            return View("Registration", model);
        }

        [Route("/ContinueRegistration/{memberId}")]
        public ActionResult ContinueRegistration(int memberId)
        {
            var member = _memberService.GetMemberById(memberId);
            (var ok, IList<string> msg) = _notificationService.NotifyNewMember(member);

            if (!ok)
            {
                _logger.LogError("Notification Email not sent: {0}", string.Join("\n", msg));
            }

            string token = InitialQuestionaireToken(member) ?? "";

            (ok, msg) = _notificationService.SendWelcomeEmail(member, token);

            if (!ok)
            {
                _logger.LogError("Notification Email not sent: {0}", string.Join("\n", msg));
            }
            if (token != null)
            {
                _logger.LogDebug("Redirecting to initial questionaire");
                return Redirect(Url.Content($"~/response/token/{token}"));
            }
            else
            {
                return Redirect(Url.Content($"~/RegistrationOK"));
            }
        }

        [Route("/RegistrationOk")]
        public ActionResult RegistrationOk()
        {
            return View(new ViewModelBase());
        }

        private void LoadNotifications(RegistrationViewModel model)
        {
            var introPath = System.IO.Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "sites", _settings.Site, "templates", "intro.html");
            try
            {
                model.IntroHtml = System.IO.File.ReadAllText(introPath);
            }
            catch { }

            var gdprPath = System.IO.Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "sites", _settings.Site, "templates", "gdpr.html");
            try
            {
                model.GdprHtml= System.IO.File.ReadAllText(gdprPath);
            }
            catch { }
        }
    }
}
