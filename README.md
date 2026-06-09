# \# MiniPay API

# 

# Payment processing API built with Event Sourcing, CQRS and Clean Architecture.

# 

# \## Tech Stack

# \- \*\*ASP.NET Core 8\*\* — REST API

# \- \*\*MediatR\*\* — CQRS + Pipeline Behaviors

# \- \*\*Entity Framework Core\*\* — Event Store (SQL Server)

# \- \*\*Redis\*\* — Distributed Cache

# \- \*\*Event Sourcing\*\* — Transaction state rebuilt from events

# 

# \## Architecture
# MiniPay.Domain → Aggregates, Events, Enums
# ===

# MiniPay.Application → Commands, Queries, Handlers, Behaviours

# MiniPay.Infrastructure → EventStore, CacheService, EF Core

# MiniPay.API → Controllers, Program.cs

## Getting Started

# ===

# \### Prerequisites

# \- \[.NET 8 SDK](https://dotnet.microsoft.com/download)

# \- \[Docker Desktop](https://www.docker.com/products/docker-desktop/)

# \### Run

# 1\. Start infrastructure:

# ```bash

# docker compose up -d



# 2. Apply migrations:
# ===

# dotnet ef database update --project MiniPay.Infrastructure



# 3\. Run the API:

# dotnet run --project MiniPay.API



# 4.Open Swagger: https://localhost:7149/swagger

