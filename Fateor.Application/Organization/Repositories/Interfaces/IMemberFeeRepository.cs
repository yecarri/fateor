using Fateor.Application.Organization.Dto;
using oSoft.Data;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberFeeRepository : IRepository<MemberFee>
    {
        IEnumerable<MemberFee> GetByBatchId(int batchId);
    }
}
