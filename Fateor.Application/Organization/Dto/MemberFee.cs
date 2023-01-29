using MigraDocCore.DocumentObjectModel.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Dto
{
    public class MemberFee
    {
        public int? MemberFeeId { get; set; }
        public int? MemberId { get; set; }
        public int? MemberFeeBatchId { get; set; }
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public decimal Amout { get; set; }
        public decimal AmoutPaid { get; set; }
        public DateTime? DatePaid { get; set; }
    }
}
