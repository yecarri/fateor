using System;
using System.Collections.Generic;
using System.Text;

namespace Fateor.Application.Organization.Entities
{
    [Serializable]
    public class MemberLoginToken
    {
        public MemberLoginToken(int memberId, DateTime expiration)
        {
            MemberId = memberId;
            Expiration = expiration;
        }
        public Guid Guid { get; set; } = Guid.CreateVersion7();
        public int MemberId { get; set; }
        public DateTime Expiration { get; set; }
    }
}
