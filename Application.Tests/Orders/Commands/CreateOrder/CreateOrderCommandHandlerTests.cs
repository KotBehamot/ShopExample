using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Persistence;
using Application.Orders.Commands.CreateOrder;
using Domain;
using Domain.Orders;
using NSubstitute;
using Xunit;

namespace Application.Tests.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly IOrderRepository _orderRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _orderRepositoryMock = Substitute.For<IOrderRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _handler = new CreateOrderCommandHandler(_orderRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_WhenRequestIsValid_ShouldCreateOrderAndSaveToRepository()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var command = new CreateOrderCommand
            {
                CustomerId = customerId,
                OrderItems = new List<CreateOrderItemDto>
                {
                    new(Guid.NewGuid(), 2, 150, "PLN"),
                    new(Guid.NewGuid(), 1, 400, "PLN")
                }
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result.OrderId);

            await _orderRepositoryMock.Received(1).AddAsync(
                Arg.Is<Order>(order => 
                    order.Customer.Id == customerId && 
                    order.OrderItems.Count == 2 &&
                    order.Status == Status.Draft
                ), 
                Arg.Any<CancellationToken>()
            );

            await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
