using Domain.Payments;
using System;

namespace Domain.Orders
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

    public class InvalidOrderStatusException(string message) : Exception(message)
    {
    }

    public class EmptyOrderException(string message) : Exception(message)
    {
    }

    public class ItemNotFoundException(string message) : Exception(message)
    {
    }
}