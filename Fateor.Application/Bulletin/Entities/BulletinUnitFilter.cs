namespace Fateor.Application.Bulletin.Entities
{
    public class BulletinUnitFilter
    {
        public int? BulletinUnitFilterId { get; set; }
        public int BulletinId { get; set; }
        public int UnitId { get; set; }
        public byte Action { get; set; }
    }
}
