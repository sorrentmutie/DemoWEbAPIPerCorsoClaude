/// <summary>Represents a daily weather forecast.</summary>
/// <param name="Date">Forecast date.</param>
/// <param name="TemperatureC">Temperature in Celsius.</param>
/// <param name="Summary">Short weather description.</param>
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
