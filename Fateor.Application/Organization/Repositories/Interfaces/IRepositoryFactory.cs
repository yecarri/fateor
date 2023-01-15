using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        IActivityRepository GetActivityRepository(IUnitOfWork unitOfWork);
        IActivityTypeRepository GetActivityTypeRepository(IUnitOfWork unitOfWork);
        IIdentityDocumentTypeRepository GetIdentityDocumentTypeRepository(IUnitOfWork unitOfWork);
        IMemberActivityRepository GetMemberActivityRepository(IUnitOfWork unitOfWork);
        IMemberRepository GetMemberRepository(IUnitOfWork unitOfWork);
        IMemberStatusRepository GetMemberStatusRepository(IUnitOfWork unitOfWork);
        IStatusRepository GetStatusRepository(IUnitOfWork unitOfWork);
        IUnitRepository GetUnitRepository(IUnitOfWork unitOfWork);
        IUnitOfWork GetUnitOfWork();
    }
}
