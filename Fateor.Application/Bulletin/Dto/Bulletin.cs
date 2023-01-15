using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Bulletin.Dto
{

    [Table("bul_Bulletin")]
    public class Bulletin
    {
        [Key]
        public int BulletinId { get; set; }
        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
        public byte BulletinStatusId { get; set; }
        public string BulletinStatusName { get; set; } = "";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModificationDate { get; set; } = DateTime.Now;
        public int LastModificationUserId { get; set; } = 0;
    }
}
