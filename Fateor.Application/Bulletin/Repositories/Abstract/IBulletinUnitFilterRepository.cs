using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Bulletin.Repositories.Abstract
{
    public interface IBulletinUnitFilterRepository : IRepository<Dto.BulletinUnitFilter>
    {
        IEnumerable<Dto.BulletinUnitFilter> GetByBulletinId(int bulletinId);
    }
}
