using DemoPerCorsoClaude.Domain.Models;
using DemoPerCorsoClaude.Tests.Builders;
using Shouldly;

namespace DemoPerCorsoClaude.Tests.Domain;

public class CategoryDtoTests
{
    [Fact]
    public void Should_MapAllFields_When_CategoryHasNoProducts()
    {
        var category = new CategoryBuilder()
            .WithId(1)
            .WithName("Audio")
            .WithDescription("Accessori audio")
            .Build();

        var dto = CategoryDto.FromCategory(category);

        dto.Id.ShouldBe(1);
        dto.Name.ShouldBe("Audio");
        dto.Description.ShouldBe("Accessori audio");
        dto.ProductCount.ShouldBe(0);
    }

    [Fact]
    public void Should_CountProducts_When_CategoryHasProducts()
    {
        var category = new CategoryBuilder().WithId(1).WithName("Elettronica").Build();
        category.Products.Add(new ProductBuilder().WithId(1).Build());
        category.Products.Add(new ProductBuilder().WithId(2).Build());
        category.Products.Add(new ProductBuilder().WithId(3).Build());

        var dto = CategoryDto.FromCategory(category);

        dto.ProductCount.ShouldBe(3);
    }

    [Fact]
    public void Should_MapNullDescription_When_DescriptionIsNull()
    {
        var category = new CategoryBuilder()
            .WithDescription(null)
            .Build();

        var dto = CategoryDto.FromCategory(category);

        dto.Description.ShouldBeNull();
    }
}
