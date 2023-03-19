using Fateor.Application.Gdpr.Repositories.Interfaces;
using oSoft.Data.MySql;
using System.Data;

namespace Fateor.Application.Gdpr.Repositories.MySql
{
    public class GdprMemberRepository :  Repository<Dto.GdprMember>, IGdprMemberRepository
    {
        public GdprMemberRepository(IDbConnection connection) : base(connection)
        {
        }

        public GdprMemberRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public GdprMemberRepository(string connectionString) : base(connectionString)
        {
        }
       
    }
}
