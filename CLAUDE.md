# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run (HTTP on port 5082)
dotnet run --project DemoPerCorsoClaude --launch-profile http

# Run (HTTPS on 7184, HTTP on 5082)
dotnet run --project DemoPerCorsoClaude --launch-profile https

# Test the endpoint manually
curl http://localhost:5082/weatherforecast
```

No test project exists — the `.http` file (`DemoPerCorsoClaude/DemoPerCorsoClaude.http`) can be used with VS Code REST Client or Visual Studio's HTTP client to test the API.

## Architecture

**ASP.NET Core 10 Minimal API** — no controllers, everything lives in `Program.cs`.

- Single endpoint: `GET /weatherforecast` returns a 5-element array of random weather forecasts
- Data model: `WeatherForecast` record (defined inline in `Program.cs`) with `DateOnly Date`, `int TemperatureC`, `string? Summary`, and a computed `TemperatureF` property
- OpenAPI support via `Microsoft.AspNetCore.OpenApi` (registered with `AddOpenApi()`)
- HTTPS redirection middleware enabled

This is a demo/teaching project for a programming course ("AzeroUnoProgramming").

## Reference Docs

@import docs/agent_docs/clean-code.md
<!-- Load when: writing or reviewing any C# code — naming, method length, comments, magic numbers. -->

@import docs/agent_docs/solid-principles.md
<!-- Load when: adding new types, interfaces, or DI registrations; reviewing architectural boundaries. -->

@import docs/agent_docs/error-handling.md
<!-- Load when: adding exception handling, middleware, ILogger calls, or Result<T> usage. -->

@import docs/agent_docs/testing-conventions.md
<!-- Load when: writing or reviewing tests, creating Builders, choosing between real objects and NSubstitute. -->
