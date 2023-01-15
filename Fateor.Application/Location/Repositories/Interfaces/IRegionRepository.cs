using Fateor.Application.Location.Dto;
using oSoft.Data;
using System.Collections.Generic;

namespace Fateor.Application.Location.Repositories.Interfaces
{
    public interface IRegionRepository : IRepository<Region>
    {
        Region Get(int id);
        IEnumerable<Region> GetByCountryId(int id);
    }
}