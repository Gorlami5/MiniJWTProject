using MiniJWTProject.Entities;

namespace MiniJWTProject.Repository
{
    public interface IAuthManager
    {
        public User Register(User user,string password);

        public User Login(string email,string password);

        public bool UserExist(string email);
    }
}
