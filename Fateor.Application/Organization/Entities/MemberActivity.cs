using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Entities
{
    public class MemberActivity
    {
        public int ActivityId { get; set; }
        public int MemberId { get; set; }
        public int Participation { get; set; }
        public DateTime LastModificationDate { get; set; }
        public int LastModificationUserId { get; set; }
        public string GivenName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
