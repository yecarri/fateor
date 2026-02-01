using Fateor.Application.Organization.Dto;
using oSoft.Data;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberFeeBatchRepository: IRepository<MemberFeeBatch, int>    
    {
        MemberFeeBatch Get(int memberFeeBatchId);
        IEnumerable<MemberFeeBatch> GetMemberFeeBatches(DateTime dateFrom, DateTime dateTo);
    }
}