using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Bulletin.Repositories.Abstract
{
    public interface IBulletinUnitOfWork : IUnitOfWork
    {
        IBulletinRepository BulletinRepository { get; }
        IBulletinCountryFilterRepository BulletinCountryFilterRepository { get; }
        IBulletinStatusFilterRepository BulletinStatusFilterRepository { get; }
        IBulletinStatusRepository BulletinStatusRepository { get; }
        IBulletinUnitFilterRepository BulletinUnitFilterRepository { get; }
    }
}
