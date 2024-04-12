using Fateor.Application.Organization.Dto;
using oSoft.Data;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberStatusRepository : IRepository<MemberStatus>, IDisposable
    {
        IEnumerable<MemberStatus> GetAllStatusesAt(DateTime date);
        IEnumerable<MemberStatus> GetByMemberId(int id);
        MemberStatus GetLastMemberStatus(int memberId);
        MemberStatus GetMemberStatusAtInstant(int memberId, DateTime date);
    }
}
