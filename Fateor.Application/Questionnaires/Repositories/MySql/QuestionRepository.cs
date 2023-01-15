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
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(IDbConnection connection) : base(connection)
        {
        }

        public QuestionRepository(oSoft.Data.UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public QuestionRepository(string connectionString) : base(connectionString)
        {
        }

        public void UpdateQuestionaireQuestions(int questionaireId, IEnumerable<Question> questions)
        {
            var existing = SelectByQuestionaire(questionaireId);

            var toDelete = existing.Where(e => !questions.Any(q => q.QuestionId == e.QuestionId));
            foreach (var q in toDelete)
                Delete(q);

            var toUpdate = questions.Where(q => existing.Any(e => e.QuestionId == q.QuestionId));
            foreach (var q in toDelete)
                Update(q);

            var toInsert = questions.Except(toUpdate);
            foreach (var q in toInsert)
                q.QuestionId = Insert(q);
        }

        public IEnumerable<Question> SelectByQuestionaire(int questionaireId)
        {
            var results = Connection.Query<Question>(
                @"SELECT q.* FROM qst_question q 
                WHERE q.QuestionaireId=?", new { questionaireId }, Transaction);

            return results;
        }

        public void RemoveQuestionsNotInList(int questionaireId, IEnumerable<Question> questions)
        {
            var existing = SelectByQuestionaire(questionaireId);

            var toDelete = existing.Where(e => !questions.Any(q => q.QuestionId == e.QuestionId));
            foreach (var q in toDelete)
                Delete(q);
        }

        public void InsertOrUpdate(Question dto)
        {
            if (dto.QuestionId > 0)
            {
                Update(dto);
            }
            else
            {
                int result = Insert(dto);
            }

        }
    }
}
