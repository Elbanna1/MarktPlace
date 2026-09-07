# Architecture Decisions

[← README](../README.md) · Related: [ARCHITECTURE](ARCHITECTURE.md) · [BUSINESS_RULES](BUSINESS_RULES.md) · [PERFORMANCE](PERFORMANCE.md)

**Read this before "cleaning up" something that looks unusual.** Every entry describes something that
looks like a mistake and is not.

---

## D-01 — The misspelled project folders stay

**Problem.** Two project folders are misspelled: `Infastrucre/Presitance` and
`Infastrucre/Presantion`.

**Decision.** Leave them. The **namespaces are correct** (`Persistence`, `Presentation`) and the
project files, references and solution entries are consistent.

**Why.** Renaming would touch every file in the solution for no functional gain, and carries real
risk against a working deployment. The namespaces — which is what code actually reads — are right.

**Consequence.** Folder names and namespaces differ. Navigate by namespace.

---

## D-02 — 48 independent modules instead of a shared base class

**Problem.** 48 listing types with wildly different fields, lookups and validation.

**Decision.** Each module owns a full vertical slice — entity, configuration, repository, service,
validator, mapping profile, DTOs, controller. Cross-cutting rules live in **shared seams**, not a
base class.

**Why.** A camel and a flat share almost nothing. A base class would force a shape that fits neither
and would be modified for every new module until it fitted none. The seams — model-wide EF
configuration, global action filters, catalogues — capture what is genuinely common without
constraining what is not.

**Alternative rejected.** A generic `ListingService<TEntity, TDto>`. Tried in spirit and rejected:
the conditional-field logic, module-specific filters and per-module lookups do not generalise.

**Consequence.** More files, but a change to one module cannot break another. Adding a module is a
mechanical copy of the nearest neighbour.

---

## D-03 — Moderation applied by walking the model

**Problem.** 48 modules must all be moderated, and forgetting one would silently publish unreviewed
listings.

**Decision.** `ModerationModelConfiguration.ApplyModerationConfiguration()` walks the EF model for
`IModeratedListing` and applies the columns, the indexes and the visibility filter to every one.

**Why.** Brevity is the smaller half. The real reason is that a module added tomorrow is moderated
**the moment its entity implements the interface** — there is no configuration step to forget.

**Consequence.** Do **not** add moderation configuration per entity. Implementing the interface is the
whole enrolment.

---

## D-04 — Named query filters, never bare `IgnoreQueryFilters()`

**Problem.** Two filters (soft delete, moderation) and callers who legitimately need to bypass **one**
of them.

**Decision.** Both filters are registered by name. `IncludingUnmoderated()` drops the moderation
filter **by name**.

**Why.** A bare `IgnoreQueryFilters()` drops **both** and quietly resurrects deleted listings.

**Consequence.** A guard test asserts the owner-visibility rule never calls the bare form.

---

## D-05 — The publication window is layered into the moderation filter

**Problem.** "Publicly visible" is two conditions: approved **and** inside the window.

**Decision.** One named filter carrying both, rather than two filters.

**Why.** Every caller that legitimately looks past the first also looks past the second — the owner
reading their expired listing to republish it, the administrator reviewing it. A second filter would
mean auditing **139 call sites** for a matching second call, with a silently disappearing listing
wherever one was missed.

---

## D-06 — Owner visibility is enforced in the query, not by an attribute

**Problem.** A seller who posted or edited a listing got **404** on their own advertisement until an
administrator looked at it. 47 of 48 modules.

**Root cause.** The details path never carried the caller's identity, so it treated everyone as a
stranger.

**Decision.** `ModerationQueryExtensions.VisibleToViewer(includeUnmoderated, viewerUserId)` drops the
moderation filter by name and restates it explicitly, OR-ed with ownership. The viewer id reaches the
repository through `AppDbContext.ViewerUserId`, backed by the existing `IAdminActionContext`.

**Why in the query.** It **fails closed** — a mistake returns fewer rows, never more. A service-level
gate that was forgotten would leak unapproved listings; a query that is wrong simply returns nothing.

**Why one query.** Ownership is OR-ed onto the same index seek. Measured: **13/16/15 round trips
before and after**, **3 logical reads before and after**.

**Alternatives rejected.**

| Alternative | Why not |
| --- | --- |
| Make the **global query filter** owner-aware | One file, but it would also put a seller's unapproved listings into feeds, search, strips, counts and `totalCount` — a per-viewer pagination contract |
| Thread `viewerUserId` through controllers → interfaces → services → repositories | ~126 files across three layers, and it changes the public service contracts |
| A narrow ownership pre-check, then a second load | An extra round trip on the most-hit endpoint |

**Consequence.** `GetByIdAsync` depends on ambient request state. That is why
`OwnerVisibilityTests` pins the shape, and why the viewer id can only come from the authenticated
principal.

**سيارات is excluded** — it has no `IExpiringListing` and states the same rule in its own service,
where its extra window/status refusals live.

---

## D-07 — Cross-cutting listing behaviour lives in global action filters

**Problem.** Views, admin alerts, statistics and edit-review announcements must happen for **every**
module's endpoints.

**Decision.** Five global action filters rather than per-module code.

**Why.** 48 services remembering to do the same four things is 48 chances to forget, and the card
would eventually disagree with the details page. `ListingStatsFilter` in particular costs **one
batched query per response** instead of a join per module.

**Consequence.** A new module gets views, alerts, stats and edit announcements for free.

---

## D-08 — `AdminPermissionFilter` fails closed

**Problem.** How do you guarantee every sensitive admin operation is protected?

**Decision.** An administration action with **no** `[RequireAdminPermission]` is **refused**, and the
refusal is logged as an **error**.

**Why.** The safe reading of a bug in an authorization rule is "deny". Forgetting the annotation
breaks the endpoint in development — where it simply will not work — rather than in production, where
it would have worked for everyone.

**Consequence.** A new admin endpoint must be annotated before it functions. That is the point.

---

## D-09 — Admin pages are code; grants are data

**Problem.** Where should the definition of "a dashboard page" live?

**Decision.** `AdminPageCatalog` declares the pages as **stable string keys**. The database stores
only who was granted what.

**Why.** A page key must match a controller annotation. A table of names that nothing enforces would
drift. Adding a page is one catalogue entry plus two attributes — **no migration**.

**Consequence.** Each page declares only the permissions that map to real endpoints. A permission that
maps to nothing would read as authority the holder does not have.

---

## D-10 — Two permission tables, not one bitmask column

**Decision.** `AdminPageGrants` + `AdminPagePermissions`.

**Why.** "Who may delete advertisements" has to be a **join**, not a bitwise scan over every row. The
assignment is data an operator edits and an auditor queries.

---

## D-11 — `ModeratedBy` is a plain column, not a foreign key

**Decision.** `nvarchar(450)`, no FK to `AspNetUsers`.

**Why.** Two reasons. It is an **audit trail** — it must survive the reviewer's account being
removed. And 48 more relationships into the Identity table is exactly the shape that produces SQL
Server error 1785 (multiple cascade paths).

---

## D-12 — Key-first paging

**Problem.** With `AsSplitQuery`, EF repeats the whole filtered, sorted, paged sub-query **in every
split statement**. The sort was paid once per `Include`.

**Decision.** Fetch the page's keys first, then load those rows and their collections **by key**.

**Measured (65k apartments, three collections).** Root: 10,177 logical reads / 88 ms → **3 reads**.
Each collection: another ~95 ms → **2 reads**.

**Why it cannot change results.** The second query is built from the same `IQueryable`, so every
filter is still in force and none is restated.

---

## D-13 — `ToStripAsync` only where the sort is expensive

**Problem.** `similar` and `related` had the same repeated-sort problem.

**Decision.** Apply key-first to `similar` and `related` — but **not** to `recently-added`.

**Why.** Measured. `similar` 656 → 167 ms (3.9×); `related` 1,330 → 354 ms (3.8×). But
`recently-added` got **slower**: 8.7 → 10.3 ms, because its `ORDER BY CreatedAt DESC` is already
index-served, so there is no sort to save and the extra keyed round trip is pure cost. It was
reverted.

**Rule, written in the helper's own documentation.** Use it only where the ordering is a computed
expression or a predicate the database cannot seek.

---

## D-14 — The search covering index is per module, not shared

**Problem.** Substring search scanned the 197 MB clustered index for all 200,000 rows.

**Decision.** `IX_Lands_Search` — a covering index on أراضي only.

**Measured.** 25,204 → **1,625** logical reads (15.5×); price statistics 116 → 14 ms (8.2×); sorting
182 → 65 ms; broad search 122 → 31 ms. Index size 58 MB.

**Why it cannot be shared.** Tested twice: leaving out **one** searched column (`OtherLandType`) made
the optimiser **abandon the index entirely** and return to the 25,204-read scan, for zero benefit.
Each module searches its own "أخرى" columns, so the covering set is genuinely per module.

**Why only أراضي.** It is where the gain was measured. ~58 MB per 200,000 rows each is a storage
decision to be made per module, with a measurement — not applied blindly.

**Full-text search** would fix the remaining narrow-term case but **could not be verified**: the
environment inspected reported `IsFullTextInstalled = 0` and no Arabic word breaker.

---

## D-15 — All user-facing text in one catalogue

**Problem.** 707 messages a customer reads were English. A Fayoum seller deleting an advertisement
was told *"Land listing deleted successfully."*

**Decision.** `Shared/Constants/UserMessages.cs` is the only place a customer-facing message is
written. 344 distinct strings collapsed into ~50 constants.

**Why generic.** Messages name the **thing** (إعلان، منشور), not the module — the response body
already carries the listing and the screen already says which section the user is in. Naming the
module would put back the 48-way switch the catalogue removes.

**Enforcement.** `NoEnglishInResponsesTests` scans the **source** — a `const` is inlined and an
interpolated string is gone by compile time, so the source is the only place the sentence is still
visible as one piece. It judges **one argument at a time**, because
`$"المركز لازم يكون واحد من: {string.Join(", ", Centers)}."` contains a `", "` literal inside an
interpolation hole.

---

## D-16 — FluentValidation only; ModelState 400 suppressed

**Decision.** `SuppressModelStateInvalidFilter = true` and `SuppressMapClientErrors = true`.
Validation answers **422**.

**Why.** One source of validation, one response shape. `SuppressMapClientErrors` matters
independently: MVC's RFC 7807 mapping writes its own body **before** `UseStatusCodePages` can answer,
so a 415 came back as a raw ProblemDetails document while every other failure used the platform
envelope.

**Consequence.** DataAnnotations are **inert**. Assert `422`, never `400`.

---

## D-17 — Format decided by magic bytes, name discarded

**Decision.** `ImageFormatCatalog.Detect` reads the first bytes. The submitted file name is thrown
away; files are stored as a GUID plus the **detected** extension.

**Why.** Name and MIME type are caller-supplied strings that prove nothing. Real clients get them
wrong constantly — a browser uploading a cropped `Blob` sends the name "blob" with no extension, and
some SDKs send `application/octet-stream` for everything. Reading the signature is **more permissive
for honest clients and stricter against hostile ones**.

**Consequence.** `a.png.php` and `../../../evil.png` are accepted — and stored as `<32 hex>.png`.
Path traversal and double extensions are structurally impossible. Rejecting them would only punish
users whose phone produced an odd file name.

**SVG is excluded on purpose** — XML with no signature that can carry script running against our own
origin.

---

## D-18 — Real-time delivery is best-effort

**Decision.** `NotificationService` persists and calls `SaveChanges` **first**, then pushes inside a
`try/catch` that logs a warning.

**Why.** The notification centre works either way. A dropped socket must never fail the business
operation that caused it. But the failure is logged — a transport failing for a week is something
operators must be able to see.

---

## D-19 — Expiry is a column, not a status

**Decision.** Public visibility filters on `ExpireAt`. The hourly job only stamps status and raises
notifications.

**Why.** `DateTime.UtcNow` in a query filter translates to `GETUTCDATE()`, so a listing stops being
public **the moment its window closes** rather than when a job next runs — which could be up to an
hour later.

**Consequence.** The stored status can lag the real window. **Read `ExpireAt`.**

---

## D-20 — Identity's username whitelist is emptied

**Decision.** `options.User.AllowedUserNameCharacters = string.Empty`.

**Why.** Identity's default is the ASCII set `a-zA-Z0-9-._@+`, which rejects **every Arabic
username** and would veto a name FluentValidation had already accepted — producing an Identity error
instead of the platform's Arabic message.

**Why it is not a weakening.** The decision moves to `AccountNameRules`, which is **stricter**: Arabic
and Latin letters, digits, `.` `_` `-`, and single spaces. The validator runs first and the service
stores only the normalised value.

---

## D-21 — Roles are re-read from the database on every request

**Problem.** A JWT carries the roles held when it was issued, so a promoted user stayed refused until
they signed in again — access tokens live 40 days.

**Decision.** `DatabaseRoleClaimsTransformation` (`IClaimsTransformation`) rewrites roles from
`AspNetUserRoles`.

**Why there.** A claims transformation runs inside the **authentication** middleware, ahead of the
authorization middleware that evaluates `[Authorize(Roles = …)]` — the only point early enough.

**Consequence.** Uncached on the admin surface, cached elsewhere.

---

## D-22 — CORS is a configured allow-list *(superseded the fully open policy)*

**Decision.** One policy, `MarkatPlaceCors`, built from `Cors:AllowedOrigins`:
`WithOrigins(...)`, `AllowAnyHeader`, `AllowAnyMethod`, `AllowCredentials`. In Development any
loopback origin is additionally allowed, so a developer's chosen port never needs a config change.
`Cors:AllowAnyOrigin=true` restores the previous fully open behaviour without a redeploy.

**It used to be `AllowAnyOrigin`.** That was defensible — authentication is a bearer token in the
`Authorization` header and SignalR authenticates via the `access_token` query string, so nothing
relied on cross-origin cookies. But it also meant `AllowCredentials` could never be used (the CORS
protocol forbids combining it with `*`), and an allow-list costs nothing when the set of origins is
this small and this stable.

**Why an unparseable origin fails start-up.** A typo in the list would otherwise surface as the site
being unable to call the API at all, with the reason visible only in the browser console.

**Consequence.** Apex and `www` are different origins; both are listed. A new frontend origin must be
added to `Cors:AllowedOrigins` (and, for Google Sign-In, to the Google Console's Authorized
JavaScript origins) before it can call the API.

---

## D-23 — Swagger UI entry points are `no-store`

**Problem.** A newly added Swagger document was missing from the picker while its JSON answered 200,
and redeploying did not fix it.

**Decision.** `/swagger`, `/swagger/index.html` and `/swagger/index.js` are marked
`no-store, no-cache, must-revalidate`.

**Why.** Swashbuckle serves its UI assets with `max-age=604800` (7 days) and no content hash. That is
fine for the bundled script, but `/swagger/index.js` is the **generated** file carrying the document
picker's list — so a browser replays a week-old configuration straight from disk cache and the server
is never asked.

---

## D-24 — The rate-limit policy is registered even when limiting is disabled

**Decision.** With `RateLimiting:Enabled=false`, the named `auth` policy is still registered — with
no limit.

**Why.** The authentication endpoints carry `[EnableRateLimiting("auth")]`, and an endpoint whose
policy does not exist makes the middleware **throw**. Returning early turned "disabled" into HTTP 500
on every login, registration and password reset. Disabled means "no limit", not "no policy".

---

## D-25 — Manual payments, no gateway

**Decision.** A payment is created `Pending`; the user uploads a transfer screenshot; an administrator
approves or rejects.

**Why.** No online payment provider is integrated. `PaymentRequiredException` (402) exists as the
integration point for a future one.

**Consequence.** `PaymentMethods` is a type-driven settings table — a new method is an INSERT. A
method that has been used cannot be deleted, only deactivated, so existing payments keep their
history.

---

## D-26 — The uploads root is pinned explicitly

**Problem.** Empty folders are not included by `dotnet publish`, so on a fresh deployment `wwwroot`
does not exist — and ASP.NET Core then **silently resolves the web root to the content root**.

**Decision.** `Program.cs` computes the uploads root from `ContentRootPath`, creates every folder on
start-up, tells the file service exactly where to write, and serves that same folder with its own
`PhysicalFileProvider`.

**Why.** Writer and reader can never drift apart, and the URL stored in the database always opens.

---

## D-27 — Content-type mappings are driven off the upload catalogues

**Problem.** A file whose extension has no mapping is stored, its URL is saved to the database, and
the request answers **404**.

**Decision.** `Program.cs` builds the static-file `ContentTypeProvider` by looping over
`ImageFormatCatalog`, `DocumentFormatCatalog` and `VideoFormatCatalog` — the same catalogues the
validators use.

**Why.** A format can never be accepted for upload without also being servable.
`ServeUnknownFileTypes` stays `false`.

---

## D-28 — Tests are hermetic; behaviour is proven over HTTP

**Decision.** The in-repository suite uses no database and no host. Behaviour across modules and roles
is proven by HTTP suites against a running instance.

**Why.** The hermetic suite runs in under a second on every build and guards the **shape** — that a
module still calls the shared rule, that no English reached a response, that the IIS limit still
clears the app's. Behaviour that genuinely needs data is proven where data exists.

**Consequence.** There is **no** `WebApplicationFactory` harness in the repository. That is a
deliberate split, not an omission.

---

## D-29 — Invitation links are built from the frontend URL, never from the request

**Problem.** `ReferralLinkBuilder` fell back to the incoming request's scheme and host whenever
`App:FrontendUrl` was unset. In production every request arrives at the API host, so every invitation
link came out as `https://api.shopiklopik.com/register?ref=CODE` — an address with no register page.

**Decision.** The builder resolves `App:FrontendUrl` (falling back only to `App:BaseUrl`) and throws
if neither is configured. It no longer takes `IHttpContextAccessor`. `AddAppUrlSettings` fails
start-up outside Development when the key is missing, is not an absolute `http`/`https` URL, or is a
loopback address.

**Why.** An invitation is a link to the *site*. Deriving it from the request can only ever produce
the API origin, and a silent wrong answer is worse than a start-up failure.

**Consequence.** No invitation URL is persisted. `Referral` and `ReferralLinkEvent` store the code
only, and the link is rebuilt on every read — so changing `App:FrontendUrl` fixes every invitation
that already exists, past and future, with no data migration.

---

## D-30 — Demo data cannot reach a non-development database

**Decision.** `DevelopmentDataSeeder.SeedAsync` returns a skipped report unless `IHostEnvironment` is
Development, before it resolves the `DbContext`. `DemoData:Enabled` now defaults to `false`, so the
seeder is opt-in even in Development — `appsettings.Development.json` is the only file that turns it
on.

**Why.** The `IsDevelopment()` guard in `Program.cs` was the only thing standing between a demo user
and a production database. Two independent guards, one of them inside the seeder itself, mean a
future caller cannot lose the protection by accident.

**Consequence.** `HasData` stays exactly as it was. Every seeded row is reference data the
application needs; nothing seeded belongs to a user. A test proves that by walking the EF model and
failing on any seeded entity that carries an owner column.

---

## D-31 — Google Sign-In verifies an ID token; there is no backend callback

**Decision.** `POST /api/auth/google` takes the ID token the browser obtained from Google Identity
Services and verifies it server-side with Google's own `Google.Apis.Auth`
(`GoogleJsonWebSignature.ValidateAsync`): signature against Google's published keys, issuer, expiry
with zero clock tolerance, and audience against `GoogleAuth:ClientId`. It answers with the same
`AuthResponse` the password login returns. An authorization `code` is accepted as an alternative and
exchanged server-side using the client secret.

**Why not a redirect/callback flow.** The API has no cookies and no server-side session, so a
callback landing on `api.shopiklopik.com` would have had to invent one just to hand a token back to
the site. The ID-token flow needs only an **Authorized JavaScript origin** in the Google Console —
no redirect URI, and no client secret at all.

**Why the Google subject and not the e-mail.** Identity is keyed on `(LoginProvider, ProviderKey)` =
`("Google", sub)` in Identity's existing `AspNetUserLogins` table, so **no migration was needed**. An
e-mail is only ever used to *link* to an account that already owns it, and only when Google reports
it as verified — an unverified address is refused outright, which is what closes the e-mail-only
takeover path.

**Consequence.** A Google-created account has **no password**, so it cannot be entered by guessing
one; the ordinary "forgot password" flow is how such a user adds one. It also has no phone number and
defaults to مركز الفيوم, both correctable in the profile screen — those two fields have no equivalent
in a Google profile.

---

## D-32 — One 256 MB request ceiling, and the reverse proxy is what enforces it

**Decision.** `FileUploadConstants.MaxRequestBodySizeBytes` is a single explicit **256 MB** and every
layer repeats exactly that number: Kestrel's `Limits.MaxRequestBodySize`, `FormOptions
.MultipartBodyLengthLimit`, every listing endpoint's `[RequestSizeLimit]`, Nginx's
`client_max_body_size`, and IIS's `maxAllowedContentLength`. The Nginx site is committed at
`deploy/nginx/api.shopiklopik.com.conf`, and `RequestSizeLimitTests` fails the build if any of the
five drifts.

**Why the per-module ceilings were collapsed.** They were computed — `images + video`, `video + cv +
image + slack` — and each came out different, so a request's fate depended on which of eight
arithmetic expressions its controller happened to reference. `POST /api/workshops` sat at 55 MB. One
documented number that the client can also *read* (`upload.maxRequestSizeMb` on the create-ad form)
is worth more than eight tightly-fitted ones. Nothing about what may be **stored** changed: 5 MB an
image, ten images, 50 MB a video, 10 MB a CV, magic-byte detection — all still enforced, all still
answering 400 with the precise Arabic sentence. The ceiling is only the outer bound on one HTTP
request, and it is deliberately finite.

**Why Nginx is the rejector, not Kestrel.** Nginx compares `Content-Length` before reading the body
and closes lingeringly, so the client reliably receives the reply. Kestrel can only refuse
mid-stream, and the reset that follows can cost the client the response it just wrote. So the two
limits are **equal** — Nginx is never the more generous of the two — and Nginx's `error_page 413`
returns the same Arabic JSON envelope the application would have returned, with the CORS headers on
it.

**What this fixed.** Nginx's stock `client_max_body_size` is 1 MB. Every listing posted with a few
photographs was answered 413 by Nginx, from an HTML page carrying no `Access-Control-Allow-Origin`,
so the browser reported a CORS failure and the size never surfaced. Two application bugs sat behind
it: Kestrel's `BadHttpRequestException` (413) fell into the catch-all and became a **500**, and
`GlobalExceptionHandlingMiddleware` — which runs outside `UseCors` — cleared the response headers
before writing the error body, stripping the CORS headers off **every** error it handled.

---

## D-33 — The create-advertisement breadcrumb is server-built

**Decision.** `GET /api/lookups/create-ad-form/{categoryId}/{subCategoryId?}` returns a `breadcrumb[]`
alongside the fields: `home → category → subCategory → form`, each step carrying `level`, `id`,
`name`, `nameAr`, `icon` and the site `path`. `AdFormBreadcrumbBuilder` is the only place it is
shaped, and it reads the names straight off the resolved `Category` and `SubCategory` rows.

**Why not build it in the frontend.** The alternative is a second copy of 12 category and 52
sub-category names in the client, which is precisely what `create-ad-form` and `read-config` exist to
prevent (D-1). The Arabic names on the trail are the same strings `categories-tree` publishes; there
is no second source to keep in step, and a new module gets a correct breadcrumb the day its schema is
registered.

**Why the site route is in the response.** `path` is `/create-product/{categoryId}/{subCategoryId}`,
built from `App:CreateAdPath` (default `/create-product`) and `App:HomePath`, the same way
`App:RegisterPath` already shapes an invitation link. The API already publishes the *submit*
endpoint; publishing the page route the user came from costs one configuration key and saves the
client from re-deriving it.

**Consequence.** An invalid selection has **no** breadcrumb rather than a partial one:
`CategorySelectionResolver` still throws first — 404 for an unknown category or sub-category, 400 for
a sub-category belonging to another category, all in Arabic — so a create page can never render a
trail that misdescribes where the user is.

