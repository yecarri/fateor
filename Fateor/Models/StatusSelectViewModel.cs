namespace Fateor.Models
{
    public class StatusSelectViewModel
    {
        public int StatusId { get; set; }
        public string StatusDescription { get; set; } = "";
        public bool Selected { get; set; } = false;
    }
}
