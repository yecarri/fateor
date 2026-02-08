using Fateor.Application.Organization.Dto;
using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberRepository : IRepository<Member, int>, IDisposable
    {
        IEnumerable<Member> GetActiveMembers();
        IEnumerable<Member> GetByFeeBatchId(int batchId);
        IEnumerable<Member> GetByStatusIds(int[] statusIds);
        IEnumerable<Member> GetElegibleMembers();
        int LastMemberCode();
        Member? GetMemberByEmail(string email);
        MemberLoginToken? GetMemberLoginToken(Guid guid);
        void InsertMemberLoginToken(MemberLoginToken token);
        void UseMemberLoginToken(Guid guid);
    }
}
