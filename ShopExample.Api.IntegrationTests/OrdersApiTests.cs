using System.Net;
using System.Net.Http.Json;
using Application.Orders.Commands.CreateOrder;
using Application.Orders.Queries.GetOrder;

namespace ShopExample.Api.IntegrationTests;

public sealed class OrdersApiTests : IClassFixture<ShopApiFactory>
{
    private readonly HttpClient _client;

    public OrdersApiTests(ShopApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Creates an order and then retrieves it by id.
    /// </summary>
    [Fact]
    public async Task WhenOrderIsCreatedThenItCanBeRetrieved()
    {
        var customerId = Guid.NewGuid();
        var command = new CreateOrderCommand
        {
            CustomerId = customerId,
            OrderItems =
            [
                new CreateOrderItemDto(Guid.NewGuid(), 2, 1999, "PLN")
            ]
        };

        using var createResponse = await _client.PostAsJsonAsync("/orders", command);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdOrder = await createResponse.Content.ReadFromJsonAsync<CreateOrderResultDto>();
        Assert.NotEqual(Guid.Empty, createdOrder.OrderId);

        using var getResponse = await _client.GetAsync($"/orders/{createdOrder.OrderId}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var order = await getResponse.Content.ReadFromJsonAsync<OrderDto>();
        Assert.Equal(createdOrder.OrderId, order.Id);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Single(order.Items);
    }

    /// <summary>
    /// Returns bad request when order item quantity is invalid.
    /// </summary>
    [Fact]
    public async Task WhenOrderItemQuantityIsInvalidThenCreateOrderReturnsBadRequest()
    {
        var command = new CreateOrderCommand
        {
            CustomerId = Guid.NewGuid(),
            OrderItems =
            [
                new CreateOrderItemDto(Guid.NewGuid(), 0, 1999, "PLN")
            ]
        };

        using var response = await _client.PostAsJsonAsync("/orders", command);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
