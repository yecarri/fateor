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
    public class BulletinUnitFilterRepository : Repository<BulletinUnitFilter>, IBulletinUnitFilterRepository
    {
        public BulletinUnitFilterRepository(IDbConnection connection) : base(connection)
        {
        }

        public BulletinUnitFilterRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public BulletinUnitFilterRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<BulletinUnitFilter> GetByBulletinId(int bulletinId)
        {
            return Connection.Query<BulletinUnitFilter>("SELECT * FROM bul_bulletinunitfilter WHERE BulletinId=?", new { bulletinId }, Transaction);
        }
    }
}
