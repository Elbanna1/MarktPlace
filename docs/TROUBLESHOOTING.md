# Troubleshooting

[← README](../README.md) · Related: [SETUP](SETUP.md) · [DEPLOYMENT](DEPLOYMENT.md) · [DATABASE](DATABASE.md)

Real problems this project produces, with the actual cause.

---

## Start-up

### `JwtSettings:SecretKey is missing or too short`

**Cause.** `appsettings.json` deliberately carries no signing key.

**Fix.** Development: check `appsettings.Development.json` still has its `JwtSettings.SecretKey`.
Production: set `JwtSettings__SecretKey` (≥ 32 characters).

---

### `JwtSettings:SecretKey is a placeholder or a value that has been published in source control`

**Cause.** The configured key is a known-compromised or placeholder value. Refused outside
Development on purpose.

**Fix.** Generate a new random secret of ≥ 32 characters and set `JwtSettings__SecretKey`.
**This signs every current user out.**

---

### `JWT settings are not configured.`

**Cause.** The whole `JwtSettings` section is missing — almost always because the **content root is
wrong**, so no `appsettings.json` was loaded.

**Fix.** Run from the project directory:

```bash
cd MarkatPlace && dotnet run
# or, running the DLL directly:
cd MarkatPlace && dotnet bin/Debug/net10.0/MarkatPlace.dll
```

Running `dotnet /abs/path/MarkatPlace.dll` from elsewhere resolves the content root to the *current*
directory and finds no configuration.

---

### HTTP 500.30 on IIS after a deployment

**Cause (most common).** An **empty-string** value in `appsettings.Production.json` for
`ConnectionStrings:DefaultConnection`, `JwtSettings:SecretKey` or `EmailSettings:*`. An empty string
*overrides* the base value, so the app starts with an invalid configuration.

**Fix.** Remove the empty keys entirely and supply the values as environment variables.

**To diagnose:** set `stdoutLogEnabled="true"` in `web.config` **briefly**, read `logs/stdout_*.log`,
then set it back to `"false"`.

---

### `PendingModelChangesWarning` on start-up

**Cause.** You ran `dotnet ef migrations add` and did not rebuild. `migrations add` builds the
**pre**-migration state, so the binary on disk lacks the new migration.

**Fix.**

```bash
dotnet build MarkatPlace.slnx
```

---

### `There is already an object named 'AspNetRoles' in the database`

**Cause.** The migrations folder was squashed: the old `__EFMigrationsHistory` rows name migrations
that no longer exist, so EF considers the baseline pending and runs it from the top.

**Fix.** `fix-migration-history.sql` at the repository root documents and repairs exactly this.
**Read it before running it** — it edits migration history.

---

### The application starts but `/health` is `Unhealthy`

**Cause.** The `database` health check cannot reach SQL Server.

**Fix.**

```bash
# Is the connection string what you think it is?
echo "$ConnectionStrings__DefaultConnection"

# Is LocalDB running?
sqllocaldb info
sqllocaldb start MSSQLLocalDB
```

The health response deliberately says only `"error": "See server logs."` — a probe must not describe
the database to anyone who can reach it. The real exception is in the logs.

---

### Port already in use / a stale instance answers

**Cause.** A previous run is still holding the port. Probes keep answering 200, so it looks like the
new build is running when it is not.

**Fix.**

```bash
netstat -ano | grep :5002
powershell -Command "Get-CimInstance Win32_Process -Filter \"Name='dotnet.exe'\" | Where-Object { \$_.CommandLine -like '*MarkatPlace.dll*' } | ForEach-Object { Stop-Process -Id \$_.ProcessId -Force }"
```

---

## Build

### `MSB3027 / MSB3021: cannot copy … because it is being used by another process`

**Cause.** A running API is holding the DLLs.

**Fix.** Stop it (command above), then rebuild.

---

### `dotnet ef` version mismatch

**Cause.** The global tool version differs from the EF Core packages (10.0.10).

**Fix.**

```bash
dotnet tool update --global dotnet-ef --version 10.0.10
```

---

## Database

### An unintended `RenameColumn` in a scaffolded migration

**Cause.** EF turns an unrelated drop + add pair into a `RenameColumn` and **silently reinterprets
the data**.

**Fix.** Always read the scaffolded migration. Remove it and split the change into two migrations,
or correct the generated code:

```bash
cd Infastrucre/Presitance
dotnet ef migrations remove --startup-project ../../MarkatPlace/MarkatPlace.csproj
```

---

### `Introducing FOREIGN KEY constraint … may cause cycles or multiple cascade paths` (error 1785)

**Cause.** A new cascading foreign key into `AspNetUsers`. With 48 listing tables, SQL Server refuses.

**Fix.** Use `DeleteBehavior.NoAction` for every relationship to `AspNetUsers`. `Cascade` is only for
a listing's own children.

---

### Arabic stored as mojibake after running a SQL script

**Cause.** `sqlcmd -i` reads a UTF-8 file as the ANSI codepage.

**Fix.**

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -d MyDb -I -f 65001 -i script.sql
```

`-I` also sets `QUOTED_IDENTIFIER ON`, which inserts into tables with filtered indexes require —
without it you get error 1934.

---

### A unique index is not preventing duplicates

**Cause.** EF added `WHERE [Col] IS NOT NULL` because the column is nullable.

**Fix.** `.HasFilter(null)` on the index.

---

### EF model-validation warnings (event 10622) on start-up

**Message.** *"Entity 'X' has a global query filter defined and is the required end of a relationship
with entity 'Y'."*

**Cause.** Known and harmless: a child entity's required parent can be filtered out by the moderation
or soft-delete filter.

**Fix.** None needed. Documented in `LostFoundImageConfiguration`.

---

## Authentication and authorization

### 401 on an endpoint that should be public

**Cause.** The action is missing `[AllowAnonymous]` — controllers carry `[Authorize]` by default.

---

### 403 from an admin endpoint even though the user is an administrator

**Causes, in order of likelihood:**

1. **The action has no `[RequireAdminPermission]`.** The filter **fails closed** and logs an
   **error**. Search the logs for *"declares no [RequireAdminPermission]"*.
2. The administrator has no grant for that page/permission.
3. The controller is missing `[AdminPage(...)]`, so no page key can be resolved.

---

### A promoted user is still refused

**Should not happen** — `DatabaseRoleClaimsTransformation` rewrites roles from the database inside
the authentication middleware, so a promotion takes effect on the next request without a re-login.

If it does happen, the transformation is not registered or the role cache is stale outside the admin
surface (it is deliberately uncached there).

---

### The password-reset flow answers "the reset session expired"

**Cause.** Step 3 was called without a successful step 2, or the verified window has closed.

**Fix.** Start again from `POST /api/auth/forgot-password`. The OTP is single-use and the session is
wiped after a successful reset.

---

### An owner gets 404 on their own listing

**Should not happen** for the details route — this was a real bug and is fixed. If it recurs:

1. Is the caller actually signed in? Anonymous is 404 by design.
2. Is the listing **deleted**? Soft delete beats ownership.
3. Is this an **interaction** endpoint (favourite, rating, comment, report)? Those go through the
   public cross-module read model and **correctly** 404 for a pending listing.
4. Does the module's repository still call `VisibleToViewer`? `OwnerVisibilityTests` catches this.

---

## API behaviour

### HTTP 415 on a create endpoint

**Cause.** Listing creates are **multipart only**. JSON gets 415.

**Fix.** Send `multipart/form-data`. A create with no files must still be multipart — in Python
`requests`, put every field in `files=` as `(name, (None, value))`.

### HTTP 422 with an `errors` array

Validation failure. **Not 400** — the built-in ModelState 400 is deliberately suppressed. The
messages are Arabic and describe the field in the user's terms.

### HTTP 413 on upload — and the CORS error that follows it

**Look at Nginx first.** Its stock `client_max_body_size` is **1 MB**, which is far below anything a
listing with photographs weighs. Nginx then answers 413 *itself*, from an HTML error page that has no
`Access-Control-Allow-Origin`, so the browser console shows a CORS error and the real cause — the
size — never appears. This is exactly what
`POST https://api.shopiklopik.com/api/workshops` was doing.

```bash
grep -r client_max_body_size /etc/nginx/          # expect 256m
sudo cp deploy/nginx/api.shopiklopik.com.conf /etc/nginx/sites-available/api.shopiklopik.com
sudo nginx -t && sudo systemctl reload nginx
```

Ceilings, innermost first:

| Limit | Value | Rejected with |
| --- | --- | --- |
| Per-file (image) | 5 MB | `400` + Arabic naming the file |
| Images per listing | 10 | `400` + Arabic |
| Any single file, before buffering | 50 MB | `400` + Arabic |
| Per-endpoint `[RequestSizeLimit]` | 256 MB on every listing form | `413` + Arabic |
| Kestrel / `FormOptions` | 256 MB | `413` + Arabic |
| **Nginx `client_max_body_size`** | **256 MB** | `413` + Arabic **with CORS headers** |
| IIS `maxAllowedContentLength` (Windows only) | 256 MB | HTML 404.13 |

`RequestSizeLimitTests` fails the build if any of these drops below `FileUploadConstants
.MaxRequestBodySizeBytes`, so a 413 in production means a *deployed* Nginx that does not match the
committed site file. See [DEPLOYMENT § Upload size](DEPLOYMENT.md#upload-size).

### A file uploads successfully but its URL answers 404

**Cause.** The format has no content-type mapping in `Program.cs`, so `ServeUnknownFileTypes = false`
refuses to serve it.

**Fix.** Add the format to `ImageFormatCatalog` / `DocumentFormatCatalog` / `VideoFormatCatalog` —
both loops in `Program.cs` are driven off those catalogues, which is what keeps this from happening.

### A valid image is rejected

Format is decided by **magic bytes**, not by name or MIME type. Check the file really is one of the
supported formats. **SVG is excluded on purpose.**

### 429 with `Retry-After`

Rate limited: 20/min on `/api/auth/*`, 600/min elsewhere. For local testing, raise
`RateLimiting__PermitPerMinute` and `RateLimiting__AuthPermitPerMinute`.

---

## CORS

CORS is an **allow-list** (`Cors:AllowedOrigins`), not `AllowAnyOrigin`. Before blaming CORS, read
the response status with `curl -i` — most reported "CORS errors" are another failure whose response
lost its headers.

1. **The origin is not listed.** Apex and `www` are different origins. Add one with
   `Cors__AllowedOrigins__0=https://example.com` — an index **replaces** that array element, so
   re-list the existing entries too.
2. **A 413 from Nginx.** The commonest false CORS error on this API. See
   [HTTP 413 on upload](#http-413-on-upload--and-the-cors-error-that-follows-it).
3. **Credentials.** `Cors__AllowAnyOrigin=true` (the escape hatch) disables credentialed requests.
   Use the bearer token in the `Authorization` header — do not send cookies.
4. **SignalR.** Pass the JWT as the `access_token` **query-string** value, not a header.
5. **An application error presenting as one.** `GlobalExceptionHandlingMiddleware` runs outside
   `UseCors` and clears the response before writing the error body; it re-applies the
   `Access-Control-*` and `Vary` headers across that clear, so a 404, 413 or 500 keeps them.
   `RequestSizeLimitTests` guards this. If you see an error response *without* those headers, it did
   not come from the application — check Nginx.

---

## Tests

### `NoEnglishInResponsesTests` fails

An English sentence reached a customer-facing call site. The failure names the file, line and text.
Move the wording into `Shared/Constants/UserMessages.cs`.

### `DeploymentSafetyTests` fails

One of: a `"SecretKey"` reappeared in committed configuration; an `AdminUser` section reached
production config; `web.config`'s `maxAllowedContentLength` dropped below the app ceiling;
`ASPNETCORE_DETAILEDERRORS` came back.

### `OwnerVisibilityTests` fails

A repository reverted to the pre-fix shape, a new module does not call `VisibleToViewer`, a listing
entity lacks a mapped `UserId`, or the Lost & Found view counter lost its
`catch (NotFoundException)`. The message names the file.

### Tests fail to build with a file-lock error

Stop the running API first.

---

## Development seeding

### Start-up is slow the first time

`DevelopmentDataSeeder` downloads ~289 images once. Skip it:

```bash
export DemoData__Enabled=false
```

It is additive and idempotent — existing rows are kept and downloaded images are reused. A failure is
logged and never stops the application.

### Demo images have no listings attached

**Cause.** The EF store-generated-key gotcha: images must be added through the `DbSet`, not through
the parent's navigation collection.

---

## Still stuck

Check [DECISIONS.md](DECISIONS.md) — the behaviour may be deliberate. Otherwise contact
**Abdullah Elbanna** ([README § Maintainer](../README.md#developer-and-project-maintainer)).
