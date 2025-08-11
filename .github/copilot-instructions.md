## General
* **Language**: english, C# (version 13).
* **Stack**: ASP.NET Core 9, EF Core 9, MediatR, MassTransit (RabbitMQ), PostgreSQL 16, Redis 7, React 19.


## Style Guide C#
1. `file-scoped namespace`, `required`, `collection expressions`.

## EF Core
* Always `AsSplitQuery` when multiple includes involved, `NoTracking` for read only.
* Configuration via Fluent API, not attributes.
* Extensions: `UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)`.
* For N+1 ⇒ `Include`, `ThenInclude`, or `select` into an anonymous type.



> **Copilot reminder:** if the request is not explicitly related to architecture — first suggest a template according to the rules above.
