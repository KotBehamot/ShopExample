using System;

namespace Application.Orders.Commands.CreateOrder
{
    public readonly record struct CreateOrderItemDto(Guid ProductId, int Quantity, long UnitPrice, string Currency);
}
