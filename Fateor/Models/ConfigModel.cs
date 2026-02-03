using Fateor.Application.Config.Entities;
using System;

namespace Fateor.Models
{
    public class ConfigModel : Config, IEquatable<ConfigModel>
    {
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }


        public bool Equals(ConfigModel? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.ConfigCode == other.ConfigCode &&
                   this.ConfigValue == other.ConfigValue &&
                   this.Description == other.Description &&
                   this.ConfigTypeId == other.ConfigTypeId;
        }

        public string[] Options { get; set; } = Array.Empty<string>();
    }
}
