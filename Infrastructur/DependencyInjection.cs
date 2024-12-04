using Infrastructur.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructur
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionstring)
        {


            services.AddSingleton<FakeDatabase>();

            services.AddDbContext<Realdatabase>(options =>
            {
                options.UseSqlServer(connectionstring);
            });

            return services;
        }
    }
}
