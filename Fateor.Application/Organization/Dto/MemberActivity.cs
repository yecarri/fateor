using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Dto
{
    public class MemberActivity
    {

        [ExplicitKey]
        public int ActivityId { get; set; }
        [ExplicitKey]
        public int MemberId { get; set; }
        public int Participation { get; set; }
        public string GivenName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int LastModificationUserId { get; set; }

    }
}
