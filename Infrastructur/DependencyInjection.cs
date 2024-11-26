using Infrastructur.Database;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Infrastructur
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            

            services.AddSingleton<FakeDatabase>();

            return services;
        }
    }
}
