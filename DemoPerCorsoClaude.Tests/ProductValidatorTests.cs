using DemoPerCorsoClaude.Domain;

namespace DemoPerCorsoClaude.Tests;

public class ProductValidatorTests
{
    // ── Helper ────────────────────────────────────────────────────────────────

    private static ProductRequest Valid() =>
        new("Mouse wireless", "Descrizione", 59.90m, 1000000, 1);

    // ── Caso valido ───────────────────────────────────────────────────────────

    [Fact]
    public void Validate_ValidRequest_ReturnsNoErrors()
    {
        var errors = ProductValidator.Validate(Valid());

        Assert.Empty(errors);
    }

    // ── Nome: boundary sul limite dei 20 caratteri ────────────────────────────

    [Fact]
    public void Validate_Name19Chars_IsValid()
    {
        var request = Valid() with { Name = new string('a', 19) };

        var errors = ProductValidator.Validate(request);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_Name20Chars_ReturnsError()
    {
        var request = Valid() with { Name = new string('a', 20) };

        var errors = ProductValidator.Validate(request);

        Assert.Single(errors);
    }

    [Fact]
    public void Validate_Name21Chars_ReturnsError()
    {
        var request = Valid() with { Name = new string('a', 21) };

        var errors = ProductValidator.Validate(request);

        Assert.Single(errors);
    }

    // ── Nome: casi vuoti e nulli ──────────────────────────────────────────────

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrWhitespaceName_ReturnsError(string? name)
    {
        var request = Valid() with { Name = name! };

        var errors = ProductValidator.Validate(request);

        Assert.Single(errors);
    }

    // ── Prezzo: boundary sul limite dei 100 € ─────────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(99.99)]
    [InlineData(100)]
    public void Validate_PriceWithinRange_IsValid(double price)
    {
        var request = Valid() with { Price = (decimal)price };

        var errors = ProductValidator.Validate(request);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_Price100_01_ReturnsError()
    {
        var request = Valid() with { Price = 100.01m };

        var errors = ProductValidator.Validate(request);

        Assert.Single(errors);
    }

    [Fact]
    public void Validate_Price1000000_ReturnsError()
    {
        var request = Valid() with { Price = 1_000_000m };

        var errors = ProductValidator.Validate(request);

        Assert.Single(errors);
        Assert.Contains(errors, e => e.Contains("100"));
    }

    // ── Prezzo: valori negativi ───────────────────────────────────────────────

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void Validate_NegativePrice_ReturnsError(double price)
    {
        var request = Valid() with { Price = (decimal)price };

        var errors = ProductValidator.Validate(request);

        Assert.Single(errors);
    }

    // ── CategoryId ────────────────────────────────────────────────────────────

    [Fact]
    public void Validate_CategoryId1_IsValid()
    {
        var request = Valid() with { CategoryId = 1 };

        var errors = ProductValidator.Validate(request);

        Assert.Empty(errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Validate_InvalidCategoryId_ReturnsError(int categoryId)
    {
        var request = Valid() with { CategoryId = categoryId };

        var errors = ProductValidator.Validate(request);

        Assert.Single(errors);
    }

    // ── Violazioni multiple ───────────────────────────────────────────────────

    [Fact]
    public void Validate_AllFieldsInvalid_ReturnsAllErrors()
    {
        var request = new ProductRequest(
            Name: new string('a', 25),
            Description: null,
            Price: -5m,
            Stock: 0,
            CategoryId: 0);

        var errors = ProductValidator.Validate(request);

        Assert.Equal(3, errors.Count);
    }

    [Fact]
    public void Validate_EmptyNameAndInvalidPrice_ReturnsTwoErrors()
    {
        var request = Valid() with { Name = "", Price = 150m };

        var errors = ProductValidator.Validate(request);

        Assert.Equal(2, errors.Count);
    }

    [Fact]
    public void Validate_EmptyNameAndInvalidCategoryId_ReturnsTwoErrors()
    {
        var request = Valid() with { Name = "", CategoryId = -1 };

        var errors = ProductValidator.Validate(request);

        Assert.Equal(2, errors.Count);
    }
}
