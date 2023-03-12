using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Entities;
using Fateor.Application.Questionnaires.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Fateor.Controllers
{
    public class ResponseController : FateorControllerBase
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IResponseService _responseService;
        private readonly IMemberService _memberService;
        private readonly Settings _settings;

        public ResponseController(ILogger<ResponseController> logger,
                                  IMapper mapper,
                                  IEncryptionService encryptionService,
                                  IQuestionnaireService questionnaireService,
                                  IResponseService responseService,
                                  IMemberService memberService,
                                  Settings settings) : base(logger, mapper, encryptionService, settings)
        {
            _questionnaireService=questionnaireService;
            _responseService=responseService;
            _memberService=memberService;
            _settings=settings;
        }

        // GET: Response
        [HttpGet("/response")]
        [Authorize]
        public ActionResult Index()
        {
            var model = new ResponseViewModel();
            model.Questionnaires = _questionnaireService.GetQuestionnaires(false).ToList();
            return View(model);
        }

        [HttpPost("/response/{id?}")]
        [Authorize]
        public ActionResult Index(ResponseViewModel model)
        {
            Response? response = null;
            Questionnaire? questionaire = null;

            if (model.Command == "save" && ModelState.IsValid)
            {
                response = _mapper.Map<ResponseViewModel, Response>(model);
                _responseService.Save(response, CurrentUser?.UserId ?? _settings.SystemUserId);
            }

            model.Questionnaires = _questionnaireService.GetQuestionnaires(false).ToList();


            if (model.ResponseId.HasValue)
            {
                response = _responseService.GetResponseById(model.ResponseId.Value);
                model.QuestionnaireId = response?.QuestionnaireId ?? throw new NullReferenceException(nameof(response.QuestionnaireId));
            }

            if (model.MemberId.HasValue)
            {
                var member = _memberService.GetMemberById(model.MemberId.Value);
                model.Member = member.FullName;
            }

            if (model.QuestionnaireId.HasValue)
            {
                questionaire = _questionnaireService.Get(model.QuestionnaireId.Value);

                if (model.MemberId.HasValue)
                {
                    response = _responseService.GetLastResponseByQuestionaireAndMember(model.QuestionnaireId.Value, model.MemberId.Value);
                    MapViewModel(model, response, questionaire);
                }
            }


            return View(model);
        }

        [HttpGet("response/{id:int}")]
        [Authorize]
        public ActionResult Index(int id)
        {
            var model = new ResponseViewModel();
            model.Questionnaires = _questionnaireService.GetQuestionnaires(false).ToList();
            var response = _responseService.GetResponseById(id);
            if (response != null)
            {
                var questionnaire = _questionnaireService.Get(response.QuestionnaireId);
                var member = _memberService.GetMemberById(response.MemberId);
                model.MemberId = response.MemberId;
                model.Member = member.FullName;
                model.QuestionnaireId = questionnaire.QuestionnaireId;
                model.LastModificationUserId = response.LastModificationUserId;
                model.LastModificationDate = response.LastModificationDate;
                MapViewModel(model, response, questionnaire);
            }
            return View(model);
        }

        [HttpGet("response/{token:regex([[A-Za-z0-9]]{{16,}})}")]
        public ActionResult Token(string token)
        {
            var model = new ResponseViewModel();
            var respKey = _responseService.DecryptResponseKey(token);
            model.Questionnaires = _questionnaireService.GetQuestionnaires(false).ToList();
            model.QuestionnaireId = respKey.QuestionnaireId;
            var questionaire = _questionnaireService.Get(respKey.QuestionnaireId);

            model.MemberId = respKey.MemberId;
            model.Member = _memberService.GetMemberById(respKey.MemberId).FullName;
            var response = _responseService.GetLastResponseByQuestionaireAndMember(model.QuestionnaireId.Value, model.MemberId.Value);
            MapViewModel(model, response, questionaire);
            model.Locked = true;
            return View("Index", model);
        }

        [HttpPost("response/{token:regex([[A-Za-z0-9]]{{16,}})}")]
        public ActionResult Token(string token, ResponseViewModel model)
        {
            Response? response = null;

            if (model.Command == "save" && ModelState.IsValid)
            {
                response = _mapper.Map<Models.ResponseViewModel, Response>(model);
                _responseService.Save(response, CurrentUser?.UserId ?? _settings.SystemUserId);
                var questionaire = _questionnaireService.Get(response.QuestionnaireId);
                var url = questionaire.RedirectUrl.ToLower().Replace("{responseid}", response.ResponseId.ToString());
                return Redirect(Url.Content(url));
            }
            var respKey = _responseService.DecryptResponseKey(token);
            model.Questionnaires = _questionnaireService.GetQuestionnaires(false).ToList();
            model.QuestionnaireId = respKey.QuestionnaireId;
            model.MemberId = respKey.MemberId;
            model.Member = _memberService.GetMemberById(respKey.MemberId).FullName;
            return View("Index", model);

        }

        [HttpGet("response/list/{questionnaireId:int?}")]
        [Authorize]
        public ActionResult List(int? questionnaireId)
        {
            var model = new ResponseListViewModel();
            model.Questionnaires = _questionnaireService.GetQuestionnaires().ToList();
            model.Questionnaires.Insert(0, new Questionnaire() { QuestionnaireId = 0, QuestionnaireName = "- Todos -" });
            model.QuestionnaireId = (int?)HttpContext.Session.GetInt32("RESPLIST_QUESTIONAIRE");

            if (questionnaireId.HasValue)
            {
                model.Responses = _responseService.GetResponsesByQuestionnaireId(questionnaireId.Value).ToList();
            }
            else
            {
                model.Responses = _responseService.GetAll(true).ToList();
            }


            return View(model);
        }

        [HttpPost("response/list")]
        [Authorize]
        public ActionResult List(ResponseListViewModel model)
        {
            if (model.QuestionnaireId.HasValue)
                HttpContext.Session.SetInt32("RESPLIST_QUESTIONAIRE", (int)model.QuestionnaireId);

            model.Questionnaires = _questionnaireService.GetQuestionnaires().ToList();
            model.Questionnaires.Insert(0, new Questionnaire() { QuestionnaireId = 0, QuestionnaireName = "- Todos -" });
            model.Responses = _responseService.GetResponsesByQuestionnaireId(model.QuestionnaireId
                ?? throw new NullReferenceException(nameof(model.QuestionnaireId))).ToList();

            return View(model);
        }

        [HttpGet("response/gettoken")]
        [Authorize]
        public ActionResult GetToken()
        {
            var model = new GetResponseTokenViewModel();
            model.Questionnaires = _questionnaireService.GetQuestionnaires();
            return View("GetToken", model);
        }

        [HttpPost("response/gettoken")]
        [Authorize]
        public ActionResult GetToken(GetResponseTokenViewModel model)
        {
            model.Questionnaires = _questionnaireService.GetQuestionnaires();

            if (ModelState.IsValid && model.MemberId > 0)
            {
                model.Member = _memberService.GetMemberById(model.MemberId).FullName;

                var resp = new ResponseKey(model.MemberId, model.QuestionnaireId);
                model.Token = _responseService.EncryptResponseKey(resp);
            }

            return View("GetToken", model);
        }

        private void MapViewModel(ResponseViewModel model, Response response, Questionnaire questionaire)
        {
            if (response != null)
            {
                ModelState.Clear();
                model.ResponseId = response.ResponseId;
                model.Answers = (from q in questionaire.Questions
                                 join a in response.Answers on q.QuestionId equals a.QuestionId into qa
                                 from a in qa.DefaultIfEmpty()
                                 select new { q, a })
                                 .Select(qa =>
                                    {
                                        var ans = _mapper.Map<Question, AnswerViewModel>(qa.q);
                                        if (qa.a != null)
                                        {
                                            qa.a.QuestionTypeId = qa.q.QuestionTypeId;
                                            _mapper.Map(qa.a, ans);
                                        }

                                        ans.Options = qa.q.Options.Select(o =>
                                        {
                                            var opt = _mapper.Map<Option, AnswerOptionViewModel>(o);
                                            opt.Selected = qa.a?.Options?.FirstOrDefault(x => x.OptionId == o.OptionId)?.Selected ?? false;
                                            return opt;
                                        }).OrderBy(o => o.Ord).ToList();
                                        return ans;
                                    }
                                 ).OrderBy(a => a.Ord).ToList();

            }
            else
            {
                model.Answers = questionaire.Questions.Select(q =>
                {
                    var ans = _mapper.Map<Question, AnswerViewModel>(q);
                    ans.QuestionText = q.QuestionText;
                    ans.QuestionnaireId = questionaire.QuestionnaireId;
                    ans.QuestionTypeId = q.QuestionTypeId;
                    ans.Options = q.Options.Select(o => _mapper.Map<Option, AnswerOptionViewModel>(o)).OrderBy(o => o.Ord).ToList();
                    return ans;
                }).OrderBy(a => a.Ord).ToList();
            }
        }
    }
}