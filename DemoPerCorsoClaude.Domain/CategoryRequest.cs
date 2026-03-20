namespace DemoPerCorsoClaude.Domain;

/// <summary>Payload for creating or updating a category.</summary>
/// <param name="Name">Display name of the category.</param>
public record CategoryRequest(string Name);
