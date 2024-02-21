using Fzerey.DDDStarter.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fzerey.DDDStarter.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection RegisterInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .UseNpgsql(
                        $"Server={configuration.GetSection("DatabaseConfiguration:host").Value};Port={configuration.GetSection("DatabaseConfiguration:port").Value};Database={configuration.GetSection("DatabaseConfiguration:dbName").Value};User Id={configuration.GetSection("DatabaseConfiguration:username").Value};Password='{configuration.GetSection("DatabaseConfiguration:password").Value}'",
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                    )
            );
            return services;
        }
    }
}
