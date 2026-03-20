using DemoPerCorsoClaude.Domain.Models;

namespace DemoPerCorsoClaude.Domain.Services;

/// <summary>Repository in-memory per i prodotti.</summary>
public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];
    private int _nextId = 1;

    public IReadOnlyList<Product> GetAll() => _products.AsReadOnly();

    public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public Product Add(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
        return product;
    }

    public Product? Update(int id, Product product)
    {
        var existing = GetById(id);
        if (existing is null)
            return null;

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Category = product.Category;
        existing.Stock = product.Stock;

        return existing;
    }

    public bool Delete(int id)
    {
        var existing = GetById(id);
        if (existing is null)
            return false;

        _products.Remove(existing);
        return true;
    }
}
