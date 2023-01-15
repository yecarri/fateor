using Dapper;
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
    public class ActivityRepository : Repository<Dto.Activity>, IActivityRepository
    {
        public ActivityRepository(IDbConnection connection) : base(connection)
        {
        }

        public ActivityRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ActivityRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Dto.Activity> GetActivities(DateTime? dateFrom, DateTime? dateTo)
        {
            var sql = "SELECT * FROM org_activity a WHERE a.ActivityDate>=NVL(?,a.ActivityDate) AND a.ActivityDate<=NVL(?,a.ActivityDate)";
            var param = new { dateFrom, dateTo };
            return Connection.Query<Dto.Activity>(sql, param, Transaction);
        }

        public IEnumerable<Dto.Activity> GetActivitiesByMemberId(int memberId, DateTime? dateFrom, DateTime? dateTo)
        {
            var sql = "SELECT a.* FROM org_activity a WHERE " +
                "EXISTS(SELECT * FROM org_member_activity ma WHERE ma.ActivityId=a.ActivityId AND ma.MemberId=?) AND " +
                "a.ActivityDate>=NVL(?,a.ActivityDate) AND a.ActivityDate<=NVL(?,a.ActivityDate) ";

            var param = new { memberId, dateFrom, dateTo };
            return Connection.Query<Dto.Activity>(sql, param, Transaction);
        }
    }
}
