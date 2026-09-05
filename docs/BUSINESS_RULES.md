# Business Rules

[← README](../README.md) · Related: [MODULES](MODULES.md) · [AUTHORIZATION](AUTHORIZATION.md) · [DECISIONS](DECISIONS.md)

**Read this before changing any listing code.** Every rule here is enforced somewhere in the source
and most are protected by tests. Breaking one is usually silent.

---

## 1. Moderation

### R1.1 — Nothing is published unreviewed

Every listing is created `ModerationStatus.Pending`. `Pending` is the enum's **zero** and the
column's **default**, so this holds even for a code path that says nothing about moderation — a
seeder, a future module, a bulk import.

*Where:* `ModerationModelConfiguration.ConfigureModerationColumns`.

### R1.2 — Only an administrator changes moderation status

Approve, reject and suspend live on `api/v2/admin/ads/{type}/{id}/…`. There is no public path to any
of them.

### R1.3 — A listing is publicly visible only when approved **and** inside its window

```
visible  ==  ModerationStatus == Approved  AND  (ExpireAt == null OR ExpireAt > UtcNow)
```

This is one condition, not two. It is enforced as a **query filter**, so it applies to every query
automatically.

*Where:* `ModerationModelConfiguration.ConfigureExpiring`.

### R1.4 — Editing a published listing returns it to review

Any owner edit — fields, images, gallery reorder, multi-select change — sets the listing back to
`Pending`. Applied in **one** `SaveChanges` interceptor, comparing original vs current values (not
`IsModified`), so it also covers the endpoints that change a listing without going through `Update`.

*Where:* `ListingEditModerationInterceptor`.

> ⚠️ **An edit is not a republish.** The publication window is untouched by an edit.

---

## 2. Publication window

### R2.1 — 30 days, opened on approval

`ListingLifecycle.ActiveDurationDays = 30`. The window opens when an administrator **approves** the
listing — not when it is created.

### R2.2 — `AdminAdService.OpenWindow` is the only place a window opens

For all modules. Do not set `PublishedAt` / `ExpireAt` anywhere else.

### R2.3 — The client never supplies the window

A start date, end date or duration arriving in a request body is **ignored**. Everything is computed
from the server clock.

### R2.4 — Expiry is derived from `ExpireAt`, not from a status column

Listings filter on `ExpireAt`. The background job only *stamps* status and raises notifications, and
runs hourly — so the stored status can lag the real window by up to an hour. **Read `ExpireAt`.**

### R2.5 — Republish reuses the row

Republishing resets `CreatedAt`, nulls the window and returns the listing to `Pending`.
`FirstPublishedAt` is the anchor that survives, so the free window can be measured from the first
publication. Both republish paths behave identically.

### R2.6 — "No window" ≠ "expired"

A listing that was never published has `ExpireAt == null`. It is *waiting*, not finished, and shows
differently to its owner. `ListingLifecycle.RemainingDays` returns `null` for it.

---

## 3. Ownership and visibility

### R3.1 — Owner visibility

> This was a real bug affecting 47 of 48 modules and is now enforced centrally. Do not undo it.

| Caller | Pending / Rejected / Suspended | Approved | Expired | Deleted |
| --- | --- | --- | --- | --- |
| Anonymous | 404 | 200 | 404 | 404 |
| Signed-in stranger | 404 | 200 | 404 | 404 |
| **Owner** | **200** | 200 | **200** | **404** |
| Admin via `api/v2/admin` | 200 | 200 | 200 | 404 |

Rationale: the owner must be able to read the rejection reason to fix it, and find the expired
listing they want to republish.

*Where:* `ModerationQueryExtensions.VisibleToViewer` — one query, ownership OR-ed onto the public
rule. سيارات states the same rule in `AdvertisementService`.

### R3.2 — Soft delete beats ownership

A deleted listing is invisible to **everyone**, its owner included. The soft-delete filter is
separate and nothing drops it.

### R3.3 — Identity comes from the token only

`viewerUserId` is always the authenticated principal's `NameIdentifier` claim. Never a query
parameter, route value, header or DTO field. Verified against ten IDOR vectors.

### R3.4 — Owner-or-admin for writes

Update, delete and gallery reorder: the owner (`GetOwnedAsync`) or an administrator
(`includeUnmoderated: true`). Another user gets **403 or 404** — never 200.

### R3.5 — Promotion is administrator-only

`isFeatured`, `isPremium`, `isUrgent` are set through `PUT /{id}/promotion`, which returns **403** to
the owner. An owner who could set them would set them on everything.

### R3.6 — Server-owned fields are ignored, not rejected

`ModerationStatus`, `ViewCount`, `UserId`, `PublishedAt`, `ExpireAt`, `CreatedAt`, `IsDeleted` and
the promotion flags have no effect if posted. The request still succeeds.

### R3.7 — A child row cannot attach to a listing that is not publicly resolvable

Favourites, ratings, comments and reports go through the **public** cross-module read model, so they
answer **404** for a pending listing — including for its owner. This is why the view counter
tolerates `NotFoundException` rather than failing the owner's read.

*Where:* `ListingInteractionFilter` and `LostFoundService.GetByIdAsync` both catch it.

---

## 4. Interactions

| Rule | Detail |
| --- | --- |
| **R4.1** | A view is deduplicated per viewer within a window (**6 hours** default, configurable). A repeat answers `counted: false` |
| **R4.2** | **The owner's own views never count** |
| **R4.3** | An unidentifiable caller (no user, no IP) is never counted — they cannot be deduplicated |
| **R4.4** | `views` is `int?` and **omitted from JSON** for anyone but the owner and administrators |
| **R4.5** | One rating per `(type, listingId, userId)`; a second rating **replaces** the first. 1–5 stars |
| **R4.6** | Favourite/rating double-taps are idempotent — the unique-constraint violation is caught, not surfaced |
| **R4.7** | Clearing your recently-viewed history does **not** reduce anyone's public view total |
| **R4.8** | Recently-viewed keeps one entry per listing (~100 retained); reopening moves it to the top |
| **R4.9** | Deleting a listing purges its interactions, so a favourite can never point at something unopenable |

---

## 5. Validation

### R5.1 — Validation answers 422

FluentValidation via the global `ValidationFilter`. The built-in ModelState 400 is suppressed.

### R5.2 — Egyptian mobile numbers

`^01[0-2,5][0-9]{8}$` — accepts 010, 011, 012, 015. Rejects 013, 017 and any wrong length.

> Do not tighten this. A validation rule that is too strict is indistinguishable from a broken site.

### R5.3 — Location is fixed to الفيوم

`Governorate` is always `"الفيوم"`. `Center` must be one of seven, **in this order**:

`الفيوم` · `سنورس` · `طامية` · `يوسف الصديق` · `اطسا` · `ابشواي` · `الفيوم الجديدة`

> **Order is part of the contract** — the seeded `Centers` table takes each row's id from the list
> position. A new center is **appended**; inserting one in the middle renumbers every center after
> it and silently repoints existing rows.

### R5.4 — Governorate is not a filter

Removed from all filters and `LocationFilters` (the platform serves one governorate), but
deliberately kept on entities, create DTOs and lookups.

### R5.5 — The form schema and the validator must agree

`AdFormSchemaCatalog` publishes `required`, `requiredWhen` and `visibleWhen`; the validator enforces
them. A rule in only one place is either an un-fillable field or an unenforced rule.

### R5.6 — No validation message names a property

Messages never contain a C# property name or an English sentence. `OverridePropertyName` keeps the
machine-readable key; only the text is Arabic.

---

## 6. Uploads

| Rule | Detail |
| --- | --- |
| **R6.1** | A file's format is decided by its **magic bytes**, never by its name or declared MIME type |
| **R6.2** | The submitted file name is **discarded**; files are stored as a 32-hex GUID with the extension derived from the detected format. Path traversal and double extensions are structurally impossible |
| **R6.3** | **SVG is excluded on purpose** — it is XML with no signature and can carry script that runs against our own origin |
| **R6.4** | Max 10 images per listing, 5 MB each |
| **R6.5** | CV: PDF/DOC/DOCX ≤ 10 MB. Video: MP4/MOV ≤ 50 MB (الوظائف) |
| **R6.6** | Every accepted format must have a content-type mapping in `Program.cs`, or the file is stored, its URL saved, and the request answers 404 |
| **R6.7** | Image requirements differ per module — Lost & Found optional; Workshops, Craftsmen, Antiques required. The rule must agree in `AdFormSchemaCatalog` **and** the validator/service |

---

## 7. Notifications

| Rule | Detail |
| --- | --- |
| **R7.1** | All wording lives in `NotificationCatalog`. A module raises an event with one line: `NotifyAsync(userId, ListingModuleType, NotificationAction, id, title)` |
| **R7.2** | `NotificationAction.Created` means **"submitted, pending review"** — not "published" |
| **R7.3** | Persist first, push second. Real-time delivery is best-effort and never fails the business operation |
| **R7.4** | Admin actions notify the **owner** |
| **R7.5** | اهتماماتي fan-out happens on **approval**, not creation. `ListingNotificationDispatches` is the idempotence guard |
| **R7.6** | Expiry notifications are deduplicated per publication cycle |
| **R7.7** | Notifications are addressed by user id; they never leak between users |

---

## 8. Comments

| Rule | Detail |
| --- | --- |
| **R8.1** | Only Lost & Found and اسأل واستشير have comments |
| **R8.2** | Edit: **author only**. Delete: **author or admin** |
| **R8.3** | Each comment publishes `canEdit` / `canDelete` for the caller |
| **R8.4** | The author DTO is shared: `id`, `name`, `avatar`, `isListingOwner` |

---

## 9. Payments and banners

| Rule | Detail |
| --- | --- |
| **R9.1** | Payments are **manual**. A payment is always created `Pending`; the user uploads a transfer screenshot; an administrator decides |
| **R9.2** | `PaymentMethods` is an admin-editable settings table — a new method is an INSERT, not a code change |
| **R9.3** | Payments link polymorphically via `TargetType` / `TargetId` and keep `*Snapshot` columns, so history survives a method being renamed |
| **R9.4** | A payment method that has been used **cannot be deleted** — deactivate it instead |
| **R9.5** | Banner bookings are **not** a listing module. They never go through `create-ad-form`, and they carry their own `PaymentStatus` rather than writing to `Payments` |
| **R9.6** | Banner images are validated for **exact pixel dimensions**, published per placement |
| **R9.7** | Placement price, slot count, image dimensions and **duration** are admin-editable columns, snapshotted onto each booking |
| **R9.8** | Occupancy blocks from submission, not from approval |

---

## 10. Referrals

| Rule | Detail |
| --- | --- |
| **R10.1** | One referrer per user — a unique index, not a check in code |
| **R10.2** | No self-referral — a check constraint |
| **R10.3** | The referral is written in the **account's own transaction** |
| **R10.4** | Counts are `COUNT` over rows; there is no counter column to drift |
| **R10.5** | A share is not a referral. `ReferralLinkEvents` is a separate table and the two figures must never be summed |

---

## 11. User-facing language

> **The hardest rule in the project.**

| Rule | Detail |
| --- | --- |
| **R11.1** | Every message a customer reads is Egyptian Arabic |
| **R11.2** | All wording lives in `Shared/Constants/UserMessages.cs` |
| **R11.3** | Never expose an exception type, SQL, a file path, a DTO/property name, an enum name or a status-code name |
| **R11.4** | A message says what happened, and where useful what to do next |
| **R11.5** | Logs stay technical and English. They are a separate audience |
| **R11.6** | FluentValidation's built-in fallbacks are overridden globally by `EgyptianArabicLanguageManager` |

Enforced by three test suites, including a source-wide scan that fails the build.

---

## 12. Admin

| Rule | Detail |
| --- | --- |
| **R12.1** | Administrators are **confirmed from existing users**, never created |
| **R12.2** | An admin endpoint with no permission annotation is **refused** and logged as an error |
| **R12.3** | Grants are read per request from the database, never from the JWT |
| **R12.4** | Super Admin bypasses per-page checks |
| **R12.5** | `AdminAuditLog` is append-only — never updated, never deleted, never soft-deleted |
| **R12.6** | Admin toggles that affect lookups must invalidate `ILookupCache` |
| **R12.7** | DataAnnotations are inert; FluentValidation is the only validation (422) |

---

## 13. Module-specific rules

| Module | Rule |
| --- | --- |
| سيارات | Its own `AdvertisementStatus` alongside moderation; video on the entity; polymorphic `GET /api/ads/{id}` via `adType` |
| رجال أعمال | Two create paths (`/api/suppliers` **and** `POST /api/ads` subs 9–14) — a rule in one is not enforced in the other |
| الوظائف | Shares `JobField` + `JobExperienceLevel` between both modules, per specification |
| المفقودات | One route for both post types; records its own view inline; likes and comments |
| اسأل واستشير | **No location at all** — removed from six layers. Rescue and Blood Requests keep theirs |
| Charity (all) | `IsResponsibilityAccepted` must be `true` |
| عقارات | `RealEstateListings.Keep` nulls a conditional field once its condition stops holding |
| افرش بيتك | `PlantOrnament` alone has no colours or material |
| التسوق أونلاين | No location block; warranty→electronics, expiry→cosmetics, delivery areas→homemade food |

---

## Before you change any of this

1. Find the rule above and the file it names.
2. Check `Tests/MarkatPlace.Tests` for a guard.
3. Check [DECISIONS.md](DECISIONS.md) — the odd-looking ones are usually deliberate.
4. If it is still wrong, change the **single** place it is defined, not the 48 call sites.
