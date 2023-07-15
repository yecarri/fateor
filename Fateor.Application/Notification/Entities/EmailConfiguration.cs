using oSoft.Mail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Notification.Entities
{
    public class EmailConfiguration
    {
        public bool DisableAllEmails { get; set; } = true;
        public IEnumerable<EmailTemplate>? Templates { get; set; }
        public EmailAddress? DefaultSender { get; set; }
    }
}
