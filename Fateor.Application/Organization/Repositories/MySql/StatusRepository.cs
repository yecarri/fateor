using Fateor.Application.Organization.Dto;
using Fateor.Application.Organization.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        public StatusRepository(IDbConnection connection) : base(connection)
        {
        }

        public StatusRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public StatusRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
