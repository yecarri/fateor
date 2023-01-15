using Fateor.Application.UserAccess.Dto;
using oSoft.Data;

namespace Fateor.Application.UserAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User CreateUser(string email, string hash);
        User? GetUser(int userId);
        User? GetUserByEmail(string email);
        User? GetUserByLogin(string login);
        IEnumerable<User> GetUsersByEmail(string email);
        void UpdatePassword(int id, string hash);
        void UpdatePassword(string login, string hash);
    }
}