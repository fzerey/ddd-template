using ArchUnitNET.Domain;
using ArchUnitNET.xUnitV3;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static Fzerey.DDDStarter.ArchitectureTests.ArchitectureFixture;

namespace Fzerey.DDDStarter.ArchitectureTests
{
    public class LayerDependencyTests
    {
        private static readonly IObjectProvider<IType> Domain =
            Types().That().ResideInAssembly(DomainAssembly).As("Domain");

        private static readonly IObjectProvider<IType> Application =
            Types().That().ResideInAssembly(ApplicationAssembly).As("Application");

        private static readonly IObjectProvider<IType> Infrastructure =
            Types().That().ResideInAssembly(InfrastructureAssembly).As("Infrastructure");

        private static readonly IObjectProvider<IType> WebApi =
            Types().That().ResideInAssembly(WebApiAssembly).As("WebApi");

        [Fact]
        public void Domain_does_not_depend_on_other_layers()
        {
            Types().That().Are(Domain)
                .Should().NotDependOnAny(Application)
                .AndShould().NotDependOnAny(Infrastructure)
                .AndShould().NotDependOnAny(WebApi)
                .Check(ArchitectureFixture.Architecture);
        }

        [Fact]
        public void Domain_does_not_depend_on_frameworks()
        {
            Types().That().Are(Domain)
                .Should().NotDependOnAny(Types().That().ResideInNamespaceMatching(@"^Microsoft\.EntityFrameworkCore(\..*)?$"))
                .AndShould().NotDependOnAny(Types().That().ResideInNamespaceMatching(@"^MediatR(\..*)?$"))
                .AndShould().NotDependOnAny(Types().That().ResideInNamespaceMatching(@"^Microsoft\.AspNetCore(\..*)?$"))
                .Check(ArchitectureFixture.Architecture);
        }

        [Fact]
        public void Application_does_not_depend_on_outer_layers()
        {
            Types().That().Are(Application)
                .Should().NotDependOnAny(Infrastructure)
                .AndShould().NotDependOnAny(WebApi)
                .Check(ArchitectureFixture.Architecture);
        }

        [Fact]
        public void Application_does_not_depend_on_ef_core()
        {
            Types().That().Are(Application)
                .Should().NotDependOnAny(Types().That().ResideInNamespaceMatching(@"^Microsoft\.EntityFrameworkCore(\..*)?$"))
                .Check(ArchitectureFixture.Architecture);
        }

        [Fact]
        public void Infrastructure_does_not_depend_on_web_api()
        {
            Types().That().Are(Infrastructure)
                .Should().NotDependOnAny(WebApi)
                .Check(ArchitectureFixture.Architecture);
        }

        [Fact]
        public void Controllers_go_through_application_layer()
        {
            Classes().That().ResideInNamespace("Fzerey.DDDStarter.WebApi.Controller")
                .Should().NotDependOnAny(Infrastructure)
                .AndShould().NotDependOnAny(Types().That().ResideInNamespace("Fzerey.DDDStarter.Domain.Repositories"))
                .Check(ArchitectureFixture.Architecture);
        }
    }
}
