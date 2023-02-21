namespace Fateor.Application.Organization.Entities
{
    public class MemberFee
    {
        public int? MemberFeeId { get; set; }
        public int? MemberFeeBatchId { get; set; }
        public int? MemberId { get; set; }
        public string? FeeReference { get; set; }
        public decimal Amout { get; set; }
        public decimal AmoutPaid { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? PaymentReference { get; set; }
        public DateTime? DatePaid { get; set; }
        public bool Paid => (AmoutPaid >= Amout);
        public string GivenName { get; set; } = "";
        public string Surname { get; set; } = "";
    }
}