using Fateor.Application.Organization.Dto;
using oSoft.Data;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberFeeBatchStatusRepository : IRepository<MemberFeeBatchSelectedStatus>
    {
        IEnumerable<MemberFeeBatchSelectedStatus> GetByFeeBatchId(int batchId);
        public void Update(int m0emberFeeBatchId, int[] selectedStatuses);
    }
}