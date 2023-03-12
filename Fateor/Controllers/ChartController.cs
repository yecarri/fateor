using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fateor.Controllers
{
    public class ChartController : FateorControllerBase
    {
        private readonly IResponseService _responseService;
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IMemberService _memberService;

        public ChartController(ILogger logger,
                               IMapper mapper,
                               IEncryptionService encryptionService,
                               Settings settings,
                               IResponseService responseService,
                               IQuestionnaireService questionnaireService,
                               IMemberService memberService) : base(logger, mapper, encryptionService, settings)
        {
            _responseService=responseService;
            _questionnaireService=questionnaireService;
            _memberService=memberService;
        }

        // GET: Chart
        [HttpGet("chart/2d")]
        public ActionResult Chart2D()
        {
            return View(new Models.NolanViewModel());
        }

        // GET: Chart
        [HttpGet("nolan/{responseId:int}")]
        public ActionResult Nolan(int? responseId)
        {
            var model = new Models.NolanViewModel();
            var response = _responseService.GetResponseById(responseId.Value);
            var questionaire = _questionnaireService.Get(response.QuestionnaireId);
            model.Member = _memberService.GetMemberById(response.MemberId).FullName;
            model.External = (CurrentUser == null);

            var results = (from r in response.Answers.Select(a => new { a.QuestionId, a.OptionId })
                           join q in questionaire.Questions on r.QuestionId equals q.QuestionId
                           select new { r.QuestionId, q.Ord, Option = q.Options.FirstOrDefault(o => o.OptionId == r.OptionId) });

            model.X = (double)(results.Where(o => o.Ord >= 10 && (o.Option?.Points.HasValue ?? false)).Sum(o => o.Option.Points));
            model.Y = (double)(results.Where(o => o.Ord < 10 && (o.Option?.Points.HasValue ?? false)).Sum(o => o.Option.Points));
            return View(model);
        }
    }
}