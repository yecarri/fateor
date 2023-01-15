namespace Fateor.Models
{
    public class ViewModelBase
    {
        public string? Command { get; set; }
    
        public IList<string>? Messages { get; set; } = new List<string>();
    }
}
