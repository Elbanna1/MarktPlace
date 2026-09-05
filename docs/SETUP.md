# Setup — New Developer Quick Start

[← README](../README.md) · Related: [TROUBLESHOOTING](TROUBLESHOOTING.md) · [DEVELOPMENT_GUIDE](DEVELOPMENT_GUIDE.md)

Follow these ten steps in order. At the end you will have the API running, Swagger open, an
administrator account, and a green test run.

---

## 1. Prerequisites

| Requirement | Version | Check |
| --- | --- | --- |
| .NET SDK | 10.0 | `dotnet --version` |
| SQL Server | LocalDB, Express, or a full instance | `sqllocaldb info` |
| `dotnet-ef` | 10.0.10 (pinned in `MarkatPlace/dotnet-tools.json`) | `dotnet ef --version` |
| `sqlcmd` | any | optional, for inspecting the database |

Install the EF tools if `dotnet ef` is missing:

```bash
dotnet tool install --global dotnet-ef --version 10.0.10
```

> Keep the tool version aligned with the EF Core packages (10.0.10). A mismatched global tool
> produces confusing scaffolding errors.

---

## 2. Open the project

```bash
cd MarkatPlace          # the repository root, which contains MarkatPlace.slnx
```

The solution file is `MarkatPlace.slnx` (the XML solution format, not `.sln`).

---

## 3. Restore and build

```bash
dotnet build MarkatPlace.slnx
```

Expected: **`Build succeeded. 0 Warning(s) 0 Error(s)`**. The build is warning-clean and should stay
that way.

---

## 4. Configure the database connection

> ⚠️ **`appsettings.json`'s `DefaultConnection` points at a shared hosted database.** Never develop
> or test against it. Always override it locally.

Use an environment variable rather than editing the file:

**bash / Git Bash**

```bash
export ConnectionStrings__DefaultConnection='Server=(localdb)\MSSQLLocalDB;Database=MarkatPlaceDev;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True'
```

**PowerShell**

```powershell
$env:ConnectionStrings__DefaultConnection = 'Server=(localdb)\MSSQLLocalDB;Database=MarkatPlaceDev;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True'
```

Or use user-secrets, which are not committed:

```bash
cd MarkatPlace
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\MSSQLLocalDB;Database=MarkatPlaceDev;Trusted_Connection=True;TrustServerCertificate=True"
```

---

## 5. Configure the remaining environment

In **Development** you need nothing else — `MarkatPlace/appsettings.Development.json` already
supplies a local-only JWT signing key and a bootstrap administrator.

| Setting | Development | Production |
| --- | --- | --- |
| `ConnectionStrings__DefaultConnection` | override as above | **required** env var |
| `JwtSettings__SecretKey` | from `appsettings.Development.json` | **required** env var, ≥ 32 chars — *start-up fails without it* |
| `EmailSettings__Password` | inherited (password-reset e-mail will fail without a valid one) | **required** env var |
| `AdminUser__UserName` / `__Email` / `__Password` | from `appsettings.Development.json` | set once to bootstrap, then remove |
| `DemoData__Enabled` | `true` by default — set `false` to skip a ~289-image download | has no effect |
| `Diagnostics__SqlCounter` | `false` | `false` |
| `RateLimiting__PermitPerMinute` | 600 | 600 |
| `RateLimiting__AuthPermitPerMinute` | 20 | 20 |

Details: [SECURITY.md § Secrets and credentials](SECURITY.md#secrets-and-credentials).

---

## 6. Migrations

**You do not normally run migrations by hand.** `Program.cs` calls `db.Database.MigrateAsync()` on
start-up, so an empty database is created and brought up to date the first time you run the app.

Run them manually only when you need to inspect or control the process:

```bash
cd Infastrucre/Presitance
dotnet ef database update --startup-project ../../MarkatPlace/MarkatPlace.csproj
```

Adding a migration is covered in
[DEVELOPMENT_GUIDE.md § Changing the database](DEVELOPMENT_GUIDE.md#changing-the-database).

---

## 7. Start the API

```bash
cd MarkatPlace
dotnet run
```

Launch profiles are in `MarkatPlace/Properties/launchSettings.json`:

| Profile | URLs |
| --- | --- |
| `http` | `http://localhost:5002` |
| `https` | `https://localhost:7139` and `http://localhost:5002` |

The browser opens on `/swagger`. The first start takes longer: it applies 56 migrations, seeds the
lookup tables and — unless `DemoData__Enabled=false` — downloads the demo image set once.

---

## 8. Verify it works

```bash
# process is up
curl http://localhost:5002/health/live

# database is reachable
curl http://localhost:5002/health

# a public endpoint with real data
curl http://localhost:5002/api/lookups/categories-tree
```

`GET /health` returns the status of the `database` check. Anything other than `Healthy` means the
connection string is wrong — see [TROUBLESHOOTING.md](TROUBLESHOOTING.md#the-application-starts-but-health-is-unhealthy).

Then open **`/swagger`**. The document picker at the top right offers two documents:

| Document | Contents |
| --- | --- |
| **MarkatPlace V1** | The public marketplace API — 623 operations |
| **MarkatPlace Admin V2** | The administration dashboard — 131 operations |

### Sign in

The Development administrator is created on start-up from `appsettings.Development.json`:

```bash
curl -X POST http://localhost:5002/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin@12345"}'
```

Copy `data.token` into Swagger's **Authorize** dialog as `Bearer <token>`.

> These are local development credentials that exist only because
> `appsettings.Development.json` names them. `appsettings.Production.json` deliberately omits the
> `AdminUser` section — that omission *is* the safety mechanism.

---

## 9. Run the tests

```bash
dotnet test Tests/MarkatPlace.Tests/MarkatPlace.Tests.csproj
```

Expected: **1,532 passing, 0 failing**, in well under a second. The suite is hermetic — no database,
no HTTP host — so it is safe to run at any time. See [TESTING.md](TESTING.md).

---

## 10. Where to start developing

| Goal | Start here |
| --- | --- |
| Understand the shape of the system | [ARCHITECTURE.md](ARCHITECTURE.md) |
| Understand one module deeply | [MODULES.md](MODULES.md), then read `LandsController` → `LandService` → `LandRepository` |
| Add a field to an existing module | [DEVELOPMENT_GUIDE.md § Adding a field](DEVELOPMENT_GUIDE.md#adding-a-field-to-an-existing-module) |
| Add a whole new module | [DEVELOPMENT_GUIDE.md § Adding a module](DEVELOPMENT_GUIDE.md#adding-a-new-listing-module) |
| Avoid breaking something | [BUSINESS_RULES.md](BUSINESS_RULES.md) |

---

## Optional: running against an isolated database with diagnostics

Useful for performance work and for exercising the API without touching your normal dev data:

```bash
export ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_URLS=http://127.0.0.1:5099
export ConnectionStrings__DefaultConnection='Server=(localdb)\MSSQLLocalDB;Database=MarkatPlaceTest;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True'
export DemoData__Enabled=false             # skip the image download, start in seconds
export Diagnostics__SqlCounter=true        # adds X-Sql-Count / X-Sql-Ms / X-Elapsed-Ms headers
export RateLimiting__PermitPerMinute=1000000
export RateLimiting__AuthPermitPerMinute=1000000

cd MarkatPlace && dotnet run
```

Reset that database between runs:

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "ALTER DATABASE [MarkatPlaceTest] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [MarkatPlaceTest];"
```

`MigrateAsync` rebuilds it on the next start.
