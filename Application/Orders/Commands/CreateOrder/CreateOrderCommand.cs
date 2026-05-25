using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Orders.Commands.CreateOrder
{
    public sealed class CreateOrderCommand : IRequest<CreateOrderResultDto>
    {
        public Guid CustomerId { get; init; }
        public List<CreateOrderItemDto> OrderItems { get; init; } = [];
    }
}
