using Microsoft.Extensions.DependencyInjection;
using Fzerey.DDDStarter.Infrastructure;
using Microsoft.Extensions.Configuration;


namespace Fzerey.DDDStarter.Application{
    public static class ApplicationModule{
        public static IServiceCollection RegisterApplicationModule(this IServiceCollection services){
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApplicationModule).Assembly));
            services.AddScoped<IApplicationService, ApplicationService>();
            return services;
        }
    }
}