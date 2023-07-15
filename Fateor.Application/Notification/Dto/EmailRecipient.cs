using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Notification.Dto
{
    public class EmailRecipient
    {
        public int EmailAddressId { get; set; }
        public int EmailTemplateId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
