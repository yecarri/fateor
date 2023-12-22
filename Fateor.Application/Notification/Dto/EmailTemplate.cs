using Dapper.Contrib.Extensions;

namespace Fateor.Application.Notification.Dto
{
    [Table("app_emailtemplate")]
    public class EmailTemplate
    {
        [Key]
        public int? EmailTemplateId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

    }
}
