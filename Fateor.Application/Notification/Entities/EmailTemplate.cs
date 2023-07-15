using oSoft.Mail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Notification.Entities
{
    public class EmailTemplate
    {
        public EmailTemplate() { }

        public EmailTemplate(EmailAddress? sender, IEnumerable<EmailAddress>? recipients, string templateName, string subject)
        {
            Sender=sender;
            Recipients=recipients;
            TemplateName=templateName;
            Subject=subject;
        }

        public EmailAddress? Sender { get; set; }
        public IEnumerable<EmailAddress>? Recipients { get; set; }
        public string TemplateName { get; set; } = "";
        public string Content { get; set; } = string.Empty;
        public string Subject { get; set; } = "";
    }
}
