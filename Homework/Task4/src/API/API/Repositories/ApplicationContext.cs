using API.Settings;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ApplicationContext : DbContext
    {
        private DatabaseSettings DatabaseSettings { get; set; }

        public ApplicationContext(DatabaseSettings databaseSettings)
        {
            DatabaseSettings = databaseSettings;
            Database.EnsureCreated();

        }

        public DbSet<Info> Info { get; set; }
     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(DatabaseSettings.ConnectionString);
        }
    }
}
