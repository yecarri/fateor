using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Entities
{
    public class ActivityType
    {
        public int ActivityTypeId { get; set; }
        public string Description { get; set; } = "";
        public bool Internal { get; set; }
        public bool OnLine { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModificationDate { get; set; }
        public int LastModificationUserId { get; set; }
    }
}
