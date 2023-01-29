using Dapper;
using Fateor.Application.Organization.Dto;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class MemberFeeBatchRepository: Repository<MemberFeeBatch>
    {
        public MemberFeeBatchRepository(IDbConnection connection) : base(connection)
        {
        }

        public MemberFeeBatchRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public MemberFeeBatchRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<MemberFeeBatch> GetMemberFeeBatches(DateOnly dateFrom, DateOnly dateTo)
        {
            var parameters = new {DateFrom = dateFrom, DateTo = dateTo};

            var results = Connection.Query<MemberFeeBatch>("SELECT * FROM org_MemberFeeBatch", parameters, Transaction, commandType: CommandType.Text);

            return results;
        }
    }
}
