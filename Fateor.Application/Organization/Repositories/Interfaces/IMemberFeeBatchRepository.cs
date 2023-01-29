using Fateor.Application.Organization.Dto;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberFeeBatchRepository
    {
        IEnumerable<MemberFeeBatch> GetMemberFeeBatches(DateTime dateFrom, DateTime dateTo);
    }
}