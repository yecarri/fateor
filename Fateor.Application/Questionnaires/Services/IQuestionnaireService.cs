using Fateor.Application.Questionnaires.Entities;

namespace Fateor.Application.Questionnaires.Services
{
    public interface IQuestionnaireService
    {
        Questionnaire? Get(int id);
        Questionnaire? GetQuestionaireByName(string name);
        IEnumerable<Questionnaire> GetQuestionnaires(bool includeQuestions = false);
        string GetQuestionaireToken(int memberId, int questionaireId);
        IEnumerable<QuestionType> GetQuestionTypes();
        void Save(Questionnaire questionnaire, int currentUserId);
    }
}