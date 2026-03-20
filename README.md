# DemoPerCorsoClaude

Web API demo in ASP.NET Core (.NET 10) con architettura a 3 layer e minimal API.

## Struttura del progetto

```
src/
  DemoPerCorsoClaude/                  # Web API (endpoint, Program.cs)
  DemoPerCorsoClaude.Domain/           # Entita, interfacce, validatori, DTO
  DemoPerCorsoClaude.Infrastructure/   # DbContext EF Core, repository

tests/
  DemoPerCorsoClaude.Tests/            # Unit test (xUnit + Shouldly)

docs/
  agent_docs/                          # Convenzioni per Claude Code
  (appunti-corso)/                     # Appunti del corso
```

## Requisiti

- .NET 10 SDK

## Build e avvio

```bash
dotnet build DemoPerCorsoClaude.slnx
dotnet run --project src/DemoPerCorsoClaude
```

L'app e in ascolto su `http://localhost:5082` e `https://localhost:7184`.

Swagger UI disponibile in Development su `/swagger`.

## Test

```bash
dotnet test
```

## API

| Risorsa     | Metodo | Route               | Descrizione                          |
|-------------|--------|---------------------|--------------------------------------|
| Categories  | GET    | /categories         | Lista categorie con conteggio prodotti |
| Categories  | GET    | /categories/{id}    | Categoria per id                     |
| Categories  | POST   | /categories         | Crea categoria                       |
| Categories  | PUT    | /categories/{id}    | Aggiorna categoria                   |
| Categories  | DELETE | /categories/{id}    | Elimina categoria (solo se vuota)    |
| Products    | GET    | /products           | Lista prodotti con categoria         |
| Products    | GET    | /products/{id}      | Prodotto per id                      |
| Products    | POST   | /products           | Crea prodotto                        |
| Products    | PUT    | /products/{id}      | Aggiorna prodotto                    |
| Products    | DELETE | /products/{id}      | Elimina prodotto                     |
| Weather     | GET    | /weatherforecast    | Previsioni meteo (demo)              |

## Regole di business

- Il prezzo di un prodotto non puo superare 1000 euro
- Il nome di un prodotto deve essere inferiore a 20 caratteri
- Una categoria non puo essere eliminata se ha prodotti associati

## Tecnologie

- ASP.NET Core 10 Minimal API
- Entity Framework Core (InMemory)
- Swashbuckle (Swagger UI)
- xUnit + Shouldly + NSubstitute
