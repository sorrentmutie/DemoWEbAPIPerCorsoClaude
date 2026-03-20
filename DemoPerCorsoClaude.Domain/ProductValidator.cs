namespace DemoPerCorsoClaude.Domain;

/// <summary>Validates business rules for product requests.</summary>
public static class ProductValidator
{
    private const int MaxNameLength = 20;
    private const decimal MinPrice = 0m;
    private const decimal MaxPrice = 1000m;

    /// <summary>Returns the list of business rule violations for the given request.</summary>
    public static IReadOnlyList<string> Validate(ProductRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors.Add("Il nome del prodotto non può essere vuoto.");
        }
        else if (request.Name.Length >= MaxNameLength)
        {
            errors.Add($"Il nome del prodotto deve essere inferiore a {MaxNameLength} caratteri.");
        }

        if (request.Price < MinPrice)
            errors.Add("Il prezzo del prodotto non può essere negativo.");
        else if (request.Price > MaxPrice)
            errors.Add($"Il prezzo del prodotto non può superare {MaxPrice} EUR.");

        if (request.CategoryId <= 0)
            errors.Add("Il campo CategoryId deve essere un valore positivo.");

        return errors;
    }
}
