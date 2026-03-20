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

        group.MapGet("/", GetAll).WithName("GetAllProducts");
        group.MapGet("/{id:int}", GetById).WithName("GetProductById");
        group.MapPost("/", Create).WithName("CreateProduct");
        group.MapPut("/{id:int}", Update).WithName("UpdateProduct");
        group.MapDelete("/{id:int}", Delete).WithName("DeleteProduct");
    }

    static IResult GetAll(IProductRepository repository) =>
        Results.Ok(repository.GetAll().Select(ProductDto.FromProduct));

    static IResult GetById(int id, IProductRepository repository) =>
        repository.GetById(id) is { } product
            ? Results.Ok(ProductDto.FromProduct(product))
            : Results.NotFound();

    static IResult Create(Product product, IProductRepository repository)
    {
        var errors = ProductValidator.Validate(product);
        if (errors.Count > 0)
            return Results.ValidationProblem(errors);

        var created = repository.Add(product);
        return Results.Created($"/products/{created.Id}", ProductDto.FromProduct(created));
    }

    static IResult Update(int id, Product product, IProductRepository repository)
    {
        var errors = ProductValidator.Validate(product);
        if (errors.Count > 0)
            return Results.ValidationProblem(errors);

        return repository.Update(id, product) is { } updated
            ? Results.Ok(ProductDto.FromProduct(updated))
            : Results.NotFound();
    }

    static IResult Delete(int id, IProductRepository repository) =>
        repository.Delete(id)
            ? Results.NoContent()
            : Results.NotFound();
}
