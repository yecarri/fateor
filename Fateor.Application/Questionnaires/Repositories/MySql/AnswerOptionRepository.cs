using Dapper;
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
    public class AnswerOptionRepository : Repository<Dto.AnswerOption>, IAnswerOptionRepository
    {
        public AnswerOptionRepository(IDbConnection connection) : base(connection)
        {
        }

        public AnswerOptionRepository(oSoft.Data.UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public AnswerOptionRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Dto.AnswerOption> GetOptionsByAnswer(int answerId)
        {
            return Connection.Query<Dto.AnswerOption>("SELECT * FROM qst_answeroption WHERE AnswerId=?", new { answerId }, Transaction);
        }

        public IEnumerable<Dto.AnswerOption> GetOptionsByResponse(int responseId)
        {
            return Connection.Query<Dto.AnswerOption>("SELECT o.* FROM qst_answeroption o JOIN qst_answer a on a.AnswerId=o.AnswerId WHERE a.ResponseId=?", new { responseId }, Transaction);
        }

        public void Upsert(Dto.AnswerOption optionDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("AnswerId", optionDto.AnswerId);
            parameters.Add("OptionId", optionDto.OptionId);
            parameters.Add("Selected", optionDto.Selected);

            Connection.Execute(
                @"INSERT INTO qst_answeroption (AnswerId, OptionId, Selected) 
                values  (@AnswerId, @OptionId, @Selected) 
                ON DUPLICATE KEY UPDATE  Selected=@Selected", parameters, Transaction);
        }
    }
}
