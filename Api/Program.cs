using Application.Orders.Commands.CreateOrder;
using Application.Orders.Queries.GetOrder;
using Domain.Orders;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using Application;
using Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapPost("/orders", async (CreateOrderCommand command, ISender mediator, CancellationToken cancellationToken) =>
{
    try
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/orders/{result.OrderId}", result);
    }
    catch (InvalidItemException ex)
    {
        return Results.BadRequest(ex.Message);
    }
}).WithName("CreateOrder");

app.MapGet("/orders/{id:guid}", async Task<Results<Ok<OrderDto>, NotFound>> (Guid id, ISender mediator, CancellationToken cancellationToken) =>
{
    try
    {
        var result = await mediator.Send(new GetOrderQuery { OrderId = id }, cancellationToken);
        return TypedResults.Ok(result);
    }
    catch (KeyNotFoundException)
    {
        return TypedResults.NotFound();
    }
}).WithName("GetOrderById");

app.Run();

[JsonSerializable(typeof(CreateOrderCommand))]
[JsonSerializable(typeof(CreateOrderResultDto))]
[JsonSerializable(typeof(GetOrderQuery))]
[JsonSerializable(typeof(OrderDto))]
[JsonSerializable(typeof(string))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
