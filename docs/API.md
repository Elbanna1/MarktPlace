# API

[← README](../README.md) · Related: [AUTHENTICATION](AUTHENTICATION.md) · [AUTHORIZATION](AUTHORIZATION.md) · [MODULES](MODULES.md) · [BUSINESS_RULES](BUSINESS_RULES.md)

The live OpenAPI documents are the authoritative endpoint list. This page documents the **contracts
and behaviours** the schema cannot express.

| Document | URL | Operations |
| --- | --- | --- |
| MarkatPlace V1 (public) | `/swagger/v1/swagger.json` | 623 |
| MarkatPlace Admin V2 | `/swagger/v2/swagger.json` | 131 |

Swagger UI: `/swagger`. `/` redirects there.

> The two documents are separated by `ApiExplorerSettings(GroupName)`, with an either/or inclusion
> predicate — so an admin endpoint can never leak into V1 and no public endpoint can appear in V2.
> Legacy `api/admin/*` controllers are live but carry `[ApiExplorerSettings(IgnoreApi = true)]` and
> appear in neither document, on purpose.

---

## The response envelope

**Every** endpoint — success or failure — answers in the same shape.

```jsonc
{
  "success": true,
  "message": "تم تحميل تفاصيل الإعلان.",   // always Egyptian Arabic
  "data":    { /* payload, or null */ },
  "errors":  null                            // string[] on validation failure
}
```

Even responses produced without a body of their own (unknown route, wrong method, oversized payload,
wrong content type) are formatted into this envelope by `UseStatusCodePages`.

### Status codes

| Code | Meaning | Raised by |
| --- | --- | --- |
| 200 | OK | |
| 201 | Created | Every listing create |
| 400 | Malformed request / missing body | `BadRequestException`, `ValidationFilter` |
| 401 | Not signed in | JWT challenge, `UnauthorizedException` |
| 402 | Payment required | `PaymentRequiredException` — reserved for a future provider |
| 403 | Signed in, not allowed | `ForbiddenException`, `AdminPermissionFilter` |
| 404 | Not found / not visible to you | `NotFoundException` |
| 405 | Method not allowed | |
| 409 | Conflict (duplicate) | `ConflictException` |
| 413 | Payload too large | Kestrel / IIS |
| 415 | Unsupported media type | JSON sent to a multipart endpoint |
| **422** | **Validation failed** | `ValidationFilter` — *not* 400 |
| 429 | Rate limited | Carries `Retry-After` |
| 499 | Client closed request | Client disconnected mid-request |
| 500 | Unexpected | Logged with a stack trace; the body says only "حصل خطأ غير متوقع…" |

> **Validation is 422, never 400.** The built-in ModelState 400 is deliberately suppressed. Assert
> `4xx` or `422` in tests, never `400`.

---

## Pagination

Request (`PaginationParams`, bound `[FromQuery]`):

| Parameter | Default | Clamping |
| --- | --- | --- |
| `pageIndex` | 1 | `< 1` → 1 |
| `pageSize` | 10 | `< 1` → 10; `> 50` → **50** |

Response (`PaginatedResult<T>`):

```jsonc
{
  "items": [ ... ],
  "pageIndex": 1,
  "pageSize": 10,
  "totalCount": 4312,
  "totalPages": 432,
  "hasPrevious": false,
  "hasNext": true
}
```

> Count with `totalCount`, never `items.length` — the page size is clamped server-side.

---

## Content types

| Endpoint kind | Content type |
| --- | --- |
| Listing create / update, profile image, payment screenshot, banner booking | **`multipart/form-data` only** — JSON gets **415** |
| Everything else | `application/json` |

A create request with **no files** must still be multipart. In Python `requests`, put every scalar in
`files=` as `(name, (None, value))`; using `data=` alone silently switches to urlencoded and yields a
misleading 415.

---

## Authentication endpoints — `/api/auth`

| Method | Route | Auth | Purpose |
| --- | --- | --- | --- |
| `POST` | `/register` | anonymous | Create an account → **201** with tokens |
| `POST` | `/login` | anonymous | Sign in |
| `POST` | `/google` | anonymous | Sign in with a Google credential → **200**, or **201** when it creates the account |
| `GET` | `/google/config` | anonymous | `{ enabled, clientId }` — what the browser needs to render the Google button |
| `POST` | `/refresh-token` | anonymous | Exchange an expired access token |
| `POST` | `/forgot-password` | anonymous | Start a password reset |
| `POST` | `/verify-reset-code` | anonymous + reset token | Verify the OTP |
| `POST` | `/reset-password` | anonymous + reset token | Set the new password |
| `POST` | `/logout` | user | Invalidate the refresh token |

**`POST /api/auth/register`**

```jsonc
{
  "firstName": "محمد", "secondName": "عبد الرحمن",
  "email": "user@example.com",
  "username": "mohamed",            // Arabic or Latin — see AccountNameRules
  "phone": "01012345678",           // ^01[0-2,5]\d{8}$
  "governorate": "الفيوم",
  "center": "سنورس",                // must be one of the seven مراكز
  "password": "Passw0rd!", "confirmPassword": "Passw0rd!",
  "referralCode": "ABC123XY"        // optional
}
```

**`POST /api/auth/login`** — `{ "username": "...", "password": "..." }`.
**Username, not email.**

Both return `AuthResponse`:

```jsonc
{ "token": "...", "refreshToken": "...", "expiration": "...", "refreshTokenExpiration": "...", "user": { ... } }
```

**`POST /api/auth/google`**

```jsonc
{
  "idToken": "<Google ID token>",   // or "code" for the authorization-code flow — one is required
  "code": null,
  "redirectUri": null,              // overrides GoogleAuth:RedirectUri for the code exchange
  "referralCode": "ABC123XY",       // optional — the ?ref= value, applied only to a NEW account
  "center": "سنورس"                 // optional — one of the seven مراكز; defaults to الفيوم
}
```

Returns the **same `AuthResponse`**: **200** when an existing user was signed in, **201** when the
credential created the account. The credential is verified against Google server-side; there is no
`email` field, because an e-mail is never proof of a Google identity. Full flow, Google Console
entries and referral rules: [GOOGLE_SIGN_IN.md](GOOGLE_SIGN_IN.md).

**Password reset** is a three-step session. Step 1 returns an opaque `resetToken`; steps 2 and 3 send
it in the **`X-Password-Reset-Token` header** — not in the body, and the e-mail is not repeated. The
OTP is single-use and the session is wiped after a successful reset, so it cannot be replayed. Full
flow: [AUTHENTICATION.md](AUTHENTICATION.md#password-reset).

---

## Listing modules

Every module follows the pattern in [MODULES.md § The standard listing module](MODULES.md#the-standard-listing-module).
`أراضي` is used here as the worked example; substitute the route for any other module.

### Create

```
POST /api/lands          Authorization: Bearer <token>          multipart/form-data
```

| | |
| --- | --- |
| **Auth** | Any signed-in user |
| **Body** | Every field the create form declares, plus `Images` (1–10) and optional `Video` |
| **Success** | **201** + `LandDetailsDto` |
| **Failure** | 422 with `errors[]` in Arabic; 415 if JSON; 401 if anonymous |

**Server-owned fields are ignored if sent:** `ModerationStatus`, `IsFeatured`, `IsPremium`,
`IsUrgent`, `ViewCount`, `UserId`, `PublishedAt`, `ExpireAt`, `CreatedAt`, `IsDeleted`. Posting them
does not fail the request — they simply have no effect.

Build the payload from the form schema rather than hard-coding fields:

```
GET /api/lookups/create-ad-form/11/47
```

which returns `fields[]` (with `required`, `requiredWhen`, `visibleWhen`, `optionsSource`,
`defaultValue`, `writesFields`), `lookups{}`, `submit{endpoint, method, contentType}`,
`breadcrumb[]` and `upload{}`.

### List

```
GET /api/lands?pageIndex=1&pageSize=20&landType=3&center=سنورس&minPrice=...&sortBy=priceAsc&search=أرض
```

Anonymous. Only publicly visible listings — approved and inside their publication window.
Out-of-range paging is clamped, an unknown `sortBy` falls back rather than failing, and an
injection-shaped `search` term is treated as text.

Each module's actual filter and sort parameters are published by
`GET /api/lookups/read-config/{cat}/{sub}`.

### Details

```
GET /api/lands/{id}
```

Anonymous allowed. **Serving details counts one view** (deduplicated; the owner's own views never
count).

Visibility — this is the important part:

| Caller | Pending / Rejected / Suspended | Approved | Expired | Deleted |
| --- | --- | --- | --- | --- |
| Anonymous | 404 | 200 | 404 | 404 |
| Signed-in stranger | 404 | 200 | 404 | 404 |
| **Owner** | **200** | 200 | **200** | 404 |
| Admin (this route) | 404 | 200 | 404 | 404 |
| Admin (`api/v2/admin/ads/{type}/{id}`) | 200 | 200 | 200 | 404 |

`views` is `int?` and **omitted from the JSON** for anyone who is not the owner or an administrator.

### Update / delete

```
PUT    /api/lands/{id}     multipart      owner or admin
DELETE /api/lands/{id}                    owner or admin
```

A successful `PUT` **sends the listing back to `Pending`**, applied centrally in a `SaveChanges`
interceptor. It does **not** move the publication window — editing is not republishing. `DELETE` is a
soft delete.

Another user's listing answers **403 or 404** — never 200.

### Gallery and promotion

```
PUT /api/lands/{id}/images/order    { "imageIds": [ ... ] }    owner or admin
PUT /api/lands/{id}/promotion       { "isFeatured": true }     ADMIN ONLY → 403 for the owner
```

Reorder requires **every** image id of the listing, in the wanted order; position 0 becomes primary.

### Discovery

| Route | Returns |
| --- | --- |
| `GET /api/lands/{id}/similar` | Same نوع الأرض, nearest total prices |
| `GET /api/lands/{id}/related` | Same owner first, then same مركز |
| `GET /api/lands/recently-added` | Newest |
| `GET /api/lands/price-statistics` | Count / min / max / average over the same selection the list would return |
| `GET /api/lands/search-suggestions?term=` | Type-ahead from real titles |

An unknown id on `similar` returns **200 with an empty strip**, not 404.

### Lookups

One route per list, all anonymous, e.g. `GET /api/lands/land-types`, `/area-units`, `/directions` —
22 for أراضي. All answer the same Arabic message; the data identifies itself.

---

## Cross-module interactions — `/api/advertisements`

These act on a listing of **any** module. The module is named by `type` (a `ListingModuleType`),
which **defaults to `Advertisement` (1)**.

| Method | Route | Auth | Notes |
| --- | --- | --- | --- |
| `POST` | `/{id}/view?type=` | anonymous | Deduplicated; `counted:false` on a repeat |
| `GET` | `/recently-viewed` | user | |
| `DELETE` | `/recently-viewed?type=` | user | Clears history; public totals untouched |
| `DELETE` | `/{id}/recently-viewed?type=` | user | Idempotent |
| `POST`/`DELETE` | `/{id}/favorite?type=` | user | Idempotent — a double tap is not a 500 |
| `GET` | `/favorites` | user | |
| `POST` | `/{id}/rating?type=` | user | 1–5; a second rating **replaces** the first |
| `GET` | `/{id}/rating?type=` | user | The caller's own rating |
| `GET` | `/{id}/ratings?type=` | anonymous | Aggregate |
| `DELETE` | `/{id}/rating?type=` | user | |
| `POST` | `/{id}/report` | user | ⚠️ `type` goes in the **body** |
| `POST` | `/{id}/republish` | owner | Returns the listing to Pending |
| `GET` | `/{id}/similar?type=` | anonymous | |
| `GET` | `/{id}/actions?type=` | user | What the caller may do |
| `GET` | `/metadata` | anonymous | |

**`POST /{id}/report` body:**

```jsonc
{ "type": 60, "reason": 1, "details": "إعلان مكرر" }
```

> A child row (favourite, rating, comment) cannot attach to a listing that is not publicly
> resolvable. Interactions on a pending listing answer **404** — including for its owner. Approve the
> listing first.

---

## Profile — `/api/profile`

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/` | Profile + statistics |
| `PUT` | `/` | Update profile |
| `POST` | `/image` | Avatar — multipart |
| `GET` | `/my-listings` | **All modules**, moderation nested under `moderation` |
| `GET` | `/expired-advertisements` | Republish candidates |
| `GET` | `/feedback` | The caller's ratings |
| `POST` | `/change-password` | |

All require authentication. `my-listings` shows the caller's own listings **in every state**.

---

## Account — `/api/account`

| Method | Route | Purpose |
| --- | --- | --- |
| `DELETE` | `/` | Close the signed-in account (self-service deactivation) |

Body: `{ "confirm": true, "password": "...", "reason": "..." }`.

- `confirm` must be `true` — **422** otherwise.
- `password` is required **only when the account has one**. A Google-only account (created by
  `POST /api/auth/google`, which never sets a password) closes without it. A wrong password answers
  **401**.
- The caller can only ever close **their own** account: the id comes from the `NameIdentifier`
  claim and the route accepts no id at all.
- An account holding the `Admin` or `SuperAdmin` role is refused (**403**); the last active
  `SuperAdmin` gets its own message. A Super Admin must revoke the role first.
- Effect: `Status` → `Deactivated (4)`, refresh token and password-reset session cleared, the owner's
  listings in **every module** moved to `ModerationStatus.Suspended`, and the owner's notification
  interests deleted. Notifications, comments, ratings, payments, referrals and the account row itself
  are **kept** — nothing that another record depends on is deleted.
- The already-issued access token stops working **immediately** (see
  [SECURITY](SECURITY.md#account-state-is-enforced-on-every-request)); a second call answers **403**.
- Login and Google sign-in answer **403** afterwards.

---

## Notifications — `/api/notifications`

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/` | Paged list — **`pageSize` clamps to 50** |
| `GET` | `/unread-count` | |
| `PATCH` | `/{id}/read`, `/read-all` | |
| `DELETE` | `/{id}`, `/` | One, or all |
| `GET`/`PUT` | `/preferences` | |
| `GET` | `/interests`, `/interests/options` | اهتماماتي |
| `POST`/`PUT`/`PATCH`/`DELETE` | `/interests[/{id}]` | Subscribe to a category |

`POST /interests` takes `{ "categoryId": N }` — **one per request**, not a list.

Real-time delivery: [ARCHITECTURE § Real-time](ARCHITECTURE.md#real-time-notifications).

---

## Lookups — `/api/lookups`

| Route | Purpose |
| --- | --- |
| `GET /categories-tree` | Every category and sub-category — the module inventory |
| `GET /create-ad-form/{categoryId}/{subCategoryId?}` | The whole create form: fields, conditions, lookups, submit endpoint, **breadcrumb** and **upload limits** |
| `GET /read-config/{categoryId}/{subCategoryId?}` | Every read route for that selection, with `query` pinning ids |
| `GET /governorates`, `/centers`, `/listing-types` | Reference data |

All anonymous. These two endpoints are why the frontend holds no business logic.

### Where am I? — `breadcrumb`

`create-ad-form` answers with the trail that leads to the form it just described, so the create page
can show the user's position without knowing a single category name or id:

```json
"breadcrumb": [
  { "level": "home",        "name": "Home",                 "nameAr": "الرئيسية",        "path": "/",                  "isCurrent": false },
  { "level": "category",    "id": 2, "name": "Workshops & Craftsmen", "nameAr": "الورش والحرفيين", "path": "/create-product/2",   "isCurrent": false },
  { "level": "subCategory", "id": 5, "name": "Workshops",   "nameAr": "الورش",           "path": "/create-product/2/5", "isCurrent": false },
  { "level": "form",        "name": "Create Advertisement", "nameAr": "إنشاء إعلان",     "path": "/create-product/2/5", "isCurrent": true  }
]
```

- `nameAr` is the **same** Arabic name the categories tree publishes; it is read from the lookup
  tables, never written down a second time.
- `path` is the site route for that step. `/create-product` is the default; `App:CreateAdPath` and
  `App:HomePath` change it without a code change, exactly as `App:RegisterPath` does for invitations.
- `level` is one of `home`, `category`, `subCategory`, `form` — switch on it rather than on position.
- When the route names a category but no sub-category, the response sets `requiresSubCategory: true`
  and the trail is three steps long, ending at `اختر القسم الفرعي` instead of `إنشاء إعلان`.
- An unknown category, an unknown sub-category or a sub-category belonging to another category still
  fails the way it always has — **404** `القسم 999 مش موجود.`, **404** `القسم الفرعي 9999 مش موجود.`,
  **400** `القسم الفرعي 1 مش تابع للقسم 2.` — and carries no breadcrumb at all. There is never a
  half-correct trail to render.

The same shape holds for all 52 category/sub-category pairs; `CreateAdNavigationTests` walks every
one of them.

### How much may I send? — `upload`

```json
"upload": {
  "maxRequestSizeMb": 256,
  "maxImages": 10,
  "maxImageSizeMb": 5,
  "maxVideoSizeMb": 50,
  "maxDocumentSizeMb": 10,
  "tooLargeMessage": "حجم الملفات اللي بتبعتها أكبر من المسموح. صغّر الصور أو الفيديو وجرّب تاني. أقصى حجم للطلب الواحد 256 ميجابايت."
}
```

Every value is read from the same constants the server enforces, so the form can refuse an oversized
selection before spending the upload — and say the same sentence the server would have said. Per-file
limits are also published on each file field as `maxFiles`, `maxSizeMb` and `allowedExtensions`.
See [DEPLOYMENT § Upload size](DEPLOYMENT.md#upload-size).

---

## Admin — `/api/v2/admin`

Every route requires the `Admin` role **and** the specific page permission. See
[AUTHORIZATION.md](AUTHORIZATION.md).

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/ads`, `/ads/pending`, `/ads/pending/count`, `/ads/metadata` | The review queue |
| `GET` | `/ads/{type}/{id}` | Read **any** module's listing in **any** state |
| `PATCH` | `/ads/{type}/{id}/approve` | Approve → **opens the 30-day window** |
| `PATCH` | `/ads/{type}/{id}/reject` | Reject with a reason |
| `PATCH` | `/ads/{type}/{id}/suspend` | Suspend |
| `DELETE` | `/ads/{type}/{id}` | Delete any listing |
| `GET` | `/moderation/overview` | |
| `PATCH` | `/payments/{id}/approve`, `/reject` | Manual payment decisions |
| `PATCH` | `/banner-requests/{id}/approve`, `/reject`, `/approve-payment`, `/reject-payment` | Banner bookings |
| — | `/users`, `/categories`, `/locations`, `/banners`, `/reports`, `/feedback`, `/referrals`, `/forms`, `/home`, `/settings`, `/audit-logs` | Dashboard pages |

`{type}` is a `ListingModuleType` integer (e.g. Land = 60, LostItem = 70, FoundItem = 71).

> **`AdminAdService.OpenWindow` is the only place a publication window opens**, for all modules.
> Republish (both paths) nulls the window and returns the listing to Pending.

> Admin probes must send **typed** path parameters. A GUID where an `int` is expected 404s at routing
> *before* authorization, so such a probe proves nothing about access control.

---

## Rate limiting

| Policy | Default | Applies to |
| --- | --- | --- |
| Global | 600 req/min | Every endpoint without another policy |
| `auth` | 20 req/min | `/api/auth/*` |

Partitioned by **user id** when authenticated, otherwise by remote IP (honouring forwarded headers).
Health probes are exempt. A rejection is **429** with `Retry-After` and:

```jsonc
{ "success": false, "message": "بعتّ طلبات كتير في وقت قصير. استنى شوية وجرّب تاني.", "errors": null }
```

---

## CORS

A single fully open `AllowAll` policy: any origin, any header, any method. Safe as configured because
authentication is a bearer token and SignalR uses the `access_token` query string — **no cross-origin
cookies**, so no credentials need to be allowed. See [SECURITY.md](SECURITY.md#cors).
