namespace Fateor.Application.Bulletin.Entities
{
    public class BulletinStatusFilter
    {
        public int? BulletinStatusFilterId { get; set; }
        public int BulletinId { get; set; }
        public int StatusId { get; set; }
        public byte Action { get; set; }
    }
}
