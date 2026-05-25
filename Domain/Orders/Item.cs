using Domain.Payments;
using System;

namespace Domain.Order
{
    public class Item
    {
        public Guid Id { get; init; }
        public int Quantity { get; private set; }
        public Money UnitPrice { get; private set; }
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

        internal void UpdatePrice(Money newPrice)
        {
            UnitPrice = newPrice;
        }

        internal void UpdateQuantity(int newQuantity)
        {
            Quantity = newQuantity;
        }
    }
}
