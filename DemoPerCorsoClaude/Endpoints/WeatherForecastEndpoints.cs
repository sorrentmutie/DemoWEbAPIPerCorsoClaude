internal static class WeatherForecastEndpoints
{
    private const int ForecastDays = 5;
    private const int MinTempC = -20;
    private const int MaxTempC = 55;

    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    internal static void MapWeatherForecastEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weatherforecast");
        group.MapGet("/", GetAll).WithName("GetWeatherForecast");
    }

    private static WeatherForecast[] GetAll() =>
        Enumerable.Range(1, ForecastDays)
            .Select(index => new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(MinTempC, MaxTempC),
                Summaries[Random.Shared.Next(Summaries.Length)]))
            .ToArray();
}
