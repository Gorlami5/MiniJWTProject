using Microsoft.EntityFrameworkCore;
using MiniJWTProject.Entities;

namespace MiniJWTProject.Data
{
    public class DbConnection:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = (localdb)\MSSQLLocalDB;Database = CitiesGuide;Trusted_Connection = true");
        }

        public DbSet<User> Users { get; set; }
    }
}
