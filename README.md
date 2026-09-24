# DDD Template for .NET

This repository provides a Domain-Driven Design (DDD) template implemented in .NET. It serves as a starting point for developers looking to apply DDD principles in their .NET applications. The template is designed to help structure your application in a way that separates concerns, promotes a clean architecture, and facilitates maintainability and scalability.

## Project structure

| Project | Responsibility | Depends on |
|---|---|---|
| `Fzerey.DDDStarter.Domain` | Entities, business rules, `DomainException`, repository interfaces | — |
| `Fzerey.DDDStarter.Application` | Commands and queries (MediatR), query service interfaces, response models | Domain |
| `Fzerey.DDDStarter.Infrastructure` | EF Core `DbContext`, repositories, query services, migrations (PostgreSQL) | Domain, Application |
| `Fzerey.DDDStarter.WebApi` | Controllers, request validation, error handling, startup | Application, Infrastructure |
| `Fzerey.DDDStarter.Tests` | Domain and handler tests (xUnit, no database needed) | Application |

Writes and reads take different paths:

- **Commands** load aggregates through repositories (`IOrderRepository`, `IItemRepository`), call domain methods, and save with `IUnitOfWork`. Business rules live in the entities, e.g. `Order.AddItem`.
- **Queries** go through query services (`IOrderQueries`, `IItemQueries`) that project straight to response models in a single SQL query, without loading aggregates.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Docker, for the local PostgreSQL database

## Running locally

Start PostgreSQL:

```sh
docker compose up -d
```

Run the API. Pending migrations are applied on startup.

```sh
dotnet run --project Fzerey.DDDStarter.WebApi
```

Swagger UI is available at http://localhost:5066/swagger in the Development environment. [`Fzerey.DDDStarter.WebApi.http`](Fzerey.DDDStarter.WebApi/Fzerey.DDDStarter.WebApi.http) contains sample requests for the VS Code REST Client extension.

## Configuration

Database settings are read from the `DatabaseConfiguration` section of `appsettings.json` and match the Docker Compose setup. Override any value with an environment variable, using `__` as the separator:

```sh
DatabaseConfiguration__host=db.example.com
DatabaseConfiguration__password=secret
```

## Tests

```sh
dotnet test --solution Fzerey.DDDStarter.sln
```

`global.json` switches `dotnet test` to Microsoft.Testing.Platform, which the .NET 10 SDK requires for xUnit v3.

## Migrations

Install the EF Core tools once:

```sh
dotnet tool install --global dotnet-ef
```

Add a migration after changing the model:

```sh
dotnet ef migrations add <Name> --project Fzerey.DDDStarter.Infrastructure --startup-project Fzerey.DDDStarter.WebApi
```

CI fails if the model has changes that are not covered by a migration.

## Error responses

Request validation errors (missing fields, out-of-range values) return `400` with ASP.NET Core's standard `ProblemDetails` body.

Other errors return JSON with `Message` and `ErrorCode`:

| Status | Cause |
|---|---|
| 400 | A domain rule was violated (`DomainException`) |
| 404 | The requested order or item does not exist |
| 500 | Unexpected error |

Every response carries a `CorrelationId` header. Send one with the request to reuse it; otherwise a new one is generated.
