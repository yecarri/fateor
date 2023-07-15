using oSoft.Mail.Entities;
using oSoft.Mail.Services;

namespace Fateor.Setup
{
    internal class VoidEmailService : IEmailService
    {
        private readonly ILogger<VoidEmailService> _Logger;

        public VoidEmailService(ILogger<VoidEmailService> logger)
        {
            _Logger=logger;
        }

        public (bool, IList<string>) SendMail(EmailAddress from, EmailAddress to, string subject, string htmlMessage)
        {
            _Logger.LogInformation("SendMail from {from} to {to} with subject {subject}", from.FormattedEmailAddress, to.FormattedEmailAddress, subject);
            return (true, new List<string>());
        }

        public (bool, IList<string>) SendMail(string from, string to, string subject, string htmlMessage)
        {
            _Logger.LogInformation("SendMail from {from} to {to} with subject {subject}", from, to, subject);
            return (true, new List<string>());
        }
    }
}