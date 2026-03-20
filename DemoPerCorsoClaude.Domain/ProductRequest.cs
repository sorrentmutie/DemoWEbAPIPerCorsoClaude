namespace DemoPerCorsoClaude.Domain;

/// <summary>Payload for creating or updating a product.</summary>
/// <param name="Name">Display name of the product.</param>
/// <param name="Description">Optional long description.</param>
/// <param name="Price">Unit price in EUR.</param>
/// <param name="Stock">Available quantity in stock.</param>
public record ProductRequest(string Name, string? Description, decimal Price, int Stock);
