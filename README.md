# Takwene — Track Management API + Angular UI

A small music-distribution platform: a **.NET 8 Web API** (Clean Architecture, EF Core + PostgreSQL, JWT) that manages artists, tracks, and their distribution to DSPs (Spotify, Apple Music, YouTube), plus an **Angular 17** single-page UI to browse tracks and their distribution statuses.

---

## Tech stack

| Layer | Tech |
|---|---|
| Backend | .NET 8, ASP.NET Core Web API, EF Core 8, PostgreSQL, FluentValidation, JWT Bearer, BCrypt, Swagger |
| Frontend | Angular 17 (standalone components), Tailwind CSS, lucide-angular |
| Architecture | Clean Architecture (Domain / Application / Infrastructure / Api) |

## Solution structure

```
Takwene/                         # repo root
├─ Takwene/                      # backend solution folder
│  ├─ Takwene.sln
│  ├─ Takwene.Domain/            # entities + enums (no dependencies)
│  ├─ Takwene.Application/       # DTOs, interfaces, validators, exceptions
│  ├─ Takwene.Infrastructure/    # EF Core, DbContext, migrations, services, JWT, seed
│  └─ Takwene/                   # ASP.NET Core Web API (Takwene.Api.csproj)
├─ frontend/                     # Angular 17 app
├─ README.md
└─ DECISIONS.md
```

---

## Prerequisites

- **.NET 8 SDK**
- **Node.js 18+** and npm
- **PostgreSQL** running locally (default port 5432)
- EF Core CLI tools: `dotnet tool install --global dotnet-ef` (if not already installed)

> Quick PostgreSQL via Docker (optional):
> ```bash
> docker run --name takwene-pg -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres:16
> ```

---

## Backend — setup & run

All commands are run from the **backend solution folder** (`Takwene/`).

### 1. Configure the database connection

The connection string is **not committed**. Set it via user-secrets (recommended):

```bash
dotnet user-secrets init --project Takwene
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=takwene;Username=postgres;Password=YOUR_PASSWORD" --project Takwene
```

*(Alternatively, fill in `ConnectionStrings:DefaultConnection` in `Takwene/appsettings.json`.)*

### 2. Run the migrations

```bash
dotnet ef database update --project Takwene.Infrastructure --startup-project Takwene
```

This creates the schema and seeds sample data: **3 artists, 9 tracks** (across genres and all three statuses), **3 DSPs**, and **12 track distributions**.

> To (re)generate the migration from scratch:
> ```bash
> dotnet ef migrations add InitialCreate --project Takwene.Infrastructure --startup-project Takwene --output-dir Persistence/Migrations
> ```

### 3. Run the API

```bash
dotnet run --project Takwene
```

- The API also **applies migrations and seeds the demo admin user automatically on startup**.
- Swagger UI: **http://localhost:5022/swagger**
- Base URL: `http://localhost:5022/api`

---

## How to obtain a JWT token

Mutating endpoints (`POST`/`PATCH`) are protected with `[Authorize]`. To call them, get a token from the login endpoint.

**Demo credentials:** `admin` / `Admin123!`
(The password is stored only as a **BCrypt hash** — never in plaintext. Override with user-secrets `SeedAdmin:Username` / `SeedAdmin:Password` if desired.)

```bash
curl -X POST http://localhost:5022/api/auth/login \
  -H "Content-Type: application/json" \
  -d "{\"username\":\"admin\",\"password\":\"Admin123!\"}"
```

Response:
```json
{ "token": "eyJhbGciOi...", "expiresAt": "2026-..." }
```

**Using it in Swagger:** click **Authorize**, paste the `token` value, and call the protected endpoints.
**Using it with curl:** add header `Authorization: Bearer <token>`.

---

## API endpoints

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/login` | — | Obtain a JWT |
| POST | `/api/artists` | 🔒 | Create an artist |
| GET | `/api/artists` | — | List all artists |
| POST | `/api/tracks` | 🔒 | Create a track for an artist |
| GET | `/api/tracks?artistId=&genre=&status=` | — | List tracks with filters |
| GET | `/api/tracks/{id}` | — | Track details incl. DSP distribution statuses |
| POST | `/api/tracks/{id}/distribute` | 🔒 | Submit a track to one or more DSPs |
| PATCH | `/api/tracks/{id}/status` | 🔒 | Update a track's status |

🔒 = requires `Authorization: Bearer <token>`

---

## Frontend — setup & run

From the **`frontend/`** folder:

```bash
npm install
npm start
```

Open **http://localhost:4200**. The dev server expects the API at `http://localhost:5022/api` (configured in `src/environments/environment.ts`; CORS for `http://localhost:4200` is enabled in the API).

**Views:**
- **Track List** — all tracks with artist, genre, and status; filter by status.
- **Track Detail** — click a track to see full info and which DSPs it's distributed to (with statuses).

---

## Notes

- Enums are stored as readable text in the DB (`Draft`/`Submitted`/`Distributed`, `Pending`/`Live`/`Rejected`).
- `Isrc` is unique per track; a track can't be distributed to the same DSP twice (enforced by a composite unique index).
- Validation errors return RFC 7807 `ProblemDetails` (HTTP 400 with field-level messages); 404/409 for not-found/conflict.
