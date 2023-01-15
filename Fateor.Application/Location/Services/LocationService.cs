using Fateor.Application.Location.Entities;
using Fateor.Application.Location.Repositories.Interfaces;
using Fateor.Infrastructure.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Location.Services
{
    public sealed class LocationService : ILocationService
    {
        ICountryRepository _countryRepository;
        IRegionRepository _regionRepository;
        IMapper _mapper;

        public LocationService(ICountryRepository countryRepository, IRegionRepository regionRepository, IMapper mapper)
        {
            _countryRepository=countryRepository;
            _regionRepository=regionRepository;
            _mapper=mapper;
        }

        public IEnumerable<Entities.Country> GetCountries()
        {
            return _countryRepository.GetAll().Select(c => _mapper.Map<Dto.Country, Entities.Country>(c)).ToList();
        }

        public Country GetCountryById(int countryId)
        {
            var c = _countryRepository.Get(countryId);
            return _mapper.Map<Dto.Country, Entities.Country>(c);
        }

        public Entities.Country GetCountryByIsoCode(string isoCode)
        {
            var c = _countryRepository.GetByIsoCode(isoCode);
            return _mapper.Map<Dto.Country, Entities.Country>(c);
        }

        public IEnumerable<Entities.Region> GetRegions()
        {
            return _regionRepository.GetAll().Select(r => _mapper.Map<Dto.Region, Entities.Region>(r)).ToList();
        }

        public IEnumerable<Entities.Region> GetRegionsByCountryId(int countryId)
        {
            var regions = _regionRepository.GetByCountryId(countryId);
            return regions.Select(r => _mapper.Map<Dto.Region, Entities.Region>(r)).ToList();
        }
    }
}
