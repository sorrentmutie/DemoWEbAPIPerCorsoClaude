using DemoPerCorsoClaude.Domain.Models;
using DemoPerCorsoClaude.Domain.Services;

namespace DemoPerCorsoClaude.Endpoints;

/// <summary>Definisce gli endpoint REST per i prodotti.</summary>
public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/products")
            .WithTags("Products");

        group.MapGet("/", (IProductRepository repository) =>
            Results.Ok(repository.GetAll()))
            .WithName("GetAllProducts");

        group.MapGet("/{id:int}", (int id, IProductRepository repository) =>
            repository.GetById(id) is { } product
                ? Results.Ok(product)
                : Results.NotFound())
            .WithName("GetProductById");

        group.MapPost("/", (Product product, IProductRepository repository) =>
        {
            var created = repository.Add(product);
            return Results.Created($"/products/{created.Id}", created);
        }).WithName("CreateProduct");

        group.MapPut("/{id:int}", (int id, Product product, IProductRepository repository) =>
            repository.Update(id, product) is { } updated
                ? Results.Ok(updated)
                : Results.NotFound())
            .WithName("UpdateProduct");

        group.MapDelete("/{id:int}", (int id, IProductRepository repository) =>
            repository.Delete(id)
                ? Results.NoContent()
                : Results.NotFound())
            .WithName("DeleteProduct");
    }
}
