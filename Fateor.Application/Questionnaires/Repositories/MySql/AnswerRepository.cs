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
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(IDbConnection connection) : base(connection)
        {
        }

        public AnswerRepository(oSoft.Data.UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public AnswerRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Answer> GetByResponseId(object id)
        {
            return Connection.Query<Answer>("SELECT * FROM qst_answer WHERE ResponseId=?", new { id }, Transaction);
        }

    }
}
