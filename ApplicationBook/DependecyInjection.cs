using ApplicationBook.Interfaces.RepoInterfaces;
using ApplicationBook.Users.Queries.LogIn.Helpers;
using Microsoft.Extensions.DependencyInjection;


namespace ApplicationBook
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddScoped<Tokenhelper>();
           



            return services;
        }
    }
}
