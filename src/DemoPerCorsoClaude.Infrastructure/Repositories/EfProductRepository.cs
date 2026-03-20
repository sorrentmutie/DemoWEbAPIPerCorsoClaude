using DemoPerCorsoClaude.Domain.Models;
using DemoPerCorsoClaude.Domain.Services;
using DemoPerCorsoClaude.Infrastructure.Data;

namespace DemoPerCorsoClaude.Infrastructure.Repositories;

/// <summary>Implementazione del repository prodotti con Entity Framework.</summary>
public class EfProductRepository(AppDbContext context) : IProductRepository
{
    public IReadOnlyList<Product> GetAll() =>
        context.Products.ToList().AsReadOnly();

    public Product? GetById(int id) =>
        context.Products.Find(id);

    public Product Add(Product product)
    {
        context.Products.Add(product);
        context.SaveChanges();
        return product;
    }

    public Product? Update(int id, Product product)
    {
        var existing = context.Products.Find(id);
        if (existing is null)
            return null;

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Category = product.Category;
        existing.Stock = product.Stock;

        context.SaveChanges();
        return existing;
    }

    public bool Delete(int id)
    {
        var existing = context.Products.Find(id);
        if (existing is null)
            return false;

        context.Products.Remove(existing);
        context.SaveChanges();
        return true;
    }
}
