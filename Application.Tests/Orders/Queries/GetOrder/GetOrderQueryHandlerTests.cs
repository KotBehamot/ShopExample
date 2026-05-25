using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Persistence;
using Application.Orders.Queries.GetOrder;
using Domain;
using Domain.Orders;
using Domain.Payments;
using NSubstitute;
using Xunit;

namespace Application.Tests.Orders.Queries.GetOrder
{
    public class GetOrderQueryHandlerTests
    {
        private readonly IOrderRepository _orderRepositoryMock;
        private readonly GetOrderQueryHandler _handler;

        public GetOrderQueryHandlerTests()
        {
            _orderRepositoryMock = Substitute.For<IOrderRepository>();
            _handler = new GetOrderQueryHandler(_orderRepositoryMock);
        }

        [Fact]
        public async Task Handle_WhenOrderExists_ShouldReturnOrderDto()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var customer = new Customer { Id = Guid.NewGuid() };
            var order = Order.CreateDraft(orderId, customer, new List<Item>());
            order.AddItem(3, new Money(200, "PLN"));

            _orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>())
                .Returns(order);

            var query = new GetOrderQuery { OrderId = orderId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(orderId, result.Id);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Equal("Draft", result.Status);
            Assert.Single(result.Items);
            Assert.Equal(3, result.Items[0].Quantity);
            Assert.Equal(200m, result.Items[0].Price);
        }

        [Fact]
        public async Task Handle_WhenOrderDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>())
                .Returns((Order?)null);

            var query = new GetOrderQuery { OrderId = orderId };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _handler.Handle(query, CancellationToken.None)
            );
        }
    }
}
