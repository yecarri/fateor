using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Organization.Entities
{
    public class MemberStatus
    {
        public int MemberId { get; set; }

        public int UnitId { get; set; }

        public string UnitName { get; set; } = string.Empty;

        public int StatusId { get; set; }

        public string StatusDescription { get; set; } = string.Empty;

        public DateTime StatusDate { get; set; }

        public bool IsActiveMember { get; set; }

        public int LastModificationUserId { get; set; }

        public string LastModificationUser { get; set; }= string.Empty;

        public DateTime LastModificationDate { get; set; }
        
    }
}