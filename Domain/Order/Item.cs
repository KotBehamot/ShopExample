using Domain.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Order
{
    public class Item
    {
        public Guid Id { get; init; }
        public int Quantity { get; init; }
        public Money UnitPrice { get; init; }
        public Money TotalPrice => UnitPrice.Multiply(Quantity);

        private Item(int quantity, Money unitPrice)
        {
            this.Id = Guid.NewGuid();
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
        }

        internal static Item Create(int quantity, Money unitPrice)
        {
            return new Item(quantity, unitPrice);
        }
    }
}
