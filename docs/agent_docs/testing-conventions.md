# Testing Conventions — Progetto MES (.NET 10)

Stack: **xUnit** + **FluentAssertions** + **NSubstitute**

## 1. Struttura dei test — Arrange/Act/Assert

ALWAYS SEPARATE le tre sezioni con un commento o una riga vuota.

```csharp
[Fact]
public void Should_CalculateFahrenheit_When_CelsiusIsZero()
{
    // Arrange
    var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 0, "Freezing");

    // Act
    var result = forecast.TemperatureF;

    // Assert
    result.Should().Be(32);
}
```

## 2. Naming dei test

ALWAYS USE il pattern `Should_[Expected]_When_[Condition]`.

```csharp
// Corretto
public void Should_ReturnActiveOrders_When_StatusIsActive()
public void Should_ThrowDomainException_When_QuantityIsNegative()
public void Should_ReturnEmpty_When_NoOrdersExist()

// Errato
public void TestGetOrders()
public void OrderServiceTest1()
public void ItWorks()
```

## 3. Builder pattern per entità di dominio

ALWAYS USE un Builder per costruire entità nei test. NEVER ripetere costruttori complessi in ogni test.

```csharp
// Tests/Builders/ProductionOrderBuilder.cs
public class ProductionOrderBuilder
{
    private int _id = 1;
    private string _productCode = "PROD-001";
    private DateTime _dueDate = new(2026, 1, 15);
    private int _quantity = 100;

    public ProductionOrderBuilder WithId(int id) { _id = id; return this; }
    public ProductionOrderBuilder WithQuantity(int qty) { _quantity = qty; return this; }
    public ProductionOrderBuilder WithProductCode(string code) { _productCode = code; return this; }

    public ProductionOrder Build() => new(_id, _productCode, _dueDate, _quantity);
}

// Uso nel test
var order = new ProductionOrderBuilder().WithQuantity(0).Build();
```

## 4. NSubstitute vs oggetti reali

USE **NSubstitute** per dipendenze esterne (repository, servizi HTTP, notifiche).
USE **oggetti reali** per logica di dominio, value object e record.

```csharp
// NSubstitute — dipendenza infrastrutturale
var repo = Substitute.For<IOrderRepository>();
repo.GetActiveAsync().Returns(new[] { order });

var svc = new ProductionOrderService(repo);
var result = await svc.GetActiveAsync();

result.Should().ContainSingle();
repo.Received(1).GetActiveAsync();

// Oggetto reale — logica di dominio pura, nessun mock
var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 25, "Warm");
forecast.TemperatureF.Should().BeGreaterThan(70);
```

## 5. Dove salvare i test

ALWAYS USE un progetto separato con naming `<Progetto>.Tests`.

```
DemoPerCorsoClaude/                  # codice sorgente
DemoPerCorsoClaude.Tests/            # progetto test
├── Domain/                          # test su entità e value object
├── Application/                     # test su servizi applicativi
├── Api/                             # test su endpoint (integration)
└── Builders/                        # builder condivisi
```

NEVER mischiare codice di test e codice di produzione nello stesso progetto.

## 6. Un assert per concetto

AVOID test con decine di assert non correlati. USE più assert solo se verificano lo stesso concetto.

```csharp
// Corretto — più assert sullo stesso risultato
result.Should().NotBeNull();
result.ProductCode.Should().Be("PROD-001");
result.Quantity.Should().Be(100);

// Errato — assert su concetti diversi nello stesso test
result.Should().NotBeNull();
repo.Received(1).SaveAsync(Arg.Any<ProductionOrder>());  // verifica separata
logger.Received().LogInformation(Arg.Any<string>());      // altro concetto
```
