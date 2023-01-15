using Dapper;
using Dapper.Contrib.Extensions;
using Fateor.Application.Questionnaires.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Repositories.MySql
{
    public class QuestionnaireRepository : Repository<Dto.Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(IDbConnection connection) : base(connection)
        {
        }

        public QuestionnaireRepository(oSoft.Data.UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public QuestionnaireRepository(string connectionString) : base(connectionString)
        {
        }

        public Dto.Questionnaire Get(int id)
        {
            return Connection.QueryFirst<Dto.Questionnaire>("SELECT * FROM qst_questionaire where QuestionaireId=?", new { id }, Transaction);
        }

        public Dto.Questionnaire GetByName(string name)
        {
            return Connection.QueryFirstOrDefault<Dto.Questionnaire>("SELECT * FROM qst_questionaire where QuestionaireName=?", new { QuestionaireName = name }, Transaction);
        }
    }
}
