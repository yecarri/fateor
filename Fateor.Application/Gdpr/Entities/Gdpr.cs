using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Gdpr.Entities
{
    public class Gdpr
    {
        public int GdprId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Export { get; set; }
        public List<GdprMember>? Members { get; set; }
    }
}
