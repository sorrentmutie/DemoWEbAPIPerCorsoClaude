using DemoPerCorsoClaude.Domain.Models;
using DemoPerCorsoClaude.Domain.Services;

namespace DemoPerCorsoClaude.Endpoints;

/// <summary>Definisce gli endpoint REST per le categorie.</summary>
public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/categories")
            .WithTags("Categories");

        group.MapGet("/", GetAll).WithName("GetAllCategories");
        group.MapGet("/{id:int}", GetById).WithName("GetCategoryById");
        group.MapPost("/", Create).WithName("CreateCategory");
        group.MapPut("/{id:int}", Update).WithName("UpdateCategory");
        group.MapDelete("/{id:int}", Delete).WithName("DeleteCategory");
    }

    static IResult GetAll(ICategoryRepository repository) =>
        Results.Ok(repository.GetAll().Select(CategoryDto.FromCategory));

    static IResult GetById(int id, ICategoryRepository repository) =>
        repository.GetById(id) is { } category
            ? Results.Ok(CategoryDto.FromCategory(category))
            : Results.NotFound();

    static IResult Create(Category category, ICategoryRepository repository)
    {
        var created = repository.Add(category);
        return Results.Created($"/categories/{created.Id}", CategoryDto.FromCategory(created));
    }

    static IResult Update(int id, Category category, ICategoryRepository repository) =>
        repository.Update(id, category) is { } updated
            ? Results.Ok(CategoryDto.FromCategory(updated))
            : Results.NotFound();

    static IResult Delete(int id, ICategoryRepository repository)
    {
        try
        {
            return repository.Delete(id)
                ? Results.NoContent()
                : Results.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new { error = ex.Message });
        }
    }
}
