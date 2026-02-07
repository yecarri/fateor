using System;
using System.Collections.Generic;
using System.Text;

namespace Fateor.Application.Config.Entities
{
    public class Config
    {
        public string ConfigCode { get; set; } = null!;

        public string? ConfigValue { get; set; }

        public string? Description { get; set; }

        public int ConfigTypeId { get; set; }

        public IList<string> Options { get; set; } = [];
    }
}
