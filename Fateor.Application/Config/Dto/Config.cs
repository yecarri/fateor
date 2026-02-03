using Dapper.Contrib.Extensions;

namespace Fateor.Application.Config.Dto
{

    [Table("app_Config")]
    public sealed class Config
    {
        [ExplicitKey]
        public string ConfigCode { get; set; } = null!;

        public string? ConfigValue { get; set; }

        public string? Description { get; set; }

        public int ConfigTypeId { get; set; }
    }
}
