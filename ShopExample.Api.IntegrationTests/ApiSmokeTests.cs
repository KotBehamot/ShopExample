using System.Net;

namespace ShopExample.Api.IntegrationTests;

public sealed class ApiSmokeTests : IClassFixture<ShopApiFactory>
{
    private readonly HttpClient _client;

    public ApiSmokeTests(ShopApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Verifies that the host correctly builds its DI container and responds to unknown endpoints with 404.
    /// </summary>
    [Fact]
    public async Task AppStartsAndRespondsToUnknownEndpoints()
    {
        using var response = await _client.GetAsync("/non-existent-endpoint-smoke-test");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// Verifies that getting a non-existent order correctly returns 404.
    /// This tests routing, endpoint mapping, and proper fallback behavior.
    /// </summary>
    [Fact]
    public async Task GetOrderWithRandomIdReturnsNotFound()
    {
        using var response = await _client.GetAsync($"/orders/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// Verifies that the schema of OpenAPI specification is reachable and generated correctly.
    /// </summary>
    [Fact]
    public async Task OpenApiDocumentIsReachable()
    {
        using var response = await _client.GetAsync("/openapi/v1.json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
