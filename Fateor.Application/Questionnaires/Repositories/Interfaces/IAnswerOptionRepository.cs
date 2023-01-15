using Fateor.Application.Questionnaires.Dto;
using oSoft.Data;

namespace Fateor.Application.Questionnaires.Repositories.Interfaces
{
    public interface IAnswerOptionRepository:IRepository<AnswerOption>
    {
        IEnumerable<AnswerOption> GetOptionsByAnswer(int answerId);
        IEnumerable<AnswerOption> GetOptionsByResponse(int responseId);
        void Upsert(AnswerOption optionDto);
    }
}