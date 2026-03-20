using DemoPerCorsoClaude.Domain.Models;

namespace DemoPerCorsoClaude.Tests.Builders;

public class ProductBuilder
{
    private int _id;
    private string _name = "Prodotto Test";
    private string? _description = "Descrizione test";
    private decimal _price = 49.99m;
    private int _categoryId = 1;
    private Category? _category;
    private int _stock = 10;

    public ProductBuilder WithId(int id) { _id = id; return this; }
    public ProductBuilder WithName(string name) { _name = name; return this; }
    public ProductBuilder WithDescription(string? description) { _description = description; return this; }
    public ProductBuilder WithPrice(decimal price) { _price = price; return this; }
    public ProductBuilder WithCategoryId(int categoryId) { _categoryId = categoryId; return this; }
    public ProductBuilder WithCategory(Category category) { _category = category; _categoryId = category.Id; return this; }
    public ProductBuilder WithStock(int stock) { _stock = stock; return this; }

    public Product Build() => new()
    {
        Id = _id,
        Name = _name,
        Description = _description,
        Price = _price,
        CategoryId = _categoryId,
        Category = _category,
        Stock = _stock
    };
}
