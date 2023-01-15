using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Dto
{
    [Table("org_activity")]
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }
        public int ActivityTypeId { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Description { get; set; } = "";
        public int DurationInMinutes { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModificationDate { get; set; }
        public int LastModificationUserId { get; set; }
    }
}
