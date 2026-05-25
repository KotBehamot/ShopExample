using System;
using System.Collections.Generic;

namespace Application.Orders.Queries.GetOrder
{
    public readonly record struct OrderDto(Guid Id, Guid CustomerId, string Status, IReadOnlyList<OrderItemDto> Items);
}
