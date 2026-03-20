using DemoPerCorsoClaude.Infrastructure.Data;
using DemoPerCorsoClaude.Infrastructure.Repositories;
using DemoPerCorsoClaude.Tests.Builders;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace DemoPerCorsoClaude.Tests.Infrastructure;

public class EfCategoryRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly EfCategoryRepository _repository;

    public EfCategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        var category = new CategoryBuilder().WithId(1).WithName("Elettronica").Build();
        _context.Categories.Add(category);
        _context.Categories.Add(new CategoryBuilder().WithId(2).WithName("Vuota").Build());
        _context.Products.Add(new ProductBuilder().WithId(1).WithName("Mouse").WithCategoryId(1).Build());
        _context.SaveChanges();

        _repository = new EfCategoryRepository(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public void Should_ReturnAllCategories_When_GetAllIsCalled()
    {
        var categories = _repository.GetAll();

        categories.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_IncludeProducts_When_GetAllIsCalled()
    {
        var categories = _repository.GetAll();

        var elettronica = categories.First(c => c.Name == "Elettronica");
        elettronica.Products.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_ReturnCategory_When_GetByIdFindsIt()
    {
        var category = _repository.GetById(1);

        category.ShouldNotBeNull();
        category!.Name.ShouldBe("Elettronica");
    }

    [Fact]
    public void Should_IncludeProducts_When_GetByIdFindsIt()
    {
        var category = _repository.GetById(1);

        category!.Products.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_ReturnNull_When_GetByIdDoesNotFindIt()
    {
        var category = _repository.GetById(999);

        category.ShouldBeNull();
    }

    [Fact]
    public void Should_AddCategory_When_AddIsCalled()
    {
        var newCategory = new CategoryBuilder().WithName("Audio").Build();

        var result = _repository.Add(newCategory);

        result.Id.ShouldBeGreaterThan(0);
        _context.Categories.Count().ShouldBe(3);
    }

    [Fact]
    public void Should_UpdateFields_When_CategoryExists()
    {
        var updated = new CategoryBuilder().WithName("Hi-Tech").WithDescription("Nuova descrizione").Build();

        var result = _repository.Update(1, updated);

        result.ShouldNotBeNull();
        result!.Name.ShouldBe("Hi-Tech");
        result.Description.ShouldBe("Nuova descrizione");
    }

    [Fact]
    public void Should_ReturnNull_When_UpdateCategoryDoesNotExist()
    {
        var updated = new CategoryBuilder().Build();

        var result = _repository.Update(999, updated);

        result.ShouldBeNull();
    }

    [Fact]
    public void Should_IncludeProducts_When_UpdateSucceeds()
    {
        var updated = new CategoryBuilder().WithName("Aggiornata").Build();

        var result = _repository.Update(1, updated);

        result!.Products.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_ReturnTrue_When_DeleteEmptyCategory()
    {
        var result = _repository.Delete(2);

        result.ShouldBeTrue();
        _context.Categories.Count().ShouldBe(1);
    }

    [Fact]
    public void Should_ThrowException_When_DeleteCategoryWithProducts()
    {
        var ex = Should.Throw<InvalidOperationException>(() => _repository.Delete(1));

        ex.Message.ShouldContain("prodotti associati");
    }

    [Fact]
    public void Should_ReturnFalse_When_DeleteCategoryDoesNotExist()
    {
        var result = _repository.Delete(999);

        result.ShouldBeFalse();
    }
}
