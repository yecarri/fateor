using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Bulletin.Dto
{
    [Table("bul_bulletinstatusfilter")]
    public class BulletinStatusFilter
    {
        [Key]
        public int BulletinStatusFilterId { get; set; }
        public int BulletinId { get; set; }
        public int StatusId { get; set; }
        public byte Action { get; set; }
        public DateTime LastModificationDate { get; set; } = DateTime.Now;
        public int LastModificationUserId { get; set; } = 0;
    }
}
