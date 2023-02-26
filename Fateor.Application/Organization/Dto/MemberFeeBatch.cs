namespace Fateor.Application.Organization.Dto
{
    public class MemberFeeBatch
    {
        public int MemberFeeBatchId { get; set; }
        public string BatchTitle { get; set; } = "";
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string RefferencePattern { get; set; } = "";
        public IList<int>? StatusIds { get; set; }
    }
}
