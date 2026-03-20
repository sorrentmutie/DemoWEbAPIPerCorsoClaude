# Testing Conventions — xUnit + FluentAssertions + NSubstitute

---

## Project Structure

ALWAYS place tests in a sibling project `DemoPerCorsoClaude.Tests`.
NEVER put test files inside the main project.

```
DemoWEbAPIPerCorsoClaude/
├── DemoPerCorsoClaude/          # production code
└── DemoPerCorsoClaude.Tests/    # test project (xUnit)
    ├── Domain/                  # pure domain logic tests
    ├── Endpoints/               # HTTP handler tests
    └── Builders/                # test data builders
```

---

## Test Naming

ALWAYS use `Should_[ExpectedBehaviour]_When_[Condition]`.
NEVER use generic names like `Test1` or `VerifyForecast`.

```csharp
// CORRECT
[Fact]
public void Should_ReturnTemperatureF_When_TemperatureCIsZero() { ... }

[Fact]
public void Should_Throw_When_ForecastDateIsInThePast() { ... }

// WRONG
[Fact]
public void TestWeatherForecast() { ... }
```

---

## Arrange / Act / Assert

ALWAYS separate the three phases with a blank line.
NEVER merge Act and Assert into one expression.

```csharp
// CORRECT
[Fact]
public void Should_ComputeTemperatureF_When_TemperatureCIsZero()
{
    // Arrange
    var forecast = new WeatherForecastBuilder().WithTemperatureC(0).Build();

    // Act
    var result = forecast.TemperatureF;

    // Assert
    result.Should().Be(32);
}

// WRONG — no separation, hard to diagnose failures
[Fact]
public void Test() =>
    new WeatherForecast(DateOnly.MinValue, 0, null).TemperatureF.Should().Be(32);
```

---

## Builder Pattern

ALWAYS use a Builder to construct domain entities in tests.
NEVER call constructors directly with raw literals across multiple tests.

```csharp
// CORRECT
internal sealed class WeatherForecastBuilder
{
    private DateOnly _date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
    private int _tempC = 20;
    private string? _summary = "Mild";

    public WeatherForecastBuilder WithTemperatureC(int tempC)
        { _tempC = tempC; return this; }

    public WeatherForecastBuilder WithDate(DateOnly date)
        { _date = date; return this; }

    public WeatherForecast Build() => new(_date, _tempC, _summary);
}

// WRONG — brittle, duplicated across every test file
var forecast = new WeatherForecast(DateOnly.MinValue, -20, null);
```

---

## NSubstitute vs Real Objects

USE real objects for pure domain types (`WeatherForecast`, value objects).
USE `NSubstitute` only for dependencies that cross a boundary (I/O, DI services).
NEVER mock types you own and can instantiate cheaply.

```csharp
// CORRECT — real object for domain, substitute for provider
var provider = Substitute.For<IForecastProvider>();
provider.Generate(5).Returns(new WeatherForecastBuilder().BuildArray(5));

// CORRECT — real object, no substitute needed
var forecast = new WeatherForecastBuilder().WithTemperatureC(100).Build();
forecast.TemperatureF.Should().Be(212);

// WRONG — mocking a simple record you can just construct
var forecast = Substitute.For<WeatherForecast>(...);
```

---

## FluentAssertions Rules

ALWAYS prefer semantic assertions over `Assert.Equal`.
USE `.Should().BeEquivalentTo()` for record/object graph comparison.

```csharp
// CORRECT
forecasts.Should().HaveCount(5);
forecasts.Should().AllSatisfy(f => f.TemperatureF.Should().BeGreaterThan(-100));

// WRONG
Assert.Equal(5, forecasts.Length);
```
