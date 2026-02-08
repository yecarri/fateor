using Fateor.Application.Organization.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Service
{
    public interface IMemberService
    {
        Member GetMemberById(int id, bool withStatuses = true);
        (bool ok, IList<string> messages) CreateMember(Member member, int userId);
        (bool ok, IList<string> messages) UpdateMember(Member member, int userId);
        (bool ok, IList<string> messages) AssignMemberCode(Member member, int userId);
        (bool ok, IList<string> messages) UpdateMemberStatus(MemberStatus status, int value);
        public (Member? member, IList<string> messages) LoginMember(string token);
        IList<MemberStatus> GetMemberStatusHistory(int memberId);
        IEnumerable<Member> GetAllMembers();
        IEnumerable <Member> GetElegibleMembers();
        void RequestMemberLogin(string email);
        
    }
}
