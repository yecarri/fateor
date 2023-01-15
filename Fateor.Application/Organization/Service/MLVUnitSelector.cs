using Fateor.Application.Location.Repositories.Interfaces;
using Fateor.Application.Location.Services;
using Fateor.Application.Organization.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Service
{

    public sealed class MLVUnitSelector : IUnitSelector
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitRepository _unitRepository;

        public MLVUnitSelector(ICountryRepository countryRepository,
                               IUnitRepository unitRepository)
        {
            _countryRepository=countryRepository;            
            _unitRepository=unitRepository;
        }

        public int SelectUnit(Entities.Member member)
        {
            var venezuela = _countryRepository.GetByIsoCode("VE");
            var units = _unitRepository.GetAll().ToList();
            var nacional = units.FirstOrDefault(u => u.UnitName == "Nacional");
            var internacional = units.FirstOrDefault(u => u.UnitName == "Internacional");

            if (venezuela != null)
            {
                if (nacional != null)
                {
                    if (internacional != null)
                    {
                        if (member.CountryId == venezuela.CountryId)
                        {
                            return nacional.UnitId;
                        }
                        else
                        {
                            return internacional.UnitId;
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Value not found: Internacional (Unit)");
                    }
                }
                else
                {
                    throw new ApplicationException("Value not found: Nacional (Unit)");
                }
            }
            else
            {
                throw new ApplicationException("Value not found: Venezuela (Country)");
            }
        }

    }
}
