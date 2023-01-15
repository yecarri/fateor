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
    public class BulletinCountryFilterRepository : Repository<BulletinCountryFilter>, IBulletinCountryFilterRepository
    {
        public BulletinCountryFilterRepository(IDbConnection connection) : base(connection)
        {
        }

        public BulletinCountryFilterRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public BulletinCountryFilterRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<BulletinCountryFilter> GetByBulletinId(int bulletinId)
        {
            return Connection.Query<BulletinCountryFilter>("SELECT * FROM bul_bulletincountryfilter WHERE BulletinId=?", new { bulletinId }, Transaction);
        }


    }
}
