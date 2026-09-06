# Deployment

[← README](../README.md) · Related: [SECURITY](SECURITY.md) · [TROUBLESHOOTING](TROUBLESHOOTING.md) · [SETUP](SETUP.md)

Target: **Kestrel on Linux behind Nginx**.

```
Internet → Nginx (TLS) → Kestrel → MarkatPlace
```

Kestrel is the ASP.NET Core default server and the only server production uses. Nothing in the
application requires IIS; `MarkatPlace/web.config` is inert outside IIS and is kept only for the
legacy Windows/IIS publish path (see [web.config](#webconfig)).

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

Deploy the contents of `./publish` to the application directory on the server and start it:

```bash
ASPNETCORE_ENVIRONMENT=Production dotnet MarkatPlace.dll
```

### Listening address

**No listening address is hard-coded**, in code or in `appsettings*.json`. With nothing configured
Kestrel binds `http://localhost:5000`; the environment chooses otherwise:

| Variable | Example | Notes |
| --- | --- | --- |
| `ASPNETCORE_URLS` | `http://127.0.0.1:5000` | Loopback only — Nginx is the public listener |
| `ASPNETCORE_URLS` | `http://unix:/run/markatplace.sock` | Unix socket, if you prefer one to a port |

Point Nginx's `proxy_pass` at the same address. Do not add a `Kestrel:Endpoints` section to
`appsettings*.json`: endpoint configuration **overrides** `ASPNETCORE_URLS`, so the server would lose
the ability to choose. A test enforces this.

TLS is terminated by Nginx, so Kestrel needs no certificate and binds no HTTPS endpoint.
`UseHttpsRedirection` only issues a redirect when it can resolve an HTTPS port — leave the
http → https redirect to Nginx, or set `ASPNETCORE_HTTPS_PORT=443` to have the application issue it.

`App:BaseUrl` is **only** the fallback used when a URL is built outside an HTTP request; inside a
request the scheme and host come from the request itself. Set `App__BaseUrl` to the public https
origin — the committed value is a localhost development URL.

`App:FrontendUrl` is a different thing: the public address of the **site**, not of this API. It is
the only thing an invitation link is built from, and it is never derived from the incoming request —
that would send invitees to the API host instead of the page they are meant to register on. The
committed value is `https://shopiklopik.com`; override it with `App__FrontendUrl` if the site moves.
Start-up **fails** outside Development when the key is missing, is not an absolute `http`/`https`
URL, or points at localhost.

`DemoData:Enabled` is `false` in `appsettings.json` and in `appsettings.Production.json`, and
`DevelopmentDataSeeder` refuses to run in any environment other than Development regardless of the
flag. A Production database therefore comes up with **reference data only** — categories,
sub-categories, the governorate and its centers, every module lookup, the home sections, the platform
settings row, the payment methods and the banner placements. No users, listings, notifications,
comments, referrals or payments are ever seeded.

### `web.config`

Not used on Linux — Kestrel never reads it, and `dotnet publish` ships it regardless. It matters only
if the application is published to IIS on Windows. Two things matter there:

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
`X-Forwarded-Host`. This is what makes generated upload URLs use the real public domain and scheme,
what makes HSTS and `UseHttpsRedirection` see the original https request, and what makes rate
limiting partition by the real client IP rather than the proxy's.

**Only loopback is trusted by default** — which is exactly Nginx on the same host. Headers arriving
from anywhere else are ignored, so an internet client cannot forge its own IP, scheme or host.
Configured in `MarkatPlace/Extensions/ForwardedHeadersExtensions.cs`, tuned by the
`ForwardedHeaders` section:

| Key | Default | Use when |
| --- | --- | --- |
| `KnownProxies` | *(empty)* | Nginx connects from another host — list its address |
| `KnownNetworks` | *(empty)* | The proxy's address varies — list its CIDR range |
| `ForwardLimit` | `1` | `2` if a CDN sits in front of Nginx |
| `TrustAnyProxy` | `false` | Last resort; see the warning below |

> If Nginx does **not** connect from loopback and its address is not listed, the headers are silently
> ignored: generated URLs lose `https`, and every client lands in one rate-limit partition keyed on
> the proxy's IP. That is the failure to look for first.

> `TrustAnyProxy: true` honours the headers from any caller. It is only safe when something upstream
> strips client-supplied `X-Forwarded-*` headers — this was the behaviour before the Linux/Nginx
> move, when the application ran in-process under IIS.

Nginx must send them. `proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;` and
`proxy_set_header X-Forwarded-Proto $scheme;` are the two that matter; add
`X-Forwarded-Host $host` if the public host differs from the one Nginx proxies to. WebSockets for
SignalR need `proxy_http_version 1.1`, `proxy_set_header Upgrade $http_upgrade;` and
`proxy_set_header Connection "upgrade";` on the `/hubs/` location, plus a `proxy_read_timeout` longer
than the hub's idle period.

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
- [ ] An https request returns `Strict-Transport-Security` — proof `X-Forwarded-Proto` was trusted
- [ ] An image URL in an API response starts `https://<public host>`, not `http://` or `localhost`
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
