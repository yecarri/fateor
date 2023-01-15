using Dapper.Contrib.Extensions;

namespace Fateor.Application.Bulletin.Dto
{
    [Table("bul_BulletinStatus")]
    public class BulletinStatus
    {
        [ExplicitKey]
        public byte BulletinStatusId { get; set; }
        public string BulletinStatusName { get; set; }= "";
    }
}