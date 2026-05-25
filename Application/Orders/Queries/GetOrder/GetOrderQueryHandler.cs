using Application.Abstractions.Persistence;
using MediatR;

namespace Application.Orders.Queries.GetOrder
{
    internal class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderQueryHandler(IOrderRepository orderRepository)
        {
            ArgumentNullException.ThrowIfNull(orderRepository);
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
                ?? throw new KeyNotFoundException($"Order '{request.OrderId}' was not found.");

            var items = order.OrderItems
                .Select(item => new OrderItemDto(item.Id, "N/A", item.Quantity, item.UnitPrice.Amount))
                .ToList();

            return new OrderDto(order.Id, order.Customer.Id, order.Status.ToString(), items);
        }
    }
}
