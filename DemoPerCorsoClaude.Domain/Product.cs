namespace DemoPerCorsoClaude.Domain;

/// <summary>Represents a product in the catalog.</summary>
/// <param name="Id">Unique identifier of the product.</param>
/// <param name="Name">Display name of the product.</param>
/// <param name="Description">Optional long description.</param>
/// <param name="Price">Unit price in EUR.</param>
/// <param name="Stock">Available quantity in stock.</param>
/// <param name="CategoryId">Foreign key to the owning category.</param>
public record Product(int Id, string Name, string? Description, decimal Price, int Stock, int CategoryId);
