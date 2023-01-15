using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Location.Entities
{
    public class Country
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; } = "";

        public string CountryCode { get; set; } = "";
    }
}