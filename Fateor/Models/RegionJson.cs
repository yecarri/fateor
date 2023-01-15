using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Fateor.Models
{
    //TODO: Set class name
    public class RegionJson
    {
        public string RegionName { get; set; } = string.Empty;

        public int RegionId { get; set; }
    }
}