
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/orders", async () =>
{
    await Task.Delay(5000);
    return "�� ���";
});

app.MapGet("/orders/{query}", async (string query) =>
{
    await Task.Delay(5000);
    return "�� ���";
});

app.Run();
