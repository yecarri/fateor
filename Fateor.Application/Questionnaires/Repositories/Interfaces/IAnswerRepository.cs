using Fateor.Application.Questionnaires.Dto;
using oSoft.Data;

namespace Fateor.Application.Questionnaires.Repositories.Interfaces
{
    public interface IAnswerRepository: IRepository<Answer>
    {
        IEnumerable<Answer> GetByResponseId(object id);
    }
}