using DemoPerCorsoClaude.Domain.Models;
using DemoPerCorsoClaude.Infrastructure.Data;
using DemoPerCorsoClaude.Infrastructure.Repositories;
using DemoPerCorsoClaude.Tests.Builders;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace DemoPerCorsoClaude.Tests.Infrastructure;

public class EfProductRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly EfProductRepository _repository;

    public EfProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        var category = new CategoryBuilder().WithId(1).WithName("Elettronica").Build();
        _context.Categories.Add(category);
        _context.Products.Add(new ProductBuilder().WithId(1).WithName("Mouse").WithCategoryId(1).Build());
        _context.Products.Add(new ProductBuilder().WithId(2).WithName("Tastiera").WithCategoryId(1).Build());
        _context.SaveChanges();

        _repository = new EfProductRepository(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public void Should_ReturnAllProducts_When_GetAllIsCalled()
    {
        var products = _repository.GetAll();

        products.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_IncludeCategory_When_GetAllIsCalled()
    {
        var products = _repository.GetAll();

        products.ShouldAllBe(p => p.Category != null);
    }

    [Fact]
    public void Should_ReturnProduct_When_GetByIdFindsIt()
    {
        var product = _repository.GetById(1);

        product.ShouldNotBeNull();
        product.Name.ShouldBe("Mouse");
    }

    [Fact]
    public void Should_IncludeCategory_When_GetByIdFindsIt()
    {
        var product = _repository.GetById(1);

        product!.Category.ShouldNotBeNull();
        product.Category!.Name.ShouldBe("Elettronica");
    }

    [Fact]
    public void Should_ReturnNull_When_GetByIdDoesNotFindIt()
    {
        var product = _repository.GetById(999);

        product.ShouldBeNull();
    }

    [Fact]
    public void Should_AddProductAndAssignId_When_AddIsCalled()
    {
        var newProduct = new ProductBuilder().WithName("Webcam").WithCategoryId(1).Build();

        var result = _repository.Add(newProduct);

        result.Id.ShouldBeGreaterThan(0);
        _context.Products.Count().ShouldBe(3);
    }

    [Fact]
    public void Should_LoadCategory_When_AddIsCalled()
    {
        var newProduct = new ProductBuilder().WithName("Webcam").WithCategoryId(1).Build();

        var result = _repository.Add(newProduct);

        result.Category.ShouldNotBeNull();
        result.Category!.Name.ShouldBe("Elettronica");
    }

    [Fact]
    public void Should_UpdateFields_When_ProductExists()
    {
        var updated = new ProductBuilder()
            .WithName("Mouse Pro")
            .WithPrice(59.99m)
            .WithCategoryId(1)
            .Build();

        var result = _repository.Update(1, updated);

        result.ShouldNotBeNull();
        result!.Name.ShouldBe("Mouse Pro");
        result.Price.ShouldBe(59.99m);
    }

    [Fact]
    public void Should_ReturnNull_When_UpdateProductDoesNotExist()
    {
        var updated = new ProductBuilder().Build();

        var result = _repository.Update(999, updated);

        result.ShouldBeNull();
    }

    [Fact]
    public void Should_LoadCategory_When_UpdateSucceeds()
    {
        var updated = new ProductBuilder().WithCategoryId(1).Build();

        var result = _repository.Update(1, updated);

        result!.Category.ShouldNotBeNull();
    }

    [Fact]
    public void Should_ReturnTrue_When_DeleteProductExists()
    {
        var result = _repository.Delete(1);

        result.ShouldBeTrue();
        _context.Products.Count().ShouldBe(1);
    }

    [Fact]
    public void Should_ReturnFalse_When_DeleteProductDoesNotExist()
    {
        var result = _repository.Delete(999);

        result.ShouldBeFalse();
    }
}
