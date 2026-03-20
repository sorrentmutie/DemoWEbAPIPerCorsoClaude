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

Genera il file docs/agent_docs/testing-conventions.md
per questo progetto.
Includi: struttura Arrange/Act/Assert, naming dei test
(Should_[Expected]_When_[Condition]), Builder pattern
obbligatorio per le entità di dominio, quando usare
NSubstitute vs oggetti reali, dove salvare i test.
Basati su xUnit + FluentAssertions + NSubstitute.
Max 80 righe.

Aggiorna CLAUDE.md aggiungendo i riferimenti ai
  quattro file appena creati nella sezione
  '## Reference Docs'. Usa la sintassi @import.
  Per ogni file aggiungi una riga che descrive
  quando Claude deve caricarlo.


II want to create a system prompt document for a requirements analyst that gathers informations and asksquestions to produce a REQUIREMENTS.md document that a solution architect can use to design the solution and plan the implementation
D: In che formato pensi di usarli con Claude Code?
R: File da passare con --append-system-prompt
D: In che lingua vuoi i file?
R: Italiano

---------------------------------------------------------------------------------------
questo ci permette di lanciare claude e fargli impersonare quanto scritto sopra
claude --system-prompt-file .\docs\system-prompts\analyst.md
---------------------------------------------------------------------------------------

