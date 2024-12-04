using ApplicationBook.Users.Queries.LogIn.Helpers;
using Microsoft.Extensions.DependencyInjection;


namespace ApplicationBook
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
            services.AddScoped<Tokenhelper>();


            return services;
        }
    }
}
