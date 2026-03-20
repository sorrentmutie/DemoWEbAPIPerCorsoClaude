namespace DemoPerCorsoClaude.Domain;

/// <summary>Validates business rules for category requests.</summary>
public static class CategoryValidator
{
    private const int MaxNameLength = 50;

    /// <summary>Returns the list of business rule violations for the given request.</summary>
    public static IReadOnlyList<string> Validate(CategoryRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors.Add("Il nome della categoria non può essere vuoto.");
        }
        else if (request.Name.Length >= MaxNameLength)
        {
            errors.Add($"Il nome della categoria deve essere inferiore a {MaxNameLength} caratteri.");
        }

        return errors;
    }
}
