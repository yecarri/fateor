using System;
using System.Collections.Generic;
using System.Text;

namespace Fateor.Application.Config.Entities
{
    public class ConfigOption
    {
        public string ConfigCode { get; set; } = null!;
        public int Number { get; set; }
        public string Value { get; set; } = null!;
    }
}
