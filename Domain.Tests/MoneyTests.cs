using Domain.Payments;
using System;
using Xunit;

namespace Domain.Tests
{
    public class MoneyTests
    {
        [Fact]
        public void Money_ShouldInitProperties()
        {
            // Act
            var money = new Money(100, "PLN");

            // Assert
            Assert.Equal(100, money.Amount);
            Assert.Equal("PLN", money.Currency);
        }

        [Fact]
        public void Add_WithSameCurrency_ShouldSucceed()
        {
            // Arrange
            var m1 = new Money(100, "USD");
            var m2 = new Money(50, "USD");

            // Act
            var result = m1.Add(m2);

            // Assert
            Assert.Equal(150, result.Amount);
            Assert.Equal("USD", result.Currency);
        }

        [Fact]
        public void Add_WithDifferentCurrency_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var m1 = new Money(100, "USD");
            var m2 = new Money(50, "EUR");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => m1.Add(m2));
        }

        [Fact]
        public void Subtract_WithSameCurrency_ShouldSucceed()
        {
            // Arrange
            var m1 = new Money(100, "USD");
            var m2 = new Money(30, "USD");

            // Act
            var result = m1.Subtract(m2);

            // Assert
            Assert.Equal(70, result.Amount);
            Assert.Equal("USD", result.Currency);
        }

        [Fact]
        public void Subtract_WithDifferentCurrency_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var m1 = new Money(100, "USD");
            var m2 = new Money(30, "EUR");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => m1.Subtract(m2));
        }

        [Fact]
        public void Multiply_ShouldScaleAmount()
        {
            // Arrange
            var money = new Money(50, "PLN");

            // Act
            var result = money.Multiply(3);

            // Assert
            Assert.Equal(150, result.Amount);
            Assert.Equal("PLN", result.Currency);
        }
    }
}
