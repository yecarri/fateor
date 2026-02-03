

namespace Fateor.Application.Config.Services
{
    public interface IConfigService
    {
        IEnumerable<Entities.Config> GetCongfigs();
    }
}