using DemoPerCorsoClaude.Domain.Models;
using DemoPerCorsoClaude.Tests.Builders;
using Shouldly;

namespace DemoPerCorsoClaude.Tests.Domain;

public class ProductDtoTests
{
    [Fact]
    public void Should_MapAllFields_When_ProductHasCategory()
    {
        var category = new CategoryBuilder().WithId(1).WithName("Elettronica").Build();
        var product = new ProductBuilder()
            .WithId(5)
            .WithName("Mouse")
            .WithDescription("Mouse wireless")
            .WithPrice(29.99m)
            .WithCategory(category)
            .WithStock(100)
            .Build();

        var dto = ProductDto.FromProduct(product);

        dto.Id.ShouldBe(5);
        dto.Name.ShouldBe("Mouse");
        dto.Description.ShouldBe("Mouse wireless");
        dto.Price.ShouldBe(29.99m);
        dto.Category.Id.ShouldBe(1);
        dto.Category.Name.ShouldBe("Elettronica");
        dto.Stock.ShouldBe(100);
    }

    [Fact]
    public void Should_MapCategoryProductCount_When_CategoryHasProducts()
    {
        var category = new CategoryBuilder().WithId(1).WithName("Audio").Build();
        category.Products.Add(new ProductBuilder().WithId(1).Build());
        category.Products.Add(new ProductBuilder().WithId(2).Build());
        var product = new ProductBuilder().WithCategory(category).Build();

        var dto = ProductDto.FromProduct(product);

        dto.Category.ProductCount.ShouldBe(2);
    }

    [Fact]
    public void Should_MapNullDescription_When_DescriptionIsNull()
    {
        var category = new CategoryBuilder().WithId(1).Build();
        var product = new ProductBuilder()
            .WithDescription(null)
            .WithCategory(category)
            .Build();

        var dto = ProductDto.FromProduct(product);

        dto.Description.ShouldBeNull();
    }
}
