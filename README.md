# MiniPay API

Payment processing API built with **Event Sourcing**, **CQRS** and **Clean Architecture**.

---

## Tech Stack

| | |
|---|---|
| ASP.NET Core 8 | REST API |
| MediatR | CQRS + Pipeline Behaviors |
| Entity Framework Core | Event Store (SQL Server) |
| Redis | Distributed Cache |
| xUnit | Unit Tests |

---

## Architecture

```
MiniPay.Domain          → Aggregates, Events, Enums
MiniPay.Application     → Commands, Queries, Handlers, Behaviours, Interfaces
MiniPay.Infrastructure  → EventStore, CacheService, EF Core
MiniPay.API             → Controllers, Program.cs
```

### How it works

Every transaction change is stored as an immutable event in SQL Server. The current state is rebuilt by replaying events (`Rewind`). Redis caches query results to avoid replaying on every read — cache is invalidated on every write command.

```
Request → Controller → MediatR
            → CachingBehaviour   (Redis read)
              → Handler
                → EventStore     (SQL Server)
            → CacheInvalidationBehaviour (Redis invalidate)
```

### Transaction Flow

```
Initiated → Authorized → Settled
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Run

**1. Start infrastructure**
```bash
docker compose up -d
```

**2. Apply migrations**
```bash
dotnet ef database update --project MiniPay.Infrastructure --startup-project MiniPay.API
```

**3. Run the API**
```bash
dotnet run --project MiniPay.API
```

**4. Open Swagger**

```
https://localhost:7149/swagger
```

---

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/transactions/initiate` | Create a new transaction |
| `POST` | `/api/transactions/{id}/authorize` | Authorize a transaction |
| `POST` | `/api/transactions/{id}/settle` | Settle an authorized transaction |
| `GET` | `/api/transactions/{id}` | Get transaction by ID |

### Example

```json
POST /api/transactions/initiate
{
  "amount": 100.00,
  "currency": "PLN"
}
```
