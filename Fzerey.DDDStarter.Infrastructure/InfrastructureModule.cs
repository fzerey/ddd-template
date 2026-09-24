using Fzerey.DDDStarter.Application.Common.Interfaces;
using Fzerey.DDDStarter.Domain.Repositories;
using Fzerey.DDDStarter.Infrastructure.Context;
using Fzerey.DDDStarter.Infrastructure.Persistence.Queries;
using Fzerey.DDDStarter.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Fzerey.DDDStarter.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection RegisterInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            bool isDevelopment
        )
        {
            var database = configuration.GetSection("DatabaseConfiguration");
            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = database["host"],
                Port = int.TryParse(database["port"], out var port) ? port : 5432,
                Database = database["dbName"],
                Username = database["username"],
                Password = database["password"]
            }.ConnectionString;

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                );
                if (isDevelopment)
                {
                    options.EnableDetailedErrors().EnableSensitiveDataLogging();
                }
            });
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IOrderQueries, OrderQueries>();
            services.AddScoped<IItemQueries, ItemQueries>();
            return services;
        }
    }
}
