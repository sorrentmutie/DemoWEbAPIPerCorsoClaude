# Gestione degli Errori — Progetto MES (.NET 10)

## 1. DomainException vs eccezioni di sistema

ALWAYS USE `DomainException` per violazioni di regole di business (ordine duplicato, quantità negativa).
NEVER USE eccezioni di dominio per errori infrastrutturali (DB timeout, rete) — lascia che siano le eccezioni di sistema.

```csharp
// Domain/Exceptions/DomainException.cs
public class DomainException(string message, string code) : Exception(message)
{
    public string Code { get; } = code;
}

// Uso nel dominio
if (quantity <= 0)
    throw new DomainException("La quantità deve essere positiva.", "INVALID_QUANTITY");

// Errato — eccezione di dominio per un errore infrastrutturale
throw new DomainException("Database non raggiungibile.", "DB_ERROR");
```

## 2. Middleware globale per le eccezioni

ALWAYS USE un middleware globale per tradurre le eccezioni in risposte HTTP Problem Details.
NEVER USE try/catch negli endpoint per gestire errori già coperti dal middleware.

```csharp
// Api/Middleware/ExceptionMiddleware.cs
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await next(ctx);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Errore di dominio: {Code}", ex.Code);
            ctx.Response.StatusCode = 400;
            await ctx.Response.WriteAsJsonAsync(new { ex.Code, ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Errore non gestito");
            ctx.Response.StatusCode = 500;
            await ctx.Response.WriteAsJsonAsync(new { Code = "INTERNAL", Message = "Errore interno." });
        }
    }
}

// Registrazione in Program.cs
app.UseMiddleware<ExceptionMiddleware>();
```

## 3. Try/catch locale — solo quando serve

ALWAYS USE try/catch locale solo per recupero parziale (retry, fallback, cleanup).

```csharp
// Corretto — fallback locale con logging
app.MapPost("/orders/{id}/notify", async (int id, INotificationService svc, ILogger<Program> logger) =>
{
    try
    {
        await svc.SendAsync(id);
        return Results.Ok();
    }
    catch (HttpRequestException ex)
    {
        logger.LogWarning(ex, "Notifica fallita per ordine {OrderId}, proseguo", id);
        return Results.Accepted();
    }
});

// Errato — try/catch che duplica il middleware
app.MapGet("/orders", async (IOrderRepository repo) =>
{
    try { return Results.Ok(await repo.GetActiveAsync()); }
    catch (Exception ex) { return Results.Problem(ex.Message); } // ridondante
});
```

## 4. Pattern Result (alternativa alle eccezioni di dominio)

USE il pattern Result per flussi dove l'errore è un esito atteso, non un'eccezione.

```csharp
public record Result<T>(T? Value, string? Error = null)
{
    public bool IsSuccess => Error is null;
    public static Result<T> Ok(T value) => new(value);
    public static Result<T> Fail(string error) => new(default, error);
}

// Uso nel servizio
public Result<ProductionOrder> Create(string productCode, int qty) =>
    qty <= 0 ? Result<ProductionOrder>.Fail("Quantità non valida")
             : Result<ProductionOrder>.Ok(new ProductionOrder(0, productCode, DateTime.UtcNow));
```

## 5. Logging strutturato con ILogger

ALWAYS USE placeholder `{NomeCampo}` nei messaggi — NEVER concatenare stringhe.

```csharp
// Corretto
logger.LogInformation("Ordine {OrderId} creato per prodotto {ProductCode}", order.Id, order.ProductCode);

// Errato
logger.LogInformation($"Ordine {order.Id} creato per prodotto {order.ProductCode}");
```
