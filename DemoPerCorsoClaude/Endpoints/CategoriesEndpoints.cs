using DemoPerCorsoClaude.Domain;
using DemoPerCorsoClaude.Infrastructure;
using Microsoft.EntityFrameworkCore;

internal static class CategoriesEndpoints
{
    internal static void MapCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/categories");
        group.MapGet("/", GetAll).WithName("GetCategories");
        group.MapPost("/", Create).WithName("CreateCategory");
        group.MapPut("/{id}", Update).WithName("UpdateCategory");
        group.MapDelete("/{id}", Delete).WithName("DeleteCategory");
    }

    private static async Task<CategoryDto[]> GetAll(ProductsDbContext db) =>
        await db.Categories
            .GroupJoin(db.Products,
                c => c.Id,
                p => p.CategoryId,
                (c, products) => new CategoryDto(c.Id, c.Name, products.Count()))
            .ToArrayAsync();

    private static async Task<IResult> Create(CategoryRequest request, ProductsDbContext db)
    {
        var errors = CategoryValidator.Validate(request);
        if (errors.Count > 0)
            return Results.BadRequest(errors);

        var category = new Category(0, request.Name);
        db.Categories.Add(category);
        await db.SaveChangesAsync();
        return Results.Created($"/categories/{category.Id}", new CategoryDto(category.Id, category.Name, 0));
    }

    private static async Task<IResult> Update(int id, CategoryRequest request, ProductsDbContext db)
    {
        var errors = CategoryValidator.Validate(request);
        if (errors.Count > 0)
            return Results.BadRequest(errors);

        var existing = await db.Categories.FindAsync(id);
        if (existing is null)
            return Results.NotFound();

        var updated = existing with { Name = request.Name };
        db.Entry(existing).State = EntityState.Detached;
        db.Categories.Update(updated);
        await db.SaveChangesAsync();
        return Results.Ok(await ToDto(updated, db));
    }

    private static async Task<IResult> Delete(int id, ProductsDbContext db)
    {
        var category = await db.Categories.FindAsync(id);
        if (category is null)
            return Results.NotFound();

        var hasProducts = await db.Products.AnyAsync(p => p.CategoryId == id);
        if (hasProducts)
            return Results.Conflict("Impossibile eliminare la categoria: esistono prodotti associati.");

        db.Categories.Remove(category);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<CategoryDto> ToDto(Category category, ProductsDbContext db)
    {
        var productCount = await db.Products.CountAsync(p => p.CategoryId == category.Id);
        return new CategoryDto(category.Id, category.Name, productCount);
    }
}
