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

---

## D-34 — Swagger is public in Development and password-gated everywhere else

**Decision.** Swagger and Swagger UI are registered in **every** environment. Outside Development one
middleware, `SwaggerBasicAuthMiddleware`, stands in front of them and demands HTTP Basic
authentication for every path under `/swagger` — the UI, its static assets and both generated
documents. Missing or wrong credentials get `401` with
`WWW-Authenticate: Basic realm="Swagger"`; the credentials are compared with
`CryptographicOperations.FixedTimeEquals` and come only from `SwaggerAuth__Username` and
`SwaggerAuth__Password`.

**Why not keep it Development-only.** It already was, and the consequence was that the people who
needed the API contract in production could not read it. Deleting the endpoint is not a safe default
either: something always turns out to depend on it (D-23 exists because a *missing document* was
mistaken for a caching bug). Publishing it behind a password keeps it available without publishing
the API surface to the internet.

**Why not gate it at Nginx.** `auth_basic` would work, but it puts the rule in a file that is not in
this repository, is not covered by a test, and is re-applied by hand on every server rebuild. The
middleware travels with the code, is asserted by `SwaggerAccessTests`, and behaves identically on a
developer machine.

**Why a middleware and not `[Authorize]`.** Swagger UI is served by Swashbuckle's own middleware, not
by MVC, so there is no action to decorate. An authentication *scheme* was rejected for the same
reason the gate is not a policy: adding one to the application would put a second scheme in reach of
`/api/...`, and the requirement is that the normal API cannot notice this feature exists. The
middleware returns on its first line for any path that is not `/swagger`, so it changes no status
code, no body, no header and no CORS result anywhere else.

**Fail closed, never fail loud.** With no credentials configured the gate refuses everything rather
than throwing at start-up. A forgotten Swagger password must not be able to take the site down; it
only takes `/swagger` down, and start-up says so in the log.

**HTTPS only.** Basic authentication sends a reusable password on every request, so outside
Development an http request to `/swagger` is redirected to https **before** any challenge is issued —
the browser is never invited to send the password in the clear. Behind Nginx this is decided from
`X-Forwarded-Proto`, which `UseForwardedHeaders` has already applied.


---

## D-35 — The advertiser lookup is a database `GROUP BY`, and the name is the key

**Problem.** The advertiser filter on a sub-category feed had no way to discover its own options. The
frontend was requesting the listing endpoint repeatedly — `pageSize=50`, up to ten pages — and
de-duplicating `SellerName` in JavaScript. That is up to ten HTTP round trips and ten full listing
payloads (images, descriptions, lookups) to produce a list of names, and it can only ever see the
first 500 listings, so an advertiser on page 11 is invisible.

**Decision.** `GET /api/lookups/advertisers/{categoryId}/{subCategoryId}` returns one small page of
advertiser names, searched and paginated by SQL Server:

```sql
SELECT SellerName, COUNT(*) FROM Birds
WHERE IsDeleted = 0 AND ModerationStatus = 1 AND (ExpireAt IS NULL OR ExpireAt > GETUTCDATE())
  AND SellerName LIKE @pattern ESCAPE '\'
GROUP BY SellerName ORDER BY SellerName
OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY;
```

Two round trips — the page and its `COUNT` — no matter how many advertisers exist. Visibility is the
two global query filters, so a pending, rejected, expired or deleted listing can never leak the name
of an advertiser who has nothing published.

**Why no advertiser table.** There is no advertiser entity and adding one would be wrong.
`SellerName` is free text captured per listing (`entity.SellerName = request.SellerName`), never
derived from the account, so one account can publish under several trading names. The listing filter
matches that column. Introducing an `Advertisers` table would mean either back-filling identities the
data does not contain, or keeping a second copy of a column that already exists on 14 tables.

**Why `id` is the name.** `id` carries the value the caller sends back as `?sellerName=...`. A user
id would be a key the feed cannot consume, and no new `userId` filter was added: that would mean
touching 14 repositories to solve a problem the frontend does not have. The consequence is stated
rather than hidden — **two accounts that type the same seller name are one option here, because they
are one result set in the feed.** Making them distinguishable is a listing-filter change first and a
lookup change second, in that order.

**Scope.** Only the sub-categories whose feed actually has an advertiser-name filter — الحيوانات and
التحف والأنتيكات (`sellerName`), and since D-36 the six صحتي modules (`name`). `AdvertiserLookupCatalog` is the single place that says so; `SharedTail` reads it to
decide whether `read-config` publishes the `advertisers` operation, and the `sellerName` filter
parameter points at that operation through the existing `optionsSource` mechanism. افرش بيتك and
عقارات store an advertiser name but their feeds have no such filter, so offering options there would
hand the UI a value it cannot filter with; they answer `400`.

**Index.** `IX_*_SellerName` was replaced — not supplemented — by
`(ModerationStatus, SellerName) INCLUDE (IsDeleted, ExpireAt)` on all 14 tables. The single-column
index could only ever be scanned, because every predicate on that column is `LIKE '%…%'`; the
composite seeks the status, walks `SellerName` in order so `GROUP BY`/`ORDER BY` need no sort, and
covers the two remaining predicates so there are no key lookups. Measured on 1,209 seeded rows:
14 logical reads against 58 plus a sort worktable for the same query without it.

**Caching.** 60 seconds, shared. Advertiser availability changes whenever a listing is approved or
expires, so the long `max-age=300` used for genuine reference data would be wrong; a minute is short
enough that a new advertiser appears promptly and long enough to absorb the burst a combobox
generates while someone types.

---

## D-36 — One catalog for category identity, and a parent check in every lookup

**Problem.** Adding صحتي exposed four places that each restated which sub-category belongs to which
category, by hand: the `HasData` seed in `CategoryConfiguration` / `SubCategoryConfiguration`, the
`sub → category` map in `ListingModuleCatalog`, and the Swagger example builders in
`CreateAdFormExamples` — whose `switch` had already gone stale (categories 9–11 fell through to
"Lost & Found"). Separately, `AdFormSchemaCatalog.GetSchema` and `ReadConfigCatalog.GetSchema`
ignored `categoryId` entirely (a dead `categoryId switch { _ => null }`), so only the
`CategorySelectionResolver` in front of them stopped a sub-category being served under the wrong
parent. And the advertisers endpoint kept its list of supported sub-categories in the catalog and,
separately, a hand-written `switch` in the repository, held together by a test that grepped the
source for `SubCategoryType.X =>`.

**Decision.**

- `Shared/Constants/CategoryCatalog.cs` is the **only** list of category / sub-category ids, English
  names and Arabic names. The seed, the `sub → category` map and the examples read it. Moving the
  seed onto it produced **no migration diff** for the existing 12 categories and 52 sub-categories —
  proof the values were carried over byte for byte.
- `Services.Lookups.CatalogSelection.TryResolve` checks the parent before either catalog builds a
  schema, so the static catalogs refuse `(12, 53)` themselves rather than relying on every caller.
- `AdvertiserLookupCatalog` states, per sub-category, **which list filter** its options feed
  (`sellerName` for الحيوانات / التحف, `name` for صحتي). The repository is one registration table
  (`SubCategoryType → IQueryable<string>`) that exposes its keys, and the tests compare the two sets,
  compare each read-config filter that takes `optionsSource: advertisers` against the catalog, and
  check the covering index from the EF model instead of grepping configuration files.
- `LookupRouteIntegrityTests` walks **every seeded sub-category** and asserts: a form and a read-config
  exist; the submit endpoint is a real `POST`; every advertised read operation is a real `GET`
  (via `ApiRouteIndex`, built by reflection over the controllers); the list and submit endpoints
  agree; and the module route in `NotificationCatalog` is the list route.

**The صحتي seed is guarded, not blind.** Admins can create categories with `Max(Id) + 1`
(`AdminCatalogRepository.NextCategoryIdAsync`), so a database somewhere may already own id 13. The
migration therefore inserts each seeded row only if it is missing, and **aborts with `THROW 51000`
(rolling the whole migration back) if the id is held by a different category or sub-category**.
Verified on LocalDB: fresh apply, re-run (no-op), `Down`, and a pre-existing foreign id 13 (aborts,
no tables created).

---

## D-37 — A shared directory-listing base for name / address / contact modules

**Problem.** مكاتب asked for five modules whose fields are exactly those of the صحتي facilities: a
name, ad title and description, a gallery, an address with an optional maps link, and a phone with
optional WhatsApp and e-mail. The صحتي generic base already implemented that shape end to end, but was
named and hard-wired for صحتي (it stamped category 13 and «صحتي» on every DTO). Copying it as an
"Offices" base would have meant ~800 duplicated lines kept in step by hand.

**Decision.** The base was renamed to category-neutral `Directory*` types and its two hard-wirings
removed — the DTO's category now comes from the module's sub-category via `CategoryCatalog`, and
"images required" is `DirectoryModule.ImagesRequired`. **Every module is still independent**: its own
entity, table, image table, DTOs, validators, service class and interface, repository, listing source,
controller, route and `ListingModuleType`; the base only removes the code they would otherwise
duplicate. No generic entity or catch-all route exists.

**Proof it was safe.** Only unmapped abstract classes were renamed: `dotnet ef migrations
has-pending-model-changes` reported no change, and the full suite (2,758 tests) passed before any
مكاتب code was added. The مكاتب migration touches no existing table.

**Exact name matching is per module.** `DirectoryModule.NameFilterIsExact` makes the `name` filter an
equality match. مكاتب uses it because its name filter is a select whose options are the published
names (from the advertisers lookup), so choosing «مكتب أحمد» must not also return «مكتب أحمد وشركاه».
صحتي keeps substring matching, as released.

---

## D-38 — A category that opens its form directly keeps one hidden sub-category

**Problem.** مطاعم وكافيهات must open its create form the moment the category is chosen, with no
sub-category step. But every listing module in the platform is identified by its sub-category:
`ListingModuleCatalog`, moderation, notifications, interests, the admin filters and both lookup
catalogs all key on it. A category with zero sub-categories would have needed a parallel identity
path through all of them.

**Decision.** The category has exactly one sub-category (64) and its `CategoryCatalog` entry carries
`OpensFormDirectly`. `CategorySelectionResolver` — the single gate in front of `create-ad-form`,
`read-config` and `advertisers` — resolves a category-only request for such a category to its one
sub-category, and the categories APIs publish `opensFormDirectly` / `directSubCategoryId` so the
client never shows a picker. Everything downstream sees an ordinary sub-category.

**Scope.** Opt-in per category, so no existing category (or admin-created one) changes behaviour.
Admin form-field overrides for this form are stored under `(15, 64)`; edit them through the
sub-category, which is what the create form reads.

## D-39 — Admin republish is a generic action with a per-module allow-list

**Problem.** مطاعم وكافيهات needed an administrator to restart a listing's 30-day window from five days
before it ends, or after it has ended. The platform already had the window, the derived countdown and an
owner-side republish (expired only, back to review) — but nothing an admin could use.

**Decision.** `AdminAdService.RepublishAsync` works for any `IExpiringListing` module but only runs for
modules listed in `AdminRepublishPolicy` (today: مطاعم وكافيهات). Enabling it elsewhere is one line;
not enabling it means no other module's behaviour changed. It reuses the existing columns
(`PublishedAt`, `ExpireAt`, `RepublishCount`, `FirstPublishedAt`) — **no schema change** — the existing
audit log and the existing `Republished` notification. Unlike the owner path it keeps the listing
Approved: the administrator *is* the reviewer.

**The admin list learned the cycle for every module** (`isExpired`, `isExpiringSoon`, `canRepublish`,
`republishCount`, and the `expiringWithinDays` / `republishDue` / `republished` filters), because those
are read-only facts about any expiring listing; only the action itself is gated.

---

> **Superseded in part by D-41:** the per-module allow-list is gone; every module may be republished.

---

## D-41 — The lifecycle is one processor, covers every module, and never deletes

**Problem.** Admin republish answered 400 for every module but one, and for سيارات it could never have
worked because `Advertisement` is not `IExpiringListing`. No administrator alert existed at all. The job
hard-deleted expired سيارات after 15 days. Approving an expired car reopened its window but left
`Status = Expired`, so it stayed hidden. One failing step aborted the whole pass, and two concurrent
republishes both passed validation.

**Decision.** `IListingLifecycleProcessor` holds the whole pass, in isolated steps, and takes `utcNow` so
it can be tested without a clock. `AdminAdService.StartWindow` is the single place a window starts, with
one arm per entity family. `ListingRowOperations.RunExclusiveAsync` takes `UPDLOCK` inside the EF
execution strategy for republish on all paths, and stays on `SaveChanges` so the interceptors still run.
The سيارات stamp is a conditional `ExecuteUpdate`, so a job holding a stale read cannot overwrite a
concurrent republish.

**Why `Advertisement` still does not implement `IExpiringListing`.** That reasoning stands: the two-arm
switch is still the smaller change.

**Consequence.** Rows stranded by the old approve bug (`Status = Expired`, `ExpireAt` in the future) are
not rewritten by any automatic step; administrators see them as expired and can republish them.

---

## D-40 — Listing state changes are pushed from one SaveChanges interceptor

**Problem.** After an administrator approved a listing nothing told any open screen: the only
real-time messages were per-user notifications, the client refreshed only its notification queries on
them, and anonymous visitors were not connected at all. Public feeds, the owner's my-listings and the
admin queue stayed stale until a manual refresh.

**Decision.** `ListingRealtimeInterceptor` (registered after `ListingEditModerationInterceptor`, so it
sees the review state that interceptor sets) snapshots every added / modified / deleted
`IModeratedListing` in `SavingChanges` and publishes in `SavedChanges` — **only after the save
succeeded**; `SaveChangesFailed` discards the snapshot. One place covers all 60 modules and every
path (owner create/edit/delete, admin approve/reject/suspend/delete, both republish paths). The
time-based expiry, which writes nothing, is published by `ListingLifecycleService` when it creates the
(de-duplicated) expiry notice.

The existing hub is reused: it now accepts anonymous connections for `PublicListingChanged`, puts
admins in an `admins` group on connect, and sends `ListingChanged` only to the owner (`Clients.User`)
and admins. The public payload carries identifiers only.

**Also fixed here.** A repeated approve / reject / suspend of a listing already in that state no
longer notifies the owner again (an approval that reopens an expired window still does). Expiry
notices now carry `listingType` / `categoryId` / `subCategoryId` like every other listing notification.

**Uploads.** The uploads root is configurable (`FileStorage__UploadsRootPath`) because the hard-coded
`<app>/wwwroot/uploads` could not survive a deployment that replaces the application directory —
which is what production shows. See DEPLOYMENT § Uploads directory.


## D-41 — مباني reuses the apartment lookups and rejects unbound multi-select values

**Problem.** مباني (sub 65, module 63) asks the same furnishing / finishing / age / direction / view /
licensing / ownership / payment / rent / exchange questions as شقق, with an amenities list identical to
the apartment one. A module-owned copy of each lookup (the earlier real-estate convention) would have
added ~18 duplicate tables whose ids could drift from the apartment ones.

**Decision.** Buildings stores the apartment enums and serves the apartment lookup tables from its own
routes (`/api/buildings/{key}`); only `BuildingTypes` is new. If a label ever needs to differ for
buildings, that is the point to split the lookup.

**Also.** `SuppressModelStateInvalidFilter` means a multipart value that fails to bind (e.g.
`Features=99`) is silently dropped before FluentValidation sees it. `BuildingsController` checks
`ModelState` for `Features` / `RentInclusions` and answers the standard 422. The other modules still
drop such values silently; a global fix would change behaviour for existing clients and needs its own
decision.

## D-42 — Upload files follow the database, not the other way round

**Problem.** 49 update paths deleted a removed image's file *before* `SaveChanges`, and no update path
removed newly written files when the save failed. A failed save therefore left rows pointing at deleted
files (a "missing image") or left orphan files on disk.

**Decision.** `ListingMediaFileInterceptor` (scoped, registered after the real-time interceptor) records
every `IListingImage` / `IListingVideo` row that is being deleted or added. After a successful save it
deletes the removed rows' files; after a failed save it deletes the added rows' files and keeps the
removed ones. Services no longer call `_fileService.Delete(image.ImagePath)` before saving — a test
fails if that line reappears next to `RemoveImage`. Every image-row deletion in the codebase already
deleted its file, so the interceptor adds no new deletions; it only moves them after the commit.
Soft-deleted listings keep their files, as before.

## D-43 — The governorate guide's new modules live under category 2

دليل محافظتي is category 2 (internally `WorkshopsAndCraftsmen`), so محلات تجارية (66), خدمات عامة (67) and
قرى سياحية وفنادق وملاهي (68) are sub-categories of it, not a new top-level category. Existing ids and
parents are unchanged.

## D-44 — Workshop types and craftsman specializations are retired, never deleted

**Context.** The 2026-10-11 restructure renamed, merged, replaced and removed values of `WorkshopType`
(ids 1–26) and `CraftsmanSpecialization` (ids 1–37). Those are the "categories" of الورش (sub 5) and
الحرفيين (sub 6); they are enum-backed lookup rows, not sub-categories, and listings store the id.

**Decision.**
- *Rename* keeps the id; listings follow the new label.
- *Merge* keeps the target id; migration `RestructureWorkshopTypesAndCraftsmanSpecializations` runs an
  idempotent `UPDATE Craftsmen SET Specialization = target WHERE Specialization IN (...)`, and the
  service maps a merged id from an old client to its target on write and in filters.
- *Remove*, or *replace with an unrelated service*, retires the id (`IsActive = 0` on the lookup row)
  and, for a replacement, adds a new id. Retired rows vanish from the form schema, read-config filters
  and `/types` / `/specializations`; listings on them keep their id, label, images and interactions.
- *Replace with a superset* (ورشة مطابخ → ورشة ألوميتال ومطابخ, ورشة تصنيع معدني → لحام أرجون ومعادن) is
  treated as a rename so existing listings land in the right place.

The merge `UPDATE` is not undone by `Down` — the original split cannot be recovered.

## D-45 — «ورشة ألوميتال» (WorkshopType 3) is retired in code, not in the lookup table

**Context.** After D-44 the form offered both «ورشة ألوميتال» (3) and «ورشة ألوميتال ومطابخ» (7). Only 7 is wanted.
The change had to avoid any migration or write to existing data.

**Decision.** `WorkshopCraftsmenCatalog.RetiredWorkshopTypes` now contains 3, so create validation (422), the
service guards (create, and an edit that *switches* to it → 400) and `LookupRepository.GetWorkshopTypesAsync`
(which feeds `/api/workshops/types`, the create-ad form and read-config filters) all exclude it. The seeded
`WorkshopTypes` row keeps `IsActive = 1`: seeding reads `InactiveWorkshopTypeRows` (20, 24 only), so the EF model
is unchanged and no migration exists. Existing listings on 3 keep their id and label, stay filterable and editable.

**Follow-up (optional, needs approval).** Adding 3 to `InactiveWorkshopTypeRows` would scaffold a one-row
`UpdateData` migration that aligns the table flag; behaviour would not change.
