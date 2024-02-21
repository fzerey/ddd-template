using Microsoft.Extensions.DependencyInjection;


namespace Fzerey.DDDStarter.Application{
    public static class ApplicationModule{
        public static IServiceCollection RegisterApplicationModule(this IServiceCollection services){
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApplicationModule).Assembly));
            services.AddScoped<IApplicationService, ApplicationService>();
            return services;
        }
    }
}