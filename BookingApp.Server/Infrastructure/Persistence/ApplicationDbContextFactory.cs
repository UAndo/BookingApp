using Microsoft.EntityFrameworkCore.Design;

namespace BookingApp.Server.Infrastructure.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(config.GetConnectionString("DefaultConnection"))
                .Options;

            return new ApplicationDbContext(optionsBuilder);
        }
    }
}
