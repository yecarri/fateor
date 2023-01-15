using Fateor.Application.Location.Entities;

namespace Fateor.Application.Location.Services
{
    public interface ILocationService
    {
        Country GetCountryById(int id);
        Country GetCountryByIsoCode(string isoCode);
        IEnumerable<Country> GetCountries();
        IEnumerable<Region> GetRegions();
        IEnumerable<Region> GetRegionsByCountryId(int countryId);
    }
}