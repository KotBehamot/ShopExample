using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using Domain;
using Domain.Order;
using Domain.Payment;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

Todo[] sampleTodos =
[
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
];

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos)
        .WithName("GetTodos");

todosApi.MapGet("/{id}", Results<Ok<Todo>, NotFound> (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? TypedResults.Ok(todo)
        : TypedResults.NotFound())
    .WithName("GetTodoById");

app.MapGet("/demo/order", () =>
{
    var customer = new Customer { Id = Guid.NewGuid() };
    var order = Order.CreateDraft(Guid.NewGuid(), customer, new List<Item>());
    
    order.AddItem(2, new Money(150, "PLN"));
    order.AddItem(1, new Money(400, "PLN"));
    
    order.Submit();

    // Map order domain model to DTO
    var dto = new OrderDto(
        order.Id,
        order.Customer.Id,
        order.Status.ToString(),
        order.OrderItems.Select(i => new OrderItemDto(i.Id, i.Quantity, i.UnitPrice.Amount, i.UnitPrice.Currency, i.TotalPrice.Amount)).ToList()
    );

    return TypedResults.Ok(dto);
}).WithName("GetDemoOrder");

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

public record OrderItemDto(Guid Id, int Quantity, long UnitPrice, string Currency, long TotalPrice);
public record OrderDto(Guid Id, Guid CustomerId, string Status, List<OrderItemDto> Items);

[JsonSerializable(typeof(Todo[]))]
[JsonSerializable(typeof(OrderDto))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
