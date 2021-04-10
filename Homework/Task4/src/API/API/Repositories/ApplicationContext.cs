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
        }

        public DbSet<Info> Info { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(DatabaseSettings.ConnectionString);
        }
    }
}
