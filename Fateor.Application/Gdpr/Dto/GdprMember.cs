using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Gdpr.Dto
{
    [Table("app_gdpr_member")]
    public class GdprMember
    {
        [ExplicitKey]
        public int GdprId { get; set; }
        [ExplicitKey]
        public int MemberId { get; set; }
    }
}
