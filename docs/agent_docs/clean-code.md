# Clean Code Conventions — .NET 10 Minimal API

## Naming

**Classes / Records**
ALWAYS use `PascalCase` nouns.
```csharp
// CORRECT
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);

// WRONG
internal record wforecast(DateOnly d, int tc, string? s);
```

**Methods / Endpoints**
ALWAYS name endpoint handlers with a verb + noun. USE `.WithName()` with `PascalCase`.
```csharp
// CORRECT
app.MapGet("/weatherforecast", GetWeatherForecast).WithName("GetWeatherForecast");

// WRONG
app.MapGet("/wf", () => { ... }).WithName("ep1");
```

**Variables / Parameters**
ALWAYS use `camelCase`. AVOID single-letter names except loop counters.
```csharp
// CORRECT
var forecast = Enumerable.Range(1, 5).Select(...);

// WRONG
var f = Enumerable.Range(1, 5).Select(...);
```

## Method Length

NEVER let an inline lambda exceed 10 lines. EXTRACT to a named method.
```csharp
// CORRECT
app.MapGet("/weatherforecast", BuildForecast).WithName("GetWeatherForecast");

static WeatherForecast[] BuildForecast() =>
    Enumerable.Range(1, 5)
              .Select(i => new WeatherForecast(
                  DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                  Random.Shared.Next(-20, 55),
                  Summaries[Random.Shared.Next(Summaries.Length)]))
              .ToArray();

// WRONG — logic buried inside MapGet lambda
app.MapGet("/weatherforecast", () => { /* 20 lines */ });
```

## Magic Numbers

NEVER embed unexplained literals. USE named constants or top-level `const`.
```csharp
// CORRECT
const int ForecastDays = 5;
const int MinTempC = -20;
const int MaxTempC = 55;

// WRONG
Enumerable.Range(1, 5) ...  Random.Shared.Next(-20, 55)
```

## Comments

AVOID redundant scaffold comments (`// Add services to the container.`).
USE XML doc only on public/internal types and members.
```csharp
// CORRECT
/// <summary>Represents a daily weather forecast.</summary>
/// <param name="TemperatureC">Temperature in Celsius.</param>
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);

// WRONG
// This is the weather forecast record
internal record WeatherForecast(...);
```

## Readability

USE `=>` expression body for single-expression computed properties.
NEVER compute derived values inside the constructor or record body.
```csharp
// CORRECT
public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

// WRONG
public int TemperatureF { get { return 32 + (int)(TemperatureC / 0.5556); } }
```
