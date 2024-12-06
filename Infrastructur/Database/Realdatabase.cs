using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructur.Database
{
    public class Realdatabase : DbContext
    {


        public Realdatabase(DbContextOptions<Realdatabase> options) : base(options)
        {
        }

        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<User> User { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            if (!optionBuilder.IsConfigured)
            {
                optionBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=TobyServer;Trusted_Connection=true;TrustServerCertificate=True;");
            }
        }
    }
}
