# 11 — Users, Roles and Permissions

[← Start Here](00-START-HERE.md) · Related: [09-AUTHENTICATION](09-AUTHENTICATION.md) ·
[10-AUTHORIZATION](10-AUTHORIZATION.md)

---

## The user record

One table — `AspNetUsers`, entity `ApplicationUser : IdentityUser`. There is no separate "profile"
table.

| Group | Columns |
| --- | --- |
| Identity | `Id`, `UserName`, `Email`, `EmailConfirmed`, `PhoneNumber`, `PasswordHash`, `SecurityStamp`, lockout columns |
| Name | `FirstName`, `SecondName` |
| Location | `Governorate` (always `الفيوم`), `Center` (one of 7) |
| State | `Status`, `StatusChangedAt`, `StatusChangedBy`, `StatusReason` |
| Referral | `ReferralCode` (this user's own code) |
| Avatar | `ProfileImagePath`, `ProfileImageUrl` |
| Session | `RefreshToken`, `RefreshTokenExpiryTime` |
| Reset | `PasswordResetOtpHash`, `PasswordResetOtpExpiry`, `PasswordResetTokenHash`, `PasswordResetVerified`, `PasswordResetVerifiedExpiry` |
| Audit | `CreatedAt`, `UpdatedAt` |

### Names and usernames

`AccountNameRules` permits **Arabic and Latin** names and usernames and normalises whitespace.
Identity's own `User.AllowedUserNameCharacters` is set to `string.Empty` because its default
ASCII-only allow-list vetoes Arabic usernames outright.

### Location

`LocationConstants` fixes the governorate to `الفيوم` and enumerates the seven centers:
الفيوم · سنورس · طامية · يوسف الصديق · اطسا · ابشواي · الفيوم الجديدة.

Governorate is therefore **not a filter** anywhere in the API — it was deliberately removed from
every filter and from `LocationFilters`, while being kept on entities, create DTOs and lookups so the
data stays complete and a future multi-governorate expansion is not blocked.

---

## Account status

`UserAccountStatus` (stored as `int`):

| Value | Name | Set by | Effect |
| --- | --- | --- | --- |
| 1 | `Active` | default | normal |
| 2 | `Suspended` | admin | **403** on every authenticated request; login refused |
| 3 | `Blocked` | admin | **403** on every authenticated request; login refused |
| 4 | `Deactivated` | **the user** | **403**; all of the user's listings are hidden everywhere |

Enforcement is described in [10-AUTHORIZATION.md](10-AUTHORIZATION.md): the claims transformation
puts the status in a claim, and `AccountStatusMiddleware` acts on it before authorization runs.

### Admin: changing a user's status

```
PATCH /api/v2/admin/users/{id}/status      (page: users, permission: Manage)
GET   /api/v2/admin/users/statuses         the assignable statuses
```

Suspending or blocking writes `StatusChangedAt`, `StatusChangedBy` and `StatusReason`, audit-logs the
action, and takes effect on the account's next request.

### Self-service deactivation

```
DELETE /api/account        (authenticated; the strict auth rate-limit policy applies)
```

- Sets `Status = Deactivated` — **no migration was needed**, the value already existed.
- **Administrators are refused**, and the **last `SuperAdmin`** can never deactivate themselves.
- All of the user's listings are hidden across every module. Note `Advertisement` keys the owner as
  `OwnerId`, while every other module uses `UserId` — the hiding logic handles both.
- This is reversible by an administrator; it is not a data deletion.

---

## Roles

Exactly two, in `AppRoles`:

| Role | Purpose |
| --- | --- |
| `Admin` | may reach the admin API; each operation still costs a page permission |
| `SuperAdmin` | bypasses page permissions; the only role that can manage administrators |

Both roles are **ensured at every start-up** by `IdentityDataSeeder`.

### Where roles come from at request time

**Not from the JWT.** `DatabaseRoleClaimsTransformation` rewrites the role claims from
`AspNetUserRoles` on every authenticated request — uncached on the administration surface, and cached
for 5 minutes elsewhere. The JWT's own role claim is discarded.

This exists because the role claim is evaluated by the authorization middleware *before* any MVC
filter can run, so a promoted user would otherwise have to log out and back in.

---

## Administrators

### They are promoted, never created

There is no "create admin account" endpoint that makes a new user. The flow is:

```
GET   /api/v2/admin/admins/candidates?search=…     find an existing user
GET   /api/v2/admin/admins/candidates/{userId}     inspect one
POST  /api/v2/admin/admins                         promote them, with page grants
PUT   /api/v2/admin/admins/{id}                    edit
PUT   /api/v2/admin/admins/{id}/permissions        replace their grants
PATCH /api/v2/admin/admins/{id}/status             enable / disable
DELETE /api/v2/admin/admins/{id}                   demote
GET   /api/v2/admin/admins                         list
GET   /api/v2/admin/admins/{id}                    details
```

The whole `AdminAccountsController` is `[SuperAdminOnly]`.

### Self-service endpoints, for any active administrator

```
GET /api/v2/admin/permissions/pages     every page and the permissions valid on it
GET /api/v2/admin/me/permissions        what I hold
```

### Bootstrapping the first administrator

`IdentityDataSeeder` at start-up:

1. Ensures the `Admin` and `SuperAdmin` roles exist.
2. If the `AdminUser` configuration section is present (`UserName`, `Email`, `Password`, optional
   `Phone`), creates that account if missing and grants it **both** roles.
3. If **no `SuperAdmin` exists at all**, promotes the oldest `Admin` — a one-time safety net so the
   dashboard cannot become unreachable.

| Environment | `AdminUser` section |
| --- | --- |
| Development | present in `appsettings.Development.json` — `admin` / `Admin@12345` |
| Production | **absent on purpose**, and there is no fallback in code |

To bootstrap production: set `AdminUser__UserName`, `AdminUser__Email`, `AdminUser__Password` as
environment variables, start once, then **remove them**.

> **The absence of the section is the safety mechanism.** Do not "helpfully" add default credentials
> to `appsettings.json`.

---

## Permissions

See [10-AUTHORIZATION.md](10-AUTHORIZATION.md) for the full model. In short:

- 15 pages, defined only in `AdminPageCatalog`.
- `AdminPermission` is a flags enum: View 1, Create 2, Edit 4, Delete 8, Approve 16, Reject 32, Manage 64.
- One `AdminPageGrants` row per (administrator, page) holds the mask.
- `SuperAdmin` bypasses the mask entirely.
- Checked against the database **on every request**.

### A known EF trap when editing grants

`PUT /api/v2/admin/admins/{id}/permissions` once returned 500 because grants were added through a
**tracked parent navigation**. Adding to a tracked parent's collection makes EF insert a *new*
parent. Grants must be written through their own `DbSet`. If you touch this code, keep that shape.

---

## User-facing account endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| GET | `/api/profile` | profile + statistics |
| PUT | `/api/profile` | edit name, center, phone |
| POST | `/api/profile/image` | multipart avatar upload (reuses `IFileService`) |
| GET | `/api/profile/my-listings` | every listing the user owns, **across all modules** |
| GET | `/api/profile/expired-advertisements` | expired listings, for republishing |
| GET | `/api/profile/feedback` | ratings the user has received |
| POST | `/api/profile/change-password` | change password while signed in |
| DELETE | `/api/account` | self-deactivation |

`GET /api/profile/my-listings` is extensible by design: each module registers an `IUserListingSource`
implementation and appears there with no change to the profile code.

---

## Admin user management endpoints

| Method | Route | Page → Permission |
| --- | --- | --- |
| GET | `/api/v2/admin/users` | users → View |
| GET | `/api/v2/admin/users/{id}` | users → View |
| PATCH | `/api/v2/admin/users/{id}/status` | users → Manage |
| GET | `/api/v2/admin/users/statuses` | users → View |

`GET /api/v2/admin/users/{id}` returns a rich picture (listings, payments, referrals, activity). It
was optimised from 18 SQL round trips down to 13 by folding several scalar aggregates over one table
into a single `GroupBy(_ => 1)` query — a useful pattern to copy.

---

## Referrals

Each user carries a unique `ReferralCode`. The invitation link is built **only** from
`App:FrontendUrl` + `App:RegisterPath` + `?ref=<code>` — never from the incoming request, which would
send invitees to the API host.

| Method | Route | Purpose |
| --- | --- | --- |
| GET | `/api/referrals/me` | my code and link |
| POST | `/api/referrals/me/share` | record a share event |
| GET | `/api/referrals/me/statistics` | my totals |
| GET | `/api/referrals/me/users` | who I invited |
| GET | `/api/referrals/resolve/{code}` | resolve a code (used by the register page) |
| GET | `/api/v2/admin/referrals` + `/statistics`, `/referrers`, `/statuses`, `/{id}` | admin views (page `referrals`, View) |

Guarantees: **one referrer per user** (unique index), **no self-referral** (check constraint), and
the referral row is written **inside the account-creation transaction** so an account can never exist
without its referral.
