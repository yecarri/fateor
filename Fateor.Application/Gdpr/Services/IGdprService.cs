namespace Fateor.Application.Gdpr.Services
{
    public interface IGdprService
    {
        void Save(Entities.Gdpr gdpr);
        void Save(int userId, IEnumerable<Organization.Entities.Member> members, bool export = false);
    }
}