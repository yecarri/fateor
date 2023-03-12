using oSoft.Mail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Infrastructure.Entities
{
    public class EmailTemplate
    {
        public EmailTemplate(EmailAddress? sender, IEnumerable<EmailAddress>? recipients, string templateName, string fileName, string subject)
        {
            Sender=sender;
            Recipients=recipients;
            TemplateName=templateName;
            FileName=fileName;
            Subject=subject;
        }

        public EmailAddress? Sender { get; set; }
        public IEnumerable<EmailAddress>? Recipients { get; set; }
        public string TemplateName { get; set; }
        public string FileName { get; set; }
        public string Subject { get; set; }

    }
}
