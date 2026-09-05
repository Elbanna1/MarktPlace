# Project Structure

[← README](../README.md) · Related: [ARCHITECTURE](ARCHITECTURE.md) · [DEVELOPMENT_GUIDE](DEVELOPMENT_GUIDE.md) · [CONVENTIONS](CONVENTIONS.md)

A map for locating code quickly, and for deciding where new code belongs.

---

## Top level

```
MarkatPlace.slnx                Solution — 8 projects
fix-migration-history.sql       One-off repair script (see TROUBLESHOOTING.md)
README.md                       Entry point
docs/                           This documentation
Core/                           Domain, abstractions, business logic
Infastrucre/                    Persistence + Presentation  (folder name is misspelled)
Shared/                         Contracts shared by every layer
MarkatPlace/                    The web host
Tests/                          xUnit test project
```

---

## `Core/Domain`

**Purpose.** Entity classes. The innermost layer.

**Belongs here.** Plain C# classes with properties and navigation properties. Marker interfaces
(`IModeratedListing`, `IExpiringListing`, `IListingImage`, `IOrderedListingImage`, `IListingVideo`).

**Does NOT belong here.** EF Core attributes or `using Microsoft.EntityFrameworkCore`, HTTP types,
DTOs, business logic, validation. `[NotMapped]` is the only attribute used.

| Folder | Contents |
| --- | --- |
| `Entities/` | Cars, business, jobs, workshops, notifications, payments, banners, referrals, admin |
| `Entities/Animals` `Antiques` `Charity` `Clothing` `HomeFurnishing` `OnlineShopping` `RealEstate` | One file per module |
| `Entities/Listings` | Cross-module interaction entities (views, favourites, ratings, reports) |

**Interacts with.** Referenced by every other project. References only `Shared` (for enums).

---

## `Core/ServicesAbstraction`

**Purpose.** The contracts between the business layer and everything else — 141 interface files, flat
(no sub-folders).

**Belongs here.** `I*Service` and `I*Repository` interfaces, plus infrastructure seams the core needs
without knowing their implementation: `IFileService`, `IEmailService`, `ITokenService`,
`IRealtimeNotifier`, `IAdminActionContext`, `IUnitOfWork`, `IUserListingSource`.

**Does NOT belong here.** Implementations, EF types, ASP.NET types.

> `IAdminActionContext` is the "who is calling" seam — id, name, IP, `IsAdmin` — implemented in
> Persistence by reading `IHttpContextAccessor`. It is how the core layers learn the caller without
> knowing what an HTTP request is.

---

## `Core/Services`

**Purpose.** All business logic. 266 files.

| Folder | Contents | Notes |
| --- | --- | --- |
| *(root)* | Module services that predate the category folders — `AuthService`, `AdvertisementService`, `LostFoundService`, `PaymentService`, … | |
| `AdForms/` | `AdFormSchemaCatalog`, `AdFormFieldFactory`, `CreateAdFormService` | **The single source of create-form rules** (227 KB) |
| `ReadConfigs/` | `ReadConfigCatalog` | The single source of per-module read routes (160 KB) |
| `Validation/` | 100 FluentValidation validators + shared rule helpers | |
| `Mapping/` | AutoMapper profiles, one per module | |
| `Listings/` | `ListingInteractionService` — views, favourites, ratings, reports, recently viewed | |
| `Notifications/` | Interest subscriptions, fan-out, listing context resolution | |
| `Admin/` | Admin dashboard services | |
| `Animals/` `AnimalModules/` `Antiques/` `AntiqueModules/` `BusinessModules/` `Charity/` `Clothing/` `ClothingModules/` `HomeFurnishing/` `JobModules/` `OnlineShopping/` `RealEstate/` | Per-category module services | `*Modules` folders hold Swagger example builders |
| `BannerBookings/` `Referrals/` `Settings/` `Lookups/` | Feature services | |

**Belongs here.** Business rules, orchestration, validation, mapping, catalogues.

**Does NOT belong here.** EF queries (`_context.…`), HTTP types, SQL.

---

## `Infastrucre/Presitance` → namespace `Persistence`

**Purpose.** Everything that talks to the database, the file system or the network. 334 files
(221 excluding the generated migrations).

| Folder | Contents |
| --- | --- |
| `Data/` | `AppDbContext` (369 `DbSet`s), `AppDbContextFactory`, `IdentityDataSeeder`, `RequestSqlCounter` |
| `Data/Development/` | `DevelopmentDataSeeder`, `DemoImageCatalog` — Development only |
| `Configurations/` | `IEntityTypeConfiguration<T>` classes + the model-wide seams |
| `Configurations/{Animals,Antiques,Charity,Clothing,HomeFurnishing,Listings,OnlineShopping,RealEstate}` | Per-category configurations |
| `Repositories/` | 71 repositories + `PagedListingQuery` |
| `Listings/` | `IUserListingSource` providers — the cross-module read model |
| `Interceptors/` | `ListingEditModerationInterceptor`, `SqlCountInterceptor` |
| `Services/` | `FileService`, `EmailService`, `JwtService`, `AdminActionContext`, role claims transformation |
| `BackgroundServices/` | `ListingLifecycleService`, `BannerBookingLifecycleService` |
| `Migrations/` | 56 migrations + snapshot (1.64 M generated lines — never edit by hand) |

**Key files**

| File | Why it matters |
| --- | --- |
| `Configurations/ModerationModelConfiguration.cs` | Applies moderation columns, indexes and the visibility filter to all 48 modules by walking the model |
| `Configurations/ModerationQueryExtensions.cs` | `IncludingUnmoderated()`, `VisibleToViewer()` — the only supported escapes |
| `Configurations/QueryFilterNames.cs` | The two filter names |
| `Repositories/PagedListingQuery.cs` | Key-first paging and discovery strips |
| `DependencyInjection.cs` | `AddPersistence` |

**Does NOT belong here.** Business rules. A repository that throws `NotFoundException` or decides who
may see what is in the wrong layer.

---

## `Infastrucre/Presantion` → namespace `Presentation`

**Purpose.** Controllers. Nothing else. 87 files.

| Folder | Contents |
| --- | --- |
| `Controllers/` | 67 public controllers |
| `Controllers/Admin/` | 19 admin controllers, all under `api/v2/admin` |
| `Extensions/` | `FormFileExtensions` — `IFormFile` → `UploadImageModel` |

Registered into the host with `.AddApplicationPart(typeof(AuthController).Assembly)`.

**Does NOT belong here.** Business logic, EF queries, validation, message text that is not a
`UserMessages` reference.

---

## `Shared`

**Purpose.** Contracts every layer may reference. 432 files. References only
`Microsoft.AspNetCore.App` (for `IFormFile` in multipart request DTOs).

| Folder | Contents |
| --- | --- |
| `DTOs/` | Request/response contracts, grouped by module |
| `DTOs/Common/` | `PaginatedResult<T>`, shared filter bases |
| `DTOs/Lookups/Forms` `DTOs/Lookups/Read` | Create-form and read-config payloads |
| `Enums/` | Domain enums, grouped by category |
| `Constants/` | **The catalogues** — 52 files |
| `Exceptions/` | `AppException` + 6 subclasses |
| `Responses/` | `ApiResponse` / `ApiResponse<T>` |
| `Settings/` | `JwtSettings`, `EmailSettings`, `FileStorageSettings`, `AppSettings`, `ListingInteractionSettings` |
| `Authorization/` | `AdminPage`, `RequireAdminPermission`, `SuperAdminOnly`, `AdminSelfService` attributes |

### The catalogues — read these before adding anything

| File | Owns |
| --- | --- |
| `UserMessages.cs` | Every customer-facing message |
| `NotificationCatalog.cs` | Every notification's wording, icon and deep link |
| `AdminPageCatalog.cs` | Every admin page and the permissions grantable on it |
| `ListingModuleCatalog.cs` | `ListingModuleType` ↔ category / sub-category |
| `ImageFormatCatalog.cs` | Accepted image formats + magic-byte detection |
| `DocumentFormatCatalog.cs` / `VideoFormatCatalog.cs` | CV and video formats |
| `LocationConstants.cs` | الفيوم and its seven مراكز |
| `ListingLifecycle.cs` | The 30-day publication window |
| `AdvertisementCatalog.cs` | Egyptian phone pattern + its Arabic message |
| `CarCatalog.cs`, `AnimalCatalog.cs`, `RealEstateCatalog.cs`, … | Per-category Arabic lookup names + seed data |

---

## `MarkatPlace` — the host

| Folder | Contents |
| --- | --- |
| `Program.cs` | Composition root, middleware pipeline, health checks, seeding |
| `Extensions/` | `AddJwtAuthentication`, `AddApiRateLimiting`, `AddSwaggerWithJwt`, Swagger operation filters |
| `Filters/` | The six global action filters |
| `Middleware/` | `GlobalExceptionHandlingMiddleware`, `SqlDiagnosticsMiddleware` |
| `RealTime/` | `NotificationHub`, `SignalRRealtimeNotifier` |
| `wwwroot/uploads/` | Uploaded files, served publicly |
| `appsettings*.json` | Layered configuration |
| `web.config` | Legacy IIS hosting only — request limits, logging switches. Inert under Kestrel |

---

## `Tests/MarkatPlace.Tests`

xUnit, hermetic (no database, no host). See [TESTING.md](TESTING.md).

| File | Guards |
| --- | --- |
| `EgyptianInputTests` | Phone prefixes, الفيوم مراكز and their order |
| `LandValidationTests` | Land create/update rules, conditional requirements |
| `UserFacingLanguageTests` | Every message constant is Arabic |
| `NoEnglishInResponsesTests` | Source-wide sweep for English in responses |
| `UploadSecurityTests` | Magic-byte detection, SVG exclusion, size ceilings |
| `DeploymentSafetyTests` | No committed secrets; IIS ≥ app upload limit |
| `ReverseProxyHostingTests` | Forwarded headers behind Nginx; no IIS requirement; no pinned address |
| `OwnerVisibilityTests` | The owner-visibility rule cannot be reintroduced-broken |
| `ArabicText`, `RepositoryRoot`, `CSharpSource` | Helpers (a small C# string lexer) |

---

## Where does my code go?

| I am writing… | It goes in |
| --- | --- |
| A new entity | `Core/Domain/Entities/<Category>/` |
| Its table mapping / indexes | `Infastrucre/Presitance/Configurations/<Category>/` |
| A database query | `Infastrucre/Presitance/Repositories/` |
| A business rule | `Core/Services/` |
| A validation rule | `Core/Services/Validation/` |
| A request/response shape | `Shared/DTOs/<Module>/` |
| An enum | `Shared/Enums/<Category>/` |
| Arabic wording a user sees | `Shared/Constants/UserMessages.cs` (or `NotificationCatalog`) |
| A create-form field rule | `Core/Services/AdForms/AdFormSchemaCatalog.cs` |
| A new HTTP route | `Infastrucre/Presantion/Controllers/` |
| A cross-cutting rule for all modules | A global filter in `MarkatPlace/Filters/` or a model-wide seam |
