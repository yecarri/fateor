using Fateor.Application.Questionnaires.Dto;
using oSoft.Data;

namespace Fateor.Application.Questionnaires.Repositories.Interfaces
{
    public interface IQuestionRepository:IRepository<Question>
    {
        void InsertOrUpdate(Question dto);
        void RemoveQuestionsNotInList(int questionaireId, IEnumerable<Question> questions);
        IEnumerable<Question> SelectByQuestionaire(int questionaireId);
        void UpdateQuestionaireQuestions(int questionaireId, IEnumerable<Question> questions);
    }
}