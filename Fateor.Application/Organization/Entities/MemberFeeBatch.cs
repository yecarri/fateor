namespace Fateor.Application.Organization.Entities
{
    public class MemberFeeBatch
    {
        public string BatchTitle { get; set; } = "";
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ReferencePattern { get; set; } = "";
        public int Periodicity { get; set; } = 1;
        public decimal Amount { get; set; }
        public int[] SelectedStatuses { get; set; } = new int[0];
        public IList<MemberFee>? MemberFees { get; set; }
        public DateTime? DateDue { get; internal set; }
    }
}