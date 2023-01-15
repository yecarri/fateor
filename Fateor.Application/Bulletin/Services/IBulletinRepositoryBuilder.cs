using Fateor.Application.Bulletin.Repositories.Abstract;
using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Bulletin.Services
{
    public interface IBulletinRepositoryBuilder
    {
        IUnitOfWork UnitOfWork { get; }

        (IBulletinRepository bulletinRepository,
            IBulletinStatusFilterRepository statusFilterRepository,
            IBulletinCountryFilterRepository countryFilterRepository,
            IBulletinUnitFilterRepository unitFilterRepository,
            IBulletinStatusRepository bulletinStatusRepository
        ) CreateRepositories();
    }
}
