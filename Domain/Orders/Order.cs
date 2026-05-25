using Domain.Payments;
using System;
using System.Collections.Generic;

namespace Domain.Orders
{
    public class Order
    {
        private readonly List<Item> _orderItems;

        public Guid Id { get; init; }
        public Customer Customer { get; init; }
        public Status Status { get; private set; }
        public IReadOnlyList<Item> OrderItems => _orderItems;
    
        public static Order CreateDraft(Guid id, Customer customer, List<Item> orderItems)
        {
            return new Order(id, customer, Status.Draft, orderItems);
        }
        public void AddItem(int quantity, Money price)
        {
            InvalidItemException.Validate(quantity, price);
            var item = Item.Create(quantity, price);
            _orderItems.Add(item);
        }
        public void RemoveItem(Guid itemId)
        {
            var item = _orderItems.Find(i => i.Id == itemId)
                ?? throw new KeyNotFoundException($"Item '{itemId}' was not found in the order.");
            _orderItems.Remove(item);
        }
        public void ChangeItemQuantity(Guid itemId, int newQuantity)
        {
            var item = _orderItems.Find(i => i.Id == itemId)
                ?? throw new KeyNotFoundException($"Item '{itemId}' was not found in the order.");
            InvalidItemException.Validate(newQuantity, item.UnitPrice);
            item.UpdateQuantity(newQuantity);
        }
        public void ChangeItemPrice(Guid itemId, Money newPrice)
        {
            var item = _orderItems.Find(i => i.Id == itemId)
                ?? throw new KeyNotFoundException($"Item '{itemId}' was not found in the order.");
            InvalidItemException.Validate(item.Quantity, newPrice);
            item.UpdatePrice(newPrice);
        }
        public void Submit()
        {
            if (Status != Status.Draft)
            {
                throw new InvalidOrderStatusException("Only draft orders can be submitted.");
            }
            if(_orderItems.Count <= 0) {
                throw new EmptyOrderException("Cannot submit an order with no items.");
            }
            Status = Status.Submitted;
        }
        internal static Order Create(Guid id, Customer customer, Status status, List<Item> orderItems)
        {
            return new Order(id, customer, status, orderItems);
        }
        private Order(Guid id, Customer customer, Status status, List<Item> orderItems)
        {
            Id = id;
            Customer = customer;
            Status = status;
            _orderItems = orderItems;
        }
    }
}
