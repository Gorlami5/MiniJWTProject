using Microsoft.EntityFrameworkCore.Query.Internal;
using MiniJWTProject.Data;
using MiniJWTProject.Entities;
using MiniJWTProject.Helpers;

namespace MiniJWTProject.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IAppRepository _appRepository;
        private readonly DbConnection _dbConnection;
        
        public AuthManager(IAppRepository appRepository,DbConnection dbConnection)
        {
            _appRepository = appRepository;
            _dbConnection = dbConnection;
           
        }

        public User Login(string email, string password)
        {
            var user = _dbConnection.Users.FirstOrDefault(u => u.Email == email);

            if (user == null) {
                return null;
            }
                

            if (!HashHelper.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public User Register(User user,string password)
        {
            byte[] passwordhash, passwordsalt;
            HashHelper.CreatePasswordHash(password, out passwordhash, out passwordsalt);
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordsalt;
            _appRepository.Add(user);
            
            return user;

        }

        public bool UserExist(string email)
        {
             
            if(_dbConnection.Users.Any(u => u.Email == email) == true)
            {
                return true;
            }

            return false;
        }
    }
}
