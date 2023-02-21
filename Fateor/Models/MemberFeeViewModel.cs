namespace Fateor.Models
{
    public class MemberFeeViewModel
    {
        public int MemberFeeId { get; set; }
        public decimal MemberFeeAmount { get; set; }
        public int MemberId { get; set; }
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

    }
}
