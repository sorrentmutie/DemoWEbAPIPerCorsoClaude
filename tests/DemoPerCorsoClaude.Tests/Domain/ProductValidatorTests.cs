using DemoPerCorsoClaude.Domain.Models;
using DemoPerCorsoClaude.Domain.Services;
using DemoPerCorsoClaude.Tests.Builders;
using Shouldly;

namespace DemoPerCorsoClaude.Tests.Domain;

public class ProductValidatorTests
{
    [Fact]
    public void Should_ReturnNoErrors_When_ProductIsValid()
    {
        var product = new ProductBuilder().Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void Should_ReturnError_When_NameIsExactly20Characters()
    {
        var product = new ProductBuilder()
            .WithName("12345678901234567890") // 20 caratteri
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldContainKey(nameof(Product.Name));
    }

    [Fact]
    public void Should_ReturnNoError_When_NameIs19Characters()
    {
        var product = new ProductBuilder()
            .WithName("1234567890123456789") // 19 caratteri
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldNotContainKey(nameof(Product.Name));
    }

    [Fact]
    public void Should_ReturnError_When_NameExceeds20Characters()
    {
        var product = new ProductBuilder()
            .WithName("Nome prodotto molto lungo che supera il limite")
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldContainKey(nameof(Product.Name));
    }

    [Fact]
    public void Should_ReturnNoError_When_NameIsSingleCharacter()
    {
        var product = new ProductBuilder()
            .WithName("A")
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldNotContainKey(nameof(Product.Name));
    }

    [Fact]
    public void Should_ReturnError_When_PriceExceeds1000()
    {
        var product = new ProductBuilder()
            .WithPrice(1000.01m)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldContainKey(nameof(Product.Price));
    }

    [Fact]
    public void Should_ReturnNoError_When_PriceIsExactly100()
    {
        var product = new ProductBuilder()
            .WithPrice(100m)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldNotContainKey(nameof(Product.Price));
    }

    [Fact]
    public void Should_ReturnNoError_When_PriceIsZero()
    {
        var product = new ProductBuilder()
            .WithPrice(0m)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldNotContainKey(nameof(Product.Price));
    }

    [Fact]
    public void Should_ReturnNoError_When_PriceIsNegative()
    {
        var product = new ProductBuilder()
            .WithPrice(-10m)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldNotContainKey(nameof(Product.Price));
    }

    [Fact]
    public void Should_ReturnError_When_CategoryIdIsZero()
    {
        var product = new ProductBuilder()
            .WithCategoryId(0)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldContainKey(nameof(Product.CategoryId));
    }

    [Fact]
    public void Should_ReturnError_When_CategoryIdIsNegative()
    {
        var product = new ProductBuilder()
            .WithCategoryId(-1)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldContainKey(nameof(Product.CategoryId));
    }

    [Fact]
    public void Should_ReturnMultipleErrors_When_AllRulesViolated()
    {
        var product = new ProductBuilder()
            .WithName("Nome lunghissimo che supera il limite massimo")
            .WithPrice(9999m)
            .WithCategoryId(0)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.Count.ShouldBe(3);
        errors.ShouldContainKey(nameof(Product.Name));
        errors.ShouldContainKey(nameof(Product.Price));
        errors.ShouldContainKey(nameof(Product.CategoryId));
    }

    [Fact]
    public void Should_ReturnOnlyNameError_When_OnlyNameIsInvalid()
    {
        var product = new ProductBuilder()
            .WithName("Nome troppo lungo per passare")
            .WithPrice(100m)
            .WithCategoryId(1)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.Count.ShouldBe(1);
        errors.ShouldContainKey(nameof(Product.Name));
    }

    [Fact]
    public void Should_ReturnOnlyPriceError_When_OnlyPriceIsInvalid()
    {
        var product = new ProductBuilder()
            .WithName("Valido")
            .WithPrice(5000m)
            .WithCategoryId(1)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.Count.ShouldBe(1);
        errors.ShouldContainKey(nameof(Product.Price));
    }

    [Fact]
    public void Should_ReturnOnlyCategoryError_When_OnlyCategoryIsInvalid()
    {
        var product = new ProductBuilder()
            .WithName("Valido")
            .WithPrice(100m)
            .WithCategoryId(0)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.Count.ShouldBe(1);
        errors.ShouldContainKey(nameof(Product.CategoryId));
    }

    [Fact]
    public void Should_ContainMaxLengthInMessage_When_NameIsTooLong()
    {
        var product = new ProductBuilder()
            .WithName("Nome troppo lungo per passare")
            .Build();

        var errors = ProductValidator.Validate(product);

        errors[nameof(Product.Name)].ShouldContain(e => e.Contains("20"));
    }

    [Fact]
    public void Should_ContainMaxPriceInMessage_When_PriceIsTooHigh()
    {
        var product = new ProductBuilder()
            .WithPrice(2000m)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors[nameof(Product.Price)].ShouldContain(e => e.Contains("100"));
    }

    [Fact]
    public void Should_ReturnNoError_When_PriceIsAtDecimalBoundary()
    {
        var product = new ProductBuilder()
            .WithPrice(99.99m)
            .Build();

        var errors = ProductValidator.Validate(product);

        errors.ShouldNotContainKey(nameof(Product.Price));
    }
}
