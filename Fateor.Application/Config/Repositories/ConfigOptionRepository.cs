using Fateor.Application.Config.Dto;
using oSoft.Data;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace Fateor.Application.Config.Repositories
{
    public class ConfigOptionRepository : Repository<Dto.ConfigOption>, IConfigOptionRepository
    {
        public ConfigOptionRepository(IDbConnection connection) : base(connection)
        {
        }

        public ConfigOptionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ConfigOptionRepository(string connectionString) : base(connectionString)
        {
        }

        public override IEnumerable<ConfigOption> GetAll()
        {
            if(Connection is null)
                throw new NullReferenceException(nameof(Connection));
            string sql = "SELECT ConfigCode, Number, Value FROM app_configoption ORDER BY ConfigCode, Number";
            return Connection.Query<ConfigOption>(sql);
        }
    }
}
