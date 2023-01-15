using Fateor.Application.Organization.Entities;

namespace Fateor.Application.Organization.Service
{
    public interface IUnitService
    {
        Unit GetUnitById(int id);
        IEnumerable<Unit> GetUnits();
        (bool ok, List<string> messages) CreateUnit(Unit unit, int currentUserId);
        (bool ok, List<string> messages) UpdateUnit(Unit unit, int currentUserId);
    }
}