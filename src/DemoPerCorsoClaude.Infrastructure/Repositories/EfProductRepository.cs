using DemoPerCorsoClaude.Domain.Models;
using DemoPerCorsoClaude.Domain.Services;
using DemoPerCorsoClaude.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoPerCorsoClaude.Infrastructure.Repositories;

/// <summary>Implementazione del repository prodotti con Entity Framework.</summary>
public class EfProductRepository(AppDbContext context) : IProductRepository
{
    public IReadOnlyList<Product> GetAll() =>
        context.Products.Include(p => p.Category).ToList().AsReadOnly();

    public Product? GetById(int id) =>
        context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);

    public Product Add(Product product)
    {
        context.Products.Add(product);
        context.SaveChanges();
        context.Entry(product).Reference(p => p.Category).Load();
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
        existing.CategoryId = product.CategoryId;
        existing.Stock = product.Stock;

        context.SaveChanges();
        context.Entry(existing).Reference(p => p.Category).Load();
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
