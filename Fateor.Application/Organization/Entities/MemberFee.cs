namespace Fateor.Application.Organization.Entities
{
    public class MemberFee
    {
        public int? MemberFeeId { get; set; }
        public int? MemberId { get; set; }
        public int? MemberFeeBatchId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal Amout { get; set; }
        public decimal AmoutPaid { get; set; }
        public DateTime? DatePaid { get; set; }
    }
}