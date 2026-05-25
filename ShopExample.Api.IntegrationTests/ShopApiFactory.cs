using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ShopExample.Api.IntegrationTests;

/// <summary>
/// Factory for creating test instances of the Shop API for integration testing.
/// Uses in-memory repositories configured in Infrastructure layer.
/// </summary>
public sealed class ShopApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureServices(services =>
        {
            // Infrastructure already registers InMemoryOrderRepository and InMemoryUnitOfWork
            // as singletons, so we don't need to replace anything here.
            // The test instance will use the same in-memory implementations as production.
        });
    }
}
