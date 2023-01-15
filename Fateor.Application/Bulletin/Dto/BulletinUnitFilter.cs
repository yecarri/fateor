using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Bulletin.Dto
{
    [Table("bul_bulletinunitfilter")]
    public class BulletinUnitFilter
    {
        [Key]
        public int BulletinUnitFilterId { get; set; }
        public int BulletinId { get; set; }
        public int UnitId { get; set; }
        public byte Action { get; set; }
        public DateTime LastModificationDate { get; set; } = DateTime.Now;
        public int LastModificationUserId { get; set; } = 0;
    }
}
