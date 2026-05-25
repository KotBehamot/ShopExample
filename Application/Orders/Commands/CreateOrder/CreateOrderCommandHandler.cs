using Application.Abstractions.Persistence;
using Domain;
using Domain.Orders;
using Domain.Payments;
using MediatR;

namespace Application.Orders.Commands.CreateOrder
{
    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderResultDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(orderRepository);
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateOrderResultDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var customer = new Customer { Id = request.CustomerId };
            var orderId = Guid.NewGuid();
            var order = Order.CreateDraft(orderId, customer, []);

            foreach (var orderItem in request.OrderItems)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var money = new Money(orderItem.UnitPrice, orderItem.Currency);
                order.AddItem(orderItem.Quantity, money);
            }

            await _orderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateOrderResultDto(orderId);
        }
    }
}
