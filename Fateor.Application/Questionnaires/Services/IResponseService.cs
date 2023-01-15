using Fateor.Application.Questionnaires.Entities;

namespace Fateor.Application.Questionnaires.Services
{
    public interface IResponseService
    {
        ResponseKey DecryptResponseKey(string token);
        string EncryptResponseKey(ResponseKey key);
        Response GenerateEmptyResponse(Questionnaire questionaire, int memberId);
        Response? GetLastResponseByQuestionaireAndMember(int questionaireId, int memberId);
        Response? GetResponseById(int responseId);
        IEnumerable<Response> GetAll();
        IEnumerable<Response> GetAll(bool includeQuestionnaireName);
        IEnumerable<Response> GetResponsesByQuestionnaireId(int questionaireId); 
        IEnumerable<Response> GetResponsesByMember(int memberId, bool includeQuestionnaireName);        
        (bool ok, IList<string> messages) Save(Response response, int userId);
    }
}