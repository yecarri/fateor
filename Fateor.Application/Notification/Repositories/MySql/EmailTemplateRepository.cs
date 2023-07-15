using Fateor.Application.Notification.Dto;
using Fateor.Application.Notification.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Notification.Repositories.MySql
{
    public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(IDbConnection connection) : base(connection)
        {
        }

        public EmailTemplateRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public EmailTemplateRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
