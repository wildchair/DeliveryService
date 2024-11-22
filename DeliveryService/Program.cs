using DeliveryService.Models;
using DeliveryService.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeliveryDb>(x => x.UseInMemoryDatabase("Delivery"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/orders", async (DeliveryDb db) =>
    await db.Orders.ToListAsync());

app.MapGet("/orders/{query}", async (string query, DeliveryDb db) =>
    {
        return await db.Orders.FindAsync(query)
            is Order order ? Results.Ok(order) : Results.NotFound();
    });

app.MapPost("/orders", async (Order order, DeliveryDb db) =>
{
    db.Orders.Add(order);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{order.Id}", order);
});

app.MapPost("/cargos", async (Cargo cargo, DeliveryDb db) =>
{
    db.Cargos.Add(cargo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{cargo.Id}", cargo);
});

app.MapPut("/orders/{id}", async (int id, Order inputOrder, DeliveryDb db) =>
{
    var todo = await db.Orders.FindAsync(id);

    if (todo is null) return Results.NotFound();

    //todo.Name = inputTodo.Name;
    //todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/orders/{id}", async (int id, DeliveryDb db) =>
{
    if (await db.Orders.FindAsync(id) is Order order)
    {
        db.Orders.Remove(order);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
