using Fateor.Application.Organization.Dto;
using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
