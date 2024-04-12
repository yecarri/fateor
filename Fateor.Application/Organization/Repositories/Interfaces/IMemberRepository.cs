using Fateor.Application.Organization.Dto;
using oSoft.Data;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberRepository : IRepository<Member>, IDisposable
    {
        Member Get(int id);
        IEnumerable<Member> GetActiveMembers();
        IEnumerable<Member> GetByFeeBatchId(int batchId);
        IEnumerable<Member> GetByStatusIds(int[] statusIds);
        IEnumerable<Member> GetElegibleMembers();
        int LastMemberCode();
    }
}
