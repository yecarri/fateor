using Fateor.Application.Organization.Dto;
using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
