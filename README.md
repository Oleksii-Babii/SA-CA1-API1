# Bonsai API

A RESTful Web API for managing Bonsai trees and species information, built with ASP.NET Core 10 and Entity Framework Core.

## Tech Stack

- **Framework**: ASP.NET Core 10.0 / .NET 10.0
- **ORM**: Entity Framework Core 10.0.2
- **Database**: SQL Server (production) / In-Memory (testing & non-Windows dev)
- **API Docs**: Swagger / OpenAPI (Swashbuckle)
- **Testing**: xUnit 2.9.3 + ASP.NET Core Mvc.Testing
- **CI/CD**: GitHub Actions → Azure App Service

## Project Structure

```
SA-CA1-API1/
├── .github/workflows/deploy-api.yml   # CI/CD pipeline
└── API/
    ├── BonsaiAPI/                      # Main API project
    │   ├── Controllers/
    │   │   ├── TreesController.cs
    │   │   └── SpeciesController.cs
    │   ├── Models/
    │   │   ├── Tree.cs
    │   │   └── Species.cs
    │   ├── Data/
    │   │   └── BonsaiContext.cs        # DbContext with seed data
    │   ├── Migrations/
    │   ├── Program.cs
    │   └── wwwroot/uploads/            # Uploaded tree images
    └── BonsaiAPI.Tests/                # xUnit integration tests
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server / LocalDB (Windows) — or use the automatic in-memory fallback on macOS/Linux

### Run locally

```bash
cd API/BonsaiAPI
dotnet run
```

The API starts at `http://localhost:5220`. Swagger UI opens automatically in Development mode at `http://localhost:5220/swagger`.

**Production Swagger**: [https://bonsaiapi-ewe7gdd0hfd8a8dv.westeurope-01.azurewebsites.net/swagger/index.html](https://bonsaiapi-ewe7gdd0hfd8a8dv.westeurope-01.azurewebsites.net/swagger/index.html)

> On macOS/Linux the app falls back to an in-memory database automatically — no SQL Server required.

### Run tests

```bash
cd API/BonsaiAPI.Tests
dotnet test
```

Tests use an isolated in-memory database (unique per run), so no external database is needed.

## API Reference

### Trees — `/api/trees`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/trees` | List all trees (ordered by nickname) |
| GET | `/api/trees/{id}` | Get tree by ID (includes species) |
| GET | `/api/trees/search?name={query}` | Case-insensitive nickname search |
| POST | `/api/trees` | Create a new tree |
| PUT | `/api/trees/{id}` | Update a tree |
| DELETE | `/api/trees/{id}` | Delete a tree |
| POST | `/api/trees/{id}/image` | Upload a tree image (multipart/form-data) |

**Tree fields**

| Field | Type | Constraints |
|-------|------|-------------|
| `id` | int | PK, auto-generated |
| `nickname` | string | Required, max 100 chars |
| `age` | int | 0–5000 |
| `height` | decimal | 0–10000, 2 decimal places |
| `lastWateredDate` | DateTime | |
| `notes` | string | Optional |
| `imageUrl` | string | Optional, set by image upload |
| `speciesId` | int | FK → Species |

**Image upload** accepts `.jpg`, `.jpeg`, `.png`, `.webp` up to 10 MB.

---

### Species — `/api/species`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/species` | List all species (ordered by name) |
| GET | `/api/species/{id}` | Get species by ID |

**Seeded species**: Japanese Maple, Chinese Elm, Juniper, Ficus, Azalea

## Database

The app selects its database provider at startup:

| Condition | Provider |
|-----------|----------|
| `ASPNETCORE_ENVIRONMENT=Testing` | In-Memory |
| Connection string missing or empty | In-Memory |
| LocalDB connection string on non-Windows OS | In-Memory |
| Any other valid connection string | SQL Server |

Migrations are applied automatically on startup.

## CI/CD

The GitHub Actions pipeline (`.github/workflows/deploy-api.yml`) runs on every push or PR targeting `main`:

1. **Build** — restore, build (Release), publish artifact
2. **Test** — run xUnit tests, upload TRX results
3. **Deploy** *(push to `main` only)* — apply EF migrations → deploy to Azure App Service (`bonsaiapi`)

**Required GitHub secrets**

| Secret | Purpose |
|--------|---------|
| `AZURE_SQL_CONNECTION_STRING` | EF migration target |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Azure App Service deploy profile |

## Development Notes

- CORS is fully open (all origins, methods, headers) — tighten for production use.
- Static files are served from `wwwroot/`; the `uploads/` directory is created automatically.
- Use `bonsai.http` (VS Code REST Client) for quick manual endpoint testing against `http://localhost:5220`.
