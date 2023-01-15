using Dapper;
using Fateor.Application.Questionnaires.Dto;
using Fateor.Application.Questionnaires.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Repositories.MySql
{
    public class OptionRepository : Repository<Option>, IOptionRepository
    {
        public OptionRepository(IDbConnection connection) : base(connection)
        {
        }

        public OptionRepository(oSoft.Data.UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public OptionRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Option> SelectByQuestionaire(int questionaireId)
        {
            var results = Connection.Query<Option>(
                @"SELECT o.* FROM qst_option o 
                join qst_question q on o.QuestionId=q.QuestionId 
                WHERE q.QuestionaireId=?", new { questionaireId }, Transaction);

            return results;
        }

        public void UpdateQuestionaireOptions(int questionaireId, IEnumerable<Option> options)
        {
            var existing = SelectByQuestionaire(questionaireId);

            var toDelete = existing.Where(e => !options.Any(o => o.OptionId == e.OptionId));
            foreach (var o in toDelete)
                Delete(o);

            var toUpdate = options.Where(o => existing.Any(e => e.OptionId == o.OptionId));
            foreach (var o in toUpdate)
                Update(o);

            var toInsert = options.Except(toUpdate);
            foreach (var o in toInsert)
                o.OptionId = Insert(o);
        }
    }
}
