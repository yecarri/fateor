using Dapper;
using Dapper.Contrib.Extensions;
using Fateor.Application.Organization.Dto;
using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Repositories.Interfaces;
using Google.Protobuf.WellKnownTypes;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class MemberFeeBatchStatusRepository : Repository<MemberFeeBatchSelectedStatus>, IMemberFeeBatchStatusRepository
    {
        public MemberFeeBatchStatusRepository(IDbConnection connection) : base(connection)
        {
        }

        public MemberFeeBatchStatusRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public MemberFeeBatchStatusRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<MemberFeeBatchSelectedStatus> GetByFeeBatchId(int memberFeeBatchId)
        {
            string sql = @"SELECT * FROM org_memberfeebatch_status WHERE MemberFeeBatchId=@MemberFeeBatchId";
            object param = new { MemberFeeBatchId = memberFeeBatchId };
            return Connection.Query<MemberFeeBatchSelectedStatus>(sql, param, Transaction);
        }
        public void Update(int memberFeeBatchId, int[] selectedStatuses)
        {
            var current = GetByFeeBatchId(memberFeeBatchId).ToArray();
            var toInsert = selectedStatuses.Where(id => !current.Any(s1 => s1.StatusId==id));
            foreach (var s in toInsert)
            {
                Connection.Insert(new MemberFeeBatchSelectedStatus { MemberFeeBatchId=memberFeeBatchId, StatusId=s });
            }
            var toDelete = current.Where(s => !selectedStatuses.Contains(s.StatusId));
            foreach (var s in toDelete)
            {
                Connection.Delete(new MemberFeeBatchSelectedStatus { MemberFeeBatchId=memberFeeBatchId, StatusId=s.StatusId });
            }
        }
      
    }
}
