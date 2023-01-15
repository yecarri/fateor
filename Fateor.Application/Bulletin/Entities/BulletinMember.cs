using System;

namespace Fateor.Application.Bulletin.Entities
{
    public class BulletinMember
    {
        public int BulletinMemberId { get; set; }
        public int BulletinId { get; set; }
        public int MemberId { get; set; }
        public string FullName { get; set; } = "";
        public byte Status { get; set; }
        public DateTime? DateSent { get; set; }
    }
}
