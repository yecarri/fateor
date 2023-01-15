using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Bulletin.Repositories.Abstract
{
    public interface IBulletinCountryFilterRepository : IRepository<Dto.BulletinCountryFilter>
    {
        IEnumerable<Dto.BulletinCountryFilter> GetByBulletinId(int bulletinId);
    }
}
