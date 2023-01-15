using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Bulletin.Dto
{
    [Table("bul_bulletin_member")]
    public class BulletinMember
    {
        [Key]
        public int BulletinMemberId { get; set; }
        public int BulletinId { get; set; }
        public int MemberId { get; set; }
        public byte Status { get; set; } = 0;
        public DateTime? DateSent { get; set; }
    }
}
