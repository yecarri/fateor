using Dapper.Contrib.Extensions;

namespace Fateor.Application.Notification.Dto
{
    [Table("app_emailrecipient")]
    public class EmailRecipient
    {
        public int EmailAddressId { get; set; }
        public int EmailTemplateId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
