using Fateor.Application.Location.Dto;
using oSoft.Data;

namespace Fateor.Application.Location.Repositories.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Country Get(int countryId);
        Country GetByIsoCode(string countryCode);
    }
}