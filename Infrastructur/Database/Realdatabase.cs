using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

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

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            if (!optionBuilder.IsConfigured)
            {
                optionBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=TobyServer;Trusted_Connection=true;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.AId);
                entity.Property(a => a.FirstName).IsRequired();
                entity.Property(a => a.LastName).IsRequired();

                entity.HasMany(a => a.Books)
                      .WithOne(b => b.Author)
                      .HasForeignKey(b => b.AId);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.BId);
                entity.Property(b => b.Title).IsRequired();
                entity.Property(b => b.Description).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UId);
                entity.Property(u => u.UserName).IsRequired();
                entity.Property(u => u.Password).IsRequired();

                entity.HasMany(u => u.Books)
                      .WithMany(b => b.Users)
                      .UsingEntity(j => j.ToTable("BookUsers"));

                entity.HasMany(u => u.Authors)
                      .WithMany(a => a.Users)
                      .UsingEntity(j => j.ToTable("AuthorUsers"));
            });
        }

        public static void SeedDatabase(Realdatabase context)
        {
            if (!context.Authors.Any())
            {
                var authors = new List<Author>
                {
                     new Author
                     {
                         FirstName = "Jane",
                         LastName = "Austen",
                         Books = new List<Book>
                         {
                             new Book { Title = "Pride and Prejudice", Description = "Romance" },
                             new Book { Title = "Sense and Sensibility", Description = "Romance" }
                         }
                     },
                     new Author
                     {
                         FirstName = "George",
                         LastName = "Orwell",
                         Books = new List<Book>
                         {
                             new Book { Title = "1984", Description = "Dystopian" },
                             new Book { Title = "Animal Farm", Description = "Satire" }
                         }
                     }
                };

                context.Authors.AddRange(authors);
                context.SaveChanges(); // Spara efter författare och böcker
            }

            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        UserName = "User1",
                        Password = "Password1"
                    },
                    new User
                    {
                        UserName = "User2",
                        Password = "Password2"
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges(); // Spara efter användare
            }
        }


            
        


    }
}
