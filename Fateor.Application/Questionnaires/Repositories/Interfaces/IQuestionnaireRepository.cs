using Fateor.Application.Questionnaires.Dto;
using oSoft.Data;

namespace Fateor.Application.Questionnaires.Repositories.Interfaces
{
    public interface IQuestionnaireRepository:IRepository<Questionnaire>
    {
        Questionnaire Get(int id);
        Questionnaire GetByName(string name);
    }
}