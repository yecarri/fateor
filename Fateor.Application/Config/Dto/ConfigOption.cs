using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fateor.Application.Config.Dto
{
    [Table("app_configoption")]
    public class ConfigOption
    {
        public string ConfigCode { get; set; } = null!;
        public int Number { get; set; }
        public string Value { get; set; } = null!;
    }
}
