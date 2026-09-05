# Testing

[← README](../README.md) · Related: [DEVELOPMENT_GUIDE](DEVELOPMENT_GUIDE.md) · [SECURITY](SECURITY.md) · [PERFORMANCE](PERFORMANCE.md)

Testing is split in two on purpose:

| | In-repository | External |
| --- | --- | --- |
| **What** | `Tests/MarkatPlace.Tests` (xUnit) | HTTP suites run against a running instance |
| **Guards** | Structure, invariants, validation logic | Behaviour across all modules and roles |
| **Speed** | 1,532 tests, < 1 s | Minutes |
| **Needs** | Nothing | A running API + a database |

Hermetic tests guard the *shape* so a bug cannot be reintroduced silently; HTTP suites prove the
*behaviour*.

---

## Running the test project

```bash
# everything
dotnet test Tests/MarkatPlace.Tests/MarkatPlace.Tests.csproj

# one class
dotnet test Tests/MarkatPlace.Tests/MarkatPlace.Tests.csproj \
  --filter "FullyQualifiedName~OwnerVisibilityTests"

# one test
dotnet test Tests/MarkatPlace.Tests/MarkatPlace.Tests.csproj \
  --filter "FullyQualifiedName~A_listing_a_real_seller_would_post_is_accepted"

# release configuration
dotnet test Tests/MarkatPlace.Tests/MarkatPlace.Tests.csproj -c Release
```

Expected: **1,532 passing, 0 failing, 0 skipped**.

> Stop any running API first, or the build cannot overwrite the locked DLLs.

---

## Project layout

`Tests/MarkatPlace.Tests` — xUnit, net10.0, referencing every project.

| File | Guards |
| --- | --- |
| `EgyptianInputTests` | Egyptian mobile prefixes, الفيوم مراكز **and their order** |
| `LandValidationTests` | Land create/update rules, conditional requirements, no property names in messages |
| `UserFacingLanguageTests` | Every `UserMessages` constant is Arabic and free of Latin prose |
| `NoEnglishInResponsesTests` | Source-wide sweep of every response/exception call site |
| `UploadSecurityTests` | Magic-byte detection, executables, SVG exclusion, size ceilings |
| `DeploymentSafetyTests` | No committed secrets, IIS ≥ app upload limit, diagnostics off |
| `ReverseProxyHostingTests` | Forwarded headers behind Nginx, no IIS requirement, no pinned listening address |
| `OwnerVisibilityTests` | The owner-visibility rule cannot be reintroduced-broken |
| `ArabicText`, `RepositoryRoot`, `CSharpSource` | Helpers |

### Why some tests read the source, not the assemblies

A `const` is inlined by the compiler and an interpolated string is gone by then — the source is the
only place a sentence a user will read is still visible as one piece. `RepositoryRoot` walks up from
the test binary to find `MarkatPlace.slnx`, so these work from an IDE, from the CLI and in CI.

`CSharpSource` is a small C# string lexer. It exists because a naive scanner mis-reads interpolated
strings: `$"المركز لازم يكون واحد من: {string.Join(", ", Centers)}."` contains a `", "` literal
inside an interpolation hole, and treating it as a separate literal reports every such message as
English.

---

## What is protected

### Language

`UserFacingLanguageTests` + `NoEnglishInResponsesTests` fail the build if an English sentence appears
in `ApiResponse.Ok/Fail`, any of the six application exceptions, or FluentValidation's
`WithMessage`. Judged **one argument at a time**, never one literal at a time.

This is not theoretical: 707 messages were English before these tests existed.

### Configuration safety

`DeploymentSafetyTests` pins defects that were actually present:

| Test | Prevents |
| --- | --- |
| `The_committed_configuration_carries_no_JWT_signing_key` | A signing key in a committed file |
| `The_production_configuration_carries_no_JWT_signing_key` | The same, via the production file |
| `No_administrator_account_is_configured_for_production` | A bootstrap admin reaching production |
| `IIS_accepts_at_least_as_large_a_body_as_the_application_does` | IIS rejecting uploads the app accepts |
| `Detailed_errors_and_stdout_logging_are_off_in_the_shipped_web_config` | Stack traces rendered to the browser |
| `Every_accepted_upload_format_declares_a_content_type…` | A file stored whose URL answers 404 |
| `Nginx_on_the_same_host_is_trusted_without_any_configuration` | Losing https on generated URLs behind the proxy |
| `An_unknown_caller_cannot_forge_the_scheme_the_host_or_the_client_ip` | Trusting `X-Forwarded-*` from the internet |
| `No_production_code_path_requires_IIS` | IIS creeping back into a Linux deployment |
| `The_shipped_configuration_hard_codes_no_listening_address` | A pinned endpoint overriding `ASPNETCORE_URLS` |

> The web.config test strips XML comments before asserting, so it is unaffected by whether the file
> carries explanatory comments.

### Owner visibility

`OwnerVisibilityTests` (10 tests) pins the shape the fix depends on:

- no module still gates its by-id read on `includeUnmoderated` alone
- every module with that flag calls `VisibleToViewer`
- every windowed listing entity has a mapped `string UserId`
- the rule never drops the soft-delete filter
- the public branch still tests approval **and** the window
- an anonymous caller leaves the query untouched
- both view counters tolerate an unresolvable listing
- the viewer identity comes only from the authenticated principal

### Egyptian input

A validation rule that is too strict is indistinguishable from a broken site. These tests assert that
**every** real Egyptian mobile prefix (010/011/012/015) is accepted, and that the مراكز list order is
stable — the seeded `Centers` ids come from list position.

### Upload safety

Format is decided by content. Executables and scripts are rejected; SVG is excluded on purpose; the
canonical extension can never come from the submitted file name.

---

## Mutation testing

A guard that has never failed is a guard you cannot trust. Every guard in this project has been
verified by reintroducing the bug:

| Mutation | Caught by |
| --- | --- |
| An English success message in `LandsController` | `NoEnglishInResponsesTests` |
| IIS ceiling lowered back to 55 MB | `DeploymentSafetyTests` |
| The compromised JWT key back in `appsettings.json` | `DeploymentSafetyTests` |
| Phone rule tightened to reject WE's 015 | `EgyptianInputTests` |
| `LandRepository` reverted to the pre-fix shape | `OwnerVisibilityTests` |
| The Lost & Found `catch (NotFoundException)` removed | `OwnerVisibilityTests` |

**Do this for any new guard.** Reintroduce, confirm red, revert, confirm green.

---

## HTTP suites

Behaviour that needs a database is proven against a running instance. These suites are **not** in the
repository — they are operational tooling. They are documented here so their coverage is known and
they can be rebuilt.

### Running the API for testing

```bash
export ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_URLS=http://127.0.0.1:5099
export ConnectionStrings__DefaultConnection='Server=(localdb)\MSSQLLocalDB;Database=MarkatPlaceTest;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True'
export DemoData__Enabled=false
export Diagnostics__SqlCounter=true
export RateLimiting__PermitPerMinute=1000000
export RateLimiting__AuthPermitPerMinute=1000000

cd MarkatPlace && dotnet run
```

> **Never test against `appsettings.json`'s default connection string** — it is the shared hosted
> database.

### The schema-driven engine

The key idea: **build every payload from the module's own published form schema**, so nothing
hard-codes a field list.

1. `GET /api/lookups/create-ad-form/{cat}/{sub}`
2. Fill every unconditionally-required field
3. Loop, filling every field whose `requiredWhen` the payload now satisfies, until stable —
   conditions chain
4. `POST` to `submit.endpoint`

Omitting one required field at a time and expecting 4xx is what proves form ⇄ validator agreement
across all modules.

### Coverage achieved

| Suite | Assertions | Covers |
| --- | --- | --- |
| **Owner visibility — all modules** | 676 | Every one of the **52 sub-categories** × the A–J matrix: anonymous/stranger/owner/admin × pending/approved/rejected/deleted/unknown-id |
| **Owner visibility — deep** | 38 | 10 IDOR vectors, edit→re-review, suspension, expiry, soft delete |
| **Land module** | 226 | Every Land endpoint × every role; validation, boundaries, paging, filters, idempotence |
| **Security** | 69 | All 131 admin operations × anonymous + non-admin; mass assignment; enumeration; uploads; injection; error hygiene; headers |
| **Real-time** | 25 | A real SignalR client over WebSocket: delivery, isolation, Arabic content, persistence, best-effort |
| **Rate limiting** | 8 | Auth + global policies, Arabic 429, `Retry-After`, health exemption, recovery |
| **5xx sweep** | 1,242 requests | 414 GET operations × 3 roles — **0 unexpected 5xx** |

### Contract details that cost time

Learned the hard way; check these before writing a test:

- Login is `{username, password}` — **not** email.
- Registration needs a real Egyptian mobile and one of the seven مراكز.
- Creates are **multipart only** → JSON gets 415. Put *every* field in `files=` as
  `(name, (None, value))`; `data=` alone silently switches to urlencoded.
- Validation is **422**, not 400.
- `POST /{id}/report` takes `type` in the **body**; the other interactions take it in the query.
- Notification lists clamp `pageSize` to 50 — count with `totalCount`.
- Admin probes need **typed** path parameters: a GUID where an `int` is expected 404s at routing
  before authorization, proving nothing.
- `LostItem = 70`, `FoundItem = 71` — a module-type probe loop must clear 71.
- سيارات and رجال أعمال share `/api/ads`; the payload's `SubCategoryId` decides which validator
  branch applies.
- Banner images are validated for **exact pixel dimensions**.

### Performance measurement

Set `Diagnostics__SqlCounter=true` and read the response headers:

| Header | Meaning |
| --- | --- |
| `X-Sql-Count` | Database round trips for the request |
| `X-Sql-Ms` | Time spent in SQL |
| `X-Elapsed-Ms` | Total request time |

**The real N+1 test is that cost does not grow with page size or image count.** A constant is not a
defect. Measured constants: public feed 9, details 13–16, my-listings 2, admin dashboard 33.

See [PERFORMANCE.md](PERFORMANCE.md).

---

## What is not covered

Stated plainly so nobody assumes otherwise:

- **No integration tests in the repository.** There is no `WebApplicationFactory` harness; the
  in-repo suite is deliberately hermetic.
- **No load/soak testing.** Latency figures come from single-client measurement.
- **No CI configuration** was found in the repository.
- **No frontend tests** — this repository is the backend only.
