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
    public class MemberFeeRepository : Repository<MemberFee>, IMemberFeeRepository
    {
        public MemberFeeRepository(IDbConnection connection) : base(connection)
        {
        }

        public MemberFeeRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public MemberFeeRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<MemberFee> GetByBatchId(int batchId)
        {
            var parameters = new { MemberFeeBatchId = batchId };
                return Connection.Query<MemberFee>(@"SELECT mf.*
                FROM org_memberfee mf 
                WHERE mf.MemberFeeBatchId=@MemberFeeBatchId", parameters, Transaction);
        }
    }
}
