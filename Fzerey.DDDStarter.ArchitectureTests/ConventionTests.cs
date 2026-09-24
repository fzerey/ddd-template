using System.Reflection;
using Fzerey.DDDStarter.Domain.Model;
using MediatR;
using static Fzerey.DDDStarter.ArchitectureTests.ArchitectureFixture;

namespace Fzerey.DDDStarter.ArchitectureTests
{
    public class ConventionTests
    {
        private static IEnumerable<Type> EntityTypes() =>
            DomainAssembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(Entity).IsAssignableFrom(t));

        private static IEnumerable<Type> RequestHandlerTypes() =>
            ApplicationAssembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(IsRequestHandler));

        private static bool IsRequestHandler(Type type) =>
            type.IsGenericType
            && (type.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                || type.GetGenericTypeDefinition() == typeof(IRequestHandler<>));

        [Fact]
        public void Entities_have_no_public_setters()
        {
            var violations = EntityTypes()
                .SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                .Where(p => p.SetMethod?.IsPublic == true)
                .Select(p => $"{p.DeclaringType!.Name}.{p.Name}")
                .Distinct()
                .ToList();

            Assert.Empty(violations);
        }

        [Fact]
        public void Entities_have_a_non_public_parameterless_constructor_for_ef_core()
        {
            var violations = EntityTypes()
                .Where(t => t.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes) is null)
                .Select(t => t.Name)
                .ToList();

            Assert.Empty(violations);
        }

        [Fact]
        public void Request_handlers_are_named_after_their_request()
        {
            var violations = RequestHandlerTypes()
                .Select(t => (Handler: t, Request: t.GetInterfaces().First(IsRequestHandler).GenericTypeArguments[0]))
                .Where(x => x.Handler.Name != $"{x.Request.Name}Handler")
                .Select(x => $"{x.Handler.Name} handles {x.Request.Name}")
                .ToList();

            Assert.NotEmpty(RequestHandlerTypes());
            Assert.Empty(violations);
        }

        [Fact]
        public void Every_request_has_a_handler()
        {
            var handled = RequestHandlerTypes()
                .Select(t => t.GetInterfaces().First(IsRequestHandler).GenericTypeArguments[0])
                .ToHashSet();

            var unhandled = ApplicationAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IBaseRequest).IsAssignableFrom(t))
                .Where(t => !handled.Contains(t))
                .Select(t => t.Name)
                .ToList();

            Assert.Empty(unhandled);
        }
    }
}
