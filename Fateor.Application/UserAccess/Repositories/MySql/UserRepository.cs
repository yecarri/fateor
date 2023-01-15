using Dapper.Contrib.Extensions;
using Dapper;
using MySql.Data.MySqlClient;
using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fateor.Application.UserAccess.Dto;
using Fateor.Application.UserAccess.Repositories.Interfaces;

namespace Fateor.Application.UserAccess.Repositories.MySql
{
    internal class UserRepository : oSoft.Data.MySql.Repository<User>, IUserRepository
    {
        public UserRepository(IDbConnection connection) : base(connection)
        {
        }

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public UserRepository(string connectionString) : base(connectionString)
        {
        }

        public User? GetUserByLogin(string login)
        {
            if (Connection == null) throw new NullReferenceException(nameof(Connection));

            return Connection.Query<User>(
                "SELECT * FROM app_user where Login=@Login",
                new { Login = login.ToLowerInvariant() }, Transaction).FirstOrDefault();

        }

        public User? GetUserByEmail(string email)
        {
            if (Connection == null) throw new NullReferenceException(nameof(Connection));

            return Connection.Query<User>(
                "SELECT * FROM app_user where Email=@Email",
                new { Email = email.ToLowerInvariant() }, Transaction).FirstOrDefault();
        }

        public IEnumerable<User> GetUsersByEmail(string email)
        {
            if (Connection == null) throw new NullReferenceException(nameof(Connection));

            return Connection.Query<User>(
                "SELECT * FROM app_user where Email=@Email",
                new { Email = email.ToLowerInvariant() }, Transaction);
        }

        public User? GetUser(int userId)
        {
            if (Connection == null) throw new NullReferenceException(nameof(Connection));

            return Connection.Query<User>(
                "SELECT * FROM app_user where UserId=@UserId",
                new { UserId = userId }, Transaction).FirstOrDefault();
        }

        public User CreateUser(string email, string hash)
        {
            User user = new User
            {
                Hash = hash,
                Login = email.ToLowerInvariant()
            };

            int id = (int)Connection.Insert(user, Transaction);
            user.UserId = id;
            return user;
        }

        public override int Insert(User data)
        {
            try
            {
                return base.Insert(data);
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        throw new Exception("Ya existe un registro con el mismo Login", ex);
                    default:
                        throw new Exception("Unable to insert record", ex);
                }
            }
        }

        public void UpdatePassword(string login, string hash)
        {
            Connection.Execute("UPDATE app_user SET hash=@Hash WHERE login=@Login",
                new { Hash = hash, Login = login.ToLowerInvariant() },
                Transaction,
                null,
                CommandType.Text);
        }

        public void UpdatePassword(int id, string hash)
        {
            Connection.Execute("UPDATE app_user SET Hash=@Hash WHERE UserId=@Id",
                new { Hash = hash, Id = id },
                Transaction,
                null,
                CommandType.Text);
        }
    }
}
