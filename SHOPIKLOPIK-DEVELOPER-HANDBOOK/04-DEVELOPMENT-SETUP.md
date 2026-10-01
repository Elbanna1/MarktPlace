# 04 — Development Setup

[← Start Here](00-START-HERE.md) · Related: [05-CONFIGURATION](05-CONFIGURATION.md) ·
[08-DATABASE-MIGRATIONS](08-DATABASE-MIGRATIONS.md) · [30-TROUBLESHOOTING](30-TROUBLESHOOTING.md)

Target: a running API, a reachable Swagger page, and a green test run — in about fifteen minutes.

---

## The one thing that will bite you

> ### `appsettings.json`'s `DefaultConnection` points at the SHARED HOSTED database.
>
> If you run the project without overriding it, you are developing against **production-adjacent
> data**, and — because `appsettings.Development.json` sets `DemoData:Enabled = true` — you will
> **seed demo users, listings and images into it**.
>
> Override `ConnectionStrings__DefaultConnection` *before* your first `dotnet run`. Every time. On
> every machine.

---

## Step 1 — Prerequisites

| Requirement | Version | Check with |
| --- | --- | --- |
| .NET SDK | **10.0.x** (verified working: 10.0.401) | `dotnet --version` |
| SQL Server | LocalDB, Developer, or Express | `sqllocaldb info` |
| `dotnet-ef` | 10.0.x — the repo pins **10.0.10** in `MarkatPlace/dotnet-tools.json` | `dotnet ef --version` |
| Git | any recent | `git --version` |

Install the EF tool if you do not have it:

```bash
dotnet tool install --global dotnet-ef --version 10.0.10
# or, to use the version the repository pins:
cd MarkatPlace && dotnet tool restore
```

> A globally installed `dotnet-ef` whose version differs from the EF Core packages usually still
> works, but mismatches have caused confusing errors here before. If `dotnet ef` misbehaves, check
> the two versions match before doing anything else.

---

## Step 2 — Clone and build

```bash
git clone <repository-url>
cd MarkatPlace
dotnet build MarkatPlace.slnx
```

A clean build restores 8 projects. Warnings are normal; errors are not.

---

## Step 3 — Point at a local database

Pick a connection string and export it. **Do not edit `appsettings.json`** — that file is committed
and you will eventually push your local connection string.

**Windows (PowerShell):**

```powershell
$env:ConnectionStrings__DefaultConnection =
  'Server=(localdb)\MSSQLLocalDB;Database=ShopiklopikDev;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True'
```

**Linux / macOS / Git Bash:**

```bash
export ConnectionStrings__DefaultConnection='Server=localhost,1433;Database=ShopiklopikDev;User Id=sa;Password=<YOUR_LOCAL_PASSWORD>;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True'
```

The `__` (double underscore) is the section separator: `ConnectionStrings__DefaultConnection`
overrides `ConnectionStrings:DefaultConnection`.

> **A permanent alternative:** use .NET user secrets, which are stored outside the repository.
> ```bash
> cd MarkatPlace
> dotnet user-secrets init
> dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your local connection string>"
> ```
> This is the safest option and survives new terminals.

---

## Step 4 — Decide about demo data

`appsettings.Development.json` sets `DemoData:Enabled = true`. On start-up, in **Development only**,
`DevelopmentDataSeeder` will:

- download a library of free images into `MarkatPlace/wwwroot/uploads/` (once — a
  `demo-images.json` manifest makes later runs reuse them),
- create demo users (password `Demo@12345`, e-mails under `demo.markatplace.local`),
- create demo advertisements, workshops, craftsmen and Lost & Found posts, with images.

It **refuses to run outside Development** regardless of the flag, so a production database can never
receive demo data.

To skip it (faster start-up, no network access needed):

```bash
export DemoData__Enabled=false
```

---

## Step 5 — Run

```bash
cd MarkatPlace
dotnet run
```

On start-up the application will, in this order:

1. create every uploads folder under `wwwroot/uploads/`,
2. validate configuration (JWT secret, frontend URL, CORS origins, forwarded headers) — and **fail
   fast with a clear message** if something is wrong,
3. apply all pending EF migrations (`db.Database.MigrateAsync()`),
4. ensure the `Admin` and `SuperAdmin` roles exist and seed the bootstrap administrator,
5. seed demo data (Development only).

| Profile | URL |
| --- | --- |
| `https` (default) | https://localhost:7139 and http://localhost:5002 |
| `http` | http://localhost:5002 |

> **`dotnet run` always forces `ASPNETCORE_ENVIRONMENT=Development`** because
> `launchSettings.json` sets it in both profiles. Setting the variable yourself does **not** win. To
> run as another environment, run the built DLL instead:
> ```bash
> dotnet build -c Release
> ASPNETCORE_ENVIRONMENT=Staging dotnet MarkatPlace/bin/Release/net10.0/MarkatPlace.dll
> ```

---

## Step 6 — Open Swagger

**https://localhost:7139/swagger**

In Development, `/` also redirects there. The document picker at the top right has two entries:

- **MarkatPlace V1** — the public API
- **MarkatPlace Admin V2** — everything under `/api/v2/admin`

Swagger is **not** password-protected in Development unless you configure `SwaggerAuth`.

### The local administrator

`appsettings.Development.json` seeds one:

| Field | Value |
| --- | --- |
| Username | `admin` |
| Password | `Admin@12345` |
| E-mail | `admin@markatplace.local` |

These are **development-only credentials in a committed file**, and that is deliberate: the
`AdminUser` section is absent from `appsettings.json` and `appsettings.Production.json`, and
`IdentityDataSeeder` has **no fallback**, so this account can never appear in production.

### A five-minute smoke test in Swagger

1. `GET /api/lookups/categories-tree` → the 12 categories and their sub-categories.
2. `GET /api/lookups/create-ad-form/6/21` → the whole dynamic form for "birds".
3. `POST /api/auth/login` with `admin` / `Admin@12345` → copy `data.token`.
4. Click **Authorize**, paste `Bearer <token>`.
5. `GET /api/v2/admin/ads/pending` → the review queue.

---

## Step 7 — Run the tests

```bash
dotnet test Tests/MarkatPlace.Tests/MarkatPlace.Tests.csproj
```

Expected (verified 2026-09-25):

```
Passed!  - Failed: 0, Passed: 2217, Skipped: 0, Total: 2217, Duration: 9 s
```

The suite needs **no database and no network** — it uses EF Core InMemory and scans source files on
disk. If it needs either, something has been added incorrectly.

---

## Working with migrations locally

```bash
# from the repository root
dotnet ef migrations add <Name> \
  --project Infastrucre/Presitance/Presitance.csproj \
  --startup-project MarkatPlace/MarkatPlace.csproj

# ALWAYS read the generated file before applying it — see 08-DATABASE-MIGRATIONS.md
dotnet ef database update \
  --project Infastrucre/Presitance/Presitance.csproj \
  --startup-project MarkatPlace/MarkatPlace.csproj
```

`AppDbContextFactory` also lets EF tools work without the host, reading the environment variable
**`MARKATPLACE_CONNECTION`** (note: *not* `ConnectionStrings__DefaultConnection`) and falling back to
`Server=.;Database=MarkatPlaceDb;…`.

> **After `migrations add`, build before you run.** Running with `--no-build` against a stale
> assembly produces a false "PendingModelChanges" error at start-up.

---

## Common setup failures

| Symptom | Cause | Fix |
| --- | --- | --- |
| `JwtSettings:SecretKey is missing or too short` at start-up | No secret key in the current environment | It is in `appsettings.Development.json`; check `ASPNETCORE_ENVIRONMENT=Development`, or set `JwtSettings__SecretKey` to ≥ 32 characters |
| `JwtSettings:SecretKey is a placeholder or a value that has been published in source control` | Running non-Development with a known-compromised key | Generate a fresh random key and pass it as an environment variable |
| `App:FrontendUrl is not configured` / `points at localhost` | Running non-Development without `App__FrontendUrl` | Set it to the public site URL |
| `Connection string 'DefaultConnection' is not configured` | Empty-string override | Never set the variable to `""` — an empty string *overrides* the base value |
| `A network-related or instance-specific error …` | SQL Server not running, or wrong instance name | `sqllocaldb start MSSQLLocalDB`, or correct the server name |
| `Login failed for user` | Wrong credentials, or `Encrypt=True` without `TrustServerCertificate=True` on a self-signed local server | Add `TrustServerCertificate=True` for local development |
| `PendingModelChanges` on start-up after adding a migration | Stale build | `dotnet build`, then run |
| Port 5002 or 7139 already in use | A previous `dotnet run` never exited | Kill the orphan process; on Windows `Get-Process dotnet` then `Stop-Process` |
| Demo seeding logs failures | No internet access for the image download | Set `DemoData__Enabled=false`; the app starts fine without it |
| Uploaded images 404 locally | `wwwroot/uploads` was deleted | Restart — `Program.cs` recreates every folder at start-up (the files themselves are gone for good) |
| Swagger shows only one document | Swashbuckle's cached `index.js` | Hard-refresh; `Program.cs` already marks the entry points `no-store` |

---

## Recommended local environment (all of it)

```bash
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection='Server=(localdb)\MSSQLLocalDB;Database=ShopiklopikDev;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True'
export DemoData__Enabled=true          # false for a faster, offline start-up
export Diagnostics__SqlCounter=true    # adds X-Sql-Count / X-Elapsed-Ms response headers
```

`Diagnostics:SqlCounter` is the single most useful development switch in the project: it tells you
how many SQL round trips a request cost. See [diagrams/REQUEST-FLOW.md](diagrams/REQUEST-FLOW.md).

---

## Never do these locally

- ❌ Edit `appsettings.json` to add your connection string — it is committed.
- ❌ Run against the hosted database with `DemoData:Enabled = true`.
- ❌ Commit a real secret of any kind. See [22-SECURITY.md](22-SECURITY.md).
- ❌ Apply a scaffolded migration without reading it first — EF rewrites unrelated drop+add pairs as
  `RenameColumn` and silently reinterprets data.
