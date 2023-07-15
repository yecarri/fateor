using AutoMapper;
using Fateor.Application.UserAccess.Entities;
using Fateor.Application.UserAccess.Repositories.Interfaces;
using Fateor.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Fateor.Application.UserAccess.Services.PasswordService;

namespace Fateor.Application.UserAccess.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IEncryptionService encryptionService, IMapper mapper)
        {
            _userRepository=userRepository;
            _mapper=mapper;
            _encryptionService=encryptionService;
        }

        public Entities.User? Login(string email, string password, out List<string> messages)
        {
            messages = new List<string>();
            var hash = ComputeHash(password);

            var usr = _userRepository.GetUserByLogin(email);
            if (usr == null)
            {
                messages.Add("Usuario/Password erróneo");
                return null;
            }
            else
            {
                if (usr.Hash == hash)
                {
                    return _mapper.Map<Dto.User, Entities.User>(usr);
                }
                else
                {
                    messages.Add("Usuario/Password erróneo");
                    return null;
                }
            }
        }

        public Entities.User? GetUser(int userId)
        {
            var u = _userRepository.GetUser(userId);
            var user = _mapper.Map<Dto.User, Entities.User>(u);
            return user;
        }

        public Entities.User? GetUser(string login, string email)
        {
            var u = _userRepository.GetAll()
                .Where(x => string.Compare(x.Login, login, true)==0 || string.Compare(x.Email, email, true)==0)
                .OrderBy(x => string.Compare(x.Login, login, true)==0 ? 0 : 1)
                .ThenBy(x => string.Compare(x.Email, email, true)==0 ? 0 : 1)
                .FirstOrDefault();

            if (u != null)
            {
                var user = _mapper.Map<Dto.User, Entities.User>(u);
                return user;
            }
            return null;
        }

        public Entities.User? GetUserByEmail(string email)
        {
            var u = _userRepository.GetUserByEmail(email);
            var user = _mapper.Map<Dto.User, Entities.User>(u);
            return user;
        }

        public IEnumerable<Entities.User> GetAllUsers()
        {
            var users = _userRepository.GetAll().Select(u => _mapper.Map<Dto.User, Entities.User>(u));
            return users;
        }

        public (bool, List<string>) CreateUser(Entities.User user, int currentUserId)
        {
            try
            {
                var usr = _mapper.Map<Entities.User, Dto.User>(user);
                usr.Hash = ComputeHash(user.Password);
                usr.LastModificationUserId = currentUserId;
                _userRepository.Insert(usr);
                user.UserId = usr.UserId;
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return (false, new List<string> { $"Error al crear usuario: {ex.Message}" });
            }
        }

        public (bool, List<string>) UpdateUser(Entities.User user, int currentUserId)
        {
            if (user.UserId.HasValue)
            {
                try
                {
                    var oldusr = _userRepository.GetUser(user.UserId.Value);
                    if (oldusr == null) return (false, new List<string>() { "El usuario no existe" });

                    var usr = _mapper.Map<Entities.User, Dto.User>(user);

                    usr.Hash = oldusr.Hash;
                    usr.LastModificationUserId=currentUserId;

                    _userRepository.Update(usr);

                    return (true, new List<string>());
                }
                catch (Exception ex)
                {
                    return (false, new List<string> { "No se pudo actualizar el registro", ex.Message });
                }
            }
            return (false, new List<string> { "El Id del usuario es null" });
        }

        public (bool, List<string>) UpdatePassword(string email, string password)
        {
            var hash = ComputeHash(password);
            try
            {
                _userRepository.UpdatePassword(email, hash);
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return (false, new List<string> { $"Error al actualizar el password: {ex.Message}" });
            }
        }

        public (bool, List<string>) UpdatePassword(int userId, string password)
        {
            var hash = ComputeHash(password);
            try
            {
                _userRepository.UpdatePassword(userId, hash);
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return (false, new List<string> { $"Error al actualizar el password: {ex.Message}" });
            }

        }

        public (bool, List<string>) UnlockUser(IUserRepository userRepository, Dto.User usr)
        {
            return (false, new List<string>());
        }

        public string GetPasswordResetToken(int userId)
        {
            var token = new PasswordResetToken(userId, DateTime.Now.AddMinutes(20));
            return _encryptionService.EncryptHex(token);
        }

        public PasswordResetToken DecryptPasswordResetToken(string token)
        {
            return _encryptionService.DecryptHex<PasswordResetToken>(token);
        }
    }
}
