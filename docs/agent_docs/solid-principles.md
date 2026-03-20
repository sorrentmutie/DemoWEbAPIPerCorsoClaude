# SOLID Principles — .NET 10 Minimal API

---

## S — Single Responsibility Principle
**Definition:** Every class/record has exactly one reason to change.

NEVER put temperature conversion logic, data generation, and HTTP routing
in the same unit. ALWAYS split each concern into its own type or method.

```csharp
// CORRECT — WeatherForecast only models data + unit conversion
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// Separate handler owns generation logic
static WeatherForecast[] BuildForecast(IForecastProvider provider) =>
    provider.Generate(ForecastDays);

// WRONG — record responsible for both data AND generation
internal record WeatherForecast(...)
{
    public static WeatherForecast[] GenerateRandom(int days) { ... }
}
```

---

## O — Open/Closed Principle
**Definition:** Open for extension, closed for modification.

NEVER add `if/switch` blocks to vary forecast behaviour. ALWAYS extend
via a new `IForecastProvider` implementation registered in DI.

```csharp
// CORRECT
interface IForecastProvider { WeatherForecast[] Generate(int days); }

class RandomForecastProvider : IForecastProvider { ... }
class HistoricalForecastProvider : IForecastProvider { ... }

builder.Services.AddScoped<IForecastProvider, RandomForecastProvider>();

// WRONG
static WeatherForecast[] Build(string mode) =>
    mode == "random" ? GenerateRandom() : LoadHistorical(); // grows forever
```

---

## L — Liskov Substitution Principle
**Definition:** A subtype must be usable wherever its base type is expected.

ALWAYS ensure every `IForecastProvider` implementation returns a non-null,
non-empty array of valid `WeatherForecast` items. NEVER throw from `Generate`.

```csharp
// CORRECT — both implementations honour the contract
class StubForecastProvider : IForecastProvider
{
    public WeatherForecast[] Generate(int days) =>
        Enumerable.Range(1, days)
                  .Select(i => new WeatherForecast(
                      DateOnly.FromDateTime(DateTime.Now.AddDays(i)), 20, "Mild"))
                  .ToArray();
}

// WRONG — breaks caller assumptions
class BrokenProvider : IForecastProvider
{
    public WeatherForecast[] Generate(int days) =>
        throw new NotImplementedException(); // caller can't substitute safely
}
```

---

## I — Interface Segregation Principle
**Definition:** Clients must not depend on methods they do not use.

NEVER create a fat `IWeatherService` with unrelated methods. ALWAYS keep
interfaces focused on a single capability.

```csharp
// CORRECT — endpoint only needs Generate
interface IForecastProvider { WeatherForecast[] Generate(int days); }

// WRONG — endpoint forced to depend on persistence it never calls
interface IWeatherService
{
    WeatherForecast[] Generate(int days);
    void Save(WeatherForecast forecast);
    void Delete(Guid id);
}
```

---

## D — Dependency Inversion Principle
**Definition:** High-level modules depend on abstractions, not concretions.

ALWAYS inject `IForecastProvider` into endpoint handlers via DI.
NEVER instantiate `RandomForecastProvider` directly inside the handler.

```csharp
// CORRECT
app.MapGet("/weatherforecast", (IForecastProvider provider) =>
    provider.Generate(ForecastDays))
   .WithName("GetWeatherForecast");

builder.Services.AddScoped<IForecastProvider, RandomForecastProvider>();

// WRONG — high-level routing code depends on a concrete class
app.MapGet("/weatherforecast", () =>
    new RandomForecastProvider().Generate(5));
```
