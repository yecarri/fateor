namespace Fateor.Application.Bulletin.Services
{
    public interface IBulletinService
    {
        Entities.Bulletin GetBulletin(int bulletinId);
        IEnumerable<Entities.Bulletin> GetBulletins();
        Task<IEnumerable<Entities.Bulletin>> GetBulletinsAsync();
        void Save(Entities.Bulletin bulletin, int currentUserId);
    }
}