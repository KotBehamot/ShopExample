using Application.Abstractions.Persistence;
using Domain.Orders;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Persistence;

internal sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly IMemoryCache _cache;

    public InMemoryOrderRepository(IMemoryCache cache)
    {
        ArgumentNullException.ThrowIfNull(cache);
        _cache = cache;
    }

    public Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(order);
        cancellationToken.ThrowIfCancellationRequested();

        _cache.Set(order.Id, order);

        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _cache.TryGetValue(orderId, out Order? order);

        return Task.FromResult(order);
    }
}
