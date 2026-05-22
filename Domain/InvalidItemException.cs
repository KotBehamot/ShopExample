using Domain.Payment;
using System;

namespace Domain.Order
{
    internal class InvalidItemException : Exception
    {
        public static void Validate(int quantity, Money price)
        {
            if (quantity <= 0)
            {
                throw new InvalidItemException("Quantity must be greater than zero.");
            }
            if (price.Amount <= 0)
            {
                throw new InvalidItemException("Price must be greater than zero.");
            }
        }

        private InvalidItemException(string message) : base(message)
        {
        }
    }

    internal class InvalidOrderStatusException : Exception
    {
        public InvalidOrderStatusException(string message) : base(message)
        {
        }
    }

    internal class EmptyOrderException : Exception
    {
        public EmptyOrderException(string message) : base(message)
        {
        }
    }

    internal class OrderSubmittedEvent
    {
        public void Raise(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);
        }
    }
}