using System.Text.Json.Serialization;

namespace DemoPerCorsoClaude.Domain.Models;

/// <summary>Rappresenta un prodotto nel catalogo.</summary>
public record Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    public int Stock { get; set; }
}
