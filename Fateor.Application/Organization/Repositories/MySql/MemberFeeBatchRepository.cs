using Dapper;
using Fateor.Application.Organization.Dto;
using Fateor.Application.Organization.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class MemberFeeBatchRepository : Repository<MemberFeeBatch>, IMemberFeeBatchRepository
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

        public MemberFeeBatch Get(int memberFeeBatchId)
        {
            var parameters = new { MemberFeeBatchId  = memberFeeBatchId };
            return Connection.QueryFirst<MemberFeeBatch>("SELECT * FROM org_memberfeebatch WHERE MemberFeeBatchId=@MemberFeeBatchId",
                                                         parameters,
                                                         Transaction);
        }

        public IEnumerable<MemberFeeBatch> GetMemberFeeBatches(DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new { DateFrom = dateFrom, DateTo = dateTo };

            var results = Connection.Query<MemberFeeBatch>("SELECT * FROM org_memberfeebatch WHERE DateFrom<=@DateFrom and DateTo>=@DateTo",
                                                           parameters,
                                                           Transaction,
                                                           commandType: CommandType.Text);

            return results;
        }
    }
}
