using Dapper;
using Dapper.Contrib.Extensions;
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
    public class BulletinStatusRepository : Repository<BulletinStatus>, IBulletinStatusRepository
    {
        public BulletinStatusRepository(IDbConnection connection) : base(connection)
        {
        }

        public BulletinStatusRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public BulletinStatusRepository(string connectionString) : base(connectionString)
        {
        }

        public override BulletinStatus Get(dynamic id)
        {
            return Connection.Get<BulletinStatus>((int)id, Transaction);
        }
    }
}
