# 06 — Environment Variables

[← Start Here](00-START-HERE.md) · Related: [05-CONFIGURATION](05-CONFIGURATION.md) ·
[29-SYSTEMD](29-SYSTEMD.md) · [22-SECURITY](22-SECURITY.md)

Environment variables are the **highest-precedence** configuration source and the **only** place
secrets belong.

---

## The naming rule

| Configuration path | Environment variable |
| --- | --- |
| `JwtSettings:SecretKey` | `JwtSettings__SecretKey` |
| `ConnectionStrings:DefaultConnection` | `ConnectionStrings__DefaultConnection` |
| `Cors:AllowedOrigins[0]` | `Cors__AllowedOrigins__0` |
| `RateLimiting:Anonymous:PermitPerWindow` | `RateLimiting__Anonymous__PermitPerWindow` |

`:` becomes `__` (two underscores). Array elements are indexed from `0`, and setting an index
**replaces that element** — it does not append. If you override `Cors__AllowedOrigins__0` you must
re-list every other origin you still want.

> ⚠️ **Never set a variable to an empty string.** An empty string *overrides* the base value. This
> has caused a production outage here before: empty `ConnectionStrings__DefaultConnection` and
> `JwtSettings__SecretKey` values produced an HTTP 500.30 start-up failure. To inherit the base
> value, **do not define the variable at all**.

---

## Required in production

| Variable | Why | Failure if missing |
| --- | --- | --- |
| `ASPNETCORE_ENVIRONMENT=Production` | selects `appsettings.Production.json` and enables the production security posture | Development behaviour in production: no HSTS, open Swagger, demo-seeding attempts |
| `ConnectionStrings__DefaultConnection` | the database | **start-up fails** |
| `JwtSettings__SecretKey` | token signing | **start-up fails** — deliberate |
| `EmailSettings__Password` | SMTP login | app starts, but password-reset mail is **never delivered** (failures are logged, not surfaced) |
| `App__FrontendUrl` | invitation links | **start-up fails** outside Development if missing, relative, or loopback |
| `ASPNETCORE_URLS` | where Kestrel listens | defaults to `http://localhost:5000`; must match the Nginx `upstream` |

## Strongly recommended in production

| Variable | Example | Why |
| --- | --- | --- |
| `App__BaseUrl` | `https://api.shopiklopik.com` | the fallback origin for file URLs built outside a request (background jobs). The committed value is a localhost dev URL. |
| `SwaggerAuth__Username` / `SwaggerAuth__Password` | — | without **both**, `/swagger` answers 401 to everyone |
| `DOTNET_PRINT_TELEMETRY_MESSAGE=false` | — | quieter service logs |

## Optional

| Variable | Default | Purpose |
| --- | --- | --- |
| `GoogleAuth__ClientSecret` | unset | only needed for the authorization-**code** Google flow; the default ID-token flow needs no secret |
| `GoogleAuth__ClientId` | committed public value | override only if the OAuth client is replaced |
| `GoogleAuth__RequireVerifiedEmail` | `true` | `false` accepts unverified Google e-mails |
| `GoogleAuth__LinkVerifiedEmailToExistingAccount` | `true` | `false` refuses linking Google to an existing password account |
| `Cors__AllowedOrigins__0`, `__1`, … | committed list | override the browser allow-list |
| `Cors__AllowAnyOrigin` | `false` | emergency escape hatch (also disables credentialed requests) |
| `RateLimiting__Enabled` | `true` | `false` disables rate limiting entirely — emergency only |
| `RateLimiting__Anonymous__PermitPerWindow` | `1200` | per-minute ceiling for anonymous callers |
| `RateLimiting__Authenticated__PermitPerWindow` | `600` | per-minute ceiling per signed-in account |
| `RateLimiting__Auth__PermitPerWindow` | `20` | per-minute ceiling on login/register/reset |
| `Database__MaxRetryCount` | `5` | transient-failure retries |
| `Database__CommandTimeoutSeconds` | `60` | per-command timeout |
| `Diagnostics__SqlCounter` | `false` | adds `X-Sql-Count` / `X-Sql-Ms` / `X-Elapsed-Ms` headers |
| `ForwardedHeaders__ForwardLimit` | `1` | `2` with a CDN in front of Nginx |
| `ForwardedHeaders__KnownProxies__0` | — | the proxy's IP, if it is not on loopback |
| `ForwardedHeaders__KnownNetworks__0` | — | the proxy's CIDR range |
| `ListingInteractions__ViewDedupeWindowMinutes` | `360` | view de-duplication window |
| `DemoData__Enabled` | `false` outside Development | demo seeding (Development only regardless) |
| `ASPNETCORE_HTTPS_PORT` | unset | set to `443` if you want the **application** to issue the http→https redirect instead of Nginx |

## Bootstrap only — set once, start once, then delete

| Variable | Purpose |
| --- | --- |
| `AdminUser__UserName` | creates the first administrator on a fresh database |
| `AdminUser__Email` | |
| `AdminUser__Password` | |
| `AdminUser__Phone` | optional; defaults to `01000000000` |

Leaving these set means the credentials sit in the service unit indefinitely. Remove them after the
account exists, and reload the unit.

## Development-only

| Variable | Purpose |
| --- | --- |
| `MARKATPLACE_CONNECTION` | read by `AppDbContextFactory` for design-time `dotnet ef` commands. **Not** the same variable the running app uses. |
| `DemoData__Enabled=false` | skip demo seeding for a faster, offline start-up |

---

## Which variables are secrets

| Secret | Rotate by |
| --- | --- |
| `ConnectionStrings__DefaultConnection` (contains the SQL password) | changing the SQL login password, then updating the variable |
| `JwtSettings__SecretKey` | generating a new key — **signs every user out** |
| `EmailSettings__Password` | issuing a new Gmail app password |
| `GoogleAuth__ClientSecret` | rotating it in the Google Cloud console |
| `SwaggerAuth__Password` | changing the variable |
| `AdminUser__Password` | irrelevant after bootstrap — delete the variable |

> **Any credential that has ever been committed to this repository must be treated as public and
> rotated**, including the database password and the SMTP password. See
> [34-TECHNICAL-DEBT.md](34-TECHNICAL-DEBT.md).

---

## Where to set them, by environment

### Local development

Prefer .NET user secrets — stored outside the repository:

```bash
cd MarkatPlace
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<local connection string>"
```

Or export in your shell (see [04-DEVELOPMENT-SETUP.md](04-DEVELOPMENT-SETUP.md)).

### Production (systemd)

Put them in the unit file's `Environment=` lines, or — better — in a root-owned environment file:

```ini
# /etc/shopiklopik/api.env      chmod 600, owned by root
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://127.0.0.1:5000
ConnectionStrings__DefaultConnection=Server=<host>;Database=<db>;User Id=<user>;Password=<SQL_PASSWORD>;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;
JwtSettings__SecretKey=<48+ RANDOM CHARACTERS>
EmailSettings__Password=<SMTP_APP_PASSWORD>
App__FrontendUrl=https://shopiklopik.com
App__BaseUrl=https://api.shopiklopik.com
SwaggerAuth__Username=<SWAGGER_USER>
SwaggerAuth__Password=<SWAGGER_PASSWORD>
```

```ini
# in the unit
[Service]
EnvironmentFile=/etc/shopiklopik/api.env
```

```bash
sudo chmod 600 /etc/shopiklopik/api.env
sudo chown root:root /etc/shopiklopik/api.env
```

An environment file keeps secrets out of `systemctl show` output and out of the unit file, which is
often copied around.

Full unit: [29-SYSTEMD.md](29-SYSTEMD.md).

---

## Verifying what the running process actually has

```bash
# the PID of the service
systemctl show -p MainPID --value shopiklopik-api

# its environment (root only) — redact before sharing
sudo tr '\0' '\n' < /proc/$(systemctl show -p MainPID --value shopiklopik-api)/environ

# does the app agree? these two must both succeed:
curl -f https://api.shopiklopik.com/health/live
curl -f https://api.shopiklopik.com/health
```

If `/health` reports unhealthy, the connection string is the first thing to check.
