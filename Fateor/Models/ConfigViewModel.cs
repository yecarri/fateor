namespace Fateor.Models
{
    public class ConfigViewModel: ViewModelBase
    {
        public ConfigModel[] Initial { get; set; } = [];
        public ConfigModel[] Current { get; set; } = [];        
    }
}
