# Security

[← README](../README.md) · Related: [AUTHENTICATION](AUTHENTICATION.md) · [AUTHORIZATION](AUTHORIZATION.md) · [DEPLOYMENT](DEPLOYMENT.md)

> This document contains **no** secrets. Every credential referenced here lives in an environment
> variable.

---

## Required production configuration

**Read this before any deployment.**

### 1. `JwtSettings__SecretKey` — the application will not start without it

`appsettings.json` deliberately carries **no** signing key. Production must supply one as an
environment variable: **at least 32 random characters**.

Start-up throws if the key is missing, shorter than 32 bytes, or a known placeholder / previously
published value (checked outside Development only).

> **Why the check exists.** The key used to live in `appsettings.json`, which is committed — so every
> copy of the source carried it, and anyone holding it could mint a valid token for **any** account,
> administrators included, without ever seeing a password. The fail-fast is what stops that
> configuration from ever running again.

> **Rotating the key signs every user out.** Access tokens live 40 days, refresh tokens 60.

## Secrets and credentials

### Rotate credentials that have ever been committed

Any credential that has been in this repository's history must be treated as **public**. Rotate it at
the source, then supply the new value as an environment variable:

| Credential | Environment variable |
| --- | --- |
| Database password | `ConnectionStrings__DefaultConnection` |
| SMTP password | `EmailSettings__Password` |

### 2. `GoogleAuth__ClientSecret` — never in a committed file

Google Sign-In needs two values. They are **not** equally sensitive:

| Value | Where it lives | Why |
| --- | --- | --- |
| `GoogleAuth:ClientId` | Committed in `appsettings.json` | Public — the browser sends it to Google on every sign-in |
| `GoogleAuth:ClientSecret` | `GoogleAuth__ClientSecret` **only** | A credential. It is used solely for the authorization-code exchange |

The secret is never returned by an endpoint (`GET /api/auth/google/config` publishes `enabled` and
`clientId` only), never logged, and never rendered into Swagger. `GoogleAuthConfigurationTests` fails
the build if a `GOCSPX-` value appears anywhere in the repository or in a committed configuration
file, and if any logging call in the sign-in path carries a credential.

Google ID tokens are verified and discarded — **no** Google access or refresh token is stored.

### 3. `AdminUser` must stay unset in production

`IdentityDataSeeder` has **no fallback credentials**: with the section unset it ensures the roles
exist and seeds **no account**. Its absence from `appsettings.Production.json` *is* the safety
mechanism.

To bootstrap the first administrator on a fresh database: set `AdminUser__UserName`, `__Email` and
`__Password` as environment variables, start once, then **remove them**.

### 4. Diagnostics stay off

| Switch | Production |
| --- | --- |
| `ASPNETCORE_DETAILEDERRORS` | **unset** — renders start-up stack traces into the browser |
| `stdoutLogEnabled` (web.config) | `"false"` |
| `Diagnostics__SqlCounter` | `false` |

---

## Authentication

See [AUTHENTICATION.md](AUTHENTICATION.md). Summary of the controls:

| Control | Implementation |
| --- | --- |
| Password policy | ≥ 8 chars, digit + lower + upper + non-alphanumeric |
| Lockout | 5 failed attempts → 15 minutes |
| Token signing | HMAC-SHA256, `ClockSkew = Zero` |
| HTTPS metadata | Required outside Development |
| No user enumeration | Login and forgot-password answer identically for known and unknown accounts |
| Password-reset replay | OTP single-use; the whole session is wiped after a successful reset |

**Verified:** a wrong password and an unknown username return the same status **and** the same
message; `alg=none` forgery, garbage tokens, empty bearers and missing prefixes all answer 401.

---

## Authorization

See [AUTHORIZATION.md](AUTHORIZATION.md).

| Control | Implementation |
| --- | --- |
| Admin surface | `[Authorize(Roles = Admin)]` on `api/v2/admin` |
| Per-page permissions | `AdminPermissionFilter`, **first** global filter |
| Fail closed | An admin action with no permission annotation is refused and logged as an error |
| Grants read per request | From the database, never from the JWT |
| Instant role changes | `DatabaseRoleClaimsTransformation` inside the authentication middleware |

**Verified:** all **131** admin operations answer **401** to an anonymous caller and **403** to a
signed-in non-administrator.

---

## Account state is enforced on every request

An access token lives **40 days** and cannot be revoked by signing out. Until 2026-09-08 the account
state was checked **only at login, refresh and Google sign-in**, so suspending or blocking an account
cleared its refresh token but left every access token already in the wild working for up to 40 days
on the whole non-admin surface. (The admin surface was never affected — `AdminPermissionService`
re-reads the status from the database on every request.)

`AccountStatusMiddleware` closes that gap, and is what makes self-service closure meaningful:

| Layer | What it does |
| --- | --- |
| `DatabaseRoleClaimsTransformation` | Reads `Status` **and** the roles in one query and stamps `markatplace:account-status` onto the principal. Any claim of that type arriving inside the token is **removed first** — the value is never taken from the caller |
| `IUserAccessStateCache` | 5-minute per-user cache. **Bypassed entirely on `/api/v2/admin`**, and invalidated the moment an administrator changes a status or an owner closes their account, so a change takes effect on the next request |
| `AccountStatusMiddleware` | Runs between `UseAuthentication` and `UseAuthorization`, so it covers MVC **and** the SignalR hub. Non-`Active` ⇒ **403** with an Arabic message; an account row that no longer exists ⇒ **401** |

**Verified at runtime:** the same bearer token answered 200 on `GET /api/profile`, then **403** on the
very next request after `PUT /api/v2/admin/users/{id}/status` set the account to Suspended.

Cost: **zero** extra round trips on the hot path — the state travels with the roles query that was
already there, and is cached for five minutes.

> The cache is **in-process**. On a single instance (the current deployment: one Kestrel process
> behind Nginx) invalidation is immediate. Behind more than one instance, a status change would take
> up to five minutes to reach the other instances — shorten `UserAccessStateCache.Lifetime` or move
> the cache out of process before scaling out.

---

## Account states

| Value | Name | Set by | Login | Existing access token |
| --- | --- | --- | --- | --- |
| 1 | `Active` | default; admin | allowed | works |
| 2 | `Suspended` | admin | 403 | **403** |
| 3 | `Blocked` | admin | 403 | **403** |
| 4 | `Deactivated` | **the owner only**, via `DELETE /api/account` | 403 | **403** |

`Deactivated` is deliberately **not** admin-assignable — `UserAccountCatalog.AdminAssignableStatuses`
is checked by both `UpdateUserStatusRequestValidator` (422) and `AdminUserService` (defence in
depth), so an administrator cannot stamp an account as closed-by-its-owner. An administrator can
still set it back to `Active` for a support request; the owner's listings stay `Suspended` and need
re-approval.

---

## IDOR protection

Ownership is enforced **in the query**, which fails closed — a mistake returns fewer rows, never
more.

```
(ModerationStatus == Approved AND window open)  OR  UserId == viewerUserId
```

`viewerUserId` comes from the authenticated principal's `NameIdentifier` claim and **nowhere else**.

**Verified refused** — user B against user A's pending listing:

| Vector | Result |
| --- | --- |
| Plain `GET` | 404 |
| `?userId=<A>`, `?ownerId=<A>`, `?viewerUserId=<A>` | 404 |
| `?includeUnmoderated=true`, `?isOwner=true`, `?moderationStatus=1` | 404 |
| Forged `X-User-Id` / `X-Owner-Id` / `nameid` headers | 404 |
| Tampered token | 401 |
| `PUT` with a fully valid payload | 404, and A's listing unchanged |
| `DELETE` | 404 |
| B's `my-listings` | Does not contain A's listing |

---

## Input validation

| Control | Implementation |
| --- | --- |
| Every request DTO validated | FluentValidation via the global `ValidationFilter` → **422** |
| ModelState 400 suppressed | FluentValidation is the single source |
| SQL injection | EF Core parameterises everything. Injection-shaped search terms are treated as text |
| Mass assignment | Server-owned fields are ignored, not trusted |

**Verified ignored** when posted: `IsFeatured`, `IsPremium`, `IsUrgent`, `ModerationStatus`,
`ViewCount`, `UserId`, `PublishedAt`, `ExpireAt`, `CreatedAt`, `IsDeleted`.

---

## Upload security

| Control | Implementation |
| --- | --- |
| Format decided by **magic bytes** | `ImageFormatCatalog.Detect` — never the name or declared MIME type |
| Stored name | A generated 32-hex GUID + the **detected** extension |
| Path traversal | Structurally impossible — the submitted name never reaches the file system |
| Executables | `LooksExecutable` rejects PE and ELF |
| **SVG excluded** | XML with no signature; can carry script that runs against our own origin |
| Size limits | 5 MB per image, 10 per listing; 10 MB CV; 50 MB video |
| Served with | `Content-Security-Policy: default-src 'none'; sandbox`, `X-Content-Type-Options: nosniff`, `ServeUnknownFileTypes = false` |

**Verified:** a PHP file, an HTML file, a script-bearing SVG and an empty file are all rejected. A
real PNG named `a.png.php` or `../../../evil.png` is **accepted** — correctly — and stored as
`<32 hex>.png` inside `/uploads/lands/`. Rejecting it would only punish users whose phone produced an
odd file name.

> Uploaded files are **public**. Anyone with the URL can read them. There is no private file store —
> do not put anything confidential there.

---

## Rate limiting

| Policy | Default | Applies to |
| --- | --- | --- |
| Global | 600 / min | Everything without another policy |
| `auth` | 20 / min | `/api/auth/*` |

Partitioned by **user id** when authenticated, otherwise by remote IP (honouring forwarded headers,
so a proxy does not collapse every visitor into one bucket). Health probes are exempt. A rejection is
429 with `Retry-After` and an Arabic message.

> Disabling limits (`RateLimiting:Enabled=false`) keeps the named `auth` policy **registered** with
> no limit. An endpoint whose policy does not exist makes the middleware throw — which would turn
> "disabled" into HTTP 500 on every login.

---

## Error handling

| Control | Implementation |
| --- | --- |
| Global handler | `GlobalExceptionHandlingMiddleware` wraps the whole pipeline |
| 500 body | `حصل خطأ غير متوقع. من فضلك حاول تاني بعد شوية.` — nothing else |
| Stack traces | Logged server-side only |
| Bodyless statuses | `UseStatusCodePages` formats 404/405/413/415 into the envelope |
| Client disconnects | Logged as information (499), not as an error |

**Verified across 14 error paths:** no exception type, SQL, file path, connection string, credential
column or internal identifier appears in any response body.

---

## Sensitive data

Never serialised in any payload:

`passwordHash` · `securityStamp` · `concurrencyStamp` · `refreshToken` · `refreshTokenExpiry` ·
`passwordResetOtp` · `passwordResetToken` · `passwordResetVerified` · `lockoutEnd` ·
`accessFailedCount` · `normalizedUserName` · `normalizedEmail` · `twoFactorEnabled`

**Verified** on the profile, notifications, admin user list and public feed payloads.

Logging rules: never log passwords, tokens, refresh tokens, or full request bodies containing
sensitive data. Do log authentication failures, authorization failures, background-job failures and
notification-delivery failures.

`AdminAuditLog` is append-only — never updated, never deleted, never soft-deleted. A trail that can
be edited is not evidence.

---

## CORS

An **allow-list**, built in `MarkatPlace/Extensions/CorsExtensions.cs` from `Cors:AllowedOrigins`
and applied as the single policy `MarkatPlaceCors`: the listed origins, any header, any method,
`AllowCredentials`.

| Key | Committed value | Purpose |
| --- | --- | --- |
| `Cors:AllowedOrigins` | the site (apex + `www`), the API host, and the usual local dev ports | The browser origins allowed to call the API |
| `Cors:AllowAnyOrigin` | `false` | Escape hatch — restores the old fully open policy without a redeploy |
| `Cors:AllowLocalhostInDevelopment` | `true` | In Development only, any loopback origin is also allowed, whatever port the developer picked |

> **This replaced a fully open `AllowAnyOrigin` policy.** The old one was defensible while the API
> was bearer-token-only, but an allow-list costs nothing here and is what makes credentialed
> requests possible at all — the CORS protocol forbids combining `*` with credentials.

**Adding an origin.** Apex and `www` are different origins, and so are `localhost` and `127.0.0.1`.
Add one with `Cors__AllowedOrigins__0=https://example.com`, remembering that an indexed environment
variable **replaces** the committed array element at that index — re-list the existing entries too.
A configured value that is not an absolute `http`/`https` origin **fails start-up** rather than being
silently ignored.

`Cors__AllowAnyOrigin=true` is the emergency revert if a production origin turns out to be missing;
it also disables credentialed requests. Uploaded files under `/uploads` carry their own
`Access-Control-Allow-Origin: *` and are unaffected either way.

---

## Security headers

Set on every response by middleware in `Program.cs`:

| Header | Value |
| --- | --- |
| `X-Content-Type-Options` | `nosniff` |
| `X-Frame-Options` | `DENY` |
| `Referrer-Policy` | `no-referrer` |
| `X-XSS-Protection` | `0` (the legacy auditor is a liability, not a control) |
| `Strict-Transport-Security` | via `UseHsts`, outside Development |

The Kestrel `Server` banner is **suppressed** (`AddServerHeader = false`).

On `/uploads/*` additionally: `Cache-Control: public,max-age=31536000,immutable`,
`Access-Control-Allow-Origin: *`, `Content-Security-Policy: default-src 'none'; sandbox`.

---

## Token security

| Property | Value | Note |
| --- | --- | --- |
| Algorithm | HMAC-SHA256 | |
| Clock skew | Zero | |
| Access lifetime | **40 days** | ⚠️ cannot be revoked before expiry |
| Refresh lifetime | 60 days | One per user |
| Logout | Clears the refresh token | The access token stays valid until it expires |
| Suspend / block / close | Clears the refresh token **and** the cached access state | The access token stops working on the **next request** — see [Account state](#account-state-is-enforced-on-every-request) |

### Token lifetimes

A 40-day access token is a deliberate long-session product decision, documented in `JwtSettings`. The
consequence is that a **leaked** access token cannot be revoked for over a month, and logout does not
invalidate it. Revoking the *account* is now enough, however: suspending, blocking or closing it
stops every token it holds on the next request.

If that trade-off is ever revisited, shortening `AccessTokenExpirationDays` (to hours or days) and
relying on the refresh token would restore revocability. `RefreshTokenExpirationDays` must remain
greater than `AccessTokenExpirationDays`, which start-up enforces.

---

## Production checklist

- [ ] `JwtSettings__SecretKey` set (≥ 32 random chars, never committed)
- [ ] Database password rotated and supplied via `ConnectionStrings__DefaultConnection`
- [ ] SMTP password rotated and supplied via `EmailSettings__Password`
- [ ] `AdminUser` unset (or set once, then removed)
- [ ] `ASPNETCORE_DETAILEDERRORS` unset
- [ ] `stdoutLogEnabled="false"` in `web.config`
- [ ] `Diagnostics__SqlCounter` false
- [ ] `GoogleAuth__ClientSecret` set as an environment variable if the authorization-code flow is used, and absent from every committed file
- [ ] `Cors__AllowedOrigins` lists every origin the site is actually served from
- [ ] `SwaggerAuth__Username` and `SwaggerAuth__Password` set (long, random, never committed) — without both, `/swagger` answers 401 to everyone
- [ ] HTTPS enforced; HSTS active
- [ ] `web.config` `maxAllowedContentLength` ≥ the application's ceiling
- [ ] Health probes reachable by the load balancer
- [ ] CORS reviewed against the actual frontend origins
