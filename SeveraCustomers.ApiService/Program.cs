using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using SeveraCustomers.ApiService;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddDbContextPool<SeveraPGSQLContext>(opt =>
//    opt.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb")));

builder.AddNpgsqlDbContext<SeveraPGSQLContext>(connectionName: "postgresdb");
builder.Services.AddScoped<CustomerService>();

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SeveraPGSQLContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/customers", ([FromServices]CustomerService service) =>
{
    return service.GetCustomers().ToList();
})
.WithName("GetCustomers");

app.MapGet("/customer/{Id}", ([FromRoute]int Id, [FromServices] CustomerService service) =>
{
    return service.GetCustomer(Id);
})
.WithName("GetCustomer");

app.MapPut("/editcustomer/{id}", ([FromRoute]int Id, [FromBody]Customers Dto, [FromServices] CustomerService service) =>
{
    var customer = service.GetCustomer(Id);

    if (customer is null) return Results.NotFound();

    service.UpdateCustomer(Dto);
    return Results.NoContent();
}).WithName("EditCustomer");

app.MapPost("/createcustomer/", ([FromBody] Customers Dto, [FromServices] CustomerService service) =>
{
    service.CreateCustomer(Dto);
    return Results.Created();
}).WithName("CreateCustomer");

app.MapGet("/severacustomerexists/{Id}", ([FromRoute] Guid Id, [FromServices] CustomerService service) =>
{
    return service.SeveraCustomerExists(Id);
})
.WithName("SeveraCustomerExists");

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
