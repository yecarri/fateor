using Fateor.Application.Organization.Entities;

namespace Fateor.Application.Organization.Service
{
    public interface IUnitSelector
    {
        int SelectUnit(Member member);
    }
}