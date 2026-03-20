# DemoPerCorsoClaude

Demo REST API realizzata con **ASP.NET Core 10 Minimal API** per il corso AzeroUnoProgramming.

## Avvio

```bash
# HTTP (porta 5082)
dotnet run --project DemoPerCorsoClaude --launch-profile http

# HTTPS (porta 7184)
dotnet run --project DemoPerCorsoClaude --launch-profile https
```

Swagger UI disponibile su `http://localhost:5082/swagger` (solo in Development).

---

## Struttura della soluzione

| Progetto | Tipo | Responsabilità |
|---|---|---|
| `DemoPerCorsoClaude` | Web API | Endpoint, DTO, mapping |
| `DemoPerCorsoClaude.Domain` | Class library | Entità, request model, validatori |
| `DemoPerCorsoClaude.Infrastructure` | Class library | DbContext EF Core (in-memory), seeding |
| `DemoPerCorsoClaude.Tests` | xUnit | Unit test dei validatori di dominio |

---

## Endpoint

### Categorie — `/categories`

| Metodo | URL | Descrizione | Body | Risposta |
|---|---|---|---|---|
| GET | `/categories` | Lista tutte le categorie | — | `200 CategoryDto[]` |
| POST | `/categories` | Crea una categoria | `CategoryRequest` | `201 CategoryDto` |
| PUT | `/categories/{id}` | Aggiorna una categoria | `CategoryRequest` | `200 CategoryDto` / `404` |
| DELETE | `/categories/{id}` | Elimina una categoria | — | `204` / `404` / `409` |

> `DELETE` restituisce `409 Conflict` se alla categoria sono associati prodotti.

### Prodotti — `/products`

| Metodo | URL | Descrizione | Body | Risposta |
|---|---|---|---|---|
| GET | `/products` | Lista tutti i prodotti | — | `200 ProductDto[]` |
| POST | `/products` | Crea un prodotto | `ProductRequest` | `201 ProductDto` |
| PUT | `/products/{id}` | Aggiorna un prodotto | `ProductRequest` | `200 ProductDto` / `404` |
| DELETE | `/products/{id}` | Elimina un prodotto | — | `204` / `404` |

### Meteo — `/weatherforecast`

| Metodo | URL | Descrizione | Risposta |
|---|---|---|---|
| GET | `/weatherforecast` | 5 previsioni casuali | `200 WeatherForecast[]` |

---

## Modelli

### `CategoryRequest` (body POST / PUT)

```json
{
  "name": "Periferiche"
}
```

### `CategoryDto` (risposta)

```json
{
  "id": 1,
  "name": "Periferiche",
  "productCount": 2
}
```

### `ProductRequest` (body POST / PUT)

```json
{
  "name": "Mouse wireless",
  "description": "Sensore ottico 25600 DPI",
  "price": 59.90,
  "stock": 78,
  "categoryId": 1
}
```

### `ProductDto` (risposta)

```json
{
  "id": 3,
  "name": "Mouse wireless",
  "description": "Sensore ottico 25600 DPI",
  "price": 59.90,
  "stock": 78,
  "categoryId": 1,
  "categoryName": "Periferiche"
}
```

---

## Regole di business

### Prodotto
| Campo | Regola |
|---|---|
| `Name` | Non vuoto/whitespace; lunghezza < 20 caratteri |
| `Price` | Compreso tra 0 e 1000 EUR (estremi inclusi) |
| `CategoryId` | Deve essere > 0 e riferirsi a una categoria esistente |

### Categoria
| Campo | Regola |
|---|---|
| `Name` | Non vuoto/whitespace; lunghezza < 50 caratteri |

Le violazioni restituiscono `400 Bad Request` con un array di messaggi d'errore:

```json
["Il nome del prodotto deve essere inferiore a 20 caratteri.",
 "Il prezzo del prodotto non può superare 1000 EUR."]
```

---

## Test

```bash
dotnet test
```

I test coprono `ProductValidator` e `CategoryValidator` con tutti gli edge case:
boundary sul nome, prezzo negativo / oltre il massimo, `CategoryId` non valido, input null/whitespace, violazioni multiple.
