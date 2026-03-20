namespace DemoPerCorsoClaude.Endpoints;

/// <summary>Definisce gli endpoint per le previsioni meteo.</summary>
public static class WeatherEndpoints
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public static void MapWeatherEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/weatherforecast")
            .WithTags("Weather");

        group.MapGet("/", GetForecast).WithName("GetWeatherForecast");
    }

    static WeatherForecast[] GetForecast()
    {
        const int forecastDays = 5;
        const int minTemperatureC = -20;
        const int maxTemperatureC = 55;

        return Enumerable.Range(1, forecastDays).Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(minTemperatureC, maxTemperatureC),
                Summaries[Random.Shared.Next(Summaries.Length)]
            )).ToArray();
    }
}

/// <summary>Previsione meteo giornaliera.</summary>
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
