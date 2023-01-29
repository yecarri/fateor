namespace Fateor.Models
{
    public class MemberFeeBatchViewModel: ViewModelBase
    {
        public string BatchTitle { get; set; } = "";
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public StatusSelectViewModel[]? Statuses { get; set; }
    }
}
