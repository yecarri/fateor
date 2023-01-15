using Fateor.Application.Questionnaires.Dto;
using Fateor.Application.Questionnaires.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Data;

namespace Fateor.Application.Questionnaires.Repositories.MySql
{
    public class QuestionTypeRepository : Repository<QuestionType>, IQuestionTypeRepository
    {
        public QuestionTypeRepository(IDbConnection connection) : base(connection)
        {
        }

        public QuestionTypeRepository(oSoft.Data.UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public QuestionTypeRepository(string connectionString) : base(connectionString)
        {
        }
    }
}