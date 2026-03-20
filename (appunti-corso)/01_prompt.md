Analizza il codice in e genera il file
    docs/agent_docs/clean-code.md con le convenzioni
    di clean code specifiche per questo progetto .NET 10.
    Includi regole su: naming (classi, metodi, variabili, parametri), lunghezza dei metodi, commenti XML doc,
    magic numbers, leggibilità. Per ogni regola scrivi
    un esempio C# corretto e uno errato tratti dal
    codice esistente o coerenti con il dominio MES.
    Usa imperativo diretto: ALWAYS, NEVER, USE, AVOID.
    Max 80 righe.

Genera il file docs/agent_docs/solid-principles.md
  con i 5 principi SOLID applicati a questo progetto.
  Per ogni principio: definizione in una riga, regola
  operativa per Claude (ALWAYS/NEVER), esempio C#
  tratto dalla Clean Architecture

Genera il file docs/agent_docs/error-handling.md
  per questo progetto .NET 10.
  Includi: quando usare DomainException vs eccezioni
  di sistema, come gestire le eccezioni nei Controller
  (middleware globale vs try/catch locale), Result pattern
  se appropriato, logging strutturato con ILogger.
  Basa le regole sul pattern già presente in
  DomainException.cs e OrdersController.cs.
  Max 80 righe.