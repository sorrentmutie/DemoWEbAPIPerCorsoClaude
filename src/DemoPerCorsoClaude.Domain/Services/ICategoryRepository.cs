using DemoPerCorsoClaude.Domain.Models;

namespace DemoPerCorsoClaude.Domain.Services;

/// <summary>Contratto per l'accesso ai dati delle categorie.</summary>
public interface ICategoryRepository
{
    IReadOnlyList<Category> GetAll();
    Category? GetById(int id);
    Category Add(Category category);
    Category? Update(int id, Category category);
    bool Delete(int id);
}
