using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fateor.Models;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Application.Questionnaires.Services;
using Fateor.Application.Questionnaires.Entities;

namespace Fateor.Controllers
{
    [Authorize]
    public sealed class QuestionnaireController : FateorControllerBase
    {
        private readonly IQuestionnaireService _questionnaireService;

        public QuestionnaireController(ILogger<QuestionnaireController> logger,
                                       IMapper mapper,
                                       IEncryptionService encryptionService,
                                       IQuestionnaireService questionnaireService,
                                       Settings settings) : base(logger, mapper, encryptionService, settings)
        {
            _questionnaireService=questionnaireService;
        }

        [HttpGet]
        [Route("Questionnaire/Create")]
        public ActionResult QuestionnaireCreate()
        {
            var model = new QuestionnaireViewModel()
            {
                QuestionTypes = _questionnaireService.GetQuestionTypes()
            };
            return View("Edit", model);
        }

        [HttpPost]
        [Route("Questionnaire/Create")]
        public ActionResult QuestionnaireCreate(QuestionnaireViewModel model)
        {
            if (ModelState.IsValid && model.Command == "Save")
            {
                bool saved = UpdateQuestionnaire(ref model);
                model.QuestionTypes = _questionnaireService.GetQuestionTypes();
                if (saved)
                {
                    model.Messages = new List<string>() { "Registro Guardado" };
                    return Redirect($"Edit/{model.QuestionnaireId}");
                }
            }
            else if (model.Command != "Save")
            {
                UpdateQuestionnaire(ref model);
                ModelState.Clear();
            }
            model.QuestionTypes = _questionnaireService.GetQuestionTypes();
            return View("Edit", model);
        }

        [HttpGet]
        [Route("Questionnaire/{id:int}")]
        [Route("Questionnaire/Edit/{id:int}")]
        public ActionResult QuestionnaireEdit(int id)
        {
            QuestionnaireViewModel model =
                _mapper.Map<Questionnaire, QuestionnaireViewModel>(_questionnaireService.Get(id));

            model.QuestionTypes = _questionnaireService.GetQuestionTypes();
            return View("Edit", model);
        }

        [HttpPost]
        [Route("Questionnaire/{id:int}")]
        [Route("Questionnaire/Edit/{id:int}")]
        public ActionResult QuestionnaireEdit(QuestionnaireViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool saved = UpdateQuestionnaire(ref model);
                model.QuestionTypes = _questionnaireService.GetQuestionTypes();
                if (saved)
                {
                    model.Messages = new List<string>() { "Registro Guardado" };
                }
                ModelState.Clear();
            }
            model.QuestionTypes = _questionnaireService.GetQuestionTypes();
            return View("Edit", model);
        }

        [HttpGet]
        [Route("Questionnaire/Delete/{id:int}")]
        public ActionResult QuestionnaireDelete(int id)
        {
            return View();
        }

        private bool UpdateQuestionnaire(ref QuestionnaireViewModel model)
        {
            int i = -1, j = -1;
            if (!string.IsNullOrEmpty(model.Tags))
            {
                var tags = model.Tags?.Split(',');

                if (tags.Length > 0)
                    int.TryParse(tags[0], out i);

                if (tags.Length > 1)
                    int.TryParse(tags[1], out j);
            }

            switch (model.Command)
            {
                case "AddQuestion":
                    model.Questions.Add(new QuestionViewModel());
                    break;
                case "DeleteQuestion":
                    model.Questions.RemoveAt(i);
                    break;
                case "MoveQuestionUp":
                    var q = model.Questions[i];
                    model.Questions.RemoveAt(i);
                    model.Questions.Insert(i - 1, q);
                    break;
                case "MoveQuestionDown":
                    q = model.Questions[i];
                    model.Questions.RemoveAt(i);
                    model.Questions.Insert(i + 1, q);
                    break;
                case "AddOption":
                    model.Questions[i].Options.Add(new OptionViewModel());
                    break;
                case "DeleteOption":
                    model.Questions[i].Options.RemoveAt(j);
                    break;
                case "MoveOptionUp":
                    var o = model.Questions[i].Options[j];
                    model.Questions[i].Options.RemoveAt(j);
                    model.Questions[i].Options.Insert(j - 1, o);
                    break;
                case "MoveOptionDown":
                    o = model.Questions[i].Options[j];
                    model.Questions[i].Options.RemoveAt(j);
                    model.Questions[i].Options.Insert(j + 1, o);
                    break;
                case "Save":
                    Renumber(model);
                    if (ModelState.IsValid)
                    {
                        var Questionnaire = _mapper.Map<QuestionnaireViewModel, Questionnaire>(model);
                        _questionnaireService.Save(Questionnaire, CurrentUser.UserId.Value);
                        model = _mapper.Map<Questionnaire, QuestionnaireViewModel>(Questionnaire);
                        return true;
                    }
                    break;
            }
            return false;
        }

        private static void Renumber(QuestionnaireViewModel model)
        {
            Enumerable.Range(0, model.Questions.Count)
                .Select(i =>
                {
                    var qst = model.Questions[i];

                    qst.Ord = i;

                    Enumerable.Range(0, qst.Options.Count)
                        .Select(j =>
                        {
                            var opt = qst.Options[j];
                            opt.Ord = j;
                            return opt;
                        }).ToList();

                    return qst;
                }).ToList();
        }

        [Route("Questionnaire")]
        public ActionResult List()
        {
            var model = new QuestionnaireListViewModel();
            model.Questionnaires = _questionnaireService.GetQuestionnaires(false).ToList();
            return View(model);
        }

    }
}