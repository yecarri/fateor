using Dapper;
using Fateor.Application.Organization.Dto;
using Fateor.Application.Organization.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class MemberStatusRepository : Repository<MemberStatus>, IMemberStatusRepository
    {
        private const string SELECT =
        @"SELECT ms.MemberId, ms.StatusDate, ms.StatusId, s.StatusDescription, s.CanVote, s.CanBeElected, s.IsActiveMember,
        ms.UnitId, u.UnitName, ms.LastModificationUserId, ms.LastModificationDate, ms.LastModificationUserId,
        us.Login LastModificationUser
        FROM org_member_status ms 
        JOIN org_status s on ms.StatusId=s.StatusId
        JOIN org_unit u on ms.UnitId=u.UnitId 
        JOIN app_user us on ms.LastModificationUserId = us.UserId ";

        public MemberStatusRepository(IDbConnection connection) : base(connection)
        {
        }

        public MemberStatusRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public MemberStatusRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<MemberStatus> GetAllStatusesAt(DateTime date)
        {
            var parameters = new { Date = date };
            var results = Connection.Query<MemberStatus>(
                @" SELECT 
	                ms.MemberId, ms.StatusDate, ms.StatusId, StatusDescription, ms.UnitId, u.UnitName, 
	                ms.LastModificationUserId, ms.LastModificationDate, us.Login LastModificationUser
                from org_member m, lateral (
	                select s.* from org_member_status s
	                where s.StatusDate<= ?
	                and s.MemberId=m.MemberId
	                ORDER BY s.StatusDate desc 
	                limit 1) ms
                JOIN org_status s on ms.StatusId=s.StatusId
                JOIN org_unit u on ms.UnitId=u.UnitId 
                JOIN app_user us on ms.LastModificationUserId = us.UserId ", parameters, Transaction);
            return results;
        }

        public MemberStatus GetMemberStatusAtInstant(int memberId, DateTime date)
        {
            var parameters = new { memberId, date };
            var results = Connection.Query<MemberStatus>(SELECT +
                " WHERE ms.MemberId=? AND ms.StatusDate<=? ORDER BY ms.StatusDate desc LIMIT 1", parameters, Transaction);
            return results.FirstOrDefault();
        }

        public MemberStatus GetLastMemberStatus(int memberId)
        {
            var parameters = new { memberId };
            var results = Connection.Query<MemberStatus>(SELECT +
                " WHERE ms.MemberId=? ORDER BY ms.StatusDate desc LIMIT 1", parameters, Transaction);
            return results.FirstOrDefault();
        }

        public  MemberStatus Get(dynamic id)
        {
            var parameters = new { id.MemberId, id.StatusDate };
            var results = Connection.Query<MemberStatus>(SELECT +
                " WHERE ms.MemberId=? AND ms.StatusDate=?", parameters, Transaction);
            return results.FirstOrDefault();
        }

        public override IEnumerable<MemberStatus> GetAll()
        {
            var results = Connection.Query<MemberStatus>(SELECT, Transaction);
            return results;
        }

        public IEnumerable<MemberStatus> GetByMemberId(int id)
        {
            var parameters = new { MemberId = id };
            var results = Connection.Query<MemberStatus>(SELECT +
                " WHERE ms.MemberId=?", parameters, Transaction);
            return results;
        }
    }
}