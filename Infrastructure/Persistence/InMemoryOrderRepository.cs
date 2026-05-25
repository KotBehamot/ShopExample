using System.Collections.Concurrent;
using Application.Abstractions.Persistence;
using Domain.Orders;

namespace Infrastructure.Persistence;

internal sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(order);
        cancellationToken.ThrowIfCancellationRequested();

        _orders[order.Id] = order;

        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _orders.TryGetValue(orderId, out var order);

        return Task.FromResult(order);
    }
}
