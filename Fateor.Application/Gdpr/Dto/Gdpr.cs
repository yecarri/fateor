using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Gdpr.Dto
{
    [Table("app_gdpr")]
    public class Gdpr
    {
        [Key] 
        public int GdprId { get; set; } 
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Export { get; set; }        
    }
}
