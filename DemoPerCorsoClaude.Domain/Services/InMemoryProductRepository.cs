using DemoPerCorsoClaude.Domain.Models;

namespace DemoPerCorsoClaude.Domain.Services;

/// <summary>Repository in-memory per i prodotti.</summary>
public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products;
    private int _nextId;

    public InMemoryProductRepository()
    {
        _products =
        [
            new Product { Id = 1, Name = "Laptop Pro 15", Description = "Laptop professionale 15 pollici", Price = 1299.99m, Category = "Elettronica", Stock = 25 },
            new Product { Id = 2, Name = "Mouse Wireless", Description = "Mouse ergonomico senza fili", Price = 29.99m, Category = "Elettronica", Stock = 150 },
            new Product { Id = 3, Name = "Tastiera Meccanica", Description = "Tastiera meccanica RGB", Price = 89.90m, Category = "Elettronica", Stock = 75 },
            new Product { Id = 4, Name = "Scrivania Regolabile", Description = "Scrivania sit-stand motorizzata", Price = 449.00m, Category = "Arredamento", Stock = 30 },
            new Product { Id = 5, Name = "Sedia Ergonomica", Description = "Sedia da ufficio con supporto lombare", Price = 349.00m, Category = "Arredamento", Stock = 20 },
            new Product { Id = 6, Name = "Monitor 27 4K", Description = "Monitor IPS 27 pollici 4K", Price = 399.99m, Category = "Elettronica", Stock = 40 },
            new Product { Id = 7, Name = "Cuffie Noise Cancelling", Description = "Cuffie bluetooth con cancellazione rumore", Price = 199.90m, Category = "Audio", Stock = 60 },
            new Product { Id = 8, Name = "Webcam HD", Description = "Webcam 1080p con microfono integrato", Price = 59.99m, Category = "Elettronica", Stock = 100 },
        ];
        _nextId = _products.Count + 1;
    }

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
