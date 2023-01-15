using Fateor.Application.Questionnaires.Dto;
using oSoft.Data;

namespace Fateor.Application.Questionnaires.Repositories.Interfaces
{
    public interface IOptionRepository : IRepository<Dto.Option>
    {
        IEnumerable<Option> SelectByQuestionaire(int questionaireId);
        void UpdateQuestionaireOptions(int questionaireId, IEnumerable<Option> options);
    }
}