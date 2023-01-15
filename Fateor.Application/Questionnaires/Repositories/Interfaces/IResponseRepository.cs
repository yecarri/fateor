using Fateor.Application.Questionnaires.Dto;
using oSoft.Data;

namespace Fateor.Application.Questionnaires.Repositories.Interfaces
{
    public interface IResponseRepository: IRepository<Response>
    {
        Response Get(int id);
        IEnumerable<Response> GetResponseListByMember(int memberId);
        IEnumerable<Response> GetResponsesByQuestionaireId(int questionaireId);
        Response LastResponseByQuestionaireAndMember(int questionaireId, int memberId);
    }
}