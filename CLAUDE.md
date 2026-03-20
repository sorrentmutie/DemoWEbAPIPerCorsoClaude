# CLAUDE.md

Questo file fornisce indicazioni a Claude Code (claude.ai/code) per lavorare con il codice di questo repository.

## Panoramica del Progetto

Progetto ASP.NET Core Web API (demo per corso) con target **.NET 10**. Usa lo stile minimal API (nessun controller) con architettura a 3 layer.

## Comandi di Build e Avvio

```bash
# Compilazione della solution
dotnet build DemoPerCorsoClaude.slnx

# Avvio
dotnet run --project src/DemoPerCorsoClaude

# Avvio in modalità watch (hot reload)
dotnet watch --project src/DemoPerCorsoClaude
```

L'app è in ascolto su `http://localhost:5082` (HTTP) e `https://localhost:7184` (HTTPS).

## Struttura della Solution

- `DemoPerCorsoClaude.slnx` — file di soluzione (formato slnx basato su XML)
- `src/DemoPerCorsoClaude/` — progetto Web API (entry point, endpoint)
  - `Program.cs` — configurazione servizi e mapping endpoint
  - `Endpoints/` — classi statiche con MapGroup per ogni risorsa
- `src/DemoPerCorsoClaude.Domain/` — entità, interfacce, validatori (nessuna dipendenza esterna)
- `src/DemoPerCorsoClaude.Infrastructure/` — DbContext EF Core, implementazioni repository

## Dettagli Importanti

- OpenAPI + Swagger UI abilitati in Development
- Persistenza con EF Core InMemory, isolata nel progetto Infrastructure
- Nessun controller — endpoint definiti in classi statiche con MapGroup e metodi handler
- Nullable reference types e implicit usings sono abilitati
- Non esiste ancora un progetto di test

## Documenti di Riferimento

@import docs/agent_docs/clean-code.md
Carica quando scrivi o revisioni codice C# (naming, magic numbers, leggibilità).

@import docs/agent_docs/solid-principles.md
Carica quando progetti nuove classi, servizi o modifichi l'architettura.

@import docs/agent_docs/error-handling.md
Carica quando implementi gestione errori, eccezioni o logging.

@import docs/agent_docs/testing-conventions.md
Carica quando scrivi, revisioni o correggi test.
