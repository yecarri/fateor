using Fateor.Application.Organization.Entities;

namespace Fateor.Application.Organization.Service
{
    public interface IMemberFeeService
    {
        MemberFeeBatch GetMemberFeeBatchById(int batchFeeId, bool includeMemberFees = false);
        MemberFeeBatch[] GetMemberFeeBatches(DateTime dateFrom, DateTime dateTo);
        IEnumerable<MemberFee> MockFees(MemberFeeBatch batch);
    }
}