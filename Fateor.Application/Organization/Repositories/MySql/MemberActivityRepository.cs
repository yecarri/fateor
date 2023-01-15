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
    public class MemberActivityRepository : Repository<MemberActivity>, IMemberActivityRepository
    {
        const string SELECT = @"SELECT ma.*, m.GivenName, m.Surname
            FROM org_Member_Activity ma 
            JOIN org_Member m on ma.MemberId=m.MemberId
            JOIN org_Activity a on ma.ActivityId=a.ActivityId ";


        public MemberActivityRepository(IDbConnection connection) : base(connection)
        {
        }

        public MemberActivityRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public MemberActivityRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<MemberActivity> GetByActivityId(int activityId)
        {
            var sql = SELECT + "WHERE a.ActivityId=?";
            var param = new { activityId };
            return Connection.Query<MemberActivity>(sql, param, Transaction);
        }

        public IEnumerable<MemberActivity> GetByDates(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var sql = SELECT + "WHERE a.ActivityDate>=NVL(?,a.ActivityDate) AND a.ActivityDate<=NVL(?,a.ActivityDate)";
            var param = new { dateFrom, dateTo };
            return Connection.Query<MemberActivity>(sql, param, Transaction);
        }

        public IEnumerable<MemberActivity> GetByMemberId(int memberId, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var sql = SELECT + "WHERE m.MemberId=? AND a.ActivityDate>=NVL(?,a.ActivityDate) AND a.ActivityDate<=NVL(?,a.ActivityDate)";
            var param = new { memberId, dateFrom, dateTo };
            return Connection.Query<MemberActivity>(sql, param, Transaction);
        }

        public void Update(IEnumerable<MemberActivity> participants)
        {
            var activityId = participants.FirstOrDefault().ActivityId;

            var existing = GetByActivityId(activityId);

            var toInsert = participants.Where(p => !existing.Any(x => x.MemberId == p.MemberId));

            var toDelete = existing.Where(p => !participants.Any(x => x.MemberId == p.MemberId));

            var toUpdate = participants.Where(p => existing.Any(x => x.MemberId == p.MemberId));

            foreach (var p in toInsert)
            {
                Insert(p);
            }

            foreach (var p in toDelete)
            {
                Delete(p);
            }

            foreach (var p in toUpdate)
            {
                Update(p);
            }
        }
    }
}
