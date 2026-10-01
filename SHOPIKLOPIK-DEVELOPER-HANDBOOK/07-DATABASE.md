# 07 — Database

[← Start Here](00-START-HERE.md) · Related: [08-DATABASE-MIGRATIONS](08-DATABASE-MIGRATIONS.md) ·
[diagrams/DATABASE-RELATIONSHIPS](diagrams/DATABASE-RELATIONSHIPS.md)

---

## Shape of the database

| | |
| --- | --- |
| Engine | SQL Server |
| ORM | EF Core 10.0.10, code-first |
| Context | one — `AppDbContext : IdentityDbContext<ApplicationUser>` (`Infastrucre/Presitance/Data/`) |
| `DbSet`s | ~400 |
| Migrations | 60 |
| Configuration style | `IEntityTypeConfiguration<T>` classes, applied by `ApplyConfigurationsFromAssembly` |
| Seed data | `HasData` in 53 configuration files, 163 calls — reference data only |

`OnModelCreating` does exactly three things:

```csharp
base.OnModelCreating(builder);
builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
builder.ApplyModerationConfiguration();     // ← the seam
```

`ApplyModerationConfiguration` reflects over every entity implementing `IModeratedListing` and gives
each one the moderation columns, the moderation query filter, and the feed/ordering indexes. That is
why 49 modules do not contain 49 copies of the same configuration.

---

## Table families

| Family | Tables | Notes |
| --- | --- | --- |
| **Identity** | `AspNetUsers` (`ApplicationUser`), `AspNetRoles`, `AspNetUserRoles`, `AspNetUserLogins`, `AspNetUserClaims`, `AspNetRoleClaims`, `AspNetUserTokens` | Standard Identity schema. Google sign-in identities live in `AspNetUserLogins` — **no custom table** |
| **Reference** | `Categories`, `SubCategories`, `Governorates`, `Centers`, `Features`, `ListingTypes`, `HomeSections`, `PlatformSettings`, `PaymentMethods`, `BannerPlacementSettings`, `AdminPagePermissions` | Seeded by `HasData` |
| **Legacy/shared listings** | `Advertisements` + `AdvertisementImages`, `AdvertisementFeatures`, `AdvertisementViews` | Category 1 (Cars, sub-categories 1–4) only |
| **Per-module listings** | ~48 tables such as `Birds`, `Lands`, `Apartments`, `Shops`, `MenClothing`, `Furniture`, `Rescues`, `JobRequests` | Each with its own `<Module>Images` child table, and sometimes selection tables for multi-selects |
| **Module lookups** | ~250 tables such as `LivestockBreeds`, `ApartmentFinishingTypes`, `ShopUtilities` | One per lookup list, seeded by `HasData` |
| **Cross-module interactions** | `ListingViewCounters`, `ListingViewers`, `ListingFavorites`, `ListingRatings`, `ListingReports` | Keyed by `(ListingType, ListingId)` — a polymorphic key, **not** a foreign key |
| **Notifications** | `Notifications`, `UserNotificationInterests`, `UserNotificationPreferences`, `ListingNotificationDispatches` | |
| **Commerce** | `Payments`, `PaymentMethods`, `Banners`, `BannerBookings`, `BannerPlacementSettings` | |
| **Referrals** | `Referrals`, `ReferralLinkEvents` | |
| **Administration** | `AdminAuditLogs`, `AdminPageGrants`, `AdminPagePermissions`, `AdFormFieldOverrides`, `AdFormFieldOptions` | |
| **Lost & Found / Ask&Consult social** | `LostFoundPosts` + images/likes/comments, `AskConsults` + images/likes/comments | The only two modules with social interactions of their own |

---

## Core entities

### `ApplicationUser` (table `AspNetUsers`)

Beyond the Identity columns:

| Column | Purpose |
| --- | --- |
| `FirstName`, `SecondName` | Arabic or Latin names allowed (`AccountNameRules`) |
| `Governorate`, `Center` | fixed to الفيوم and one of its 7 centers |
| `Status` (`UserAccountStatus`) | `Active` / `Suspended` / `Blocked` / `Deactivated` |
| `StatusChangedAt`, `StatusChangedBy`, `StatusReason` | audit of the state change |
| `ReferralCode` | this user's own invitation code |
| `ProfileImagePath`, `ProfileImageUrl` | avatar |
| `RefreshToken`, `RefreshTokenExpiryTime` | one live refresh token per user |
| `PasswordResetOtpHash`, `PasswordResetOtpExpiry` | SHA-256 of the 6-digit OTP, 10-minute life |
| `PasswordResetTokenHash` | SHA-256 of the opaque session token |
| `PasswordResetVerified`, `PasswordResetVerifiedExpiry` | the 15-minute verified window |

Nothing here stores a plaintext OTP or token.

### A representative listing table — `Birds`

Every per-module listing table follows this shape:

| Group | Columns |
| --- | --- |
| Identity | `Id` (Guid), `UserId` (→ `AspNetUsers`, `OnDelete: NoAction`) |
| Content | `Title`, `Description`, `Price`, module-specific typed columns (stored as `int` enum conversions) |
| Contact | `SellerName`, `Phone`, `WhatsApp`, `Address`, `GoogleMaps` |
| Moderation (from `IModeratedListing`) | `ModerationStatus` (default `Pending`), `ModeratedAt`, `ModeratedBy`, `RejectionReason`, `ModerationNotes` |
| Window (from `IExpiringListing`) | `PublishedAt`, `ExpireAt`, `FirstPublishedAt`, `RepublishCount` |
| Soft delete | `IsDeleted` |
| Timestamps | `CreatedAt`, `UpdatedAt` |

> **The foreign key to `AspNetUsers` is always `OnDelete(DeleteBehavior.NoAction)`.** This is not
> preference — cascading from `AspNetUsers` produces SQL Server error 1785 ("may cause cycles or
> multiple cascade paths") because of the many paths back to the user table. Child image tables
> cascade from their **parent listing**, which is fine.

---

## Global query filters — the most important thing to understand

Listing entities carry **two named** filters:

| Filter name | Applied to | Predicate |
| --- | --- | --- |
| `SoftDelete` | every entity with `IsDeleted` | `!x.IsDeleted` |
| `Moderation` | every `IModeratedListing` | `ModerationStatus == Approved` — plus, for `IExpiringListing`, `(ExpireAt == null || ExpireAt > UtcNow)` |

Because they are **named**, a query can drop one without dropping the other:

```csharp
// correct — only the moderation filter is dropped; deleted rows stay hidden
query.IncludingUnmoderated()         // → IgnoreQueryFilters([ModerationModelConfiguration.FilterName])

// WRONG — resurrects soft-deleted rows as well
query.IgnoreQueryFilters()
```

> ### Never call bare `IgnoreQueryFilters()`.
> Use the helpers in `Persistence/Configurations/ModerationQueryExtensions.cs`:
> `IncludingUnmoderated()`, `IncludingUnmoderatedIf(bool)`, `IncludingUnmoderatedParent()`,
> `VisibleToViewer(includeUnmoderated, viewerUserId)`.

### `VisibleToViewer` — how owners see their own pending listings

```csharp
query.IncludingUnmoderated()
     .Where(x => (x.ModerationStatus == Approved && (x.ExpireAt == null || x.ExpireAt > UtcNow))
              || EF.Property<string>(x, "UserId") == viewerUserId);
```

So a signed-in user sees public listings **plus their own**, whatever state those are in. The viewer
id comes from `AppDbContext.ViewerUserId`, which is fed by `IAdminActionContext`.

### The child-row trap

A query over a **child** row (a like, a comment, a rating) that navigates to its moderated parent
silently returns nothing while the parent is `Pending`. This has produced a 500 on likes and a 404
immediately after a committed comment insert. For by-id child lookups use
`IncludingUnmoderatedParent()`.

---

## Indexes

Three index families are generated centrally by `ModerationModelConfiguration`:

| Family | Key | Purpose |
| --- | --- | --- |
| **Visibility / feed** | `(ModerationStatus, [IsPremium, IsFeatured], CreatedAt DESC, Id DESC)` `INCLUDE (IsDeleted, ExpireAt)` | the default listing feed for every module |
| **Sort variants** | the same key with a sort column injected (price ascending, price descending, views) | SQL Server can only walk an index forwards or exactly backwards, so "premium first, then cheapest" needs its own index |
| **Expiry** | `(ExpireAt)` | the lifecycle background job |

Module configurations add their own filter indexes (`AnimalType`, `Purpose`, `Price`, `UserId`, …).

Two hand-written index families worth knowing:

| Index | Purpose |
| --- | --- |
| `IX_<Table>_Advertisers` — `(ModerationStatus, SellerName) INCLUDE (IsDeleted, ExpireAt)` on the 14 advertiser-filtering tables | the advertiser lookup — see [14-ADVERTISER-LOOKUP.md](14-ADVERTISER-LOOKUP.md) |
| `IX_Lands_Search` | the Land search — it only helps if **every** searched column is in the `INCLUDE`. Full-text search is **not** installed on the host. |

---

## Constraints you can rely on

| Constraint | Table | Why |
| --- | --- | --- |
| unique `(ListingType, ListingId, ReviewerUserId)` | `ListingRatings` | one rating per user per listing |
| unique index on the referred user | `Referrals` | one referrer per account |
| check constraint: referrer ≠ referred | `Referrals` | no self-referral |
| unique index (with `HasFilter(null)`) | `UserNotificationInterests` | one subscription per user per category |
| unique phone number | `AspNetUsers` | enforced in code *and* by an index; concurrent inserts are mapped to 409 via `IUnitOfWork.IsUniqueConstraintViolation` |

---

## Enums in the database

Enums are stored as `int` with `HasConversion<int>()`. **The numeric values are part of the database
contract.** Never renumber `ListingModuleType`, `SubCategoryType`, `CategoryType`, `ModerationStatus`
or `UserAccountStatus` — existing rows carry those numbers, and `ListingFavorites`,
`ListingRatings`, `ListingReports`, `ListingViewCounters` and `Notifications` all key on
`ListingModuleType`.

Gaps in `ListingModuleType` (11 → 20, 28 → 30, 43 → 50 …) are deliberate: each category was given a
numeric block so a new module can be added to a category without renumbering anything.

---

## Seed / reference data

Seeded via `HasData`, so it is applied by **migrations**, not by a runtime seeder:

- 12 categories, 52 sub-categories
- 1 governorate (الفيوم) and its 7 centers
- every module lookup list (breeds, materials, colours, finishing types, …) with Arabic names
- home sections, platform settings, payment methods, banner placements
- admin page permissions

Runtime seeders do only two things:

| Seeder | Runs | Does |
| --- | --- | --- |
| `IdentityDataSeeder` | always | ensures the `Admin` and `SuperAdmin` roles; creates the bootstrap administrator **only** if the `AdminUser` section exists; promotes the oldest admin to `SuperAdmin` if none exists |
| `DevelopmentDataSeeder` | Development **and** `DemoData:Enabled` | demo users, listings, workshops, craftsmen, posts, and downloaded demo images |

A production database therefore comes up with reference data only — no users, listings,
notifications, comments, referrals or payments.

---

## Transactions

```csharp
await _unitOfWork.ExecuteInTransactionAsync(async (transaction, cancellationToken) =>
{
    // … work …
    await transaction.CommitAsync(cancellationToken);
    return result;
});
```

This is mandatory: EF Core's retrying execution strategy refuses user-initiated transactions, so
`BeginTransactionAsync` throws. `ExecuteInTransactionAsync` runs the block inside the execution
strategy and clears the change tracker before each attempt.

---

## Diagnosing query cost

Set `Diagnostics__SqlCounter=true` and read the response headers:

| Header | Meaning |
| --- | --- |
| `X-Sql-Count` | number of SQL round trips for this request |
| `X-Sql-Ms` | total time spent in SQL |
| `X-Elapsed-Ms` | total request time |

Because production SQL Server is a **remote** host, round-trip count — not query complexity — is the
metric that matters. A list endpoint should be in single digits regardless of page size.
