namespace DemoPerCorsoClaude.Domain.Models;

/// <summary>DTO per la risposta API di un prodotto.</summary>
public record ProductDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    Category Category,
    int Stock)
{
    public static ProductDto FromProduct(Product product) =>
        new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Category!,
            product.Stock);
}
