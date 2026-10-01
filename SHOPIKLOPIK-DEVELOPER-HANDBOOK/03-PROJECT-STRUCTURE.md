# 03 — Project Structure

[← Start Here](00-START-HERE.md) · Related: [02-ARCHITECTURE](02-ARCHITECTURE.md)

A map of the repository. **1,491 C# files** across 8 projects. Use the "where do I put…" table at the
end when you are adding code rather than reading it.

---

## Repository root

```
MarkatPlace/                              ← repository root
├── MarkatPlace.slnx                      Solution (the new XML solution format)
├── README.md                             Project README (still the original, accurate)
├── .gitignore                            .vs/, **/bin/, **/obj/   ← note what is NOT ignored
├── fix-migration-history.sql             One-off repair script (see 30-TROUBLESHOOTING.md)
├── Core/                                 Domain + abstractions + business logic
├── Infastrucre/                          Persistence + Presentation   (folder name misspelled)
├── Shared/                               DTOs, enums, constants, settings
├── MarkatPlace/                          The ASP.NET Core host
├── Tests/MarkatPlace.Tests/              xUnit test project
├── deploy/nginx/                         Committed production Nginx site file
├── docs/                                 Original in-repo documentation (18 files)
└── SHOPIKLOPIK-DEVELOPER-HANDBOOK/       This handbook
```

> `.gitignore` only excludes `.vs/`, `bin/` and `obj/`. There is no `.env`, no secrets file pattern,
> and `appsettings*.json` **are** committed. See [22-SECURITY.md](22-SECURITY.md).

---

## `Core/Domain` — entities only (128 files)

```
Core/Domain/
├── Domain.csproj                  → references Shared + Microsoft.Extensions.Identity.Stores
└── Entities/
    ├── ApplicationUser.cs         Identity user + Shopiklopik fields (status, referral code, reset state)
    ├── Advertisement.cs           The shared Cars/legacy listing table
    ├── Category.cs, SubCategory.cs, Governorate.cs, Center.cs
    ├── Notification.cs, NotificationInterests.cs
    ├── Payment.cs, PaymentMethod.cs
    ├── Banner.cs, BannerBooking.cs, BannerPlacementSetting.cs
    ├── Referral.cs, ReferralLinkEvent.cs
    ├── AdminAuditLog.cs, AdminPageGrant.cs
    ├── IModeratedListing.cs  IExpiringListing.cs  IListingImage.cs
    │   IOrderedListingImage.cs  IListingVideo.cs        ← THE SEAM INTERFACES
    ├── Listings/                  Cross-module interaction entities
    │   (ListingViewCounter, ListingViewer, ListingFavorite, ListingRating, ListingReport)
    └── Animals/ Antiques/ Charity/ Clothing/ HomeFurnishing/ OnlineShopping/ RealEstate/
                                    One folder per category, entity + its lookup tables
```

**Rule:** an entity here has properties and, at most, small pure helpers (`Advertisement.IsExpiredAt`).
No EF attributes beyond `[NotMapped]`, no HTTP, no services.

---

## `Core/ServicesAbstraction` — the contract layer (148 files)

Flat folder of interfaces, one file per interface, named `I<Thing>Service.cs` or
`I<Thing>Repository.cs`. Also holds the infrastructure seams that keep `Core` framework-free:

| Interface | Implemented by | In |
| --- | --- | --- |
| `IEmailService` | `EmailService` (MailKit) | Persistence |
| `ITokenService` | `JwtService` | Persistence |
| `IFileService` | `FileService` | Persistence |
| `IGoogleTokenValidator` | `GoogleTokenValidator` | Persistence |
| `IRealtimeNotifier` | `SignalRRealtimeNotifier` | **host** |
| `IUnitOfWork` | `AppUnitOfWork` | Persistence |
| `IAdminActionContext` | `AdminActionContext` | Persistence |
| `IUserAccessStateCache` | `UserAccessStateCache` | Persistence |
| `ILookupCache`, `IPublicResponseCache` | `LookupCache`, `PublicResponseCache` | Services |

---

## `Core/Services` — business logic (277 files)

```
Core/Services/
├── DependencyInjection.cs         ← every service, mapper profile and validator registration
├── AuthService.cs  AccountService.cs  ProfileService.cs
├── AdvertisementService.cs        The Cars/legacy listing service
├── NotificationService.cs  HomeService.cs  LookupService.cs  PaymentService.cs  BannerService.cs
├── AdForms/                       CreateAdFormService + AdFormBreadcrumbBuilder (the dynamic form)
├── ReadConfigs/                   ReadConfigService (which GET endpoints exist per selection)
├── Lookups/                       LookupCache, AdvertiserLookupService/Catalog, CategorySelectionResolver
├── Admin/                         AdminAdService, AdminUserService, AdminPermissionService, audit, alerts…
├── Listings/                      ListingViewQueue, ListingViewFlushService, interaction + republish services
├── Notifications/                 Interest fan-out
├── Referrals/                     ReferralService
├── BannerBookings/                BannerBookingService, BannerSettingsService
├── Caching/                       PublicResponseCache
├── Validation/                    ALL FluentValidation validators
├── Mapping/                       ALL AutoMapper profiles
├── Settings/                      PublicSettingsService
└── Animals/ Antiques/ Charity/ Clothing/ HomeFurnishing/ OnlineShopping/ RealEstate/
    BusinessModules/ JobModules/ AnimalModules/ AntiqueModules/ ClothingModules/
                                    One folder per category: the per-module services
```

---

## `Infastrucre/Presitance` → namespace `Persistence` (344 files)

```
Infastrucre/Presitance/
├── DependencyInjection.cs         ← DbContext, Identity, every repository, hosted services
├── Data/
│   ├── AppDbContext.cs            ~400 DbSets + OnModelCreating
│   ├── AppDbContextFactory.cs     design-time factory for `dotnet ef`
│   ├── AppUnitOfWork.cs           ExecuteInTransactionAsync + IsUniqueConstraintViolation
│   ├── IdentityDataSeeder.cs      roles + optional bootstrap administrator
│   ├── RequestSqlCounter.cs       backing store for X-Sql-Count
│   └── Development/               DevelopmentDataSeeder (demo data, Development ONLY)
├── Configurations/                IEntityTypeConfiguration per entity + moderation/sort index config
│   └── Animals/ Antiques/ Charity/ Clothing/ HomeFurnishing/ Listings/ OnlineShopping/ RealEstate/
├── Repositories/                  ~70 repositories — every EF query in the system
├── Listings/                      IUserListingSource + IListingModerationSource implementations,
│                                  ModerationModuleRegistry
├── Interceptors/
│   └── ListingEditModerationInterceptor.cs   ← "an edit returns a listing to review"
├── Services/                      EmailService, JwtService, FileService, GoogleTokenValidator,
│                                  ReferralLinkBuilder, UserAccessStateCache,
│                                  DatabaseRoleClaimsTransformation, AdminActionContext
├── BackgroundServices/            ListingLifecycleService, BannerBookingLifecycleService
└── Migrations/                    60 migrations + AppDbContextModelSnapshot
```

---

## `Infastrucre/Presantion` → namespace `Presentation` (89 files)

```
Infastrucre/Presantion/
├── Controllers/                   68 public controllers
│   └── Admin/                     19 admin controllers, all under /api/v2/admin
├── Extensions/FormFileExtensions.cs    IFormFile → UploadImageModel (framework-agnostic)
└── Filters/HttpCacheAttribute.cs       Cache-Control + ETag + 304 handling
```

**This project cannot reference `Domain`.** Controllers therefore deal only in DTOs and interfaces.

---

## `Shared` (447 files)

```
Shared/
├── DTOs/         34 folders, one per domain area (Auth, Advertisements, Animals, Lookups, Admin, …)
├── Enums/        29 files + 7 category folders — ListingModuleType, ModerationStatus, CategoryType…
├── Constants/    The catalogues. The most important files in the repository:
│   ├── UserMessages.cs            EVERY customer-facing sentence
│   ├── NotificationCatalog.cs     Every notification's wording/icon/deep link
│   ├── AdminPageCatalog.cs        Every admin page and the permissions valid on it
│   ├── ListingModuleCatalog.cs    module ↔ sub-category ↔ category ↔ route
│   ├── ImageFormatCatalog.cs / VideoFormatCatalog.cs / DocumentFormatCatalog.cs
│   ├── FileUploadConstants.cs     Size ceilings and every upload folder name
│   ├── ImageConstants.cs          Per-image limits and folder names
│   ├── ApiVersions.cs AppRoles.cs AuthConstants.cs RateLimitPolicies.cs
│   ├── LocationConstants.cs       The governorate and its 7 centers
│   ├── AdvertisementConstants.cs  30-day window, 15-day grace, expiry warning days
│   └── <Category>Catalog.cs       Arabic lookup names + seed data per category
├── Settings/     JwtSettings, EmailSettings, CorsSettings, RateLimitingSettings, SwaggerAuthSettings,
│                 GoogleAuthSettings, AppSettings, FileStorageSettings, ListingInteractionSettings
├── Responses/    ApiResponse, ApiResponse<T>, PaginatedResult<T>, ApiJson
├── Exceptions/   AppException + BadRequest/NotFound/Unauthorized/Forbidden/Conflict/PaymentRequired
└── Authorization/ AdminPage, RequireAdminPermission, SuperAdminOnly, AdminSelfService attributes
```

---

## `MarkatPlace` — the host (30 files)

```
MarkatPlace/
├── Program.cs                     The entire pipeline, top to bottom
├── appsettings.json               Base configuration (committed)
├── appsettings.Development.json   Dev secrets + demo data flag (committed)
├── appsettings.Production.json    Production overrides + extensive _README notes (committed)
├── web.config                     IIS-only; inert on Linux. Kept for the legacy publish path.
├── MarkatPlace.http               Hand-written request samples
├── Extensions/                    AddJwtAuthentication, AddApiCors, AddApiRateLimiting,
│                                  AddReverseProxyForwardedHeaders, AddAppUrlSettings,
│                                  AddSwaggerWithJwt + 10 Swagger operation/document filters
├── Filters/                       AdminPermissionFilter, ValidationFilter, ListingInteractionFilter,
│                                  PendingListingAlertFilter, ListingStatsFilter, ListingEditReviewFilter
├── Middleware/                    GlobalExceptionHandling, AccountStatus, SwaggerBasicAuth, SqlDiagnostics
├── RealTime/                      NotificationHub, SignalRRealtimeNotifier
├── Properties/launchSettings.json http://localhost:5002 · https://localhost:7139
└── wwwroot/uploads/<folder>/      Uploaded files (created at start-up, must survive deployments)
```

---

## `Tests/MarkatPlace.Tests` (28 files)

Grouped by concern, not by mirroring the source tree:

| File | Guards |
| --- | --- |
| `AdvertiserLookupTests.cs` | the advertiser lookup end to end, including leak-proofing |
| `DeploymentSafetyTests.cs` | the Nginx file and C# constants cannot drift apart |
| `RequestSizeLimitTests.cs` | all five upload ceilings agree |
| `UploadSecurityTests.cs` | magic-byte detection, executable rejection |
| `NoEnglishInResponsesTests.cs`, `UserFacingLanguageTests.cs`, `ArabicText.cs` | every user-facing string is Arabic |
| `SwaggerAccessTests.cs` | the Swagger gate fails closed |
| `ProductionHardeningTests.cs`, `InfrastructureHardeningTests.cs`, `ReverseProxyHostingTests.cs` | production configuration invariants |
| `AccountStateEnforcementTests.cs`, `AccountDeactivationTests.cs`, `OwnerVisibilityTests.cs` | account state and visibility rules |
| `GoogleSignInTests.cs`, `GoogleAuthConfigurationTests.cs`, `GoogleSignInHarness.cs` | Google sign-in |
| `SeedDataTests.cs` | reference data integrity |
| `CachingAndViewQueueTests.cs` | cache keys and the view queue |
| `EmailTemplateTests.cs` | the password-reset e-mail |
| `CSharpSource.cs`, `RepositoryRoot.cs` | helpers for the source-scanning tests |

---

## "Where do I put…?"

| I am adding… | Put it in |
| --- | --- |
| a new endpoint | a controller in `Infastrucre/Presantion/Controllers/` |
| the logic behind it | a service in `Core/Services/` |
| its interface | `Core/ServicesAbstraction/` |
| its database query | a repository in `Infastrucre/Presitance/Repositories/` |
| a new table | entity in `Core/Domain/Entities/`, configuration in `Presitance/Configurations/`, `DbSet` in `AppDbContext`, then `dotnet ef migrations add` |
| a request/response shape | `Shared/DTOs/<Area>/` |
| a validation rule | `Core/Services/Validation/` (FluentValidation — DataAnnotations do nothing) |
| a user-facing sentence | `Shared/Constants/UserMessages.cs`, in Egyptian Arabic |
| a notification | `Shared/Constants/NotificationCatalog.cs` + one `NotifyAsync` call |
| a new enum | `Shared/Enums/` |
| a configuration knob | a POCO in `Shared/Settings/` + a section in `appsettings.json` |
| cross-cutting HTTP behaviour | `MarkatPlace/Middleware/` or `MarkatPlace/Filters/` |
| a whole new listing module | all of the above, plus registration in `ListingModuleType`, `ListingModuleCatalog`, `ModerationModuleRegistry`, `AdFormSchemaCatalog`, `ReadConfigCatalog`, an `IUserListingSource`, and both `DependencyInjection.cs` files |
