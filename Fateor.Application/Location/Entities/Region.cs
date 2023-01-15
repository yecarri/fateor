using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Location.Entities
{
    public class Region
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; } = "";
        public int CountryId { get; set; }
        public string CountryName { get; set; } = "";
        public string RegionFullName { get => $"{CountryName}{(string.IsNullOrEmpty(CountryName) ? "" : " : ")}{RegionName}"; }
    }
}