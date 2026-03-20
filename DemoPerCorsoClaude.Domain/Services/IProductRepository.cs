using DemoPerCorsoClaude.Domain.Models;

namespace DemoPerCorsoClaude.Domain.Services;

/// <summary>Contratto per l'accesso ai dati dei prodotti.</summary>
public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? GetById(int id);
    Product Add(Product product);
    Product? Update(int id, Product product);
    bool Delete(int id);
}
