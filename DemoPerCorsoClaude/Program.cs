using DemoPerCorsoClaude.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

List<Product> products =
[
    new(1, "Tastiera meccanica", "Switch Cherry MX Red, layout IT", 89.99m, 42),
    new(2, "Monitor 27\"", "4K IPS 144Hz, HDR400", 349.00m, 15),
    new(3, "Mouse wireless", "Sensore ottico 25600 DPI", 59.90m, 78),
    new(4, "Webcam HD", "1080p 60fps con microfono integrato", 74.50m, 30),
    new(5, "Hub USB-C", "7 porte, Power Delivery 100W", 44.00m, 60),
];

app.MapGet("/weatherforecast", GetWeatherForecast).WithName("GetWeatherForecast");
app.MapGet("/products", GetProducts).WithName("GetProducts");
app.MapPost("/products", CreateProduct).WithName("CreateProduct");
app.MapPut("/products/{id}", UpdateProduct).WithName("UpdateProduct");
app.MapDelete("/products/{id}", DeleteProduct).WithName("DeleteProduct");

app.Run();

static WeatherForecast[] GetWeatherForecast()
{
    const int ForecastDays = 5;
    const int MinTempC = -20;
    const int MaxTempC = 55;

    string[] summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    return Enumerable.Range(1, ForecastDays)
        .Select(index => new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(MinTempC, MaxTempC),
            summaries[Random.Shared.Next(summaries.Length)]))
        .ToArray();
}

Product[] GetProducts() => [.. products];

IResult CreateProduct(ProductRequest request)
{
    var errors = ProductValidator.Validate(request);
    if (errors.Count > 0)
        return Results.BadRequest(errors);

    var newId = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
    var product = new Product(newId, request.Name, request.Description, request.Price, request.Stock);
    products.Add(product);
    return Results.Created($"/products/{product.Id}", product);
}

IResult UpdateProduct(int id, ProductRequest request)
{
    var errors = ProductValidator.Validate(request);
    if (errors.Count > 0)
        return Results.BadRequest(errors);

    var index = products.FindIndex(p => p.Id == id);
    if (index == -1)
        return Results.NotFound();

    products[index] = new Product(id, request.Name, request.Description, request.Price, request.Stock);
    return Results.Ok(products[index]);
}

IResult DeleteProduct(int id)
{
    var product = products.Find(p => p.Id == id);
    if (product is null)
        return Results.NotFound();

    products.Remove(product);
    return Results.NoContent();
}

/// <summary>Represents a daily weather forecast.</summary>
/// <param name="Date">Forecast date.</param>
/// <param name="TemperatureC">Temperature in Celsius.</param>
/// <param name="Summary">Short weather description.</param>
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
