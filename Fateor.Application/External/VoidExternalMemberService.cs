using Fateor.Application.Organization.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.External
{
    public class VoidExternalMemberService : IExternalMemberService
    {
        public (bool, IList<string>) CreateMember(Member member)
        {
            return (true, new List<string>());
        }
    }
}
