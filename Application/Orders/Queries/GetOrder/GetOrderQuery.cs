using MediatR;
using System;

namespace Application.Orders.Queries.GetOrder
{
    public sealed class GetOrderQuery : IRequest<OrderDto>
    {
        public Guid OrderId { get; init; }
    }
}
