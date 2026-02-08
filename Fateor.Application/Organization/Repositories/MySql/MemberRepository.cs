using Dapper;
using Fateor.Application.Organization.Dto;
using Fateor.Application.Organization.Repositories.Interfaces;
using MySql.Data.MySqlClient;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(IDbConnection connection) : base(connection)
        {
        }

        public MemberRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public MemberRepository(string connectionString) : base(connectionString)
        {
        }

        public override Member Get(int id)
        {
            return Connection.QueryFirst<Member>("SELECT * FROM org_member WHERE MemberId=?", new { id }, Transaction);
        }

        public override int Insert(Member data)
        {
            try
            {
                return base.Insert(data);
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        var regex = new Regex(@"'([^']+)'");
                        var matches = regex.Matches(ex.Message);
                        throw new Exception($"Ya existe un registro con el mismo {matches[1].Groups[1].Value}", ex);
                    default:
                        throw new Exception("Unable to insert record", ex);
                }
            }
        }

        public int LastMemberCode()
        {
            var regex = new Regex(@"(?:[a-zA-Z]|\d{4})-(\d{1,4})(?:-.{2,4)*");

            var all = GetAll().Where(m => m.MemberCode != null).Select(m =>
              {
                  var r = regex.Match(m.MemberCode).Groups[1].Value;
                  int x;
                  if (int.TryParse(r, out x))
                      return x;
                  else
                      return (int?)null;
              });

            var maxCode = all.Max() ?? 0;
            return maxCode;
        }

        public IEnumerable<Member> GetActiveMembers()
        {
            return Connection.Query<Member>(
                @"select m.*
                from org_member m,
                lateral (
	                select ms.StatusDate, s.CanBeElected, s.CanVote, s.IsDeleted from org_member_status ms 
	                join org_status s  on ms.StatusId=s.StatusId
	                where m.MemberId=ms.MemberId     
                    order by ms.StatusDate desc limit 1) x
                where x.CanVote=1 and x.IsDeleted=0", null, Transaction);
        }

        public IEnumerable<Member> GetElegibleMembers()
        {
            return Connection.Query<Member>(
                @"select m.*
                from org_member m,
                lateral (
	                select ms.StatusDate, s.CanBeElected, s.CanVote, s.IsDeleted from org_member_status ms 
	                join org_status s  on ms.StatusId=s.StatusId
	                where m.MemberId=ms.MemberId     
                    order by ms.StatusDate desc limit 1) x
                where x.CanBeElected=1 and x.IsDeleted=0", null, Transaction);
        }

        public IEnumerable<Member> GetByFeeBatchId(int batchId)
        {
            return Connection.Query<Member>(
                @"SELECT m.* 
                FROM org_Member m 
                JOIN org_MemberFee mf on mf.MemberId=m.MemberId 
                WHERE mf.MemberFeeBatchId=@MemberFeeBatchId", new { MemberFeeBatchId = batchId }, Transaction);
        }

        public IEnumerable<Member> GetByStatusIds(int[] statusIds)
        {
            return Connection.Query<Member>(
                @"select m.*
                from org_member m,
                lateral (
	                select ms.StatusId, ms.StatusDate, s.CanBeElected, s.CanVote, s.IsDeleted 
                    from org_member_status ms 
	                join org_status s  on ms.StatusId=s.StatusId
	                where m.MemberId=ms.MemberId     
                    order by ms.StatusDate desc limit 1) x
                where x.StatusId in @StatusIds and x.IsDeleted=0", new { StatusIds = statusIds }, Transaction);
        }

        public Member? GetMemberByEmail(string email)
        {
            if (Connection is null)
                throw new NullReferenceException(nameof(Connection));

            return Connection.QueryFirstOrDefault<Member>("SELECT * FROM org_member WHERE Email=? AND IsDeleted=0", new { email }, Transaction);
        }

        public void InsertMemberLoginToken(MemberLoginToken token)
        {
            var sql = @"INSERT INTO 
                        org_memberlogintoken (Guid, MemberId, Expiration)
                        VALUES               (@Guid, @MemberId, @Expiration)";
            Connection.Execute(sql, new { Guid = token.Guid.ToByteArray(), token.MemberId, token.Expiration }, Transaction);
        }

        public void UseMemberLoginToken(Guid guid)
        {
            var sql = @"UPDATE org_memberlogintoken
                        SET Used = 1
                        WHERE Guid = @Guid";
            Connection.Execute(sql, new { Guid = guid }, Transaction);
        }

        public MemberLoginToken? GetMemberLoginToken(Guid guid)
        {
            var sql = "SELECT * FROM org_memberlogintoken WHERE Guid=@Guid";
            return Connection.QueryFirstOrDefault<MemberLoginToken>(sql, new { Guid = guid.ToByteArray() }, Transaction);
        }
    }
}