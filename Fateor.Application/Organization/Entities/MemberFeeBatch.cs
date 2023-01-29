namespace Fateor.Application.Organization.Entities
{
    public class MemberFeeBatch
    {
        public string BatchTitle { get; set; } = "";
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public IList<int>? StatusIds { get; set; }
    }
}