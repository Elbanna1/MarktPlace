# Conventions

[← README](../README.md) · Related: [DEVELOPMENT_GUIDE](DEVELOPMENT_GUIDE.md) · [PROJECT_STRUCTURE](PROJECT_STRUCTURE.md)

Conventions observed in the existing code. Follow them so 48 modules keep looking like one codebase.

---

## Naming

| Thing | Pattern | Example |
| --- | --- | --- |
| Entity | Singular | `Land`, `Camel`, `MenClothing` |
| Child entity | `<Entity><Child>` | `LandImage`, `LandUtilitySelection` |
| Table | Plural | `Lands`, `LandImages` |
| Repository | `<Entity>Repository` | `LandRepository` |
| Service | `<Entity>Service` | `LandService` |
| Controller | Plural + `Controller` | `LandsController` |
| Interface | `I<Name>` | `ILandRepository` |
| Create/update DTO | `Create<Entity>Request` / `Update<Entity>Request` | `CreateLandRequest` |
| List DTO | `<Entity>ListItemDto` | `LandListItemDto` |
| Details DTO | `<Entity>DetailsDto` | `LandDetailsDto` |
| Filter | `<Entity>FilterParams` | `LandFilterParams` |
| Validator | `<Request>Validator` | `CreateLandRequestValidator` |
| Mapping profile | `<Entity>MappingProfile` | `LandMappingProfile` |
| EF configuration | `<Entity>Configuration` | `LandConfiguration` |
| Catalogue | `<Area>Catalog` | `AnimalCatalog`, `NotificationCatalog` |
| Route | kebab-case plural | `/api/lands`, `/api/wholesale-traders` |
| Test | `A_sentence_describing_the_rule` | `A_listing_a_real_seller_would_post_is_accepted` |

**Namespaces do not follow folder names.** `Infastrucre/Presitance` → `Persistence`,
`Infastrucre/Presantion` → `Presentation`. Use the namespace, not the folder.

---

## Folder organisation

- Group by **category**, then by module: `Shared/DTOs/RealEstate/Land/`.
- A module's files carry the module's name — never `Helper`, `Manager`, `Utils`.
- Cross-cutting code goes in a named seam (`Listings/`, `AdForms/`, `ReadConfigs/`), not a `Common/`
  dumping ground.

---

## DTOs

- Three per listing module: create/update request, list item, details.
- Create and update are usually identical; the gallery is the difference and it is enforced in the
  **service**, not the validator (an update legitimately posts no new files).
- Multipart requests carry `List<IFormFile> Images` and `IFormFile? Video`. This is why `Shared`
  references `Microsoft.AspNetCore.App`.
- Filters extend `PaginationParams`.
- No business logic in a DTO. No `[Required]` — DataAnnotations are **inert** here; FluentValidation
  is the only validation.
- A field the card does not show does not belong on the list DTO. Payload size matters.

---

## Services

- One service per module, named after it.
- Constructor injection only.
- Every public method takes a `CancellationToken` where an async path exists.
- Failure is an `AppException` subclass — services never return status codes.
- Private helpers named for what they do: `BuildDetailsAsync`, `LoadForWriteAsync`,
  `ApplyEditableFields`, `AttachImagesAsync`.
- Standard details shape:

```csharp
private async Task<LandDetailsDto> BuildDetailsAsync(
    Guid id, CancellationToken ct = default, bool includeUnmoderated = false)
{
    var entity = await _repository.GetByIdAsync(id, asNoTracking: true, ct, includeUnmoderated)
        ?? throw new NotFoundException("إعلان الأرض مش موجود.");
    …
}
```

---

## Repositories

- EF Core only. No business rules, no exceptions — return `null` and let the service decide.
- `AsNoTracking()` for reads; tracked for writes.
- `AsSplitQuery()` when more than one collection is included.
- Paged reads go through `PagedListingQuery`.
- The by-id read applies `VisibleToViewer(includeUnmoderated, _context.ViewerUserId)`.

---

## Validation

- One validator per request DTO, in `Core/Services/Validation/`.
- Shared rules for a category live in `<Category>ValidationRules` static helpers.
- Every message is Egyptian Arabic and never names a property.
- `.When(...)` mirrors the form's `requiredWhen` exactly.
- `NotNull` and `InclusiveBetween` are separate `RuleFor` blocks.

---

## API

- Route: `[Route("api/<module>")]` or a constant from `<Category>Routes`.
- `[Authorize]` on the controller; `[AllowAnonymous]` on the public reads.
- Create returns **201**; everything else **200**.
- Always wrap in `ApiResponse<T>.Ok(data, UserMessages.X)`.
- Declare `[ProducesResponseType]` for every status returned.
- Multipart endpoints: `[Consumes("multipart/form-data")]` + `[RequestSizeLimit(...)]`.
- No XML documentation. Swagger publishes methods, routes, parameters, bodies, responses and
  schemas only — `MinimalSwaggerDocumentFilter` keeps the UI minimal and no XML comments are fed
  into it, so removing them changed nothing about the published document.

---

## Error handling

| Situation | Do |
| --- | --- |
| Business failure | Throw an `AppException` subclass with an Arabic message |
| Not found **or** not visible to you | `NotFoundException` — never 403, which would confirm the row exists |
| Unexpected | Let it reach the middleware. It logs a stack trace and answers a generic Arabic message |
| Best-effort side effect (view counter, real-time push) | Catch and log a warning. It must never fail the request |

Never `catch (Exception) { }` silently. Never return an error string in `data`.

---

## Database

- One `IEntityTypeConfiguration<T>` per entity, in the category folder.
- Explicit `HasMaxLength` on every string.
- Soft-delete filter registered by name: `HasQueryFilter(QueryFilterNames.SoftDelete, x => !x.IsDeleted)`.
- Moderation is applied model-wide — **do not** add it per entity.
- FKs into `AspNetUsers`: `DeleteBehavior.NoAction`. To a listing's own children: `Cascade`.
- Multi-select = a composite-key selection table, not a delimited column.
- Lookup tables seeded with `HasData` from a `Shared/Constants` catalogue.
- Migration names are descriptive PascalCase: `AddLandSearchCoveringIndex`.

---

## Permissions

- A page is a **stable string key** in `AdminPageCatalog`, never an id.
- A page declares only the permissions that map to real endpoints.
- Controller: `[AdminPage(key)]`. Action: `[RequireAdminPermission(...)]`.
- Grants are read per request from the database, never from the JWT.

---

## Testing

- Hermetic by default — no database, no host.
- A test name is the specification: it states the rule the test protects, as a sentence.
- Source-scanning tests use `RepositoryRoot.SourceFiles()`.
- Assert `4xx` or `422`, never `400`, for validation.
- Mutation-test a new guard before trusting it.

---

## User-facing language

> The project's hardest rule.

| Rule | Detail |
| --- | --- |
| Language | Egyptian Arabic (colloquial), not Modern Standard |
| Location | `Shared/Constants/UserMessages.cs` — nested groups: `Listings`, `Lookups`, `Auth`, `Profile`, `Notifications`, `Comments`, `Posts`, `Payments`, `Generic`, `Errors` |
| Naming in messages | Name the **thing** (إعلان، منشور، إشعار), not the module — the body already carries the listing |
| Forbidden | Exception types, SQL, paths, DTO/property names, enum names, status-code names |
| Logs | Stay technical and English — a different audience |

Examples:

| ❌ | ✅ |
| --- | --- |
| `"Land listing deleted successfully."` | `تم حذف الإعلان.` |
| `"Validation failed."` | `فيه بيانات ناقصة أو مش صحيحة. راجع الحقول وجرّب تاني.` |
| `"Unauthorized."` | `لازم تسجل دخولك الأول عشان تعمل العملية دي.` |
| `"LandLength is required."` | `من فضلك اكتب طول الأرض.` |

Latin fragments that are legitimate inside an Arabic message: units (`px`, `cm`, `km`), currency
(`EGP`), file formats (`PDF`, `MP4`), brand names (`WhatsApp`, `Google Maps`), and `Spam` — what an
Egyptian user's mail client calls the folder.

**Enforced by three suites**, including `NoEnglishInResponsesTests`, which scans the source and fails
the build.

---

## Code comments

**The source carries no comments.** Explanatory comments and XML documentation were removed from the
entire codebase; the explanations they held live in this `docs/` folder instead. The rule is:

| | |
| --- | --- |
| Source code | Clear naming, small focused methods, types and interfaces |
| Documentation | `docs/` |

Do not reintroduce `///`, `//`, `/* */` or `<!-- -->` explanations. If something needs explaining,
the right places are a name, a type, or the relevant page in `docs/` — and if a rule is being
recorded, [BUSINESS_RULES.md](BUSINESS_RULES.md) or [DECISIONS.md](DECISIONS.md).

**Preserved, and not to be removed:**

- Everything under `Infastrucre/Presitance/Migrations/` — generated by the EF scaffolder and carrying
  the `// <auto-generated />` marker that analysers honour.
- Preprocessor directives (`#region`, `#nullable`, `#pragma`, `#if`). They look like annotations but
  are compiler input, not comments.

## Git

No `.git` directory is present in the working copy inspected, and no CI configuration or commit
history was available, so **no commit-message convention could be verified**. Follow the descriptive
PascalCase style used for migration names and keep changes scoped to one concern.
