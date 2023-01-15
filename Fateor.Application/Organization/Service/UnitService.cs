using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Application.Organization.Repositories.MySql;
using Fateor.Infrastructure.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Service
{
    public sealed class UnitService : IUnitService
    {
        public readonly IUnitRepository _unitRepository;
        public readonly IMapper _mapper;

        public UnitService(IUnitRepository unitRepository, IMapper mapper)
        {
            _unitRepository=unitRepository;
            _mapper=mapper;
        }

        public IEnumerable<Unit> GetUnits()
        {
            var results = _unitRepository.GetAll();
            return results.Select(x => _mapper.Map<Organization.Dto.Unit, Unit>(x)).ToList();
        }

        public Unit GetUnitById(int id)
        {
            var results = _unitRepository.Get(id);
            return _mapper.Map<Organization.Dto.Unit, Unit>(results);
        }

        public (bool ok, List<string> messages) UpdateUnit(Unit unit, int currentUserId)
        {
            try
            {
                var u = _mapper.Map<Unit, Organization.Dto.Unit>(unit);
                u.LastModificationUserId = currentUserId;
                _unitRepository.Update(u);
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return (false, new List<string>() { "No se puedo actualizar el registro", ex.Message });
            }
        }

        public (bool ok, List<string> messages) CreateUnit(Unit unit, int currentUserId)
        {
            try
            {
                var u = _mapper.Map<Unit, Organization.Dto.Unit>(unit);
                u.LastModificationUserId = currentUserId;
                _unitRepository.Insert(u);
                unit.UnitId = u.UnitId;
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return (false, new List<string>() { "No se puedo crear el registro", ex.Message });
            }
        }
    }
}
