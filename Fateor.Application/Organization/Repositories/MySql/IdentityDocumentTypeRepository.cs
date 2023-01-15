using Fateor.Application.Organization.Dto;
using Fateor.Application.Organization.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class IdentityDocumentTypeRepository : Repository<IdentityDocumentType>, IIdentityDocumentTypeRepository
    {
        public IdentityDocumentTypeRepository(IDbConnection connection) : base(connection)
        {
        }

        public IdentityDocumentTypeRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IdentityDocumentTypeRepository(string connectionString) : base(connectionString)
        {
        }
    }
}