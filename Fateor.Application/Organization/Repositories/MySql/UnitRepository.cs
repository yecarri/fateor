using Fateor.Application.Organization.Dto;
using Fateor.Application.Organization.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class UnitRepository : Repository<Unit>, IUnitRepository
    {
        public UnitRepository(IDbConnection connection) : base(connection)
        {
        }

        public UnitRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public UnitRepository(string connectionString) : base(connectionString)
        {
        }
    }
}