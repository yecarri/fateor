using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Notification.Dto
{
    [Table("app_EmailTemplate")]
    public class EmailTemplate
    {
        [Key]
        public int? EmailTemplateId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

    }
}
