using DocumentFormat.OpenXml.Drawing;
using oSoft.Mail.Entities;
using oSoft.Mail.Services;
using System.ComponentModel;
using System.Text;

namespace Fateor.Setup
{
    internal class VoidEmailService : IEmailService
    {
        private readonly ILogger<VoidEmailService> _logger;

        public VoidEmailService(ILogger<VoidEmailService> logger)
        {
            _logger=logger;
        }

        public (bool, IList<string>) SendMail(EmailAddress from, EmailAddress to, string subject, string htmlMessage)
        {
            string hFrom = Mask(from.Email);
            string hTo = Mask(to.Email);
            _logger.LogInformation("SendMail from {from} to {to} with subject {subject}", hFrom, hTo, subject);
            return (true, new List<string>());
        }

        public static string Mask(string email)
        {
            var len = email.Length;
            var l3 = len/3;
            char[] characters = email.ToCharArray();
            for(int i=l3; i < 2*l3; i++)
            {
                if (characters[i]=='@')
                    continue;
                characters[i]='*';
            }
            return new string(characters);
        }

        public (bool, IList<string>) SendMail(string from, string to, string subject, string htmlMessage)
        {
            _logger.LogInformation("SendMail from {from} to {to} with subject {subject}", from, to, subject);
            return (true, new List<string>());
        }
    }
}