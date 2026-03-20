using DemoPerCorsoClaude.Domain;
using DemoPerCorsoClaude.Infrastructure;
using Microsoft.EntityFrameworkCore;

internal static class ProductsEndpoints
{
    internal static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");
        group.MapGet("/", GetAll).WithName("GetProducts");
        group.MapPost("/", Create).WithName("CreateProduct");
        group.MapPut("/{id}", Update).WithName("UpdateProduct");
        group.MapDelete("/{id}", Delete).WithName("DeleteProduct");
    }

    private static async Task<ProductDto[]> GetAll(ProductsDbContext db) =>
        await db.Products
            .Join(db.Categories,
                p => p.CategoryId,
                c => c.Id,
                (p, c) => ToDto(p, c))
            .ToArrayAsync();

    private static async Task<IResult> Create(ProductRequest request, ProductsDbContext db)
    {
        var errors = ProductValidator.Validate(request);
        if (errors.Count > 0)
            return Results.BadRequest(errors);

        var category = await db.Categories.FindAsync(request.CategoryId);
        if (category is null)
            return Results.BadRequest(new[] { $"La categoria con Id {request.CategoryId} non esiste." });

        var product = new Product(0, request.Name, request.Description, request.Price, request.Stock, request.CategoryId);
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return Results.Created($"/products/{product.Id}", ToDto(product, category));
    }

    private static async Task<IResult> Update(int id, ProductRequest request, ProductsDbContext db)
    {
        var errors = ProductValidator.Validate(request);
        if (errors.Count > 0)
            return Results.BadRequest(errors);

        var category = await db.Categories.FindAsync(request.CategoryId);
        if (category is null)
            return Results.BadRequest(new[] { $"La categoria con Id {request.CategoryId} non esiste." });

        var existing = await db.Products.FindAsync(id);
        if (existing is null)
            return Results.NotFound();

        var updated = existing with
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId
        };
        db.Entry(existing).State = EntityState.Detached;
        db.Products.Update(updated);
        await db.SaveChangesAsync();
        return Results.Ok(ToDto(updated, category));
    }

    private static async Task<IResult> Delete(int id, ProductsDbContext db)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null)
            return Results.NotFound();

        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static ProductDto ToDto(Product product, Category category) =>
        new(product.Id, product.Name, product.Description, product.Price, product.Stock, product.CategoryId, category.Name);
}
