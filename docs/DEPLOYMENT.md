# Deployment

[← README](../README.md) · Related: [SECURITY](SECURITY.md) · [TROUBLESHOOTING](TROUBLESHOOTING.md) · [SETUP](SETUP.md)

Target: **IIS**, in-process hosting via `AspNetCoreModuleV2`, configured by
`MarkatPlace/web.config`.

---

## Before you deploy — required configuration

> The application **will refuse to start** in a non-Development environment without
> `JwtSettings__SecretKey`. That is deliberate.

| Variable | Required | Notes |
| --- | --- | --- |
| `ConnectionStrings__DefaultConnection` | ✅ | SQL Server connection string |
| `JwtSettings__SecretKey` | ✅ | ≥ 32 random characters. **Never in a committed file** |
| `EmailSettings__Password` | ✅ | SMTP password for password-reset e-mails |
| `ASPNETCORE_ENVIRONMENT` | ✅ | `Production` |
| `AdminUser__UserName` / `__Email` / `__Password` | Bootstrap only | Set once on a fresh database, start, then **remove** |

The section separator is `__` (double underscore).

Full checklist: [SECURITY.md § Production checklist](SECURITY.md#production-checklist).

> **Rotating the JWT key signs every user out** — access tokens live 40 days. Announce it.

---

## Configuration layering

```
appsettings.json                 base (no secrets)
  → appsettings.Production.json  logging levels, AllowedHosts
    → environment variables      secrets  ← highest precedence
```

`appsettings.Production.json` deliberately **does not override** the secret keys — it inherits, and
the environment supplies them.

> ⚠️ **Never re-introduce empty-string values** for `ConnectionStrings:DefaultConnection`,
> `JwtSettings:SecretKey` or `EmailSettings:*` in `appsettings.Production.json`. An empty string
> *overrides* the base value and makes the app fail to start with **HTTP 500.30**. This has happened.

---

## Publish

```bash
dotnet publish MarkatPlace/MarkatPlace.csproj -c Release -o ./publish
```

Deploy the contents of `./publish` to the IIS application directory.

### `web.config`

Shipped with the application. Two things matter:

| Setting | Value | Why |
| --- | --- | --- |
| `maxAllowedContentLength` | `110100480` (105 MB) | Must be **≥** the application's ceiling (`FileUploadConstants.MaxRequestBodySizeBytes`). IIS rejects an oversized body **itself**, with an HTML 404.13 page, before the request reaches the app — so a lower value silently breaks every listing posted with a video |
| `stdoutLogEnabled` | `"false"` | Diagnostic switch, not a setting |
| `ASPNETCORE_DETAILEDERRORS` | **absent** | Would render start-up stack traces into the browser |

A test (`DeploymentSafetyTests.IIS_accepts_at_least_as_large_a_body_as_the_application_does`)
recomputes both ceilings and fails the build if they diverge.

### Uploads directory

Uploaded files live under `MarkatPlace/wwwroot/uploads/<module>/` and are served at permanent public
URLs — the exact URL stored in the database.

> **Empty folders are not included by `dotnet publish`**, so on a fresh deployment `wwwroot` does not
> exist and ASP.NET Core silently resolves the web root to the content root. `Program.cs` therefore
> pins the uploads root explicitly, creates every folder on start-up, and serves it with its own
> `PhysicalFileProvider`. Writer and reader can never drift apart.

**The uploads folder must persist across deployments.** Publishing over it without preserving it
deletes every uploaded image. Back it up, or place it on separate storage.

---

## Migrations

Applied **automatically on start-up** (`db.Database.MigrateAsync()` in `Program.cs`). No manual step.

Consequences to plan for:

- The first request after a deployment waits for migrations.
- A failing migration **stops the application from starting** — it will not serve traffic in a
  half-migrated state.
- Two instances starting simultaneously against one database can race. Deploy one instance first, or
  apply migrations out of band with `dotnet ef database update`.

---

## Health checks

| Endpoint | Checks | Use for |
| --- | --- | --- |
| `GET /health/live` | Process is up | Container/host restart policy |
| `GET /health` | Process **+ database** | Load-balancer traffic gating |

Both are `AllowAnonymous` and **exempt from rate limiting**, so a monitor can poll as often as it
needs.

`/health` returns JSON with per-check status and duration. A failing check reports
`"error": "See server logs."` — deliberately: a health probe must not describe the database to
anyone who can reach the endpoint.

---

## Reverse proxy

`UseForwardedHeaders` runs first and honours `X-Forwarded-For`, `X-Forwarded-Proto` and
`X-Forwarded-Host`. `KnownIPNetworks` and `KnownProxies` are **cleared**, so headers are honoured
regardless of the proxy's internal IP.

This is what makes generated upload URLs use the real public domain and scheme, and what makes
rate limiting partition by the real client IP rather than the proxy's.

> Because the proxy list is cleared, the application trusts these headers unconditionally. It must
> therefore sit **behind** a proxy that strips client-supplied `X-Forwarded-*` headers.

---

## Background services

Two hosted services start with the application:

| Service | Interval | Start delay |
| --- | --- | --- |
| `ListingLifecycleService` | 1 hour | 15 s |
| `BannerBookingLifecycleService` | 1 hour | 45 s |

The delay lets migrations finish before the first pass. With multiple instances, **both run on every
instance** — there is no distributed lock. The work is idempotent (notifications are deduplicated per
publication cycle), but consider a single-instance deployment or an external scheduler if you scale
out.

---

## Swagger in production

Swagger is **enabled in every environment** and reachable at `/swagger`; `/` redirects there.

> This is a deliberate choice — the docs are always available. If exposing the API surface publicly is
> not acceptable, gate `/swagger` at the reverse proxy. Do not remove the endpoint without checking
> who depends on it.

The generated entry points (`/swagger`, `/swagger/index.html`, `/swagger/index.js`) are marked
`no-store`, because Swashbuckle otherwise caches the **generated** document picker for seven days —
so a newly added document is missing from the picker while its JSON answers 200, and no amount of
redeploying fixes it.

---

## Post-deployment verification

```bash
curl -f https://<host>/health/live
curl -f https://<host>/health              # expect "status":"Healthy"
curl -f https://<host>/api/lookups/categories-tree
curl -sI https://<host>/api/lookups/categories-tree | grep -iE "x-content-type|x-frame|^server"
```

- [ ] `/health` reports `Healthy`
- [ ] A public endpoint returns data
- [ ] Security headers present; **no `Server: Kestrel`**
- [ ] An uploaded image URL from before the deployment still opens
- [ ] Login works and returns a token
- [ ] SignalR connects: `wss://<host>/hubs/notifications?access_token=<jwt>`
- [ ] An error response contains no stack trace
- [ ] Logs show both background services started

---

## Rollback

The application is stateless apart from the database and the uploads folder.

1. Redeploy the previous build.
2. **Migrations do not roll back automatically.** If the new build added a migration, the database is
   ahead of the old code. Additive migrations (a new index, a new nullable column) are usually
   compatible; destructive ones are not.
3. Never restore an old `wwwroot/uploads` over a newer one — you would delete files uploaded since.

---

## Logging

| Environment | Default level |
| --- | --- |
| Development | `Information` (`Microsoft.AspNetCore`: `Warning`) |
| Production | `Warning`; `Microsoft.EntityFrameworkCore`: `Warning`; `MarkatPlace`: `Information` |

Logs go to the standard ASP.NET Core providers. `stdoutLogEnabled` is off; turn it on **briefly**
while diagnosing a start-up failure, then off again.
