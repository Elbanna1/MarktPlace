# 01 — Project Overview

[← Start Here](00-START-HERE.md)

Everything in this document was read out of the repository. Nothing is inferred from convention, and
no technology is listed that the code does not actually reference.

---

## Product

| | |
| --- | --- |
| **Product name** | **Shopiklopik** (Arabic: ماركت بليس) |
| **Public site** | `https://shopiklopik.com` |
| **Public API** | `https://api.shopiklopik.com` |
| **Repository / solution name** | `MarkatPlace` — the original name, still used everywhere in code |
| **Market** | Fayoum governorate (الفيوم), Egypt — a single governorate, deliberately |
| **User-facing language** | Egyptian Arabic, for **every** message, error, notification and e-mail |
| **Maintainer** | Abdullah Elbanna (contact details in the repository `README.md`) |

> **Naming:** the *brand* is Shopiklopik; the *codebase* is MarkatPlace. Both appear, and that is
> expected. The rebrand touched user-visible strings (e-mail branding, the frontend URL, CORS
> origins, the Nginx site name) and deliberately did **not** touch namespaces, project names,
> assembly names or the database. See [33-ARCHITECTURE-DECISIONS.md](33-ARCHITECTURE-DECISIONS.md).

---

## Backend purpose

The backend is the **only** source of business logic in the platform. That is a deliberate product
decision with a concrete consequence you will see everywhere:

> The frontend holds **zero** business rules. It asks
> `GET /api/lookups/create-ad-form/{categoryId}/{subCategoryId}` and renders whatever fields,
> validation rules, lookup options and submit URL come back.

So the backend owns:

- what fields a listing type has, which are required, and which appear only under a condition;
- what a valid value is, and the Arabic sentence shown when it is not;
- whether a listing is visible, to whom, and for how long;
- who may approve, reject, suspend or delete anything;
- every piece of text a user ever sees.

---

## Technology inventory

### Runtime and language

| Item | Value | Where it is declared |
| --- | --- | --- |
| Target framework | `net10.0` | every `*.csproj` |
| SDK | `Microsoft.NET.Sdk.Web` (host), `Microsoft.NET.Sdk` (libraries) | `*.csproj` |
| Nullable reference types | `enable` | every `*.csproj` |
| Implicit usings | `enable` | every `*.csproj` |
| XML documentation file | generated for the host (`GenerateDocumentationFile`) | `MarkatPlace.csproj` |

### Packages actually referenced

| Package | Version | Project | Purpose |
| --- | --- | --- | --- |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.10 | host | JWT bearer authentication |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.10 | host | design-time migrations support |
| `Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore` | 10.0.10 | host | the `/health` database check |
| `Swashbuckle.AspNetCore` | 9.0.6 | host | Swagger / OpenAPI |
| `FluentValidation` + `.DependencyInjectionExtensions` | 12.1.1 | host, Services, tests | request validation |
| `Microsoft.EntityFrameworkCore.InMemory` | 10.0.10 | tests | hermetic repository tests |
| `xunit` / `xunit.runner.visualstudio` / `Microsoft.NET.Test.Sdk` | 2.9.3 / 3.1.0 / 17.14.1 | tests | test framework |
| MailKit / MimeKit | (referenced by `Presitance`) | persistence | SMTP e-mail |
| AutoMapper | (referenced by `Services`) | services | entity ↔ DTO mapping |

### Architecture of the API

| Aspect | Choice |
| --- | --- |
| Style | REST-ish JSON over HTTP, controller-based MVC (`AddControllers`) |
| Envelope | Every response is `ApiResponse` / `ApiResponse<T>` — `{ success, message, data, errors }` |
| Versioning | Two OpenAPI **documents**, not two route versions: public `v1`, admin `v2` (`/api/v2/admin/**`) |
| Content types | JSON everywhere except listing creation/editing, which is **multipart/form-data** (files) |
| Errors | Arabic messages, never technical detail. 400 / 401 / 402 / 403 / 404 / 409 / 413 / 422 / 429 / 500 |
| Pagination | `PaginationParams` — `pageIndex` (1-based), `pageSize` (max 50), returns `PaginatedResult<T>` |

### Data

| Aspect | Choice |
| --- | --- |
| Engine | SQL Server |
| ORM | EF Core 10, code-first |
| Context | one — `AppDbContext : IdentityDbContext<ApplicationUser>` |
| Migrations | 60 migration classes in `Infastrucre/Presitance/Migrations/` |
| Migration application | automatic, at start-up, in `Program.cs` |
| Retry policy | `EnableRetryOnFailure` (5 attempts, 10 s cap) — configurable under `Database:` |
| Command timeout | 60 s by default (`Database:CommandTimeoutSeconds`) |
| Transport security | `Encrypt=True` in the connection string |

### Identity and authorization

| Aspect | Choice |
| --- | --- |
| Framework | ASP.NET Core Identity (`AddIdentityCore` + roles + EF stores + default token providers) |
| Token | JWT, HMAC-SHA256, 40-day access token, 60-day refresh token |
| Roles | `Admin`, `SuperAdmin` |
| Fine-grained | per-page permission grants (`AdminPageGrant`), checked against the database per request |
| External | Google sign-in, ID-token verification server-side; authorization-code flow also supported |
| Password policy | ≥ 8 chars, upper + lower + digit + non-alphanumeric; lockout after 5 failures for 15 min |

### Everything else

| Concern | Technology |
| --- | --- |
| Real-time | SignalR hub `/hubs/notifications`, `[Authorize]`, token accepted via `?access_token=` |
| E-mail | MailKit `SmtpClient` → Gmail SMTP (`smtp.gmail.com:587`, STARTTLS) |
| Caching | ASP.NET Core response caching + ETags, plus an in-process `IMemoryCache` with a version stamp |
| Rate limiting | built-in `AddRateLimiter`, sliding window, partitioned by user id or client address |
| Compression | Brotli + Gzip response compression, `EnableForHttps` |
| Background work | 3 `BackgroundService`s (listing lifecycle, banner lifecycle, view-queue flush) |
| Reverse proxy | Nginx (site file committed at `deploy/nginx/api.shopiklopik.com.conf`) |
| Process host | systemd on Ubuntu, `dotnet MarkatPlace.dll` |
| TLS | Let's Encrypt, terminated at Nginx |

---

## Scale of the domain

| Dimension | Count | Source |
| --- | --- | --- |
| Categories | 12 | `CategoryType` enum |
| Sub-categories | 52 | `SubCategoryType` enum |
| Listing modules | 49 | `ListingModuleType` enum |
| Controllers | 87 | `Infastrucre/Presantion/Controllers/**` |
| Repositories registered | ~70 | `Persistence/DependencyInjection.cs` |
| Services registered | ~80 | `Services/DependencyInjection.cs` |
| AutoMapper profiles | ~50 | `Services/DependencyInjection.cs` |
| EF migrations | 60 | `Presitance/Migrations/` |
| Tests | 2,217 | `dotnet test`, run 2026-09-25 |

### The 12 categories

| Id | Arabic | English | Sub-category ids |
| --- | --- | --- | --- |
| 1 | سيارات | Cars | 1–4 |
| 2 | ورش وحرفيين | Workshops & Craftsmen | 5–6 |
| 3 | المفقودات | Lost & Found | 7–8 |
| 4 | رجال أعمال | Business | 9–14 |
| 5 | الوظائف | Jobs | 15–16 |
| 6 | الحيوانات | Animals | 17–25 |
| 7 | التحف والأنتيكات | Antiques | 26–30 |
| 8 | الملابس | Clothing | 31–33 |
| 9 | التسوق أونلاين | Online Shopping | 34–39 |
| 10 | افرش بيتك | Home Furnishing | 40–46 |
| 11 | عقارات | Real Estate | 47–49 |
| 12 | بوابة الخيرات | Charity | 50–52 |

Category 1 (Cars) is the only one still stored in the shared `Advertisements` table; every other
category has one table per sub-category. See [12-LISTINGS.md](12-LISTINGS.md).

---

## What is *not* in this system

Stated explicitly so you do not go looking:

- **No payment gateway.** Payments are manual: the user uploads a screenshot of a bank/wallet
  transfer and an administrator approves it.
- **No message queue, no Redis, no distributed cache.** All caching is in-process.
- **No container images, no Kubernetes, no CI/CD pipeline** committed to the repository.
- **No distributed lock** — background services run on every instance (see
  [20-BACKGROUND-SERVICES.md](20-BACKGROUND-SERVICES.md)).
- **No structured logging sink** (Serilog/Seq/ELK). Logging uses the default ASP.NET Core providers.
- **No object storage.** Uploaded files live on the server's local disk.
- **No multi-tenancy, no multi-governorate support.** The governorate is fixed to الفيوم.
