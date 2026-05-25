using Domain.Payments;
using System;

namespace Domain.Order
{
    public class InvalidItemException : Exception
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

    public class InvalidOrderStatusException : Exception
    {
        public InvalidOrderStatusException(string message) : base(message)
        {
        }
    }

    public class EmptyOrderException : Exception
    {
        public EmptyOrderException(string message) : base(message)
        {
        }
    }

    public class OrderSubmittedEvent
    {
        public void Raise(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);
        }
    }
}