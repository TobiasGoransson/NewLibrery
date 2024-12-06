using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructur.Database
{
    public class RealdatabaseFactory : IDesignTimeDbContextFactory<Realdatabase>
    {
        public Realdatabase CreateDbContext(string[] args)
        {
            // Bygg konfigurationen från appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // Hämtar nuvarande arbetskatalog
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


            // Hämta anslutningssträngen
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Bygg DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<Realdatabase>();
            optionsBuilder.UseSqlServer(connectionString);

            return new Realdatabase(optionsBuilder.Options);
        }
    }
}

