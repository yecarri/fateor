using Fateor.Application.External;
using Fateor.Application.Location.Services;
using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fateor.Controllers
{
    [Authorize(Roles = "member")]
    public class MemberSelfService : MemberControllerBase
    {
        private readonly IResponseService _responseService;

        public MemberSelfService(
            IMemberService memberService,
            IIdentityDocumentService identityDocumentService,
            ILocationService locationService,
            IQuestionnaireService questionaireService,
            ILogger<MemberSelfService> logger,
                                 IMapper mapper,
                                 IEncryptionService encryptionService,
                                 Settings settings,
                                 IUnitSelector unitSelector,
                                 IExternalMemberService externalMemberService,
                                 IHostEnvironment hostEnvironment,
                                 IResponseService responseService) :
            base(
                memberService,
                identityDocumentService,
                locationService,
                questionaireService,
                encryptionService,
                logger,
                mapper,
                settings)
        {
            _responseService=responseService;
        }


        [HttpGet("Member/SelfService"), Authorize(Roles = "member")]
        public ActionResult SelfService()
        {
            // Get the MemberId from the claims
            var memberId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get the member details
            if (memberId == null)
            {
                return Redirect(Url.Content("~/MemberLogin"));
            }

            var member = _memberService.GetMemberById(int.Parse(memberId));
            var model = _mapper.Map<Member, MemberViewModel>(member);
            LoadModel(model);

            var responses = _responseService.GetResponsesByMember(model.MemberId, true);

            // This is a quick and dirty way to display the initial poll button,
            // should we refactor the view model to include this information?
            if (!responses.Any(r => r.Questionnaire==_settings.Default_InitialPoll))
            {
                ViewBag.ShowInitialPoll = true;
                ViewBag.InitialPollToken = InitialQuestionaireToken(member);
            }
            else
            {
                ViewBag.ShowInitialPoll = false;
            }

            model.Responses = responses
                .Select(r =>
                {
                    var re = _mapper.Map<Application.Questionnaires.Entities.Response, ResponseViewModel>(r);
                    re.Token = _questionaireService.GetQuestionaireToken(member.MemberId, r.QuestionnaireId);
                    return re;
                })
                .ToList();
            return View(model);
        }

        [HttpGet("MemberLogin"), AllowAnonymous]
        public ActionResult MemberLogin()
        {
            return View(new MemberLoginViewModel());
        }

        [HttpPost("MemberLogin"), AllowAnonymous]
        public ActionResult MemberLogin(MemberLoginViewModel model)
        {
            // If Model is valid
            if (ModelState.IsValid)
            {
                // Check if user exists
                _memberService.RequestMemberLogin(model.Email);
                model.Messages= ["Se envió un correo con el enlace para iniciar sesión. Por favor revisa tu bandeja de entrada."];
            }
            return View(model);
        }

        [HttpGet("MemberLogin/{token}"), AllowAnonymous]
        public async Task<ActionResult> MemberLogin(string token)
        {
            var (member, messages) = _memberService.LoginMember(token);
            if (member is null)
            {
                TempData["Messages"]= messages;
                return Redirect("~/MemberLogin");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, member.Email),
                new Claim(ClaimTypes.NameIdentifier, member.MemberId.ToString()),
                new Claim(ClaimTypes.Role, "member")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect(Url.Content("~/Member/SelfService"));
        }


    }
}
