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
    class ActivityTypeRepository : Repository<Dto.ActivityType>, IActivityTypeRepository
    {
        public ActivityTypeRepository(IDbConnection connection) : base(connection)
        {
        }

        public ActivityTypeRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ActivityTypeRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
