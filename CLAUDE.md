# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ASP.NET Core Web API project (demo/course project) targeting **.NET 10**. Uses the minimal API style (no controllers). Currently has a single `/weatherforecast` endpoint.

## Build & Run Commands

```bash
# Restore, build, run
dotnet build DemoPerCorsoClaude/DemoPerCorsoClaude.csproj
dotnet run --project DemoPerCorsoClaude

# Run in watch mode (hot reload)
dotnet watch --project DemoPerCorsoClaude
```

The app listens on `http://localhost:5082` (HTTP) and `https://localhost:7184` (HTTPS).

## Solution Structure

- `DemoPerCorsoClaude.slnx` — solution file (XML-based slnx format)
- `DemoPerCorsoClaude/` — the single Web API project
  - `Program.cs` — application entry point with all endpoint definitions (minimal API style, no Startup class)

## Key Details

- OpenAPI is enabled in Development via `AddOpenApi()` / `MapOpenApi()`
- No test project exists yet
- No controllers — all endpoints are defined inline in `Program.cs` using `app.MapGet()` / `app.MapPost()` etc.
- Nullable reference types and implicit usings are enabled
