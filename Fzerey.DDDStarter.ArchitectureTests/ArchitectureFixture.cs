using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using Fzerey.DDDStarter.Application;
using Fzerey.DDDStarter.Domain.Model;
using Fzerey.DDDStarter.Infrastructure;
using Fzerey.DDDStarter.WebApi;
using Assembly = System.Reflection.Assembly;

namespace Fzerey.DDDStarter.ArchitectureTests
{
    internal static class ArchitectureFixture
    {
        public static readonly Assembly DomainAssembly = typeof(Entity).Assembly;
        public static readonly Assembly ApplicationAssembly = typeof(ApplicationModule).Assembly;
        public static readonly Assembly InfrastructureAssembly = typeof(InfrastructureModule).Assembly;
        public static readonly Assembly WebApiAssembly = typeof(ApiModule).Assembly;

        public static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(DomainAssembly, ApplicationAssembly, InfrastructureAssembly, WebApiAssembly)
            .LoadAssemblies(
                typeof(Microsoft.EntityFrameworkCore.DbContext).Assembly,
                typeof(Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions).Assembly,
                typeof(Microsoft.EntityFrameworkCore.IndexAttribute).Assembly,
                typeof(MediatR.IMediator).Assembly,
                typeof(Microsoft.AspNetCore.Mvc.ControllerBase).Assembly,
                typeof(Microsoft.AspNetCore.Http.HttpContext).Assembly)
            .Build();
    }
}
