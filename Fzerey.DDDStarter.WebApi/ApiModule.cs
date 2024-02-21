using Fzerey.DDDStarter.Application;
using Fzerey.DDDStarter.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;

namespace Fzerey.DDDStarter.WebApi
{
    public static class ApiModule
    {
        public static IServiceCollection RegisterApiModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterInfrastructure(configuration);
            services.RegisterApplicationModule();

         
            services.AddControllers();
            services.AddMemoryCache();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                  new OpenApiSecurityScheme
                  {
                    Reference = new OpenApiReference
                      {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                      },
                      Scheme = "oauth2",
                      Name = "Bearer",
                      In = ParameterLocation.Header,
                    },
                    new List<string>()
                  }
                });
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });

            return services;
        }
    }
}