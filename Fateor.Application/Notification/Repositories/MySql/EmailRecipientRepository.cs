using Dapper;
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
    public class EmailRecipientRepository : Repository<EmailRecipient>, IEmailRecipientRepository
    {
        public EmailRecipientRepository(IDbConnection connection) : base(connection)
        {
        }

        public EmailRecipientRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public EmailRecipientRepository(string connectionString) : base(connectionString)
        {
        }

        public override IEnumerable<EmailRecipient> GetAll()
        {
            string sql = @"SELECT 
                r.EmailAddressId, r.EmailTemplateId, Address, Name 
                FROM app_emailrecipient r 
                JOIN app_emailaddress a ON r.EmailAddressId=a.EmailAddressId";
            return Connection.Query<EmailRecipient>(sql, transaction: Transaction, commandType: CommandType.Text);                
        }
    }
}
