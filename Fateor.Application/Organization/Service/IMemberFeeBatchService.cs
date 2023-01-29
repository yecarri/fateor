using Fateor.Application.Organization.Entities;

namespace Fateor.Application.Organization.Service
{
    public interface IMemberFeeBatchService
    {
        MemberFeeBatch[] GetMemberFeeBatches(DateOnly dateFrom, DateOnly dateTo);
    }
}