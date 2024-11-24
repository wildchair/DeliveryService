using DeliveryService.Dto;
using DeliveryService.Extensions;
using DeliveryService.Models;
using DeliveryService.Repository;
using DeliveryService.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeliveryDb>(x => x.UseInMemoryDatabase("Delivery"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddOpenApi();
var app = builder.Build();
app.MapOpenApi();

app.MapGet("/orders", async (DeliveryDb db) =>
{
    return await db.Orders.Include(x => x.Courier).Include(x => x.Cargo).ToListAsync();
});

app.MapGet("/orders/{id}", async (int id, DeliveryDb db) =>
{
    var order = await db.Orders.FindAsync(id);
    if (order == null)
        return Results.NotFound(id);

    await db.Entry(order).Reference(p => p.Cargo).LoadAsync();
    await db.Entry(order).Reference(p => p.Courier).LoadAsync();
    return Results.Ok(order);
});

app.MapGet("/orders/search/{query}", async (string query, DeliveryDb db) =>
{
    query = query.ToLower();
    // поиск по всем полям ¯\_(ツ)_ /¯
    return await db.Orders.Include(x => x.Cargo).Include(x => x.Courier).Where(order =>
    order.Id.ToString().ToLower().Contains(query) ||
    order.Address.ToLower().Contains(query) ||
    order.DeliveryTime.ToString().ToLower().Contains(query) ||
    order.Description.ToLower().Contains(query) ||
    order.State.ToString().ToLower().Contains(query) ||
    (
    order.Cargo != null &&
    (order.Cargo.Id.ToString().ToLower().Contains(query) ||
    order.Cargo.Name.ToLower().Contains(query) ||
    order.Cargo.Weight.ToString().ToLower().Contains(query) ||
    order.Cargo.SizeClass.ToString().ToLower().Contains(query))
    )
    ||
    (
    order.Courier != null && //nullable-courier ведет к nullable-полям?
    (((int?)(order.Courier.Id)).ToString().ToLower().Contains(query) ||
    ((string?)order.Courier.Name).ToLower().Contains(query) ||
    ((string?)order.Courier.Surname).ToLower().Contains(query) ||
    ((bool?)order.Courier.IsCarCourier).ToString().ToLower().Contains(query) ||
    ((string?)order.Courier.Phone).ToLower().Contains(query))
    )
    ).ToListAsync();
});

app.MapPost("/orders", async (NewOrderDto newOrder, DeliveryDb db) =>
{
    var order = new Order()
    {
        Id = newOrder.Id,
        Cargo = newOrder.Cargo,
        Address = newOrder.Address,
        DeliveryTime = newOrder.DeliveryTime,
        State = OrderState.New
    };

    if (await db.Orders.FindAsync(order.Id) != null)
        return Results.BadRequest($"Заявка с ID:{order.Id} уже существует.");
    if (await db.Cargos.FindAsync(order.Cargo.Id) != null)
        return Results.BadRequest($"Груз с ID:{order.Cargo.Id} уже существует.");

    if (newOrder.CourierId != null)
    {
        order.Courier = await db.Couriers.FindAsync(newOrder.CourierId);
        if (order.Courier == null)
            return Results.NotFound($"Курьер с ID {newOrder.CourierId} не найден.");
    }
    db.Orders.Add(order);
    await db.SaveChangesAsync();

    return Results.Created($"/orders/{order.Id}", order);
});

app.MapPost("/orders/initialize", async (Order[] orders, DeliveryDb db) =>
{
    if (db.Orders.Count() > 0)
        return Results.Ok();

    db.Orders.AddRange(orders);
    await db.SaveChangesAsync();

    return Results.Created($"/orders/initialize", orders);
});

app.MapPut("/orders/{id}", async (int id, UpdateOrderDto inputOrder, DeliveryDb db) =>
{
    var order = await db.Orders.FindAsync(id);
    var inputCourier = await db.Couriers.FindAsync(inputOrder.CourierId);

    if (order is null || inputCourier is null) return Results.NotFound();

    await db.Entry(order).Reference(p => p.Cargo).LoadAsync();
    await db.Entry(order).Reference(p => p.Courier).LoadAsync();

    var inputCourierId = inputOrder.CourierId;

    var changedProperties = PropertyComparator.FindChangedProperties(order, inputOrder);

    if (changedProperties.Count() == 0 && inputCourier == order.Courier)
        return Results.NoContent();

    // нездоровое увлечение рефлексией
    switch (order.State)
    {
        case OrderState.New:
            if (changedProperties.Any(x => x.original.Name == nameof(order.State)))
            {
                if (inputOrder.State == OrderState.Cancelled && String.IsNullOrEmpty(inputOrder.Description))
                    return Results.BadRequest("Для отмены заказа необходимо указать комментарий.");

                if ((inputOrder.State == OrderState.InProgress || inputOrder.State == OrderState.Completed) &&
                     order.Courier == null && inputOrder.CourierId == null)
                    return Results.BadRequest($"Для перевода заказа в состояние {inputOrder.State} необходимо назначить курьера.");
            }
            break;
        case OrderState.InProgress:
            if (inputOrder.State == OrderState.InProgress)
                return Results.BadRequest("На этом этапе изменение полей заказа невозможно.");

            if (inputOrder.State == OrderState.Completed && changedProperties.Any(x => x.original.Name != nameof(order.State)))
                return Results.BadRequest("Заказ нельзя изменять на этом этапе.");

            if (inputOrder.State == OrderState.Cancelled && (String.IsNullOrEmpty(inputOrder.Description) ||
               changedProperties.Any(x => x.original.Name != nameof(order.State) && x.original.Name != nameof(order.Description))) ||
               order.Courier.Id != inputCourier.Id)
                return Results.BadRequest("При отмене заказа необходимо указать только комментарий.");

            break;
        default:
            return Results.BadRequest("Заказ нельзя изменять на этом этапе.");
    }

    order.Courier = inputCourier;

    foreach (var property in changedProperties)
    {
        if (property.original.Name == nameof(order.Cargo))
        {
            order.Cargo.CopyFrom(inputOrder.Cargo);

            continue;
        }

        //if (property.original.Name == nameof(order.Courier))
        //{
        //    order.Courier = order.Courier == null ? inputOrder.Courier : order.Courier.CopyFrom(inputOrder.Courier);
        //    //order.Courier.CopyFrom(inputOrder.Courier);
        //    continue;
        //}

        property.original.SetValue(order, property.changed.GetValue(inputOrder));
    }

    //order.FromUpdateOrderDto(inputOrder);

    db.Orders.Update(order);
    await db.SaveChangesAsync();

    return Results.Ok(order);
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