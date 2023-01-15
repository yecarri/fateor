using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Location.Dto
{
    [Table("loc_region")]
    public class Region
    {
        [Key]
        public int RegionId { get; set; }
        public string RegionName { get; set; } = "";
        public int CountryId { get; set; }

        [Computed]
        public string CountryName { get; set; } = "";
    }
}