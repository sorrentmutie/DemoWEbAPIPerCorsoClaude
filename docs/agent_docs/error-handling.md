# Error Handling — .NET 10 Minimal API

> Note: the project currently has no `DomainException.cs` or controllers.
> Rules below define the target pattern to adopt when the codebase grows.

---

## DomainException vs System Exceptions

ALWAYS throw `DomainException` (or a subclass) for business rule violations.
NEVER use `ArgumentException` / `InvalidOperationException` for domain logic.

```csharp
// CORRECT — domain rule violation
internal sealed class DomainException(string message) : Exception(message);

// throw when a forecast date is in the past
if (date < DateOnly.FromDateTime(DateTime.Now))
    throw new DomainException($"Forecast date {date} cannot be in the past.");

// WRONG — leaks infrastructure semantics into domain
throw new InvalidOperationException("Date is in the past.");
```

---

## Global Exception Middleware

ALWAYS handle exceptions centrally with `app.UseExceptionHandler`.
NEVER add `try/catch` in every endpoint handler for cross-cutting concerns.

```csharp
// CORRECT — in Program.cs, before endpoint mapping
app.UseExceptionHandler(errApp => errApp.Run(async ctx =>
{
    var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
    var (status, message) = ex switch
    {
        DomainException d => (StatusCodes.Status422UnprocessableEntity, d.Message),
        _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
    };
    ctx.Response.StatusCode = status;
    await ctx.Response.WriteAsJsonAsync(new { error = message });
}));

// WRONG — duplicated per endpoint
app.MapGet("/weatherforecast", () =>
{
    try { return BuildForecast(); }
    catch (Exception ex) { return Results.Problem(ex.Message); }
});
```

---

## Result Pattern

USE `Result<T>` for operations where failure is an expected outcome (not exceptional).
AVOID it for truly unexpected failures (I/O errors, null refs) — those remain exceptions.

```csharp
// CORRECT — validation with known failure modes
readonly record struct Result<T>(T? Value, string? Error)
{
    public bool IsSuccess => Error is null;
    public static Result<T> Ok(T value) => new(value, null);
    public static Result<T> Fail(string error) => new(default, error);
}

app.MapPost("/weatherforecast", (ForecastRequest req) =>
{
    var result = ForecastValidator.Validate(req);
    return result.IsSuccess
        ? Results.Ok(result.Value)
        : Results.UnprocessableEntity(new { error = result.Error });
});
```

---

## Structured Logging with ILogger

ALWAYS use structured logging with named placeholders. NEVER use string interpolation.

```csharp
// CORRECT
app.MapGet("/weatherforecast", (ILogger<Program> logger) =>
{
    logger.LogInformation("Generating forecast for {Days} days", ForecastDays);
    var forecast = BuildForecast();
    logger.LogDebug("Forecast generated: {Count} items", forecast.Length);
    return forecast;
});

// WRONG — breaks structured log sinks (Seq, Application Insights)
logger.LogInformation($"Generating forecast for {ForecastDays} days");
```

ALWAYS log exceptions with `LogError(ex, ...)` — the middleware does this once globally.
NEVER swallow exceptions with an empty `catch`.
