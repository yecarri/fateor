using oSoft.Data;
using oSoft.Data.MySql;
using System.Data;

namespace Fateor.Application.Config.Repositories
{
    public sealed class ConfigRepository : Repository<Dto.Config>, IConfigRepository
    {
        public ConfigRepository(IDbConnection connection) : base(connection)
        {
        }

        public ConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ConfigRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
