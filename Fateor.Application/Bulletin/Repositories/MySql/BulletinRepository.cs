using Dapper.Contrib.Extensions;
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
    public class BulletinRepository : Repository<Dto.Bulletin>, IBulletinRepository
    {
        public BulletinRepository(IDbConnection connection) : base(connection)
        {
        }

        public BulletinRepository(oSoft.Data.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public BulletinRepository(string connectionString) : base(connectionString)
        {
        }

        public override int Insert(Dto.Bulletin data)
        {
            var ret = Connection.Insert(data);
            return (int)ret;
        }

        public override Dto.Bulletin Get(dynamic id)
        {
            var ret = Connection.Get<Dto.Bulletin>((int)id);
            return ret;
        }
    }
}
