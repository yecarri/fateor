using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.External
{
    public interface IExternalMemberService
    {
        (bool, IList<string>) CreateMember(Organization.Entities.Member member);
    }
}
