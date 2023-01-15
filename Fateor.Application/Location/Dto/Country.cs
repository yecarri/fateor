using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Location.Dto
{
    [Table("loc_country")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        public string CountryName { get; set; } = "";

        public string CountryCode { get; set; } = "";

    }
}