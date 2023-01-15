using Fateor.Application.Questionnaires.Dto;
using Fateor.Application.Questionnaires.Entities;
using Fateor.Application.Questionnaires.Repositories.Interfaces;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly IResponseUnitOfWork _uow;

        public ResponseService(IResponseUnitOfWork uow, IMapper mapper, IEncryptionService encryptionService)
        {
            _encryptionService=encryptionService;
            _mapper=mapper;
            _uow=uow;
        }

        public ResponseKey DecryptResponseKey(string token)
        {
            return _encryptionService.DecryptHex<ResponseKey>(token);
        }

        public string EncryptResponseKey(ResponseKey key)
        {
            return _encryptionService.EncryptHex<ResponseKey>(key);
        }

        private Entities.Response MapResponse(Dto.Response responseDto, IEnumerable<Dto.Answer> anserDtos, IEnumerable<Dto.AnswerOption> optionsDto)
        {
            var response = _mapper.Map<Dto.Response, Entities.Response>(responseDto);
            response.Answers = anserDtos.Select(a =>
            {
                var ans = _mapper.Map<Dto.Answer, Entities.Answer>(a);
                ans.Options = optionsDto
                    .Where(o => o.AnswerId == ans.AnswerId)
                    .Select(o => _mapper.Map<Dto.AnswerOption, Entities.AnswerOption>(o)).ToList();
                return ans;
            }).ToList();

            return response;
        }

        public Entities.Response GenerateEmptyResponse(Entities.Questionnaire questionaire, int memberId)
        {
            var response = new Entities.Response()
            {
                MemberId = memberId,
                Answers = questionaire.Questions.Select(q => { var a = _mapper.Map<Entities.Question, Entities.Answer>(q); return a; }).ToList()
            };
            return response;
        }

        public Entities.Response? GetResponseById(int responseId)
        {
            var responseDto = _uow.ResponseRepository.Get(responseId);
            if (responseDto != null)
            {
                var anserDtos = _uow.AnswerRepository.GetByResponseId(responseDto.ResponseId);
                var optionsDto = _uow.AnswerOptionRepository.GetOptionsByResponse(responseDto.ResponseId);
                return MapResponse(responseDto, anserDtos, optionsDto);
            }
            return null;
        }

        public Entities.Response? GetLastResponseByQuestionaireAndMember(int questionaireId, int memberId)
        {
            var responseDto = _uow.ResponseRepository.LastResponseByQuestionaireAndMember(questionaireId, memberId);
            if (responseDto != null)
            {
                var anserDtos = _uow.AnswerRepository.GetByResponseId(responseDto.ResponseId);
                var optionsDto = _uow.AnswerOptionRepository.GetOptionsByResponse(responseDto.ResponseId);
                return MapResponse(responseDto, anserDtos, optionsDto);
            }
            return null;
        }

        public IEnumerable<Entities.Response> GetAll(bool includeQuestionnaireName)
        {
            var ents = GetAll();
            if (includeQuestionnaireName)
                GetQuestionnaireNames(ents);
            return ents;
        }

        public IEnumerable<Entities.Response> GetAll()
        {
            var dtos = _uow.ResponseRepository.GetAll();
            var ents = dtos.Select(r => _mapper.Map<Dto.Response, Entities.Response>(r));
            return ents.ToList();
        }

        public IEnumerable<Entities.Response> GetResponsesByMember(int memberId, bool includeQuestionnaireNames)
        {
            var dtos = _uow.ResponseRepository.GetResponseListByMember(memberId);
            var ents = dtos.Select(r => _mapper.Map<Dto.Response, Entities.Response>(r));

            if (includeQuestionnaireNames)
            {
                GetQuestionnaireNames(ents);
            }

            return ents.ToList();
        }

        public IEnumerable<Entities.Response> GetResponsesByQuestionnaireId(int questionaireId)
        {
            var dtos = _uow.ResponseRepository.GetResponsesByQuestionaireId(questionaireId);
            var ents = dtos.Select(r => _mapper.Map<Dto.Response, Entities.Response>(r));
            return ents.ToList();
        }

        private void GetQuestionnaireNames(IEnumerable<Entities.Response> ents)
        {
            foreach (var e in ents)
            {
                e.Questionnaire = _uow.QuestionnaireRepository.Get(e.QuestionnaireId)?.QuestionaireName ?? "";
            }
        }

        public (bool ok, IList<string> messages) Save(Entities.Response response, int userId)
        {
            try
            {
                var responseDto = _mapper.Map<Entities.Response, Dto.Response>(response);
                responseDto.LastModificationDate = DateTime.Now;
                responseDto.LastModificationUserId = userId;

                if (responseDto.ResponseId > 0)
                {
                    _uow.ResponseRepository.Update(responseDto);
                }
                else
                {
                    response.ResponseId = _uow.ResponseRepository.Insert(responseDto);
                }

                foreach (var a in response.Answers)
                {
                    a.ResponseId = response.ResponseId;
                    var answerDto = _mapper.Map<Entities.Answer, Dto.Answer>(a);
                    answerDto.LastModificationDate = DateTime.Now;
                    answerDto.LastModificationUserId = userId;

                    if (answerDto.AnswerId > 0)
                        _uow.AnswerRepository.Update(answerDto);
                    else
                        a.AnswerId = _uow.AnswerRepository.Insert(answerDto);

                    if ((a.QuestionTypeId & 2) == 2)
                        foreach (var o in a.Options)
                        {
                            var optionDto = _mapper.Map<Entities.AnswerOption, Dto.AnswerOption>(o);
                            optionDto.AnswerId = a.AnswerId;
                            _uow.AnswerOptionRepository.Upsert(optionDto);
                        }
                }
                _uow.Commit();
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return (false, new List<string>() { "Unable to save response", ex.Message });
            }
        }

    }
}
