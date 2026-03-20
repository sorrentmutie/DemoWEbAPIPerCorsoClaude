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
        Results.Ok(repository.GetAll());

    static IResult GetById(int id, ICategoryRepository repository) =>
        repository.GetById(id) is { } category
            ? Results.Ok(category)
            : Results.NotFound();

    static IResult Create(Category category, ICategoryRepository repository)
    {
        var created = repository.Add(category);
        return Results.Created($"/categories/{created.Id}", created);
    }

    static IResult Update(int id, Category category, ICategoryRepository repository) =>
        repository.Update(id, category) is { } updated
            ? Results.Ok(updated)
            : Results.NotFound();

    static IResult Delete(int id, ICategoryRepository repository) =>
        repository.Delete(id)
            ? Results.NoContent()
            : Results.NotFound();
}
