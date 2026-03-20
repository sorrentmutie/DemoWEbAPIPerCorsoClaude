namespace DemoPerCorsoClaude.Domain.Models;

/// <summary>DTO per la risposta API di una categoria.</summary>
public record CategoryDto(
    int Id,
    string Name,
    string? Description,
    int ProductCount)
{
    public static CategoryDto FromCategory(Category category) =>
        new(
            category.Id,
            category.Name,
            category.Description,
            category.Products.Count);
}
