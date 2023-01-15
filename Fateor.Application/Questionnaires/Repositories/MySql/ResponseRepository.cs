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
    public class ResponseRepository : Repository<Response>, IResponseRepository
    {
        private const string SELECT = @"
            SELECT r.*, q.QuestionaireName as Questionaire, 
                concat(m.GivenName, ' ', m.Surname) as `Member`, 
                u.Login LastModificationUser
            FROM qst_response r 
            JOIN org_member m on m.MemberId=r.MemberId 
            JOIN qst_questionaire q on r.QuestionaireId=q.QuestionaireId
            JOIN app_user u on r.LastModificationUserId=u.UserId ";

        public ResponseRepository(IDbConnection connection) : base(connection)
        {
        }

        public ResponseRepository(oSoft.Data.UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ResponseRepository(string connectionString) : base(connectionString)
        {
        }

        public Response Get(int id)
        {
            return Connection.QueryFirstOrDefault<Response>(SELECT + " WHERE ResponseId=?", new { id });
        }

        public Response LastResponseByQuestionaireAndMember(int questionaireId, int memberId)
        {
            return Connection.QueryFirstOrDefault<Response>(SELECT + " WHERE r.QuestionaireId=? AND r.MemberId=? ORDER BY r.LastModificationDate DESC LIMIT 1", new { questionaireId, memberId });
        }

        public IEnumerable<Response> GetResponseListByMember(int memberId)
        {
            return Connection.Query<Response>("SELECT r.* FROM qst_questionaire q JOIN LATERAL (SELECT * FROM qst_response r WHERE MemberId=? AND QuestionaireId=q.QuestionaireId ORDER BY LastModificationDate DESC LIMIT 1) r ON 1=1", new { memberId });
        }

        public override IEnumerable<Response> GetAll()
        {
            return Connection.Query<Response>(SELECT);
        }

        public IEnumerable<Response> GetResponsesByQuestionaireId(int questionaireId)
        {
            return Connection.Query<Response>(SELECT + " WHERE r.QuestionaireId=?", new { questionaireId });
        }
    }
}
