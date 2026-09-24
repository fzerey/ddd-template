using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

[assembly: AssemblyFixture(typeof(Fzerey.DDDStarter.IntegrationTests.ApiFactory))]

namespace Fzerey.DDDStarter.IntegrationTests
{
    public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("applicationdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        public int DatabasePort => _postgres.GetMappedPublicPort(PostgreSqlBuilder.PostgreSqlPort);

        public async ValueTask InitializeAsync()
        {
            await _postgres.StartAsync();
            _ = Server;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.UseSetting("DatabaseConfiguration:host", _postgres.Hostname);
            builder.UseSetting("DatabaseConfiguration:port", DatabasePort.ToString());
            builder.UseSetting("DatabaseConfiguration:dbName", "applicationdb");
            builder.UseSetting("DatabaseConfiguration:username", "postgres");
            builder.UseSetting("DatabaseConfiguration:password", "postgres");
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _postgres.DisposeAsync();
        }
    }
}
