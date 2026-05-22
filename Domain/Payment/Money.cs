using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Payment
{
    internal readonly record struct Money : IEquatable<Money>
    {
        public long Amount { get; init; }
        public string Currency { get; init; }

        public Money(long amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public readonly Money Add(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException("Cannot add money with different currencies.");
            }
            return new Money(Amount + other.Amount, Currency);
        }

        public readonly Money Subtract(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException("Cannot subtract money with different currencies.");
            }
            return new Money(Amount - other.Amount, Currency);
        }

        public readonly Money Multiply(int multiplier)
        {
            return new Money { Amount = Amount * multiplier, Currency = Currency };
        }
    }
}
