using DemoPerCorsoClaude.Domain.Services;
using DemoPerCorsoClaude.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapWeatherEndpoints();
app.MapProductEndpoints();

app.Run();
