# 05 — Configuration

[← Start Here](00-START-HERE.md) · Related: [06-ENVIRONMENT-VARIABLES](06-ENVIRONMENT-VARIABLES.md) ·
[22-SECURITY](22-SECURITY.md)

Every configuration section the application actually reads, what it does, and what happens when it is
wrong. **No real secret appears in this document.**

---

## How configuration is layered

```
appsettings.json                    base — committed, no secrets should live here
  → appsettings.{Environment}.json  per-environment — committed
    → environment variables         highest precedence — where secrets belong
```

Section separator for environment variables is `__` (double underscore):
`JwtSettings:SecretKey` → `JwtSettings__SecretKey`.

The three committed files:

| File | Purpose |
| --- | --- |
| `MarkatPlace/appsettings.json` | base values: URLs, CORS list, rate limits, database retry, JWT issuer/audience/lifetimes, SMTP host and sender |
| `MarkatPlace/appsettings.Development.json` | the development JWT key, the development administrator, `DemoData:Enabled = true` |
| `MarkatPlace/appsettings.Production.json` | production logging levels, `App:FrontendUrl`, `ForwardedHeaders`, and a set of `_README*` keys that document the production contract in place |

> The `_README*` keys in `appsettings.Production.json` are **not settings** — they are long
> explanatory strings deliberately kept next to the configuration they describe. Read them; they
> record hard-won production lessons.

---

## Start-up validation — the application refuses to start when these are wrong

This is a real feature, not an accident. Failing at start-up beats running mis-configured.

| Check | Where | Failure |
| --- | --- | --- |
| `JwtSettings:SecretKey` present and ≥ 32 bytes | `JwtAuthenticationExtensions` | `InvalidOperationException` |
| `JwtSettings:SecretKey` is not a known-published placeholder (non-Development) | `JwtAuthenticationExtensions` | `InvalidOperationException` |
| `AccessTokenExpirationDays > 0` | `JwtAuthenticationExtensions` | `InvalidOperationException` |
| `RefreshTokenExpirationDays > AccessTokenExpirationDays` | `JwtAuthenticationExtensions` | `InvalidOperationException` |
| `ConnectionStrings:DefaultConnection` non-empty | `Persistence.DependencyInjection` | `InvalidOperationException` |
| `App:FrontendUrl` present, absolute http/https, not loopback (non-Development) | `AppUrlSettingsExtensions` | `InvalidOperationException` |
| every `Cors:AllowedOrigins` entry is an absolute http/https origin | `CorsExtensions` | `InvalidOperationException` |
| `ForwardedHeaders:ForwardLimit ≥ 1`, `KnownProxies` parse as IPs, `KnownNetworks` parse as CIDR | `ForwardedHeadersExtensions` | `InvalidOperationException` |

---

## Section reference

### `ConnectionStrings`

| Key | Required | Dev | Prod | Notes |
| --- | --- | --- | --- | --- |
| `DefaultConnection` | ✅ | override locally | **environment variable** | SQL Server connection string |

Safe example:

```
Server=<host>;Database=<db>;User Id=<user>;Password=<SQL_PASSWORD>;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;
```

`Encrypt=True` encrypts the wire. `TrustServerCertificate=True` is set because the managed host
presents a certificate from an authority that is not publicly trusted — it means the connection is
encrypted but the **server is not authenticated**. See [22-SECURITY.md](22-SECURITY.md).

> ⚠️ The committed `appsettings.json` contains a **real hosted connection string including a
> password**. Treat that credential as public and rotate it. See
> [34-TECHNICAL-DEBT.md](34-TECHNICAL-DEBT.md).

---

### `Database`

| Key | Default | Purpose |
| --- | --- | --- |
| `MaxRetryCount` | `5` | EF Core retry attempts on transient failures |
| `MaxRetryDelaySeconds` | `10` | cap on the exponential back-off |
| `CommandTimeoutSeconds` | `60` | per-command timeout against a remote database host |

Because a retrying strategy is enabled, **explicit transactions must go through
`IUnitOfWork.ExecuteInTransactionAsync`**. Calling `BeginTransactionAsync` directly throws.

---

### `JwtSettings`

| Key | Committed value | Required in prod | Notes |
| --- | --- | --- | --- |
| `Issuer` | `MarkatPlaceAPI` | inherited | Validated on every token |
| `Audience` | `MarkatPlaceClient` | inherited | Validated on every token |
| `SecretKey` | **absent from `appsettings.json`**; a development key is in `appsettings.Development.json` | ✅ **start-up fails without it** | ≥ 32 random characters, HMAC-SHA256 |
| `AccessTokenExpirationDays` | `40` | inherited | Note: **days**, not minutes |
| `RefreshTokenExpirationDays` | `60` | inherited | Must exceed the access-token lifetime |

> Changing `SecretKey` invalidates **every** issued access and refresh token — every user is signed
> out. Announce it before doing it.

Generate one:

```bash
openssl rand -base64 48
```

---

### `EmailSettings`

| Key | Committed value | Required in prod | Notes |
| --- | --- | --- | --- |
| `SmtpServer` | `smtp.gmail.com` | inherited | |
| `Port` | `587` | inherited | 587 → STARTTLS; 465 → implicit SSL |
| `Email` | `shopiklopiksupport@gmail.com` | inherited | sender address **and** SMTP username |
| `DisplayName` | `Shopiklopik` | inherited | the "From" name |
| `LogoUrl` | *(unset)* | optional | absolute URL of a logo for the e-mail header; falls back to a text monogram |
| `EnableSsl` | `true` | inherited | |
| `Password` | **never committed** | ✅ | `EmailSettings__Password` — a Gmail **app password** |

Without `EmailSettings__Password` the SMTP login fails and **password-reset mail is silently never
delivered** — the API still answers 200, because `ForgotPasswordAsync` catches and logs send
failures on purpose (so the endpoint cannot be used to enumerate accounts).

---

### `App`

| Key | Committed value | Purpose |
| --- | --- | --- |
| `BaseUrl` | `https://localhost:7139` | Fallback origin for building uploaded-file URLs **outside an HTTP request** (background jobs). Inside a request the real scheme/host is used. Set it to the public API origin in production. |
| `FrontendUrl` | `https://shopiklopik.com` | The public address of the **site**. The only thing invitation links are built from. Start-up fails outside Development if missing/loopback/relative. |
| `RegisterPath` | `/register` | Appended to `FrontendUrl` for invitation links |
| `ReferralQueryParameter` | `ref` | The query-string key → `https://shopiklopik.com/register?ref=CODE` |
| `CreateAdPath` | `/create-product` (default) | Used in notification deep links |
| `HomePath` | `/` (default) | Used in notification deep links |

`BaseUrl` (API) and `FrontendUrl` (site) are deliberately different things. Invitation links are
never derived from the incoming request, because that would send invitees to the API host.

---

### `Cors`

| Key | Committed value | Purpose |
| --- | --- | --- |
| `AllowedOrigins` | the two production origins, the API origin, and `localhost:5173/3000/4200`, `127.0.0.1:5173` | Browser allow-list. Credentialed requests are enabled for these. |
| `AllowAnyOrigin` | `false` | Emergency escape hatch — restores the old permissive policy **and disables credentials** |
| `AllowLocalhostInDevelopment` | `true` | In Development, any loopback origin is also accepted |

Apex and `www` are **different origins**; both are listed. Overriding an array from the environment
replaces one element at a time (`Cors__AllowedOrigins__0=…`), so re-list the entries you want to keep.

Uploads under `/uploads` are served with their own `Access-Control-Allow-Origin: *` and are
unaffected by this policy.

---

### `GoogleAuth`

| Key | Committed | Required | Notes |
| --- | --- | --- | --- |
| `ClientId` | ✅ (public by design — the browser sends it to Google anyway) | for Google sign-in | |
| `ClientSecret` | ❌ never | only for the authorization-**code** flow | `GoogleAuth__ClientSecret` |
| `RedirectUri` | `postmessage` | | the default popup flow |
| `RequireVerifiedEmail` | `true` | | `false` accepts accounts whose Google e-mail is unverified |
| `LinkVerifiedEmailToExistingAccount` | `true` | | `false` refuses to link Google to an existing password account with the same verified e-mail |

`GET /api/auth/google/config` publishes only `enabled` and `clientId` — never the secret.

---

### `RateLimiting`

| Key | Committed value |
| --- | --- |
| `Enabled` | `true` |
| `Anonymous.PermitPerWindow` / `WindowSeconds` / `SegmentsPerWindow` / `QueueLimit` | `1200` / `60` / `6` / `0` |
| `Authenticated.*` | `600` / `60` / `6` / `0` |
| `Auth.*` | `20` / `60` / `4` / `0` |

Full explanation of *why* these numbers: [19-RATE-LIMITING.md](19-RATE-LIMITING.md).

---

### `ForwardedHeaders` (production)

| Key | Committed value | Use when |
| --- | --- | --- |
| `ForwardLimit` | `1` | `2` if a CDN sits in front of Nginx |
| `KnownProxies` | `[]` | Nginx connects from another host — list its IP |
| `KnownNetworks` | `[]` | the proxy's address varies — list its CIDR |
| `TrustAnyProxy` | `false` | last resort only; safe only if something upstream strips client `X-Forwarded-*` |

Loopback is **always** trusted, which covers Nginx on the same host.

---

### `SwaggerAuth`

| Key | Committed | Effect |
| --- | --- | --- |
| `Username` | ❌ never | HTTP Basic user for `/swagger` |
| `Password` | ❌ never | HTTP Basic password for `/swagger` |

Outside Development, if **either** is missing the gate **fails closed**: every `/swagger` path
answers 401, a warning is logged at start-up, and the rest of the API is untouched. Losing the
Swagger password can never take the site down.

---

### `DemoData`

| Key | `appsettings.json` | Development | Production |
| --- | --- | --- | --- |
| `Enabled` | `false` | `true` | `false` |

Belt and braces: `DevelopmentDataSeeder` also refuses to run in any environment other than
Development, regardless of the flag.

---

### `AdminUser` (bootstrap only)

| Key | Where |
| --- | --- |
| `UserName`, `Email`, `Password`, `Phone` | `appsettings.Development.json` only |

`IdentityDataSeeder` has **no fallback credentials**. With the section unset it only ensures the
`Admin` and `SuperAdmin` roles exist. To bootstrap a production administrator: set
`AdminUser__UserName`, `AdminUser__Email`, `AdminUser__Password` as environment variables, start
once, then **remove them**.

There is also a one-time safety net: if no `SuperAdmin` exists, the oldest `Admin` is promoted so the
dashboard stays reachable.

---

### `ListingInteractions`

| Key | Default | Purpose |
| --- | --- | --- |
| `ViewDedupeWindowMinutes` | `360` (6 h) | how long before the same viewer counts as a new view |
| `RecentlyViewedRetentionCount` | `100` | rows kept per user in "recently viewed" |

---

### `Diagnostics`

| Key | Default | Purpose |
| --- | --- | --- |
| `SqlCounter` | `false` | adds `X-Sql-Count`, `X-Sql-Ms`, `X-Elapsed-Ms` response headers and registers an EF command interceptor |

Safe to enable temporarily in production to diagnose a slow endpoint; it adds an interceptor to every
command, so turn it off afterwards.

---

### `Logging`

| Environment | `Default` | `Microsoft.AspNetCore` | `Microsoft.EntityFrameworkCore` | `MarkatPlace` |
| --- | --- | --- | --- | --- |
| base / Development | `Information` | `Warning` | — | — |
| Production | `Warning` | `Warning` | `Warning` | `Information` |

---

## Settings classes (the typed view)

Each section binds to a POCO in `Shared/Settings/`:

| Section | Class | Bound in |
| --- | --- | --- |
| `JwtSettings` | `JwtSettings` | `Persistence.DependencyInjection` |
| `EmailSettings` | `EmailSettings` | `Persistence.DependencyInjection` |
| `App` | `AppSettings` | `Persistence.DependencyInjection` + validated in the host |
| `GoogleAuth` | `GoogleAuthSettings` | `Persistence.DependencyInjection` |
| `Cors` | `CorsSettings` | `CorsExtensions` |
| `RateLimiting` | `RateLimitingSettings` | `RateLimitingExtensions` |
| `SwaggerAuth` | `SwaggerAuthSettings` | `Program.cs` |
| `ListingInteractions` | `ListingInteractionSettings` | `Program.cs` |
| *(computed)* | `FileStorageSettings` | `Program.cs` — the uploads root is pinned, not configured |

Several of these POCOs normalise in their setters (trimming slashes, dropping blanks, clamping
numbers), so an oddly formatted value is often corrected rather than rejected.

---

## An `.env`-style reference

The application does **not** read `.env` files — this is only a convenient list to copy into a
systemd unit, a shell profile, or a secrets manager.

```bash
# ---- required in production ----
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Server=<host>;Database=<db>;User Id=<user>;Password=<SQL_PASSWORD>;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;
JwtSettings__SecretKey=<48+ RANDOM CHARACTERS>
EmailSettings__Password=<SMTP_APP_PASSWORD>
App__FrontendUrl=https://shopiklopik.com
App__BaseUrl=https://api.shopiklopik.com
ASPNETCORE_URLS=http://127.0.0.1:5000

# ---- optional ----
SwaggerAuth__Username=<SWAGGER_USER>
SwaggerAuth__Password=<SWAGGER_PASSWORD>
GoogleAuth__ClientSecret=<GOOGLE_CLIENT_SECRET>
Diagnostics__SqlCounter=false
RateLimiting__Anonymous__PermitPerWindow=1200

# ---- bootstrap only: set once, start once, then DELETE ----
# AdminUser__UserName=<ADMIN_USERNAME>
# AdminUser__Email=<ADMIN_EMAIL>
# AdminUser__Password=<ADMIN_PASSWORD>
```

Never commit a file containing the real values of any of the above.
