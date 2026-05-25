using Domain.Order;

namespace Application.Abstractions.Persistence;

public interface IOrderRepository
{
    /// <summary>
    /// Adds a new order to persistence.
    /// </summary>
    Task AddAsync(Order order, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an order by identifier.
    /// </summary>
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
}
