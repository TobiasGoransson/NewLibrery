using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using Infrastructur.Database;
using Infrastructur.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructur
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionstring)
        {


            services.AddDbContext<Realdatabase>(options =>
            {
                options.UseSqlServer(connectionstring);
            });
            services.AddScoped<IRepository<Author>, Repository<Author>>();
            services.AddScoped<IRepository<Book>, Repository<Book>>();

            return services;
        }
    }
}
