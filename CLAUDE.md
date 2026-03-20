# CLAUDE.md

Questo file fornisce indicazioni a Claude Code (claude.ai/code) per lavorare con il codice di questo repository.

## Panoramica del Progetto

Progetto ASP.NET Core Web API (demo per corso) con target **.NET 10**. Usa lo stile minimal API (nessun controller). Attualmente ha un singolo endpoint `/weatherforecast`.

## Comandi di Build e Avvio

```bash
# Ripristino, compilazione, avvio
dotnet build DemoPerCorsoClaude/DemoPerCorsoClaude.csproj
dotnet run --project DemoPerCorsoClaude

# Avvio in modalità watch (hot reload)
dotnet watch --project DemoPerCorsoClaude
```

L'app è in ascolto su `http://localhost:5082` (HTTP) e `https://localhost:7184` (HTTPS).

## Struttura della Solution

- `DemoPerCorsoClaude.slnx` — file di soluzione (formato slnx basato su XML)
- `DemoPerCorsoClaude/` — l'unico progetto Web API
  - `Program.cs` — entry point dell'applicazione con tutte le definizioni degli endpoint (stile minimal API, nessuna classe Startup)

## Dettagli Importanti

- OpenAPI è abilitato in Development tramite `AddOpenApi()` / `MapOpenApi()`
- Non esiste ancora un progetto di test
- Nessun controller — tutti gli endpoint sono definiti inline in `Program.cs` con `app.MapGet()` / `app.MapPost()` ecc.
- Nullable reference types e implicit usings sono abilitati

## Documenti di Riferimento

@import docs/agent_docs/clean-code.md
Carica quando scrivi o revisioni codice C# (naming, magic numbers, leggibilità).

@import docs/agent_docs/solid-principles.md
Carica quando progetti nuove classi, servizi o modifichi l'architettura.

@import docs/agent_docs/error-handling.md
Carica quando implementi gestione errori, eccezioni o logging.

@import docs/agent_docs/testing-conventions.md
Carica quando scrivi, revisioni o correggi test.
