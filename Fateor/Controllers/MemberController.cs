using Fateor.Application.Credentials.Services;
using Fateor.Application.External;
using Fateor.Application.Gdpr.Services;
using Fateor.Application.Location.Entities;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification.Services;
using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Google.Protobuf;
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
        private readonly IGdprService _gdprService;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IIdentificationNumberValidator _identifierNumberValidator;

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
                                IGdprService gdprService,
                                Settings settings,
                                IHostEnvironment hostEnvironment,
                                IIdentificationNumberValidator identifierNumberValidator) : base(logger, mapper, encryptionService, settings)
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
            _gdprService=gdprService;
            _hostEnvironment=hostEnvironment;
            _identifierNumberValidator=identifierNumberValidator;
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
        [Route("Member/Download")]
        public FileResult Download(
            [FromQuery] string? name = null,
            [FromQuery] int countryId = -1,
            [FromQuery] int regionId = -1,
            [FromQuery] int unitId = -1,
            [FromQuery] int statusId = -1,
            [FromQuery] bool includeDeleted = false)
        {
            if (CurrentUser != null && CurrentUser.UserId != null)
            {
                var ms = new MemoryStream();
                var wb = new ClosedXML.Excel.XLWorkbook();
                var ws = wb.Worksheets.Add("Miembros");
                var countries = _locationService.GetCountries();
                var regions = _locationService.GetRegions();
                var documents = _identityDocumentService.GetDocumentTypes();

                var members = GetMembers(name, countryId, regionId, unitId, statusId, includeDeleted);

                var data = members
                    .Join(countries, m => m.CountryId, c => c.CountryId, (Member, Country) => new { Member, Country })
                    .Join(regions, m => m.Member.RegionId, r => r.RegionId, (x, Region) => new { x.Member, x.Country, Region })
                    .Join(documents, m => m.Member.IdentityDocumentTypeId, d => d.IdentityDocumentTypeId, (x, Document) => new { x.Member, x.Country, x.Region, Document })
                    .Select(m => new
                    {
                        m.Member.MemberId,
                        m.Member.GivenName,
                        m.Member.Surname,
                        m.Member.Email,
                        m.Document.IdentityDocument,
                        m.Member.IdentityDocumentNumber,
                        m.Member.BirthDate,
                        m.Member.Phone,
                        m.Member.Address1,
                        m.Member.Address2,
                        Country = m.Country.CountryName,
                        Region = m.Region.RegionName,
                        m.Member.City,
                        m.Member.PostCode,
                        m.Member.MemberCode,
                        Unit = m.Member.LastStatus?.UnitName,
                        Status = m.Member.LastStatus?.StatusDescription,
                        m.Member.LastStatus?.StatusDate,
                        StatusMofificationDate = m.Member.LastStatus?.LastModificationDate
                    });
                ws.Cell(1, 1).InsertTable(data);
                wb.SaveAs(ms);
                _gdprService.Save(CurrentUser.UserId.Value, members, true);
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Members-{DateTime.Now:yyMMdd-HHmmss}.xlsx");
            }
            else
            {
                throw new NullReferenceException(nameof(CurrentUser));
            }
        }

        [HttpGet]
        [Route("Member/Create")]
        public ActionResult Create()
        {
            var model = new MemberViewModel();
            LoadModel(model);
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
                _logger.LogDebug("Changed selected country");
                model.Command = "";
                LoadModel(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _logger.LogDebug("Create Member");

                    var documentTypes = _identityDocumentService.GetDocumentTypes().ToList();
                    var selectedDocType = (documentTypes
                        ?.FirstOrDefault(t => t.IdentityDocumentTypeId==model.IdentityDocumentTypeId)
                        ?.IdentityDocument) ?? "";

                    if (_identifierNumberValidator.IsValid(selectedDocType, model.IdentityDocumentNumber) ==
                    IIdentificationNumberValidator.IdentificationNumberValidity.Invalid)
                    {
                        ModelState.AddModelError("", $"Por favor, ingrese un {selectedDocType} válido");
                    }
                    else
                    {
                        var member = _mapper.Map<MemberViewModel, Member>(model);
                        (bool ok, IList<string> messages) = _memberService.CreateMember(member, CurrentUser?.UserId ?? 0);
                        if (ok)
                        {
                            _logger.LogDebug("Member Created, redirecting to Edit");
                            return RedirectToAction("Edit", new { Id = member.MemberId });
                        }
                        _logger.LogDebug("Member Created, redirecting to Edit");
                        LoadModel(model);
                        model.Messages = messages;
                    }
                }
                else
                {
                    model.Command = string.Empty;
                    LoadModel(model);
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
            LoadModel(model);

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
                LoadModel(model);
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
                        case "ResendWelcomeEmail":
                            string? token = InitialQuestionaireToken(member);
                            (ok, messages) = _notificationService.SendWelcomeEmail(member, token ?? "");
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
                LoadModel(model);
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
        [AllowAnonymous]
        [Route("")]
        [Route("Registration")]
        public ActionResult Registration(RegistrationViewModel model)
        {
            bool ok = false;
            IList<string> messages;

            if (ModelState.IsValid)
            {
                LoadModel(model);
                LoadNotifications(model);

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

                    model.Messages = messages;
                }
            }
            else
            {
                model.Command = string.Empty;
                LoadModel(model);
            }
            return View("Registration", model);
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

        [AllowAnonymous]
        [Route("ContinueRegistration/{memberId}")]
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

        [AllowAnonymous]
        [Route("RegistrationOk")]
        public ActionResult RegistrationOk()
        {
            return View(new ViewModelBase());
        }

        private void LoadMemberListAggregations(MemberListViewModel model)
        {
            if (CurrentUser != null && CurrentUser.UserId != null)
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
                model.Members = GetMembers(model.Name, model.CountryId, model.RegionId, model.UnitId, model.StatusId, model.IncludeDeleted);

                _gdprService.Save(CurrentUser.UserId.Value, model.Members);
                _logger.LogDebug("OK");
            }
            else
            {
                throw new NullReferenceException(nameof(CurrentUser));
            }
        }

        private List<Member> GetMembers(string? name, int countryId, int regionId, int unitId, int statusId, bool includeDeleted)
        {
            return _memberService.GetAllMembers().ToList().Where(m =>
                (countryId == -1 || m.CountryId == countryId) &&
                (regionId == -1 || m.RegionId == regionId) &&
                (unitId == -1 || m.LastStatus?.UnitId == unitId) &&
                (statusId == -1 || m.LastStatus?.StatusId == statusId) &&
                (name == null || m.FullName.ToLower().Contains(name.ToLower())) &&
                (includeDeleted || !m.IsDeleted)
            ).ToList();
        }

        private void LoadModel(MemberViewModel model)
        {
            if (model.CountryId.HasValue)
                model.Regions = _locationService.GetRegionsByCountryId(model.CountryId.Value).ToList();
            else
                model.Regions = new List<Region>();

            var countries = _locationService.GetCountries().ToList();
            countries.Insert(0, new Country() { CountryId = -1, CountryName = "- selecciona un país- " });
            model.Countries = countries;

            model.DocumentTypes = _identityDocumentService.GetDocumentTypes().ToList();

            _logger.LogDebug("OK");
        }

        private void LoadStatusAggregations(UpdateStatusViewModel model)
        {
            model.Units = _unitService.GetUnits();
            model.Statuses = _statusService.GetStatuses();
            _logger.LogDebug("OK");
        }

        private string? InitialQuestionaireToken(Member member)
        {
            var questionaire = _questionaireService.GetQuestionaireByName(_settings.Default_InitialPoll);
            if (questionaire != null)
            {
                return _questionaireService.GetQuestionaireToken(member.MemberId, questionaire.QuestionnaireId);
            }
            return null;
        }

    }
}
