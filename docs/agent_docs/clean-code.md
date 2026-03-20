# Clean Code Conventions — Progetto MES (.NET 10)

## 1. Naming — Classi e Record

ALWAYS USE PascalCase per classi, record e tipi.

```csharp
// Corretto
internal record ProductionOrder(int OrderId, string ProductCode);

// Errato
internal record production_order(int order_id, string product_code);
```

## 2. Naming — Metodi e Proprietà

ALWAYS USE PascalCase. USE verbi per i metodi, sostantivi per le proprietà.

```csharp
// Corretto
public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
app.MapGet("/orders", () => GetActiveOrders());

// Errato
public int get_temp_f() => 32 + (int)(TemperatureC / 0.5556);
```

## 3. Naming — Variabili locali e parametri

ALWAYS USE camelCase. NEVER USE abbreviazioni ambigue.

```csharp
// Corretto
var forecast = Enumerable.Range(1, 5).Select(index => ...);
var machineStatus = GetStatus(machineId);

// Errato
var f = Enumerable.Range(1, 5).Select(i => ...);
var ms = GetStatus(mId);
```

## 4. Magic Numbers

NEVER USE numeri letterali inline. USE costanti con nome descrittivo.

```csharp
// Corretto
const int ForecastDays = 5;
const int MinTemperatureC = -20;
const int MaxTemperatureC = 55;
var forecast = Enumerable.Range(1, ForecastDays).Select(index =>
    new WeatherForecast(
        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        Random.Shared.Next(MinTemperatureC, MaxTemperatureC),
        summaries[Random.Shared.Next(summaries.Length)]));

// Errato (dal codice attuale)
var forecast = Enumerable.Range(1, 5).Select(index =>
    new WeatherForecast(
        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        Random.Shared.Next(-20, 55),
        summaries[Random.Shared.Next(summaries.Length)]));
```

## 5. Lunghezza dei Metodi

ALWAYS KEEP lambda e metodi sotto le 15 righe. USE metodi estratti per logica complessa.

```csharp
// Corretto
app.MapGet("/weatherforecast", () => GenerateForecasts(summaries));

static WeatherForecast[] GenerateForecasts(string[] summaries) =>
    Enumerable.Range(1, ForecastDays)
        .Select(index => CreateForecast(index, summaries))
        .ToArray();

// Errato — logica complessa inline nella lambda del MapGet
app.MapGet("/weatherforecast", () =>
{
    // 20+ righe di logica, validazione, mapping, ecc.
});
```

## 6. XML Doc Comments

ALWAYS ADD `<summary>` sui tipi pubblici e sugli endpoint esposti.

```csharp
// Corretto
/// <summary>Previsione meteo giornaliera.</summary>
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);

// Errato — nessuna documentazione sul record esposto via API
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);
```

## 7. Leggibilità — Commenti

NEVER WRITE commenti ovvi che ripetono il codice. USE commenti solo per il "perché".

```csharp
// Corretto
// Conversione Celsius→Fahrenheit con formula NOAA
public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

// Errato
// Add services to the container.
builder.Services.AddOpenApi();
```

## 8. Leggibilità — Organizzazione

ALWAYS SEPARATE le sezioni di Program.cs: registrazione servizi, middleware, endpoint.
AVOID mischiare definizioni di tipo e configurazione nello stesso blocco.
