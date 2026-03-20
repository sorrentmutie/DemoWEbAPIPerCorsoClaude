# Principi SOLID — Progetto MES (.NET 10, Clean Architecture)

## S — Single Responsibility Principle

Ogni classe ha una sola ragione per cambiare.

ALWAYS SEPARATE endpoint registration, business logic e data access in classi distinte.

```csharp
// Corretto — responsabilità isolate
// Domain/Entities/ProductionOrder.cs
public record ProductionOrder(int Id, string ProductCode, DateTime DueDate);

// Application/Services/ProductionOrderService.cs
public class ProductionOrderService(IProductionOrderRepository repo)
{
    public Task<ProductionOrder[]> GetActiveAsync() => repo.GetActiveAsync();
}

// Api/Endpoints/ProductionOrderEndpoints.cs
public static class ProductionOrderEndpoints
{
    public static void Map(WebApplication app) =>
        app.MapGet("/orders", (ProductionOrderService svc) => svc.GetActiveAsync());
}

// Errato — tutto in Program.cs
app.MapGet("/orders", async (MesDbContext db) =>
{
    // query, validazione, mapping, logging tutto inline
    var orders = await db.Orders.Where(o => o.IsActive).ToListAsync();
    return orders.Select(o => new { o.Id, o.ProductCode });
});
```

## O — Open/Closed Principle

Le classi sono aperte all'estensione, chiuse alla modifica.

ALWAYS USE interfacce e DI per estendere il comportamento senza modificare codice esistente.

```csharp
// Corretto — nuovo calcolo aggiunto senza toccare il servizio
public interface ITemperatureConverter
{
    int Convert(int temperatureC);
}

public class FahrenheitConverter : ITemperatureConverter
{
    public int Convert(int temperatureC) => 32 + (int)(temperatureC / 0.5556);
}

public class KelvinConverter : ITemperatureConverter
{
    public int Convert(int temperatureC) => temperatureC + 273;
}

// Errato — if/switch che cresce ad ogni nuova unità
public int Convert(int tempC, string unit) => unit switch
{
    "F" => 32 + (int)(tempC / 0.5556),
    "K" => tempC + 273,
    // ogni nuova unità modifica questo metodo
};
```

## L — Liskov Substitution Principle

Ogni sottotipo deve essere sostituibile al proprio tipo base senza alterare il comportamento.

NEVER OVERRIDE metodi base con logica che viola il contratto del tipo padre.

```csharp
// Corretto — entrambi i repository rispettano il contratto
public interface IOrderRepository
{
    Task<IReadOnlyList<ProductionOrder>> GetActiveAsync();
}

public class SqlOrderRepository(MesDbContext db) : IOrderRepository
{
    public async Task<IReadOnlyList<ProductionOrder>> GetActiveAsync() =>
        await db.Orders.Where(o => o.IsActive).ToListAsync();
}

public class CachedOrderRepository(IOrderRepository inner, IMemoryCache cache) : IOrderRepository
{
    public async Task<IReadOnlyList<ProductionOrder>> GetActiveAsync() =>
        await cache.GetOrCreateAsync("active-orders", _ => inner.GetActiveAsync());
}

// Errato — il decorator cambia la semantica (filtra risultati inaspettatamente)
public class FilteredOrderRepository : IOrderRepository
{
    public async Task<IReadOnlyList<ProductionOrder>> GetActiveAsync() =>
        (await inner.GetActiveAsync()).Where(o => o.DueDate > DateTime.Now).ToList();
    // viola il contratto: "active" diventa "active e futuri"
}
```

## I — Interface Segregation Principle

Nessun client deve dipendere da metodi che non usa.

ALWAYS SPLIT interfacce grandi in contratti focalizzati. NEVER creare interfacce "god" con decine di metodi.

```csharp
// Corretto — interfacce focalizzate
public interface IOrderReader
{
    Task<ProductionOrder?> GetByIdAsync(int id);
}

public interface IOrderWriter
{
    Task CreateAsync(ProductionOrder order);
}

// Errato — interfaccia monolitica
public interface IOrderRepository
{
    Task<ProductionOrder?> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductionOrder>> GetAllAsync();
    Task CreateAsync(ProductionOrder order);
    Task UpdateAsync(ProductionOrder order);
    Task DeleteAsync(int id);
    Task<Report> GenerateReportAsync(); // non c'entra col repository
}
```

## D — Dependency Inversion Principle

I moduli di alto livello non dipendono da quelli di basso livello; entrambi dipendono da astrazioni.

ALWAYS USE `builder.Services.AddScoped<IService, Implementation>()` per iniettare dipendenze. NEVER istanziare servizi con `new` negli endpoint.

```csharp
// Corretto — il servizio dipende dall'astrazione, registrata in DI
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddScoped<ProductionOrderService>();

app.MapGet("/orders", (ProductionOrderService svc) => svc.GetActiveAsync());

// Errato — dipendenza concreta creata inline
app.MapGet("/orders", (MesDbContext db) =>
{
    var svc = new ProductionOrderService(new SqlOrderRepository(db));
    return svc.GetActiveAsync();
});
```
