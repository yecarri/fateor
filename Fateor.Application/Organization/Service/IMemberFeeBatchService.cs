using Fateor.Application.Organization.Entities;

namespace Fateor.Application.Organization.Service
{
    public interface IMemberFeeBatchService
    {
        MemberFeeBatch[] GetMemberFeeBatches(DateTime dateFrom, DateTime dateTo);
    }
}