using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructur.Database
{
    public class Realdatabase(DbContextOptions<Realdatabase> options) : DbContext(options)
    {
       

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
       


        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=TobyServer;Trusted_Connection=true;TrustServerCertificate=True;");
        }
    }
}
