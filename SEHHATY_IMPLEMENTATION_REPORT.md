# صحتي (Sehhaty) — Implementation Report

Date: 2026-10-09 · Branch: `main` (uncommitted working tree) · Baseline commit: `6149fb7`

---

## A. Executive Summary

**Added:** a complete new marketplace category **صحتي** (`CategoryType.Sehhaty = 13`) with six
independent listing modules — الأطباء, معامل التحاليل الطبية, معامل الأسنان, معامل الأشعة, مراكز طبية,
مستشفيات (sub-categories **53–58**, `ListingModuleType` **80–85**) — across every backend layer:
entities, EF configuration and migration, repositories, services, validators, AutoMapper, controllers,
the three lookup endpoints, moderation, my-listings, notifications, admin V2, views/favourites/ratings,
and the advertiser lookup.

**Lookup cleanup:** one `CategoryCatalog` now owns category/sub-category identity (seed, sub→category
map, Swagger examples read it); both static schema catalogs now refuse a sub-category under the wrong
parent; the advertisers endpoint is driven by one registration table per side with set-equality
tests; and a new route-integrity suite proves, for **every one of the 58 seeded sub-categories**, that
every advertised submit/read route is served by a real controller action.

**Status:** **IMPLEMENTED WITH DOCUMENTED LIMITATIONS.** The backend is complete and verified (build,
2,745 automated tests, migration applied/reverted on LocalDB, and real HTTP requests against the
running API — 201 of 204 first-pass assertions passed, the other 3 were wrong test assumptions
confirmed correct by a follow-up probe). Two limitations: the **frontend repository is not in this workspace** (section I), and
the **migration has not been applied to the shared/production database** (needs an authorised deploy).

---

## B. Existing Architecture Discovered

- **Onion layout** — `Shared` (enums, constants, DTOs) → `Core/Domain` → `Core/ServicesAbstraction` →
  `Core/Services` → `Infastrucre/Presitance` (EF Core / SQL Server) → `Infastrucre/Presantion`
  (controllers) → `MarkatPlace` (host, global filters). Folder names are misspelled on purpose
  (`docs/DECISIONS.md` D-01).
- **Category identity** — integer enums `CategoryType` / `SubCategoryType` seeded through EF `HasData`;
  ids are `ValueGeneratedNever`. Admins can also create categories at `Max(Id)+1`
  (`AdminCatalogRepository`).
- **Module registration** — there is no reflection-based discovery. A module becomes visible to the
  platform by appearing in: `ListingModuleType`, `ListingModuleCatalog` (module↔sub↔category),
  `NotificationCatalog.Subjects` (route + Arabic entity name — this drives
  `ListingModuleCatalog.RouteOf/ModuleOfRoute`, which the global `ListingInteractionFilter` and
  `PendingListingAlertFilter` use to resolve a module from the request path),
  `ModerationModuleRegistry` (admin approve/reject/suspend, expiry job, edit-re-review interceptor,
  view counters), an `IUserListingSource` (my-listings, admin listings), `ImageConstants.AllFolders`,
  and the two lookup catalogs.
- **Dynamic forms** — `AdFormSchemaCatalog` (+ per-category `*FormSchemas`) emits `FormFieldDto`s via
  `AdFormFieldFactory`; conditional fields use `visibleWhen` / `requiredWhen`. Admin field overrides
  are applied on top by `AdFormOverrideApplier`. Validation is FluentValidation only (HTTP **422**).
- **Lookup endpoints** — `LookupsController` → `CreateAdFormService` / `ReadConfigService` /
  `AdvertiserLookupService`, all three resolving the pair through `CategorySelectionResolver`
  (unknown id → 404, sub under the wrong parent → 400). `ReadConfigCatalog` publishes each module's
  read routes and filter parameters; `AdvertiserLookupCatalog` decides which sub-categories have an
  advertiser filter.
- **Persistence / search** — one table per module; global query filters for soft delete and moderation
  (`Approved` and not expired) applied automatically to every `IModeratedListing`; owners see their
  own listings via `VisibleToViewer`. Filtering is server-side `IQueryable` (`LIKE`, enum equality),
  paginated in SQL.

---

## C. Category and Subcategory Registration

| Item | Id | Arabic | English |
| --- | --- | --- | --- |
| Category | **13** | صحتي | Sehhaty |
| Sub-category | 53 | الأطباء | Doctors |
| Sub-category | 54 | معامل التحاليل الطبية | Medical Laboratories |
| Sub-category | 55 | معامل الأسنان | Dental Laboratories |
| Sub-category | 56 | معامل الأشعة | Radiology Laboratories |
| Sub-category | 57 | مراكز طبية | Medical Centers |
| Sub-category | 58 | مستشفيات | Hospitals |

**How the ids were chosen and verified.** 13 / 53 are the next members after the existing enums
(max 12 / 52). The live API (`GET https://api.shopiklopik.com/api/lookups/categories-tree`, read-only)
tops out at category 12 / sub 52. Because that endpoint hides inactive rows and admins can create
categories at `Max(Id)+1`, the migration does **not** trust that observation — see "duplicate
prevention" below. `ListingModuleType` 80–85 follow the existing practice of leaving gaps between
categories (65–67 Charity, 70–71 Lost & Found).

**Registration files changed:** `Shared/Enums/AdvertisementEnums.cs` (enum members +
`SehhatySubCategories`), `Shared/Enums/UserListingEnums.cs`, `Shared/Constants/CategoryCatalog.cs`
(new), `Shared/Constants/ListingModuleCatalog.cs`, `Shared/Constants/NotificationCatalog.cs`,
`Shared/Constants/NotificationReferenceTypes.cs`, `Shared/Constants/FileUploadConstants.cs`,
`Shared/Constants/ImageConstants.cs`, `Infastrucre/Presitance/Configurations/CategoryConfiguration.cs`,
`…/SubCategoryConfiguration.cs`, `…/Listings/ModerationModuleRegistry.cs`,
`…/Configurations/ListingSortIndexCatalog.cs`, `…/DependencyInjection.cs` (both layers),
`AdFormSchemaCatalog.cs`, `ReadConfigCatalog.cs`, `AdvertiserLookupCatalog.cs`.

**Duplicate prevention.** EF's generated `InsertData` was replaced in the migration by guarded SQL:
each seeded row is inserted only if missing; if id 13 (or 53–58) is already held by a *different*
row, the migration raises `THROW 51000` and the whole migration rolls back. Verified on LocalDB:
fresh apply ✔, re-run on a seeded DB is a no-op ✔, `Down` removes it ✔, foreign id 13 present → abort
with zero tables created ✔.

---

## D. Lookup Endpoint Changes

### 1. `GET /api/lookups/create-ad-form/{categoryId}/{subCategoryId}`

- **Previous:** 404 for category 13. `AdFormSchemaCatalog.GetSchema` ignored `categoryId` (a dead
  `categoryId switch { _ => null }`).
- **New:** returns the six schemas below (module key, `submit.endpoint`, fields with type, Arabic
  label, placeholder, `required`, `visibleWhen`/`requiredWhen`, pattern + Arabic pattern message,
  options, image limits, breadcrumb, upload limits). `create-ad-form/13` (no sub) returns the
  sub-category picker with the six options. `GetSchema` now returns `null` for a sub under the wrong
  parent (`CatalogSelection.TryResolve`).
- **Files:** `Core/Services/AdForms/SehhatyFormSchemas.cs` (new), `AdFormSchemaCatalog.cs`,
  `Core/Services/Lookups/CatalogSelection.cs` (new), `CreateAdFormExamples.cs`.
- **Errors:** unknown category/sub → 404; valid sub with wrong parent (`13/48`, `12/53`) → 400; all
  messages Arabic (existing resolver).
- **Tests:** `SehhatyCategoryTests` (schema per module ×6, specialty options, conditional field,
  facilities have no specialty, category-only picker, invalid combos), `LookupRouteIntegrityTests`
  (all 58 subs). HTTP: 6/6 forms 200 with correct module/submit/name label; invalid combos 400/404
  with Arabic messages.

### 2. `GET /api/lookups/read-config/{categoryId}/{subCategoryId}`

- **Previous:** 404 for category 13; `ReadConfigCatalog.GetSchema` had the same dead switch.
- **New:** per module — `list` (`/api/<module>`, paginated, query parameters `search`, `name`
  [`optionsSource: advertisers`, `optionsValue: name`], `specialty` for doctors
  [`optionsSource: doctorSpecialties`], `sortBy` [1 الأحدث, 2 الأقدم, 3 الأكثر مشاهدة], `pageIndex`,
  `pageSize`), `details`, unified `myListings` pinned to the module type, `doctorSpecialties`
  (doctors only), `advertisers`, the shared tail (categories, sub-categories, governorates, centers,
  listing actions / similar / recently viewed / favourites / interaction metadata, `createForm`,
  `readConfig`).
- **Files:** `ReadConfigCatalog.cs` (`Sehhaty(...)` builder + six my-listings `type` options),
  `Shared/DTOs/Lookups/Read/ReadOperationKeys.cs` (`DoctorSpecialties`), `ReadConfigExamples.cs`.
- **Tests:** per-module contract test ×6, doctor specialty lookup test, "my-listings type options
  cover every `ListingModuleType`" test, route integrity. HTTP: every anonymous operation advertised
  for each of the six modules was **called** and answered 200 (or 404 for the placeholder id).

### 3. `GET /api/lookups/advertisers/{categoryId}/{subCategoryId}`

- **Contract (unchanged):** paginated `{ id, name, listingsCount }`, `search`, `pageIndex`,
  `pageSize` (default 20, cap 50), `Cache-Control: max-age=60`; unsupported sub → 400.
- **Behaviour decided from the contract:** its only consumer is a list filter whose
  `optionsSource` is `advertisers`. For صحتي the requested filters are *by name* (doctor / lab /
  centre / hospital), and that name is exactly what identifies the advertiser, so the six modules now
  serve their distinct published names here, feeding the `name` filter.
- **Previous internals:** `AdvertiserLookupCatalog` (supported set + an unused module-name map + an
  unused `FilterParameter = "sellerName"` const) and a hand-written `switch` in
  `AdvertiserLookupRepository`, kept in step by a test that grepped the source.
- **New internals:** the catalog states the filter parameter per sub-category (`sellerName` /
  `name`); the repository is one `SubCategoryType → IQueryable<string>` table exposing its keys.
- **Tests:** rewritten `AdvertiserLookupTests` (catalog = repository set; every read-config filter fed
  by `advertisers` names the catalog's parameter, both directions; covering index checked on the EF
  model), plus Sehhaty tests (empty page per module, counts + search for doctors, facility table
  isolation, pending listings never leak). HTTP: six empty pages 200; after approval both doctor
  names listed with counts; search works; `12/53` → 400; `13/999` → 404; `11/48` → 400.

### Lookup cleanup summary

| Finding | Action |
| --- | --- |
| Category/sub names restated in 4 places; Swagger example `switch` stale (categories 9–11 fell through to "Lost & Found") | `CategoryCatalog` is the single source; seed, `ListingModuleCatalog` map and examples read it. **No migration diff** for the existing 64 rows proves the values were preserved |
| Static catalogs ignored `categoryId` | `CatalogSelection.TryResolve` parent check in both |
| Advertiser support stated twice + dead config | One table each side, set-equality test, dead members removed |
| Advertiser tests coupled to source text and the literal `SellerName` | Contract- and EF-model-level tests |
| No guard that advertised routes exist | `ApiRouteIndex` (reflection over controllers) + `LookupRouteIntegrityTests` over all 58 subs |
| Existing modules | The route-integrity suite found **no** existing drift: every legacy submit/read route resolves |

**Known remaining (not changed, out of scope):** the my-listings `type` option labels in
`ReadConfigCatalog.UnifiedMyListings` are still a hand list (now guarded by a coverage test);
`SharedTail` publishes governorates/centers for every module including صحتي (harmless, shared tail).

---

## E. Form Schema Details

Common to all six (types: `text`, `textarea`, `select`, `image`):

| Field | Type | Required | Label | Placeholder / rule |
| --- | --- | --- | --- | --- |
| `Name` | text | ✔ | per module (below) | per module example; max 150 |
| `Title` | text | ✔ | عنوان الإعلان | اكتب عنوانًا واضحًا ومختصرًا; max 150 |
| `Description` | textarea | ✔ | وصف الإعلان | module-specific «اذكر في الوصف: • …»; max 4000 |
| `Images` | image | ✔ (min 1, max 10) | الصور | existing pipeline, magic-byte validated |
| `Phone` | text | ✔ | رقم الهاتف | 01012345678; pattern `^01[0-2,5]{1}[0-9]{8}$`, message «من فضلك اكتب رقم موبايل مصري صحيح (مثال: 01012345678).» |
| `WhatsApp` | text | – | رقم الواتساب | same pattern/message when supplied |
| `Email` | text | – | البريد الإلكتروني | name@example.com; «من فضلك اكتب بريد إلكتروني صحيح.» |
| `Address` | text | ✔ | العنوان | الشارع، العلامة المميزة…; max 300 |
| `GoogleMapsUrl` | text | – | رابط الموقع على خرائط جوجل | https://maps.google.com/…; must be http/https |

| Sub | Name label | Field order |
| --- | --- | --- |
| 53 الأطباء | اسم الطبيب | Name, Specialty, OtherSpecialty, Address, GoogleMapsUrl, Phone, WhatsApp, Email, Title, Description, Images |
| 54 معامل التحاليل الطبية | اسم المعمل | Name, Title, Description, Images, Phone, WhatsApp, Email, Address, GoogleMapsUrl |
| 55 معامل الأسنان | اسم معمل الأسنان | same as 54 |
| 56 معامل الأشعة | اسم معمل الأشعة | same as 54 |
| 57 مراكز طبية | اسم المركز الطبي | same as 54 |
| 58 مستشفيات | اسم المستشفى | same as 54 |

(Order follows the order of the specification for each form.)

**Doctor specialty** — `Specialty`, select, required, searchable, label «تخصص الطبيب», 56 options with
values 1–55 in the exact order and wording of the specification, then **أخرى = 99**:
القلب والأوعية الدموية، طب الأطفال، الجلدية والتجميل، العظام، المخ والأعصاب، النساء والتوليد، طب وجراحة
العيون، طب وجراحة الأسنان، الباطنة العامة، الأنف والأذن والحنجرة، المسالك البولية، الجراحة العامة، جراحة
المخ والأعصاب، الطب النفسي، العلاج الطبيعي والتأهيل، الأشعة التشخيصية، التحاليل الطبية، طب الأسرة، طب
الطوارئ، الصدر والحساسية، أمراض الكلى، الأورام، أمراض الدم، الروماتيزم والمناعة، التخدير والعناية
المركزة، التغذية العلاجية، أمراض الذكورة والعقم، جراحة القلب والصدر، جراحة التجميل، جراحة الأوعية
الدموية، أمراض الجهاز الهضمي والكبد، الغدد الصماء والسكري، طب الشيخوخة، طب الألم، طب النوم، طب
الأورام الإشعاعي، طب نقل الدم، الطب الرياضي، جراحة العمود الفقري، جراحة الأطفال، جراحة الوجه والفكين،
طب الأسنان التجميلي، تقويم الأسنان، علاج الجذور والأعصاب، أمراض اللثة، طب أسنان الأطفال، طب حديثي
الولادة، طب الوراثة، الطب النووي، الطب الشرعي، الصحة المهنية، طب المجتمع، الطب الوقائي، طب السفر،
أمراض المناعة والحساسية، أخرى.

**«أخرى» behaviour** — `OtherSpecialty` («اكتب التخصص», max 150) is hidden and optional, with
`visibleWhen` and `requiredWhen` = `Specialty ∈ [99]`. Backend: required and non-blank (whitespace
rejected) when `Specialty = 99` — «من فضلك اكتب تخصص الطبيب لما تختار «أخرى».»; `Specialty` itself is
always required («تخصص الطبيب مطلوب.»), so a custom value can never replace it. Stored as
`Specialty = 99` **plus** `OtherSpecialty`; for any other specialty `OtherSpecialty` is nulled
server-side. Responses carry `specialty`, `specialtyName` («أخرى»), `otherSpecialty` and
`displaySpecialty` (the custom text for أخرى, else the label).

---

## F. Search and Filters

| Module | Filters (query) | Backend behaviour |
| --- | --- | --- |
| All six | `name` | `LIKE '%…%' ESCAPE '\'` on `Name` (wildcards `% _ [` matched literally) |
| All six | `search` | name / title / description / address; doctors also `OtherSpecialty` |
| All six | `sortBy` | 1 newest, 2 oldest, 3 most viewed |
| Doctors | `specialty` | equality on `Specialty`; `99` returns all «أخرى» doctors |

All filtering is in SQL (`IQueryable`), counted then paged; pending/rejected/expired/deleted rows
are excluded by the global filters. Indexes: `IX_<Table>_Advertisers (ModerationStatus, Name)
INCLUDE (IsDeleted, ExpireAt)` on all six; `(ModerationStatus, Specialty, CreatedAt) INCLUDE
(IsDeleted, ExpireAt)` on `Doctors`; the platform's shared visibility and most-viewed indexes are
generated automatically. **Tests:** repository tests (name, specialty, أخرى, custom-text search,
wildcard literal, per-facility isolation); HTTP: name, specialty, `specialty=99`, custom-text search,
hospital name, sort oldest — all passed.

---

## G. API and Route Inventory

New (`{m}` ∈ `doctors`, `medical-laboratories`, `dental-laboratories`, `radiology-laboratories`,
`medical-centers`, `hospitals`); implementation `Infastrucre/Presantion/Controllers/SehhatyControllers.cs`:

| Method | Route | Auth | Purpose | Verified |
| --- | --- | --- | --- | --- |
| POST | `/api/{m}` | user | Create (multipart; 201; goes to Pending) | HTTP ✔ ×6 |
| GET | `/api/{m}` | anonymous | Paged, filtered list | HTTP ✔ |
| GET | `/api/{m}/{id}` | anonymous (owner sees own pending) | Details (+view) | HTTP ✔ |
| PUT | `/api/{m}/{id}` | owner or admin | Update (multipart; owner edit → re-review) | HTTP ✔ |
| DELETE | `/api/{m}/{id}` | owner or admin | Soft delete | HTTP ✔ |
| GET | `/api/doctors/specialties` | anonymous | 56 specialty options | HTTP ✔ |

Modified: the three lookup endpoints (section D). Existing generic endpoints that now cover the
new modules without code changes: `PATCH /api/v2/admin/ads/{80–85}/{id}/approve|reject|suspend`
(HTTP ✔ approve ×6), `GET /api/profile/my-listings?type=80…85` (HTTP ✔), `/api/advertisements/{id}/…`
interactions with `type=80…85`. OpenAPI: `/swagger/v1/swagger.json` generates (31 Sehhaty
operations).

---

## H. Database and Migrations

- **Entities:** `Core/Domain/Entities/Sehhaty/SehhatyListings.cs` — abstract `SehhatyListing`
  (`IModeratedListing`, `IExpiringListing`), `SehhatyListing<TImage>`, `SehhatyListingImage`, and
  `Doctor` (+`Specialty`, `OtherSpecialty`), `MedicalLaboratory`, `DentalLaboratory`,
  `RadiologyLaboratory`, `MedicalCenter`, `Hospital`, each with its image type.
- **Why new tables:** the shared `Advertisements` table belongs to سيارات only (every module since
  has its own table — D-02), so reusing it would contradict the established architecture.
- **Configuration:** `Infastrucre/Presitance/Configurations/Sehhaty/SehhatyConfigurations.cs`;
  `AspNetUsers` FK `NoAction` (avoids SQL Server error 1785), images cascade from their listing.
- **Migration:** `20261009124101_AddSehhatyCategoryModules` — 12 `CreateTable`, 43 `CreateIndex`,
  guarded seed SQL; no `RenameColumn` / `AlterColumn` / drops in `Up`. The model snapshot diff is
  large only because EF re-orders blocks; semantically it changes `Category`, `SubCategory` and adds
  the 12 entities (verified block by block).
- **Consistency:** `dotnet ef migrations has-pending-model-changes` → "No changes have been made to
  the model since the last migration."
- **Applied:** to throwaway LocalDB databases only (then dropped). **Not applied to the shared /
  production database.**

---

## I. Frontend Changes

**The frontend repository is not accessible from this workspace** (`E:\Elfaume` contains only the
backend, a DB backup folder and the spec text). Nothing was changed there. Because forms and read
configuration are server-driven, the remaining frontend work is small:

1. **Category navigation** — if any screen hard-codes category ids/icons, add 13 «صحتي» (the
   `categories-tree` endpoint already returns it with its six sub-categories).
2. **Create flow** — no field work: render `create-ad-form/13/{53…58}` with the existing dynamic form
   renderer; it already honours `visibleWhen`/`requiredWhen` (the أخرى field), `pattern` /
   `patternMessage`, `searchable` selects and image limits.
3. **List pages** — render each module's `read-config` `list` operation; show the `name` filter as an
   autocomplete fed by the `advertisers` operation (same component as the animals `sellerName`
   filter) and, for doctors, the `specialty` select fed by `doctorSpecialties`.
4. **Cards / details** — the six listing shapes are new: show `name`, `title`, `description`,
   `address`, `googleMapsUrl` (link), `phone` / `whatsApp` / `email` (call / chat / mail actions),
   images, and for doctors `displaySpecialty`.
5. **Edit flow** — `PUT /api/{m}/{id}` multipart with `RemoveImageIds`; at least one image must
   remain.
6. **Routes** — share links and notification deep links use `/{m}/{id}` (e.g. `/doctors/{id}`).

Build / lint / type-check: **not run** (repository unavailable).

---

## J. Files Changed

### Backend — new
| File | Purpose |
| --- | --- |
| `Shared/Enums/SehhatyEnums.cs` | `DoctorSpecialty` (1–55, Other 99), `SehhatySortBy` |
| `Shared/Constants/SehhatyRoutes.cs` | The six public routes |
| `Shared/Constants/SehhatyCatalog.cs` | Every Sehhaty label, placeholder, message, folder, route; specialty list |
| `Shared/Constants/CategoryCatalog.cs` | Single source of category / sub-category identity (all 13 / 58) |
| `Shared/DTOs/Sehhaty/SehhatyDtos.cs` | Create/update requests, response DTOs, filter params |
| `Core/Domain/Entities/Sehhaty/SehhatyListings.cs` | Entities |
| `Core/ServicesAbstraction/ISehhatyListings.cs` | Generic repository + service interfaces |
| `Core/Services/Sehhaty/SehhatyListingService.cs` | Generic service + six module services |
| `Core/Services/Mapping/SehhatyMappingProfile.cs` | AutoMapper maps |
| `Core/Services/Validation/SehhatyRequestValidators.cs` | Create/update/filter validators |
| `Core/Services/AdForms/SehhatyFormSchemas.cs` | Six form schemas |
| `Core/Services/Lookups/CatalogSelection.cs` | Parent-category check for both catalogs |
| `Infastrucre/Presitance/Configurations/Sehhaty/SehhatyConfigurations.cs` | EF mapping + indexes |
| `Infastrucre/Presitance/Repositories/SehhatyRepositories.cs` | Generic repository + six closings |
| `Infastrucre/Presitance/Listings/SehhatyListingSources.cs` | my-listings / admin sources |
| `Infastrucre/Presantion/Controllers/SehhatyControllers.cs` | Generic controller base + six controllers |

### Backend — modified
| File | Change |
| --- | --- |
| `Shared/Enums/AdvertisementEnums.cs`, `UserListingEnums.cs` | New enum members, `SehhatySubCategories` |
| `Shared/Constants/ListingModuleCatalog.cs` | Six modules; sub→category map now derived from `CategoryCatalog` |
| `Shared/Constants/NotificationCatalog.cs`, `NotificationReferenceTypes.cs` | Six notification subjects (route + Arabic name) |
| `Shared/Constants/FileUploadConstants.cs`, `ImageConstants.cs` | Six upload folders |
| `Shared/DTOs/Lookups/Read/ReadOperationKeys.cs` | `doctorSpecialties` |
| `Core/Services/AdForms/AdFormSchemaCatalog.cs` | Sehhaty arm; parent check replaces dead switch |
| `Core/Services/AdForms/CreateAdFormExamples.cs` | Names from `CategoryCatalog`; Sehhaty examples |
| `Core/Services/ReadConfigs/ReadConfigCatalog.cs` | Sehhaty builder; parent check; my-listings options |
| `Core/Services/ReadConfigs/ReadConfigExamples.cs` | Sehhaty examples |
| `Core/Services/Lookups/AdvertiserLookupCatalog.cs` | Filter parameter per sub; dead members removed; six modules |
| `Core/Services/DependencyInjection.cs` | Six services + mapping profile |
| `Infastrucre/Presitance/Repositories/AdvertiserLookupRepository.cs` | Registration table; six modules |
| `Infastrucre/Presitance/Configurations/CategoryConfiguration.cs`, `SubCategoryConfiguration.cs` | Seed from `CategoryCatalog` |
| `Infastrucre/Presitance/Configurations/ListingSortIndexCatalog.cs` | Most-viewed sort index for six tables |
| `Infastrucre/Presitance/Data/AppDbContext.cs` | 12 DbSets |
| `Infastrucre/Presitance/DependencyInjection.cs` | Six repositories + six listing sources |
| `Infastrucre/Presitance/Listings/ModerationModuleRegistry.cs` | Six modules |

### Database
| File | Purpose |
| --- | --- |
| `Infastrucre/Presitance/Migrations/20261009124101_AddSehhatyCategoryModules.cs` (+ `.Designer.cs`) | Tables, indexes, guarded seed |
| `Infastrucre/Presitance/Migrations/AppDbContextModelSnapshot.cs` | Regenerated snapshot |

### Tests
| File | Purpose |
| --- | --- |
| `Tests/MarkatPlace.Tests/SehhatyCategoryTests.cs` (new, 84 tests) | Registration, three endpoints, schemas, specialty, validation, filters, service behaviour, ownership |
| `Tests/MarkatPlace.Tests/LookupRouteIntegrityTests.cs` (new, 349 tests) | Route/catalog consistency for all 58 sub-categories |
| `Tests/MarkatPlace.Tests/ApiRouteIndex.cs` (new) | Reflection index of every controller route |
| `Tests/MarkatPlace.Tests/AdvertiserLookupTests.cs` (modified) | Three source-coupled tests replaced by contract/model tests |

### Documentation
| File | Change |
| --- | --- |
| `README.md` | 54 modules, 13 categories → 58 sub-categories |
| `docs/MODULES.md` | Category 13 section |
| `docs/DECISIONS.md` | **D-36** (catalog, parent check, advertiser table, guarded seed); D-35 scope |
| `docs/API.md` | Advertisers endpoint now covers صحتي (`name`) |
| `docs/ARCHITECTURE.md`, `DATABASE.md`, `BUSINESS_RULES.md`, `PROJECT_STRUCTURE.md` | Counts, rule row, folders |
| `SEHHATY_IMPLEMENTATION_REPORT.md` | This report |

---

## K. Verification Results

| Command / check | Result |
| --- | --- |
| `dotnet test` (baseline, before changes) | ✔ 2,277 passed, 0 failed |
| `dotnet build MarkatPlace.slnx` | ✔ 0 warnings, 0 errors |
| `dotnet test Tests/MarkatPlace.Tests` (final) | ✔ **2,745 passed, 0 failed, 0 skipped** (+468: 84 Sehhaty, 349 route integrity, the rest existing data-driven suites expanding over the new category) |
| `dotnet ef migrations has-pending-model-changes` | ✔ no pending changes |
| `dotnet ef database update` on LocalDB (all 60 migrations) | ✔ |
| Seed guard: re-run on seeded DB / `Down` / foreign id 13 | ✔ no-op / ✔ reverted / ✔ aborted with full rollback |
| Arabic seed values in SQL Server (UTF-8 query file) | ✔ |
| Real HTTP run against the built API on LocalDB (`e2e.mjs`) | 201 / 204 passed; the 3 misses were test assumptions, each re-checked by a follow-up probe (below) |
| `/swagger/v1/swagger.json` generation | ✔ 200, 31 Sehhaty operations |

**Failures introduced and fixed during implementation**
- Three `AdvertiserLookupTests` failed because they grepped source for the old repository `switch`
  and the literal `SellerName`; replaced with contract/model-level tests.
- An empty phone produced two errors («مطلوب» and «صيغة»); fixed with `CascadeMode.Stop`.
- Test wiring: the in-test AutoMapper lacked the `Category → CategoryDto` profile (production
  registers all profiles); the test now loads every profile.
- Three first-run HTTP assertions were wrong assumptions, each confirmed by a follow-up probe:
  admin edits are exempt from re-review by design (an **owner** edit returns the listing to Pending —
  verified); the advertisers response is cached 60 s (fresh query shows both doctors); null
  `otherSpecialty` is omitted from JSON by the API's serializer settings.

**Could not be run:** frontend lint / type-check / build (repository not present).
**Pre-existing failures:** none.

---

## L. Security and Regression Review

- **AuthZ:** create requires a signed-in user; update/delete are owner-or-admin (`GetOwnedAsync` vs
  admin path) — a stranger gets 404 (HTTP ✔). Admin moderation reuses the existing permissioned V2
  endpoints. Global account-state gate and rate limits apply unchanged.
- **Visibility:** new listings start `Pending`; anonymous users get 404 until approval; the owner sees
  their own pending listing (HTTP ✔). Owner edits return the listing to review (existing interceptor).
- **Validation:** FluentValidation only, all messages Egyptian Arabic (`NoEnglishInResponsesTests`
  and `UserFacingLanguageTests` pass); the backend re-checks every rule the form advertises.
- **Uploads:** the existing `IFileService` pipeline (magic-byte detection, size/count limits, request
  size limit); files deleted if the create fails; images required on create and cannot drop to zero
  on update.
- **Search:** user input in `LIKE` patterns is escaped; no in-memory filtering.
- **Database safety:** additive migration; seed guarded against id collisions; no existing row
  changed (proved by the empty diff for the existing 64 category rows).
- **Regressions:** all 2,277 pre-existing tests still pass; legacy lookup contracts re-checked over
  HTTP (cars, birds, paintings, apartments, ask-consults, furniture) and by the 58-sub route suite.

---

## M. Remaining Work and Deployment Checklist

1. **Frontend** (section I) — in the frontend repository.
2. **Review and commit** the working tree (nothing has been committed).
3. **Deploy** (manual, per `docs/DEPLOYMENT.md`): publish, copy, restart. `Program.cs` runs
   `MigrateAsync` at start-up, so the migration applies on first start against the configured
   database. Back up the production database first. If start-up fails with
   `THROW 51000 … id 13 is already used`, an admin-created category occupies the id — decide on new
   ids before retrying (the migration rolled back cleanly).
4. **After deploy:** `GET /api/lookups/categories-tree` shows صحتي; `create-ad-form/13/53` returns
   the doctor form.
5. Optionally set category/sub-category icons and sort order from the admin catalog screen.

---

## N. Final Status

**IMPLEMENTED WITH DOCUMENTED LIMITATIONS**

Evidence: the backend feature, the three lookup endpoints and the lookup cleanup are implemented and
verified by a clean build, 2,745 passing tests, a migration proven on LocalDB in all four paths, and
real HTTP runs against the built API (201/204 first-pass assertions, the remaining 3 resolved as test
assumptions by a follow-up probe). It is not "COMPLETE AND VERIFIED" because the
frontend could not be modified or built from this workspace, and the migration has not been applied
to the shared/production database.
