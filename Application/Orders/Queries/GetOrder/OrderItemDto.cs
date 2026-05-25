using System;

namespace Application.Orders.Queries.GetOrder
{
    public readonly record struct OrderItemDto(Guid Id, string Name, int Quantity, long Price);
}
