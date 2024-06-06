using Dapper.Contrib.Extensions;

namespace Fateor.Application.Organization.Dto
{
    [Table("org_memberfeebatch")]
    public class MemberFeeBatch
    {
        [Key]
        public int MemberFeeBatchId { get; set; }
        public string BatchTitle { get; set; } = "";
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ReferencePattern { get; set; } = "";
    }
}
