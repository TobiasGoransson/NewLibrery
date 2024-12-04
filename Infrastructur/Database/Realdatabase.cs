using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructur.Database
{
    public class Realdatabase : DbContext
    {
        public Realdatabase(DbContextOptions<Realdatabase> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
