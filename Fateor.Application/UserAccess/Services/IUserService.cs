using Fateor.Application.UserAccess.Entities;
using Fateor.Application.UserAccess.Repositories.Interfaces;

namespace Fateor.Application.UserAccess.Services
{
    public interface IUserService
    {
        IEnumerable<Entities.User> GetAllUsers();
        Entities.User? GetUser(int userId);
        Entities.User? GetUser(string login, string email);
        Entities.User? GetUserByEmail(string email);
        Entities.User? Login(string email, string password, out List<string> messages);
        (bool, List<string>) CreateUser(Entities.User user, int currentUserId);
        (bool, List<string>) UnlockUser(IUserRepository userRepository, Dto.User usr);
        (bool, List<string>) UpdatePassword(int userId, string password);
        (bool, List<string>) UpdatePassword(string email, string password);
        (bool, List<string>) UpdateUser(Entities.User user, int currentUserId);
        string GetPasswordResetToken(int value);
        PasswordResetToken DecryptPasswordResetToken(string token);
    }
}