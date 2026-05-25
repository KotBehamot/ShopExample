using System;
using System.Collections.Generic;
using Domain.Payments;
using Xunit;
using OrderClass = Domain.Orders.Order;
using Domain.Orders;

namespace Domain.Tests
{
    public class OrderTests
    {
        private readonly Customer _testCustomer;

        public OrderTests()
        {
            _testCustomer = new Customer { Id = Guid.NewGuid() };
        }

        [Fact]
        public void CreateDraft_ShouldCreateOrderWithDraftStatusAndEmptyItems()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var items = new List<Item>();

            // Act
            var order = OrderClass.CreateDraft(orderId, _testCustomer, items);

            // Assert
            Assert.Equal(orderId, order.Id);
            Assert.Equal(_testCustomer, order.Customer);
            Assert.Equal(Status.Draft, order.Status);
            Assert.Equal(items, order.OrderItems);
        }

        [Fact]
        public void AddItem_WhenValid_ShouldAddItemToOrder()
        {
            // Arrange
            var order = OrderClass.CreateDraft(Guid.NewGuid(), _testCustomer, new List<Item>());
            var initialCount = order.OrderItems.Count;
            var price = new Money(100, "PLN");

            // Act
            order.AddItem(2, price);

            // Assert
            Assert.Equal(initialCount + 1, order.OrderItems.Count);
            var addedItem = order.OrderItems[0];
            Assert.Equal(2, addedItem.Quantity);
            Assert.Equal(price, addedItem.UnitPrice);
            Assert.Equal(200, addedItem.TotalPrice.Amount);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddItem_WhenQuantityIsZeroOrNegative_ShouldThrowInvalidItemException(int invalidQuantity)
        {
            // Arrange
            var order = OrderClass.CreateDraft(Guid.NewGuid(), _testCustomer, new List<Item>());
            var price = new Money(50, "PLN");

            // Act & Assert
            var exception = Assert.Throws<InvalidItemException>((Action)(() => order.AddItem(invalidQuantity, price)));
            Assert.Contains("Quantity must be greater than zero.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void AddItem_WhenPriceIsZeroOrNegative_ShouldThrowInvalidItemException(long invalidAmount)
        {
            // Arrange
            var order = OrderClass.CreateDraft(Guid.NewGuid(), _testCustomer, new List<Item>());
            var price = new Money(invalidAmount, "PLN");

            // Act & Assert
            var exception = Assert.Throws<InvalidItemException>((Action)(() => order.AddItem(5, price)));
            Assert.Contains("Price must be greater than zero.", exception.Message);
        }

        [Fact]
        public void RemoveItem_WhenItemExists_ShouldRemoveItem()
        {
            // Arrange
            var order = OrderClass.CreateDraft(Guid.NewGuid(), _testCustomer, new List<Item>());
            order.AddItem(3, new Money(150, "EUR"));
            var addedItemId = order.OrderItems[0].Id;

            // Act
            order.RemoveItem(addedItemId);

            // Assert
            Assert.Empty(order.OrderItems);
        }

        [Fact]
        public void ChangeItemQuantity_WhenItemExists_ShouldUpdateQuantity()
        {
            // Arrange
            var order = OrderClass.CreateDraft(Guid.NewGuid(), _testCustomer, new List<Item>());
            order.AddItem(1, new Money(500, "PLN"));
            var addedItemId = order.OrderItems[0].Id;

            // Act
            order.ChangeItemQuantity(addedItemId, 3);

            // Assert
            var updatedItem = order.OrderItems[0];
            Assert.Equal(3, updatedItem.Quantity);
            Assert.Equal(1500, updatedItem.TotalPrice.Amount);
        }

        [Fact]
        public void ChangeItemPrice_WhenItemExists_ShouldUpdatePrice()
        {
            // Arrange
            var order = OrderClass.CreateDraft(Guid.NewGuid(), _testCustomer, new List<Item>());
            order.AddItem(2, new Money(100, "USD"));
            var addedItemId = order.OrderItems[0].Id;
            var newPrice = new Money(150, "USD");

            // Act
            order.ChangeItemPrice(addedItemId, newPrice);

            // Assert
            var updatedItem = order.OrderItems[0];
            Assert.Equal(newPrice, updatedItem.UnitPrice);
            Assert.Equal(300, updatedItem.TotalPrice.Amount);
        }

        [Fact]
        public void Submit_WhenValidDraftWithItems_ShouldChangeStatusToSubmitted()
        {
            // Arrange
            var order = OrderClass.CreateDraft(Guid.NewGuid(), _testCustomer, new List<Item>());
            order.AddItem(1, new Money(100, "PLN"));

            // Act
            order.Submit();

            // Assert
            Assert.Equal(Status.Submitted, order.Status);
        }

        [Fact]
        public void Submit_WhenOrderIsAlreadySubmitted_ShouldThrowInvalidOrderStatusException()
        {
            // Arrange
            var order = OrderClass.CreateDraft(Guid.NewGuid(), _testCustomer, new List<Item>());
            order.AddItem(1, new Money(100, "PLN"));
            order.Submit(); // Initial submit

            // Act & Assert
            var exception = Assert.Throws<InvalidOrderStatusException>((Action)(() => order.Submit()));
            Assert.Contains("Only draft orders can be submitted.", exception.Message);
        }

        [Fact]
        public void Submit_WhenDraftHasNoItems_ShouldThrowEmptyOrderException()
        {
            // Arrange
            var order = OrderClass.CreateDraft(Guid.NewGuid(), _testCustomer, new List<Item>());

            // Act & Assert
            var exception = Assert.Throws<EmptyOrderException>((Action)(() => order.Submit()));
            Assert.Contains("Cannot submit an order with no items.", exception.Message);
        }
    }
}
