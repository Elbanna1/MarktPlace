# Architecture

[← README](../README.md) · Related: [PROJECT_STRUCTURE](PROJECT_STRUCTURE.md) · [DECISIONS](DECISIONS.md) · [MODULES](MODULES.md)

---

## The shape of the problem

MarkatPlace has **48 independent listing modules**. A camel and a flat share almost nothing: different
fields, different lookups, different validation, different filters. Copying a base class over them
would force a shape that does not fit; copying the code 48 times would guarantee drift.

The answer this codebase settles on: **each module owns its own vertical slice, and every
cross-cutting rule lives in exactly one shared seam.** Understanding those seams is the fastest way
to understand the system — they are listed in [§ Seams](#seams) below.

---

## Projects and layers

Onion architecture. Dependencies point inward. `Domain` references nothing but `Shared`.

| Project | Namespace | Contains | May depend on |
| --- | --- | --- | --- |
| `Core/Domain` | `Domain.Entities` | Entities only | `Shared` |
| `Core/ServicesAbstraction` | `ServicesAbstraction` | Service + repository interfaces | `Domain`, `Shared` |
| `Core/Services` | `Services` | Business logic, validators, catalogues, mapping | `ServicesAbstraction`, `Domain`, `Shared` |
| `Infastrucre/Presitance` | **`Persistence`** | `AppDbContext`, EF configurations, repositories, migrations, background jobs, file/e-mail/JWT services | `ServicesAbstraction`, `Domain`, `Shared` |
| `Infastrucre/Presantion` | **`Presentation`** | Controllers only | `ServicesAbstraction`, `Shared` |
| `Shared` | `Shared.*` | DTOs, enums, constants/catalogues, exceptions, `ApiResponse` | — |
| `MarkatPlace` | `MarkatPlace` | Host: `Program.cs`, middleware, global filters, SignalR, `wwwroot` | all |

> **Folder names are misspelled; namespaces are not.** `Presitance` → `Persistence`,
> `Presantion` → `Presentation`. Do not "fix" this — see [DECISIONS § D-01](DECISIONS.md#d-01-the-misspelled-project-folders-stay).

### A request, end to end

```mermaid
sequenceDiagram
    participant C as Client
    participant M as Middleware
    participant F as Global filters
    participant Ctl as Controller
    participant Svc as Service
    participant Repo as Repository
    participant DB as SQL Server

    C->>M: HTTP request
    M->>M: ForwardedHeaders → GlobalException → StatusCodePages<br/>→ security headers → CORS → Authentication → Authorization → RateLimiter
    M->>F: AdminPermissionFilter → ValidationFilter
    F->>Ctl: action arguments (already validated)
    Ctl->>Svc: DTO + caller identity
    Svc->>Repo: query / write
    Repo->>DB: EF Core (global query filters applied)
    DB-->>Repo: rows
    Repo-->>Svc: entities
    Svc-->>Ctl: DTO (AutoMapper)
    Ctl-->>F: ApiResponse&lt;T&gt;
    F->>F: ListingInteractionFilter (record view)<br/>PendingListingAlertFilter · ListingStatsFilter · ListingEditReviewFilter
    F-->>C: {success, message, data, errors}
```

---

## Layer responsibilities

### Controllers — `Infastrucre/Presantion/Controllers`

**What.** 86 controllers (67 public + 19 admin) that bind a request, call one service method and wrap
the result in `ApiResponse<T>`.

**Rules.** A controller must contain no business logic, no EF query and no validation. The only
decisions it makes are:

- extracting the caller's identity from claims (`User.FindFirstValue(ClaimTypes.NameIdentifier)`),
- `IsAdmin` from `User.IsInRole(AppRoles.Admin)`,
- converting `IFormFile` to the framework-agnostic `UploadImageModel` via `ToUploadModelsAsync`,
- choosing the HTTP status code and the `UserMessages` constant.

Admin-only rules such as promotion are the one exception — those throw `ForbiddenException`
directly, because the check is a single claim test with no data behind it.

### Services — `Core/Services`

**What.** All business logic: validation orchestration, moderation decisions, ownership checks,
notifications, file handling, DTO mapping.

**How they signal failure.** By throwing an `AppException` subclass. The global middleware turns each
into its HTTP status and its Arabic message. Services never return status codes.

| Exception | Status |
| --- | --- |
| `BadRequestException` | 400 |
| `UnauthorizedException` | 401 |
| `PaymentRequiredException` | 402 |
| `ForbiddenException` | 403 |
| `NotFoundException` | 404 |
| `ConflictException` | 409 |

### Repositories — `Infastrucre/Presitance/Repositories`

**What.** 71 repository classes, one per module plus the cross-cutting ones. They own EF Core and
nothing else — no business rules, no `throw new NotFoundException`.

Every module repository exposes roughly the same surface:

```csharp
Task<T?>  GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct, bool includeUnmoderated);
Task<T?>  GetOwnedAsync(Guid id, string userId, CancellationToken ct);
Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(TFilter filter, CancellationToken ct);
Task<IReadOnlyList<T>> GetSimilarAsync / GetRelatedAsync / GetRecentlyAddedAsync(...);
```

### DTOs — `Shared/DTOs`

Grouped by module. Three shapes per listing module:

| Shape | Used by | Notes |
| --- | --- | --- |
| `Create*Request` / `Update*Request` | POST / PUT | Multipart; carry `IFormFile` |
| `*ListItemDto` | list endpoints | The card |
| `*DetailsDto` | details endpoint | The full record |

Filter parameters are `*FilterParams`, bound `[FromQuery]`.

### Entities — `Core/Domain/Entities`

Plain C# classes, no data annotations except `[NotMapped]`. Mapping is entirely in EF configuration
classes. Two marker interfaces carry the cross-cutting behaviour:

| Interface | Meaning |
| --- | --- |
| `IModeratedListing` | Has moderation columns; the visibility filter is applied automatically |
| `IExpiringListing` | Has a publication window (`PublishedAt`, `ExpireAt`, `FirstPublishedAt`, `RepublishCount`) |

**48 concrete types implement `IModeratedListing`; 47 also implement `IExpiringListing`.** The
exception is `Advertisement` (سيارات), which had a richer window of its own first.

### Validation — `Core/Services/Validation`

100 FluentValidation validator files. Registered by assembly scan
(`AddValidatorsFromAssemblyContaining<RegisterRequestValidator>`, scoped) and executed by the global
`ValidationFilter`. Failures answer **HTTP 422** with `{success:false, message, errors[]}`. The
built-in ASP.NET `ModelState` 400 response is deliberately suppressed so FluentValidation is the only
source of validation.

---

## Seams

These are the pieces that make 48 modules maintainable. Learn them first.

| Seam | Where | What it centralises |
| --- | --- | --- |
| `ModerationModelConfiguration` | `Presitance/Configurations` | Applies moderation columns, indexes and the visibility query filter to **every** `IModeratedListing` by walking the model — no per-module wiring |
| `ModerationQueryExtensions` | `Presitance/Configurations` | The only supported way to look past the visibility rule: `IncludingUnmoderated()`, `VisibleToViewer()` |
| `AdFormSchemaCatalog` | `Services/AdForms` | The **only** place a create-form's fields, conditions and validation live |
| `ReadConfigCatalog` | `Services/ReadConfigs` | The only place a module's read endpoints are declared |
| `NotificationCatalog` | `Shared/Constants` | The only place notification wording lives |
| `AdminPageCatalog` | `Shared/Constants` | The only definition of an admin page and its permissions |
| `ListingModuleCatalog` | `Shared/Constants` | Maps `ListingModuleType` ↔ category / sub-category |
| `UserMessages` | `Shared/Constants` | The only place a customer-facing message is written |
| `PagedListingQuery` | `Presitance/Repositories` | Key-first paging + discovery strips (see [PERFORMANCE](PERFORMANCE.md)) |
| `ImageFormatCatalog` | `Shared/Constants` | Accepted formats, magic-byte detection, stored extension, static-file content types |

### Global query filters

Two **named** filters, so one can be dropped without the other:

| Name | Rule | Dropped by |
| --- | --- | --- |
| `SoftDelete` | `!IsDeleted` | **nothing** — no caller in the system switches it off |
| `Moderation` | `ModerationStatus == Approved && (ExpireAt == null \|\| ExpireAt > UtcNow)` | `IncludingUnmoderated()` / `VisibleToViewer()` |

> Never call bare `IgnoreQueryFilters()`. It drops **both**, resurrecting deleted rows.

---

## Global action filters

Registered in `Program.cs` in this order. Order matters.

| # | Filter | What it does |
| --- | --- | --- |
| 1 | `AdminPermissionFilter` | Enforces admin page permissions. **Fails closed** — an admin action with no permission annotation is refused and logged as an error. First, so an unauthorised caller is refused before their payload is read, before a view is recorded and before a notification fires |
| 2 | `ValidationFilter` | Runs the matching FluentValidation validator → 422 |
| 3 | `ListingInteractionFilter` | Records a view when any module's details endpoint succeeds; purges interactions when any delete succeeds |
| 4 | `PendingListingAlertFilter` | Tells administrators a new listing arrived |
| 5 | `ListingStatsFilter` | Fills `views`, `averageRating`, `ratingsCount`, `isFavorite` into every listing DTO in one batched query per response |
| 6 | `ListingEditReviewFilter` | Announces listings that an edit sent back to review |

Filters 3–6 are global precisely because they must cover all 48 modules; implementing them per module
would be 48 copies that eventually disagree.

---

## Middleware pipeline

From `MarkatPlace/Program.cs`, in order:

1. `UseForwardedHeaders` — so scheme/host are correct behind IIS
2. `GlobalExceptionHandlingMiddleware` — wraps everything
3. `SqlDiagnosticsMiddleware` — **only when `Diagnostics:SqlCounter` is true**
4. `UseStatusCodePages` — gives 404/405/413/415 an Arabic `ApiResponse` body
5. `UseHsts` (non-Development)
6. Security headers — `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, `X-XSS-Protection`
7. Swagger no-store headers, then `UseSwagger` / `UseSwaggerUI`
8. `UseHttpsRedirection`
9. `UseResponseCompression` (Brotli + Gzip)
10. `UseStaticFiles` × 2 — uploads with an explicit `PhysicalFileProvider`, then the rest of `wwwroot`
11. `UseResponseCaching`
12. `UseCors("AllowAll")`
13. `UseAuthentication` → `UseAuthorization`
14. `UseRateLimiter` — **after** authentication, so a signed-in caller is limited by user id rather than by a shared NAT address
15. `MapControllers`, health checks, `MapHub<NotificationHub>`

Then, inside a start-up scope: `MigrateAsync()` → `IdentityDataSeeder` → (Development only)
`DevelopmentDataSeeder`.

---

## Dependency injection

Three extension methods, called from `Program.cs`:

| Call | Defined in | Registers |
| --- | --- | --- |
| `AddPersistence(configuration)` | `Presitance/DependencyInjection.cs` | Settings binding, `AppDbContext`, Identity, all repositories, `IAdminActionContext`, role claims transformation, both hosted services |
| `AddApplicationServices()` | `Services/DependencyInjection.cs` | All business services, AutoMapper profiles, FluentValidation validators, the Egyptian Arabic language manager |
| `AddJwtAuthentication(configuration, environment)` | `MarkatPlace/Extensions` | JWT bearer + fail-fast key validation |

**Lifetimes:**

| Lifetime | Used for |
| --- | --- |
| Singleton | `ITokenService` (`JwtService`), `IMapper`, `IUserRoleCache` |
| Scoped | Everything request-bound: repositories, services, `IAdminActionContext`, `IUnitOfWork`, `IRealtimeNotifier`, validators |
| Transient | `IEmailService` |

Interceptors registered on `AppDbContext`:

| Interceptor | Purpose |
| --- | --- |
| `ListingEditModerationInterceptor` | Sends an edited listing back to review, in `SaveChanges`, for all modules |
| `SqlCountInterceptor` | Per-request SQL accounting — **only constructed when `Diagnostics:SqlCounter` is true** |

---

## Background services

Both are `BackgroundService` with a `PeriodicTimer`.

| Service | Interval | Start delay | Does |
| --- | --- | --- | --- |
| `ListingLifecycleService` | 1 hour | 15 s | Expiry warnings, the "انتهت مدة إعلانك" notice, سيارات' grace-period deletion. It only raises notifications and tidies up — **public visibility is decided by the `ExpireAt` column**, not by this job |
| `BannerBookingLifecycleService` | 1 hour | 45 s | Publishes scheduled bookings, expires finished ones, sweeps the preview folder (24 h retention) |

They are separate on purpose: different lifecycles over different tables, and a failure in one must
not stop the other.

---

## Real-time notifications

**What.** `NotificationHub` at `/hubs/notifications`, `[Authorize]`.

**How.** Clients pass their JWT as the `access_token` **query-string** value, because a WebSocket
cannot send an `Authorization` header. `JwtBearerEvents.OnMessageReceived` reads it, but only for
paths starting `/hubs`.

Server-to-client methods:

| Method | Payload |
| --- | --- |
| `ReceiveNotification` | `NotificationDto` |
| `UnreadCountChanged` | `int` |

**Delivery is best-effort and never affects persistence.** `NotificationService` persists and calls
`SaveChanges` *first*, then pushes inside a `try/catch` that logs a warning. A dropped socket cannot
fail the business operation.

Addressing is `Clients.User(userId)`, keyed on the `NameIdentifier` claim — so notifications cannot
leak between users.

---

## External integrations

| Integration | Where | Notes |
| --- | --- | --- |
| SMTP (MailKit) | `Presitance/Services/EmailService.cs` | Password-reset OTP only. RTL Arabic HTML body |
| File system | `Presitance/Services/FileService.cs` | Uploads under `wwwroot/uploads/<module>/`, served as static files |

There is **no** payment gateway. Payments are manual: the user uploads a transfer screenshot and an
administrator approves it. `PaymentRequiredException` (402) exists as the integration point for a
future provider.

---

## Cross-module read model

`IUserListingSource` providers (`Presitance/Listings`) give the cross-module features — favourites,
recently viewed, ratings, reports, `my-listings` — one uniform way to read a listing of any module by
`(ListingModuleType, Guid)`.

> This read model is **public by design**: it applies the moderation filter. That is why an owner
> reading their own pending listing succeeds but no view is counted — and why the view bookkeeping
> tolerates a `NotFoundException` rather than failing the read. See
> [BUSINESS_RULES § Owner visibility](BUSINESS_RULES.md#3-ownership-and-visibility).
