using System;
using System.Collections.Generic;

namespace Fateor.Application.Bulletin.Entities
{

    public class Bulletin
    {
        public int? BulletinId { get; set; }
        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
        public byte BulletinStatusId { get; set; }
        public string BulletinStatusName { get; set; } = "";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<BulletinMember> Members { get; set; } = new List<BulletinMember>();
        public IEnumerable<BulletinStatusFilter> StatusFilters { get; set; } = new List<BulletinStatusFilter>();
        public IEnumerable<BulletinCountryFilter> CountryFilters { get; set; } = new List<BulletinCountryFilter>();
        public IEnumerable<BulletinUnitFilter> UnitFilters { get; set; } = new List<BulletinUnitFilter>();
        public DateTime LastModificationDate { get; set; }
    }
}
