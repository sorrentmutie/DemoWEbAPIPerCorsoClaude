using DemoPerCorsoClaude.Domain.Models;
using DemoPerCorsoClaude.Domain.Services;
using DemoPerCorsoClaude.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoPerCorsoClaude.Infrastructure.Repositories;

/// <summary>Implementazione del repository categorie con Entity Framework.</summary>
public class EfCategoryRepository(AppDbContext context) : ICategoryRepository
{
    public IReadOnlyList<Category> GetAll() =>
        context.Categories.Include(c => c.Products).ToList().AsReadOnly();

    public Category? GetById(int id) =>
        context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);

    public Category Add(Category category)
    {
        context.Categories.Add(category);
        context.SaveChanges();
        return category;
    }

    public Category? Update(int id, Category category)
    {
        var existing = context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return null;

        existing.Name = category.Name;
        existing.Description = category.Description;

        context.SaveChanges();
        return existing;
    }

    public bool Delete(int id)
    {
        var existing = context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return false;

        if (existing.Products.Count > 0)
            throw new InvalidOperationException("Impossibile eliminare una categoria con prodotti associati.");

        context.Categories.Remove(existing);
        context.SaveChanges();
        return true;
    }
}
