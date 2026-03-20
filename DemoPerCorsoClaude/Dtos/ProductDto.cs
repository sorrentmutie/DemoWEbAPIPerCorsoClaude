/// <summary>Read model for a product, including the category name.</summary>
internal record ProductDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    int CategoryId,
    string CategoryName);
