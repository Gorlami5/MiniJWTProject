using MiniJWTProject.Entities;

namespace MiniJWTProject.Data
{
    public class AppRepository : IAppRepository
    {
        private readonly DbConnection _connection; 
        public AppRepository(DbConnection connection)
        {
            _connection = connection;   
        }
        public void Add(User user)
        {          
            _connection.Add(user);
            _connection.SaveChanges();
        }

       
    }
}
