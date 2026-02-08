using System;
using System.Collections.Generic;
using System.Text;

namespace Fateor.Application.Organization.Dto
{
    public class MemberLoginToken
    {
        public Guid Guid { get; set; } 
        public int MemberId { get; set; }
        public DateTime Expiration { get; set; }
    }
}
