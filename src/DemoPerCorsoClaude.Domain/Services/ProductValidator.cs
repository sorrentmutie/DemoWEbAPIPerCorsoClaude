using DemoPerCorsoClaude.Domain.Models;

namespace DemoPerCorsoClaude.Domain.Services;

/// <summary>Validatore per le regole di business dei prodotti.</summary>
public static class ProductValidator
{
    public const decimal MaxPrice = 100m;
    public const int MaxNameLength = 20;

    public static Dictionary<string, string[]> Validate(Product product)
    {
        var errors = new Dictionary<string, string[]>();

        if (product.Name.Length >= MaxNameLength)
            errors[nameof(Product.Name)] = [$"Il nome del prodotto deve essere inferiore a {MaxNameLength} caratteri."];

        if (product.Price > MaxPrice)
            errors[nameof(Product.Price)] = [$"Il prezzo del prodotto non può superare {MaxPrice}€."];

        if (product.CategoryId <= 0)
            errors[nameof(Product.CategoryId)] = ["La categoria è obbligatoria."];

        return errors;
    }
}
