using Fzerey.DDDStarter.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fzerey.DDDStarter.IntegrationTests
{
    public class InfrastructureTests(ApiFactory factory)
    {
        [Fact]
        public void App_uses_the_test_container_database()
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            Assert.Contains($"Port={factory.DatabasePort}", db.Database.GetConnectionString());
        }

        [Fact]
        public async Task All_migrations_are_applied_on_startup()
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var pending = await db.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken);

            Assert.Empty(pending);
            Assert.NotEmpty(await db.Database.GetAppliedMigrationsAsync(TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task Correlation_id_from_request_is_returned()
        {
            var client = factory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/item");
            request.Headers.Add("CorrelationId", "test-correlation");

            var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

            Assert.Equal("test-correlation", Assert.Single(response.Headers.GetValues("CorrelationId")));
        }

        [Fact]
        public async Task Correlation_id_is_generated_when_missing()
        {
            var response = await factory.CreateClient().GetAsync("/api/item", TestContext.Current.CancellationToken);

            Assert.True(Guid.TryParse(Assert.Single(response.Headers.GetValues("CorrelationId")), out _));
        }
    }
}
