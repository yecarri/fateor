using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Notification.Services
{
    public interface IEmailTemplateService
    {
        IEnumerable<Entities.EmailTemplate> GetEmailTemplates();
    }
}
