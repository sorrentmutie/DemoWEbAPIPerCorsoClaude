using DemoPerCorsoClaude.Domain;

namespace DemoPerCorsoClaude.Tests;

public class CategoryValidatorTests
{
    // ── Helper ────────────────────────────────────────────────────────────────

    private static CategoryRequest Valid() => new("Periferiche");

    // ── Caso valido ───────────────────────────────────────────────────────────

    [Fact]
    public void Validate_ValidRequest_ReturnsNoErrors()
    {
        var errors = CategoryValidator.Validate(Valid());

        Assert.Empty(errors);
    }

    // ── Nome: boundary sul limite dei 50 caratteri ────────────────────────────

    [Fact]
    public void Validate_Name1Char_IsValid()
    {
        var request = new CategoryRequest(new string('a', 1));

        var errors = CategoryValidator.Validate(request);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_Name49Chars_IsValid()
    {
        var request = new CategoryRequest(new string('a', 49));

        var errors = CategoryValidator.Validate(request);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_Name50Chars_ReturnsError()
    {
        var request = new CategoryRequest(new string('a', 50));

        var errors = CategoryValidator.Validate(request);

        Assert.Single(errors);
    }

    [Fact]
    public void Validate_Name51Chars_ReturnsError()
    {
        var request = new CategoryRequest(new string('a', 51));

        var errors = CategoryValidator.Validate(request);

        Assert.Single(errors);
    }

    // ── Nome: casi vuoti, bianchi e nulli ─────────────────────────────────────

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Validate_NullOrWhitespaceName_ReturnsError(string? name)
    {
        var request = new CategoryRequest(name!);

        var errors = CategoryValidator.Validate(request);

        Assert.Single(errors);
    }

    // ── Testo lungo esattamente al boundary non deve produrre errore "vuoto" ──

    [Fact]
    public void Validate_NameAtMaxLength_ReturnsLengthErrorNotEmptyError()
    {
        var request = new CategoryRequest(new string('a', 50));

        var errors = CategoryValidator.Validate(request);

        Assert.Single(errors);
        Assert.DoesNotContain(errors, e => e.Contains("vuoto"));
    }

    // ── Non devono accumularsi due errori per lo stesso campo nome ────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyOrWhitespaceName_ReturnsExactlyOneError(string name)
    {
        var request = new CategoryRequest(name);

        var errors = CategoryValidator.Validate(request);

        Assert.Single(errors);
    }
}
