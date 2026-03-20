using DemoPerCorsoClaude.Domain.Models;

namespace DemoPerCorsoClaude.Tests.Builders;

public class CategoryBuilder
{
    private int _id;
    private string _name = "Categoria Test";
    private string? _description = "Descrizione categoria test";
    private List<Product> _products = [];

    public CategoryBuilder WithId(int id) { _id = id; return this; }
    public CategoryBuilder WithName(string name) { _name = name; return this; }
    public CategoryBuilder WithDescription(string? description) { _description = description; return this; }
    public CategoryBuilder WithProducts(List<Product> products) { _products = products; return this; }

    public Category Build() => new()
    {
        Id = _id,
        Name = _name,
        Description = _description,
        Products = _products
    };
}
