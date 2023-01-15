namespace Fateor.Application.Bulletin.Entities
{
    public class BulletinCountryFilter
    {
        public int? BulletinCountryFilterId { get; set; }
        public int BulletinId { get; set; }
        public int CountryId { get; set; }
        public byte Action { get; set; }
    }
}
