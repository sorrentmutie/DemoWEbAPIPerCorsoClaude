using System.Text.Json.Serialization;

namespace DemoPerCorsoClaude.Domain.Models;

/// <summary>Rappresenta una categoria di prodotti.</summary>
public record Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    [JsonIgnore]
    public List<Product> Products { get; set; } = [];
}
