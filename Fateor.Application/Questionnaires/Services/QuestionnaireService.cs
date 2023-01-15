
using Fateor.Application.Questionnaires.Repositories.Interfaces;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Questionnaires.Services
{
    public sealed class QuestionnaireService : IQuestionnaireService
    {
        private readonly IQuestionnaireUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;

        public QuestionnaireService(IQuestionnaireUnitOfWork uow, IMapper mapper, IEncryptionService encryptionService)
        {
            _uow=uow;
            _mapper=mapper;
            _encryptionService=encryptionService;
        }

        private Entities.Questionnaire MapQuestionaire(IEnumerable<Dto.Question> questionDtos, IEnumerable<Dto.Option> optionDtos, Entities.Questionnaire questionaire)
        {
            questionaire.Questions = questionDtos
                .OrderBy(q => q.Ord)
                .Select(q =>
                {
                    var question = _mapper.Map<Dto.Question, Entities.Question>(q);

                    question.Options = optionDtos.Where(o => o.QuestionId == question.QuestionId)
                        .OrderBy(o => o.Ord)
                        .Select(o => _mapper.Map<Dto.Option, Entities.Option>(o)).ToList();

                    return question;
                }).ToList();

            return questionaire;
        }

        public Entities.Questionnaire? Get(int id)
        {

            var questionaireDto = _uow.QuestionnaireRepository.Get(id);
            var questionDtos = _uow.QuestionRepository.SelectByQuestionaire(id);
            var optionDtos = _uow.OptionRepository.SelectByQuestionaire(id);

            var questionaire = _mapper.Map<Dto.Questionnaire, Entities.Questionnaire>(questionaireDto);
            return MapQuestionaire(questionDtos, optionDtos, questionaire);

        }

        public Entities.Questionnaire? GetQuestionaireByName(string name)
        {
            var questionaireDto = _uow.QuestionnaireRepository.GetByName(name);
            if (questionaireDto != null)
            {
                var questionDtos = _uow.QuestionRepository.SelectByQuestionaire(questionaireDto.QuestionaireId);
                var optionDtos = _uow.OptionRepository.SelectByQuestionaire(questionaireDto.QuestionaireId);
                var questionaire = _mapper.Map<Dto.Questionnaire, Entities.Questionnaire>(questionaireDto);
                return MapQuestionaire(questionDtos, optionDtos, questionaire);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Entities.Questionnaire> GetQuestionnaires(bool includeQuestions = false)
        {

            var questionaireDtos = _uow.QuestionnaireRepository.GetAll();
            var questionaires = questionaireDtos.Select(q => _mapper.Map<Dto.Questionnaire, Entities.Questionnaire>(q));

            if (includeQuestions)
            {
                var questionDtos = _uow.QuestionRepository.GetAll();
                var optionDtos = _uow.OptionRepository.GetAll();

                foreach (var questionaire in questionaires)
                {

                    questionaire.Questions = questionDtos
                        .OrderBy(q => q.Ord)
                        .Select(q =>
                        {
                            var question = _mapper.Map<Dto.Question, Entities.Question>(q);

                            question.Options = optionDtos.Where(o => o.QuestionId == question.QuestionId)
                                .OrderBy(o => o.Ord)
                                .Select(o => _mapper.Map<Dto.Option, Entities.Option>(o)).ToList();

                            return question;
                        }).ToList();
                }
            }
            return questionaires.ToList();
        }

        public IEnumerable<Entities.QuestionType> GetQuestionTypes()
        {
            var result = _uow.QuestionTypeRepository.GetAll();
            return result.Select(qt =>
                _mapper.Map<Dto.QuestionType, Entities.QuestionType>(qt));
        }

        public void Save(Entities.Questionnaire questionnaire, int currentUserId)
        {

            var questionaireDto = _mapper.Map<Entities.Questionnaire, Dto.Questionnaire>(questionnaire);
            questionaireDto.LastModificationDate = DateTime.Now;
            questionaireDto.LastModificationUserId = currentUserId;

            if (questionnaire.QuestionnaireId > 0)
            {
                _uow.QuestionnaireRepository.Update(questionaireDto);
            }
            else
            {
                questionnaire.QuestionnaireId = _uow.QuestionnaireRepository.Insert(questionaireDto);
            }

            var questionDtos = questionnaire.Questions.Select(q =>
            {
                q.QuestionaireId = questionnaire.QuestionnaireId;

                var dto = _mapper.Map<Entities.Question, Dto.Question>(q);

                dto.LastModificationUserId = currentUserId;
                dto.LastModificationDate = DateTime.Now;
                _uow.QuestionRepository.InsertOrUpdate(dto);

                q.QuestionId = dto.QuestionId;

                return dto;
            }).ToList();

            var optionDtos = questionnaire.Questions.SelectMany(q => q.Options.Select(o =>
            {
                o.QuestionId = q.QuestionId;
                var dto = _mapper.Map<Entities.Option, Dto.Option>(o);
                dto.LastModificationDate = DateTime.Now;
                dto.LastModificationUserId = currentUserId;
                return dto;
            }
            )).ToList();

            _uow.OptionRepository.UpdateQuestionaireOptions(questionnaire.QuestionnaireId, optionDtos);
            _uow.QuestionRepository.RemoveQuestionsNotInList(questionnaire.QuestionnaireId, questionDtos);
            _uow.Commit();

        }

        public string GetQuestionaireToken(int memberId, int questionaireId)
        {
            var resp = new Entities.ResponseKey(memberId, questionaireId);
            return _encryptionService.EncryptHex(resp);
        }
    }
}