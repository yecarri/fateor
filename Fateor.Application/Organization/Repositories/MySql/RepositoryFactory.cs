using Fateor.Application.Organization.Repositories.Interfaces;
using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class RepositoryFactory : IRepositoryFactory
    {
        readonly string _ConnectionString;
        public RepositoryFactory(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public IActivityRepository GetActivityRepository(IUnitOfWork unitOfWork)
        {
            return new ActivityRepository(unitOfWork);
        }

        public IActivityTypeRepository GetActivityTypeRepository(IUnitOfWork unitOfWork)
        {
            return new ActivityTypeRepository(unitOfWork);
        }

        public IIdentityDocumentTypeRepository GetIdentityDocumentTypeRepository(IUnitOfWork unitOfWork)
        {
            return new IdentityDocumentTypeRepository(unitOfWork);
        }

        public IMemberActivityRepository GetMemberActivityRepository(IUnitOfWork unitOfWork)
        {
            return new MemberActivityRepository(unitOfWork);
        }

        public IMemberRepository GetMemberRepository(IUnitOfWork unitOfWork)
        {
            return new MemberRepository(unitOfWork);
        }

        public IMemberStatusRepository GetMemberStatusRepository(IUnitOfWork unitOfWork)
        {
            return new MemberStatusRepository(unitOfWork);
        }

        public IStatusRepository GetStatusRepository(IUnitOfWork unitOfWork)
        {
            return new StatusRepository(unitOfWork);
        }

        public IUnitRepository GetUnitRepository(IUnitOfWork unitOfWork)
        {
            return new UnitRepository(unitOfWork);
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return new oSoft.Data.MySql.UnitOfWork(_ConnectionString);
        }

    }
}
