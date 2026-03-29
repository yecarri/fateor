namespace Fateor.Application.Organization.Entities
{
    public class MemberFee
    {
        public int? MemberFeeId { get; set; }
        public int? MemberFeeBatchId { get; set; }
        public int? MemberId { get; set; }
        public string? FeeReference { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedByUserId { get; set; }
        public bool IsIssued { get; set; }
        public DateTime? DateIssued { get; set; }
        public int? IssuedByUserId { get; set; }
        public DateTime? DateDue { get; set; }
        public decimal Amout { get; set; }
        public decimal PaidAmount { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? PaymentReference { get; set; }
        public DateTime? DatePaid { get; set; }
        public int? PaymentRegisteredByUserId { get; set; }
        public bool IsPaid => (PaidAmount >= Amout);
        public string GivenName { get; set; } = "";
        public string Surname { get; set; } = "";
    }
}
