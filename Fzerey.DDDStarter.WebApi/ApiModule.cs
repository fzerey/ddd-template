using Fzerey.DDDStarter.Application;
using Fzerey.DDDStarter.Infrastructure;

namespace Fzerey.DDDStarter.WebApi
{
    public static class ApiModule
    {
        public static IServiceCollection RegisterApiModule(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.RegisterInfrastructure(configuration, environment.IsDevelopment());
            services.RegisterApplicationModule();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
