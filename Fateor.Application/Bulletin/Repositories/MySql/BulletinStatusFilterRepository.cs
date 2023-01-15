using Dapper;
using Fateor.Application.Bulletin.Dto;
using Fateor.Application.Bulletin.Repositories.Abstract;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Bulletin.Repositories.MySql
{
    public class BulletinStatusFilterRepository : Repository<BulletinStatusFilter>, IBulletinStatusFilterRepository
    {
        public BulletinStatusFilterRepository(IDbConnection connection) : base(connection)
        {
        }

        public BulletinStatusFilterRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public BulletinStatusFilterRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<BulletinStatusFilter> GetByBulletinId(int bulletinId)
        {
            return Connection.Query<BulletinStatusFilter>("SELECT * FROM bul_bulletinstatusfilter WHERE BulletinId=?", new { bulletinId }, Transaction);
        }
    }
}
