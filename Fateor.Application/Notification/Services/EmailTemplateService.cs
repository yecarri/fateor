using Fateor.Application.Notification.Entities;
using Fateor.Application.Notification.Repositories.Interfaces;
using Fateor.Infrastructure.Mappers;
using oSoft.Mail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Notification.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        protected readonly IEmailTemplateRepository _EmailTemplateRepository;
        protected readonly IEmailRecipientRepository _EmailRecipientRepository;
        protected readonly IMapper _Mapper;

        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, IEmailRecipientRepository emailRecipientRepository, IMapper mapper)
        {
            _EmailTemplateRepository=emailTemplateRepository??throw new ArgumentNullException(nameof(emailTemplateRepository));
            _EmailRecipientRepository=emailRecipientRepository??throw new ArgumentNullException(nameof(emailRecipientRepository));
            _Mapper=mapper??throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<EmailTemplate> GetEmailTemplates()
        {
            var templates = _EmailTemplateRepository.GetAll();
            var recipients = _EmailRecipientRepository.GetAll();
            var result = templates.Select(t =>
            {
                var template = _Mapper.Map<Dto.EmailTemplate, EmailTemplate>(t);
                template.Recipients = recipients
                    .Where(r => r.EmailTemplateId == t.EmailTemplateId)
                    .Select(r => _Mapper.Map<Dto.EmailRecipient, EmailAddress>(r));
                return template;
            });
            return result;
        }
    }
}
