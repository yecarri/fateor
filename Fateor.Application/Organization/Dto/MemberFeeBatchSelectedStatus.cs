using Dapper.Contrib.Extensions;

namespace Fateor.Application.Organization.Dto
{
    [Table("org_memberfeebatch_status")]
    public class MemberFeeBatchSelectedStatus
    {
        [ExplicitKey] 
        public int MemberFeeBatchId { get; set; }

        [ExplicitKey]
        public int StatusId { get; set; }
    }
}
