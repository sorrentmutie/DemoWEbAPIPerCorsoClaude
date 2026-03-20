namespace DemoPerCorsoClaude.Domain;

/// <summary>Validates business rules for product requests.</summary>
public static class ProductValidator
{
    private const int MaxNameLength = 20;
    private const decimal MaxPrice = 1000m;

    /// <summary>Returns the list of business rule violations for the given request.</summary>
    public static IReadOnlyList<string> Validate(ProductRequest request)
    {
        var errors = new List<string>();

        if (request.Name.Length >= MaxNameLength)
            errors.Add($"Il nome del prodotto deve essere inferiore a {MaxNameLength} caratteri.");

        if (request.Price > MaxPrice)
            errors.Add($"Il prezzo del prodotto non può superare {MaxPrice} EUR.");

        return errors;
    }
}
