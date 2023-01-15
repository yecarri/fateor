using Fateor.Application.Credentials.Services;
using Fateor.Application.External;
using Fateor.Application.Location.Entities;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification;
using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fateor.Controllers
{
    [Authorize]
    public class MemberController : FateorControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IResponseService _responseService;
        private readonly IUnitSelector _unitSelector;
        private readonly ILocationService _locationService;
        private readonly IExternalMemberService _externalMemberService;
        private readonly INotificationService _notificationService;
        private readonly IStatusService _statusService;
        private readonly IQuestionnaireService _questionaireService;
        private readonly IUnitService _unitService;
        private readonly IIdentityDocumentService _identityDocumentService;
        private readonly ICredentialService _credentialService;
        private readonly Settings _settings;
        public MemberController(ILogger<MemberController> logger,
                                IMapper mapper,
                                IEncryptionService encryptionService,
                                IMemberService memberService,
                                ILocationService locationService,
                                IResponseService responseService,
                                IUnitSelector unitSelector,
                                IExternalMemberService externalMemberService,
                                IQuestionnaireService questionaireService,
                                INotificationService notificationService,
                                IStatusService statusService,
                                IUnitService unitService,
                                IIdentityDocumentService identityDocumentService,
                                ICredentialService credentialService,
                                Settings settings) : base(logger, mapper, encryptionService)
        {
            _memberService=memberService;
            _locationService=locationService;
            _responseService=responseService;
            _unitSelector=unitSelector;
            _externalMemberService=externalMemberService;
            _questionaireService=questionaireService;
            _notificationService=notificationService;
            _statusService=statusService;
            _unitService=unitService;
            _identityDocumentService=identityDocumentService;
            _credentialService=credentialService;
            _settings = settings;
        }

        [HttpGet]
        [Route("Member")]
        public ActionResult List()
        {
            var model = new MemberListViewModel();
            LoadMemberListAggregations(model);
            return View(model);
        }

        [HttpPost]
        [Route("Member")]
        public ActionResult List(MemberListViewModel model)
        {
            LoadMemberListAggregations(model);

            return View(model);
        }

        [HttpGet]
        [Route("Member/Create")]
        public ActionResult Create()
        {
            var model = new MemberViewModel();
            LoadMeberAggregations(model);
            model.New = true;
            return View("Edit", model);
        }

        [HttpPost]
        [Route("Member/Create")]
        public ActionResult Create(MemberViewModel model)
        {
            model.New = true;

            if (model.Command == "CountryChanged")
            {
                _logger.LogInformation("Changed selected country");
                model.Command = "";
                LoadMeberAggregations(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Create Member");
                    var member = _mapper.Map<MemberViewModel, Member>(model);
                    (bool ok, IList<string> messages) = _memberService.CreateMember(member, CurrentUser?.UserId ?? 0);
                    if (ok)
                    {
                        _logger.LogInformation("Member Created, redirecting to Edit");
                        return RedirectToAction("Edit", new { Id = member.MemberId });
                    }
                    _logger.LogInformation("Member Created, redirecting to Edit");
                    LoadMeberAggregations(model);
                    model.Messages = messages;
                }
                else
                {
                    model.Command = string.Empty;
                    LoadMeberAggregations(model);
                }
            }
            return View("Edit", model);
        }

        [HttpGet]
        [Route("Member/{id:int}")]
        [Route("Member/Edit/{id:int}")]
        public ActionResult Edit(int id)
        {
            MemberViewModel model = _mapper.Map<Member, MemberViewModel>(_memberService.GetMemberById(id));
            if (model == null)
            {
                TempData["messages"] = new List<string> { $"No se ha encontrado ningún miembro con id {id}", "Por favor revisa la dirección e intenta de nuevo." };
                return RedirectToAction("Index", "Home");
            }
            LoadMeberAggregations(model);

            model.Responses = _responseService.GetResponsesByMember(model.MemberId, true)
             .Select(r =>
             {
                 var rv = _mapper.Map<Application.Questionnaires.Entities.Response, ResponseViewModel>(r);
                 return rv;
             }
             ).ToList();
            return View(model);
        }

        [HttpPost]
        [Route("Member/{id:int}")]
        [Route("Member/Edit/{id:int}")]
        public IActionResult Edit(MemberViewModel model)
        {
            try
            {
                LoadMeberAggregations(model);
                if (ModelState.IsValid)
                {
                    bool ok;
                    IList<string> messages;
                    var member = _mapper.Map<MemberViewModel, Member>(model);
                    switch (model.Command)
                    {
                        case "AssignMemberCode":
                            (ok, messages) = _memberService.AssignMemberCode(member, CurrentUser?.UserId ?? 0);
                            if (ok)
                            {
                                model.MemberCode = member.MemberCode;
                                ModelState.Clear();
                            }
                            break;
                        case "PrintCredential":
                            var ms = new MemoryStream();
                            (ok, messages) = _credentialService.Generate(ms, member);
                            if (ok)
                            {
                                ms.Flush();
                                return File(ms, "application/pdf", $"{member.MemberCode}.pdf");
                            }
                            break;
                        default:
                            (ok, messages) = _memberService.UpdateMember(member, CurrentUser?.UserId ?? throw new NullReferenceException("CurrentUser.UserId"));
                            break;
                    }

                    if (!ok)
                    {
                        model.Messages = messages;
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                model.Messages.Add($"No se pudo guardar: {ex.Message}");
                LoadMeberAggregations(model);
                return View(model);
            }
        }

        [HttpGet]
        [Route("Member/UpdateStatus/{id:int}")]
        public ActionResult UpdateStatus(int id)
        {
            UpdateStatusViewModel model = new UpdateStatusViewModel();

            var member = _memberService.GetMemberById(id);
            if (member is not null)
            {
                var lastStatus = member?.MemberStatuses?.OrderByDescending(s => s.StatusDate).FirstOrDefault();

                model.MemberId = member.MemberId;
                model.GivenName = member.GivenName;
                model.Surname = member.Surname;
                model.StatusDate = DateTime.Today;
                if (lastStatus != null)
                {
                    model.UnitId = lastStatus.UnitId;
                    model.StatusId = lastStatus.StatusId;
                    model.MemberStatuses = member.MemberStatuses;

                }
            }
            else
            {
                model.Messages = new List<string>() { "Miembro no encontrado" };
            }
            LoadStatusAggregations(model);

            return View(model);
        }

        [HttpPost]
        [Route("Member/UpdateStatus/{id:int}")]
        public ActionResult UpdateStatus(UpdateStatusViewModel model)
        {
            LoadStatusAggregations(model);
            var status = _mapper.Map<UpdateStatusViewModel, MemberStatus>(model);
            (var ok, var messages) = _memberService.UpdateMemberStatus(status, CurrentUser?.UserId ?? 0);
            if (ok)
            {
                model.MemberStatuses = _memberService.GetMemberStatusHistory(model.MemberId);
            }
            model.Messages = messages.ToList();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]        
        [Route("")]        
        [Route("Registration")]
        public ActionResult Registration()
        {
            var model = new RegistrationViewModel();

            model.CreateExternalUser = _settings.CreateExternalUsers;
            if (!_settings.CreateExternalUsers)
            {
                model.AlreadyInIntranet = false;
                model.Login = "dummy";
                model.Password = "dummy";
                model.Password2 = "dummy";

            }
            LoadMeberAggregations(model);
            model.New = true;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("")]
        [Route("Registration")]
        public ActionResult Registration(RegistrationViewModel model)
        {
            bool ok = false;
            IList<string> messages;

            if (ModelState.IsValid)
            {
                LoadMeberAggregations(model);

                var member = _mapper.Map<RegistrationViewModel, Member>(model);
                (ok, messages) = _memberService.CreateMember(member, _settings.SystemUserId);
                if (ok && _settings.CreateExternalUsers)
                {
                    (ok, messages) = _externalMemberService.CreateMember(member);
                }
                if (ok)
                {
                    _notificationService.NotifyNewMember(member);
                    string? token = InitialQuestionaireToken(member);
                    _notificationService.SendWelcomeEmail(member, token ?? "");
                    if (token != null)
                    {
                        _logger.LogInformation("Redirecting to initial questionaire");
                        return Redirect(Url.Content($"~/response/token/{token}"));
                    }
                    else
                    {
                        return Redirect(Url.Content($"~/RegistrationOK"));
                    }
                }
                model.Messages = messages;
            }
            else
            {
                model.Command = string.Empty;
                LoadMeberAggregations(model);
            }
            return View("Registration", model);
        }

        [AllowAnonymous]
        [Route("ContinueRegistration/{memberId}")]
        public ActionResult ContinueRegistration(int memberId)
        {
            var member = _memberService.GetMemberById(memberId);
            _notificationService.NotifyNewMember(member);
            string token = InitialQuestionaireToken(member) ?? "";
            _notificationService.SendWelcomeEmail(member, token);
            if (token != null)
            {
                _logger.LogInformation("Redirecting to initial questionaire");
                return Redirect(Url.Content($"~/response/token/{token}"));
            }
            else
            {
                return Redirect(Url.Content($"~/RegistrationOK"));
            }
        }
        
        [AllowAnonymous]
        [Route("RegistrationOk")]
        public ActionResult RegistrationOk()
        {
            return View(new ViewModelBase());
        }

        private void LoadMemberListAggregations(MemberListViewModel model)
        {
            model.Members = _memberService.GetAllMembers().ToList();
            model.Countries = _locationService.GetCountries()
                .Where(c => model.Members.Any(m => m.CountryId == c.CountryId))
                .OrderBy(c => c.CountryName)
                .ToList();

            model.Countries.Insert(0, new Application.Location.Entities.Country() { CountryId = -1, CountryName = "- Todos -" });

            model.Regions = model.Countries.SelectMany(c => _locationService.GetRegionsByCountryId(c.CountryId))
                .Where(l => model.Members.Any(m => m.RegionId == l.RegionId))
                .OrderBy(r => r.CountryName)
                .ThenBy(r => r.RegionName)
                .ToList();

            model.Regions.Insert(0, new Application.Location.Entities.Region() { RegionId = -1, RegionName = "- Todos -" });

            model.Statuses =_statusService.GetStatuses()
                .Where(s => !s.IsDeleted)
                .ToList();

            model.Statuses.Insert(0, new Status() { StatusId = -1, StatusDescription = "- Todos -" });

            model.Units = _unitService.GetUnits().Where(u => !u.IsDeleted).ToList();
            model.Units.Insert(0, new Unit() { UnitId = -1, UnitName = "- Todas -" });

            model.Members = model.Members.Where(m =>
                (model.CountryId == -1 || m.CountryId == model.CountryId) &&
                (model.RegionId == -1 || m.RegionId == model.RegionId) &&
                (model.UnitId == -1 || m.LastStatus?.UnitId == model.UnitId) &&
                (model.StatusId == -1 || m.LastStatus?.StatusId == model.StatusId) &&
                (model.IncludeDeleted || !m.IsDeleted)
            ).ToList();

            _logger.LogInformation("OK");
        }

        private void LoadMeberAggregations(MemberViewModel model)
        {
            if (model.CountryId.HasValue)
                model.Regions = _locationService.GetRegionsByCountryId(model.CountryId.Value).ToList();
            else
                model.Regions = new List<Region>();

            var countries = _locationService.GetCountries().ToList();
            countries.Insert(0, new Country() { CountryId = -1, CountryName = "- selecciona un país- " });
            model.Countries = countries;

            model.DocumentTypes = _identityDocumentService.GetDocumentTypes().ToList();
            _logger.LogInformation("OK");
        }

        private void LoadStatusAggregations(UpdateStatusViewModel model)
        {
            model.Units = _unitService.GetUnits();
            model.Statuses = _statusService.GetStatuses();
            _logger.LogInformation("OK");
        }

        private string? InitialQuestionaireToken(Member member)
        {
            var questionaire = _questionaireService.GetQuestionaireByName(_settings.InitialQuestionnaire);
            if (questionaire != null)
            {
                return _questionaireService.GetQuestionaireToken(member.MemberId, questionaire.QuestionnaireId);
            }
            return null;
        }

    }
}
