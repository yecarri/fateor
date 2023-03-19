using Fateor.Application.Gdpr.Repositories.Interfaces;
using oSoft.Data.MySql;
using System.Data;

namespace Fateor.Application.Gdpr.Repositories.MySql
{
    public class GdprRepository : Repository<Dto.Gdpr>, IGdprRepository
    {
        public GdprRepository(IDbConnection connection) : base(connection)
        {
        }

        public GdprRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public GdprRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
