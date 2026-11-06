# Payment System Audit & Frontend Integration Report

Date: 2026-11-06 · Scope: every payment-related file in the backend · Base commit: `7c983f4`; changes committed on top of it

Everything here was read from the source or produced by a command run during the audit. Where something
does not exist, it is marked **MISSING** or **NOT IMPLEMENTED** instead of being described as if it did.

---

## 1. Executive summary

The backend has **one payment ledger** (`Payments` table) used by two flows, plus a separate, older
payment state machine inside **banner bookings**:

| Flow | What is paid for | Price source | Activates something? |
|---|---|---|---|
| **Ad publication payment** (`Purpose = 6`) | Publishing/republishing a listing in any of the **65 listing modules** | `AdPaymentSettings` (admin, one global price) — server-side only | Yes: an approved payment is a *credit* that the moderation approval consumes to open a 30-day window |
| **General manual payment** (`Purpose = 0`) | Nothing specific (untargeted "I paid X") | Self-declared by the user | **No** — approving it activates nothing |
| **Banner booking** | A banner slot | `BannerPlacements` price (admin) — server-side | Yes, via its own `PaymentStatus` on the booking (not a `Payments` row) |

Payment methods are fully dynamic (DB rows, admin CRUD, activation toggle, display order, per-method
instructions) and are read by every flow from the database — there is no hard-coded method list in any
flow. The ad-payment flow is complete: server-side pricing, price-drift guard, ownership checks, one
pending payment per listing (application check **and** filtered unique index), atomic guarded review
transitions, private proof storage, owner + admin notifications with deep links, admin audit log.

The audit found and fixed **8 issues** (section 4); the most important were general-payment **and
banner-booking** receipts being written to the **public** uploads folder, the admin "new payment" alert
showing the payment *method* name where the payer's name belongs, and post-commit notification failures
turning a saved payment into an HTTP 500 (which invites a duplicate resubmission). Every receipt — new or
legacy, of any kind — is now reachable only through an authenticated, ownership/permission-checked route;
legacy files were **not** moved or deleted.

**Classification: READY FOR FRONTEND INTEGRATION — not READY FOR PRODUCTION** (section 21).

---

## 2. Implemented features (verified in code and/or tests)

- Dynamic payment methods: create, edit, activate/deactivate, delete-when-unused, display order,
  instructions, type-specific fields (wallet number / InstaPay address / bank details).
- Deactivated methods disappear from every user-facing list immediately (all lists query `IsActive`);
  historical payments keep a **snapshot** of method name, account info, holder and instructions.
- A method that is still referenced by payments cannot be deleted (409, "deactivate instead"), and the last
  method offered for ad payments cannot be withdrawn while ad payments are enabled (409).
- Admin-managed ad price, currency, instructions, enable switch and optional method allowlist.
- Server-side amount: the ad submission stores `settings.Price`; any client `Amount` field is ignored.
  `ExpectedAmount`/`ExpectedCurrency` let the client detect a price change (409) — they never set the price.
- Ownership: ad payment submit/status load the listing and require `OwnerUserId == caller` (404 otherwise);
  general payment details/screenshot require owner or admin (404 otherwise).
- Duplicate prevention: one pending ad payment per listing (`HasPendingAsync` + unique filtered index
  `UX_Payments_AdPayment_OnePending`, unique-violation mapped to 409); paid listings refuse a second payment.
- Duplicate/concurrent approval prevention: decisions are `UPDATE … WHERE Status = Pending`
  (`DecideAsync`, `TransitionAsync`); 0 rows → 409 "decided by another admin".
- Self-review guard: an admin cannot approve/reject (and now refund) their own payment (403).
- Rejection with optional reason + optional feedback; owner can **resubmit** only after a rejection.
- Refund (admin, `Manage` permission) of an approved payment, with reason, audit entry and notification.
- Approval gate: an ad cannot be approved into a new window without a credit (HTTP 402) — enforced in one
  place (`AdminAdService.ApproveAsync`) for all 65 module types.
- Private proof storage for ad payments (and, after this audit, general payments) outside `wwwroot`.
- Admin queue, filters, details, proof viewer, history, statuses lookup, audit log, dashboard counters.
- Notifications persisted in `Notifications` and pushed over SignalR (`/hubs/notifications`).

## 3. Missing features (NOT IMPLEMENTED — none invented)

| Requested | Status | Note |
|---|---|---|
| Per-module / per-category prices | **NOT IMPLEMENTED** | One global ad price. All 65 modules pay the same amount. |
| Payment-method **fees** | **NOT IMPLEMENTED** | No fee column; amount = price. |
| Admin-configurable **required fields per method** | **NOT IMPLEMENTED** | Required fields are fixed per method *type* (validator). |
| User **cancel** of a pending payment | **NOT IMPLEMENTED** | No `Cancelled` status. A pending ad payment blocks a new one until reviewed. |
| Refund **request** by the user | **NOT IMPLEMENTED** | Refund is admin-initiated only; no "refund initiated/completed" split (one `Refunded` state). |
| Payment **Failed** state / online gateway / webhooks | **NOT APPLICABLE** | Manual verification only; no external provider → no webhook signatures to validate. |
| Dedicated payment statistics endpoint | **PARTIAL** | Dashboard exposes pending counts and latest/pending payments; no revenue/period statistics. |
| Rate limiting | **MISSING (whole API)** | No `AddRateLimiter` anywhere in the solution. |
| Idempotency keys | **NOT IMPLEMENTED** | Ad payments are naturally idempotent per listing (one pending); general payments are not. |

## 4. Broken features found — and fixed

| # | Problem (evidence) | Fix |
|---|---|---|
| F1 | **General payment proofs were public.** `PaymentService.SubmitAsync` saved via `IFileService.SaveAsync(…, ImageConstants.PaymentsFolder)` → `/uploads/payments/…`, which Nginx serves from disk to anyone with the URL. Ad payments had already moved to the private store. | Saved via `IPrivateFileStore` (folder `payments`). New owner endpoint `GET /api/payments/{id}/screenshot` and admin endpoint `GET /api/v2/admin/payments/{id}/screenshot` stream the file with `no-store`, `nosniff`, sandbox CSP. Owner DTOs always return the owner route, admin DTOs the admin route — **including legacy rows**, which the endpoints serve from the public uploads root (`IFileService.Open`, traversal-safe). |
| F8 | **Banner booking receipts were public.** `BannerBookingService.CreateAsync` saved the payment proof via `IFileService.SaveAsync(…, BannerBookingProofFolder)` → `/uploads/banner-bookings-proof/…`; `BannerBookingDto.paymentProofUrl` exposed that URL to the owner and admins. | Saved via `IPrivateFileStore` (folder `banner-bookings-proof`); `PaymentProofUrl` stored as `/api/banner-bookings/{id}/payment-proof`. New endpoint `GET /api/banner-bookings/{id}/payment-proof`: booking owner, or an admin holding **View** on the Banner Requests page (checked in-action through `IAdminPermissionService`); everyone else 404, anonymous 401. The DTO always returns that route (legacy rows too); legacy files are served from the uploads root. Delete removes the receipt from whichever store holds it. |
| F1/F8 legacy | Receipts uploaded before this change still sit in `uploads/payments` and `uploads/banner-bookings-proof`. | **Not moved, not deleted.** `Program.cs` now answers 404 for any request under those two folders (case-insensitive, before the static-file middleware), and the committed Nginx site (`deploy/nginx/api.shopiklopik.com.conf`) gains the same rule *above* the media regex. Owners/admins still see them through the authenticated routes. The Nginx rule takes effect only once the site file is installed on the server (not done — no production access). |
| F2 | **Wrong payer name in the admin alert.** `SubmitAsync` passed `method.Name` as the `payerName` argument of `NotifyNewPaymentAsync`, so admins read "Vodafone Cash" where the payer's name belongs (the ad flow had this fixed earlier). | Looks up the payer's name with `GetUserNamesAsync`. Test asserts the alert contains the payer and not the method name. |
| F3 | **Post-commit side effects could return 500 for a saved payment.** Admin alert / owner notification ran unguarded after `SaveChanges`; a notification failure returned 500 although the payment existed, inviting a duplicate submission. Same for approve/reject/refund notifications. | `RunSideEffectAsync` (same pattern as `AdPaymentService`): logged, never fails the request. |
| F4 | **Approve/reject/refund notifications had no navigation URL.** `NotifyPayerAsync` passed no `deepLink`. | Now `deepLink = /payments/{id}` and `entityName` from `NotificationCatalog.Payment`. Test asserts the deep link. |
| F5 | **Admin could refund a payment they made themselves.** Approve/reject had a self-review guard; refund did not. | 403 `ForbiddenException`. Test covers it. |
| F6 | **English user-facing errors.** `PaymentService.ApplyMethod` threw English messages; they are reachable through `PATCH /api/v2/admin/payment-methods/{id}/status`, which builds an update request internally and bypasses FluentValidation. | Arabic messages identical to the validator's. |
| F7 | **Credit consumption was not guarded at write time.** `ConsumeCreditAsync` loaded the credit and saved through change tracking; a refund landing in between would leave a row both `Refunded` and consumed. | Single `UPDATE … WHERE Id = @id AND Status = Approved AND ActivatedAt IS NULL`. |

## 5. Payment entities

| Entity | Path | Table | Responsibility |
|---|---|---|---|
| `Payment` | `Core/Domain/Entities/Payment.cs` | `Payments` | Every submitted payment proof. `Guid` id; `Amount decimal(18,2)`, `Currency`; method **snapshot** columns; `Status` (Pending/Approved/Rejected/Refunded); review fields (`ApprovedAt`, `RejectedAt`, `ApprovedBy`, `RejectReason`, `ReviewFeedback`); target (`Purpose`, `TargetType`, `TargetId`, `Quantity`); activation (`ActivatedAt`, `ActivationStartAt/EndAt`). No soft delete (financial record). |
| `PaymentMethod` | `Core/Domain/Entities/PaymentMethod.cs` | `PaymentMethods` | Admin-managed channel: `Name` (unique), `ArabicName`, `Type`, type-specific fields, `Instructions`, `IsActive`, `DisplayOrder`. |
| `AdPaymentSetting` | `Core/Domain/Entities/AdPaymentSetting.cs` | `AdPaymentSettings` | Singleton (`Id = 1`): `IsEnabled`, `Price decimal(18,2)`, `Currency`, `Instructions`, `PaymentMethodIds` (CSV allowlist, empty = all active), `EnabledAt`, `UpdatedAt/By`. |
| `BannerBooking` (payment part) | `Core/Domain/Entities/BannerBooking.cs` | `BannerBookings` | Own price snapshot, `PaymentStatus`, `PaymentProofUrl/Path/FileName`. Separate from `Payments`. |

Enums (`Shared/Enums/PaymentEnums.cs`): `PaymentMethodType` (MobileWallet 1, InstaPay 2, BankAccount 3,
Other 99), `PaymentStatus` (Pending 1, Approved 2, Rejected 3, Refunded 4), `PaymentPurpose`
(Unspecified 0, … AdvertisementPublication 6; values 1-5 are reserved and unused).

## 6. Services, interfaces, repositories

| Type | Path | Responsibility |
|---|---|---|
| `IPaymentService` / `PaymentService` | `Core/ServicesAbstraction/IPaymentService.cs`, `Core/Services/PaymentService.cs` | Method CRUD; general payment submit/list/details/screenshot; generic admin list/details/approve/reject/refund (delegates ad payments to `IAdPaymentService.ReviewAsync`). |
| `IAdPaymentService` / `AdPaymentService` | `Core/ServicesAbstraction/IAdPaymentService.cs`, `Core/Services/AdPayments/AdPaymentService.cs` | Ad payment settings, public config, modules list, submit/resubmit/status, admin queue, review, screenshot, method-withdrawal guard. |
| `IAdPaymentGate` / `AdPaymentGate` | same files, `Core/Services/AdPayments/AdPaymentGate.cs` | `EnsurePaidAsync` (402), `ConsumeAsync`, batched `DescribeAsync` for listing rows. |
| `AdPaymentRules` | `Core/Services/AdPayments/AdPaymentRules.cs` | When a window opens, grandfathering, deletion check. |
| `AdPaymentFormDescriptor` | `Core/Services/AdPayments/AdPaymentFormDescriptor.cs` | Static `payment` section of every create-ad form. |
| `IPaymentRepository` / `PaymentRepository` | `Core/ServicesAbstraction/IPaymentRepository.cs`, `Infastrucre/Presitance/Repositories/PaymentRepository.cs` | Methods, payments, admin paging, guarded `TransitionAsync`. |
| `IAdPaymentRepository` / `AdPaymentRepository` | `Core/ServicesAbstraction/IAdPaymentRepository.cs`, `Infastrucre/Presitance/Repositories/AdPaymentRepository.cs` | Settings, credits, pending check, guarded `DecideAsync`, guarded `ConsumeCreditAsync`, listing summaries. |
| `IPrivateFileStore` / `PrivateFileStore` | `Core/ServicesAbstraction/IAdPaymentService.cs`, `Infastrucre/Presitance/Services/PrivateFileStore.cs` | Non-public proof storage (`FileStorage:PrivateRootPath`). |
| `PaymentCatalog`, `AdPaymentCatalog` | `Shared/Constants/` | Limits, currency, status/type names, routes, stage resolution, exemption seam. |
| Validators | `Core/Services/Validation/PaymentMethodRequestValidators.cs`, `SubmitPaymentRequestValidator.cs`, `AdminRequestValidators.cs` (`RefundPaymentRequestValidator`) | FluentValidation (Arabic messages). |
| Mapping | `Core/Services/Mapping/PaymentMappingProfile.cs` | AutoMapper method/payment DTOs. |

Architecture note: the project uses **Service + Repository**, not CQRS/MediatR. There are no commands,
queries or handlers for payments; the audit followed the existing convention rather than adding CQRS.
No payment background job exists (ad expiry is handled by the listing lifecycle job, which never touches
payments).

## 7. API endpoints

Envelope for every JSON response: `{ "success": bool, "message": string, "data": T|null, "errors": string[]|null }`.

### User

| Method | Route | Auth | Purpose |
|---|---|---|---|
| GET | `/api/v2/public/ad-payment/config` | anonymous | Live ad price, currency, instructions, offered methods |
| GET | `/api/v2/ad-payment/config` | user | Same, `no-store` |
| GET | `/api/v2/ad-payment/modules` | user | All 65 modules with `isSubjectToPayment` |
| GET | `/api/lookups/create-ad-form/{categoryId}/{subCategoryId?}` | (existing) | Form incl. static `payment` section with endpoints |
| POST | `/api/v2/ads/{type}/{id}/payment` | owner | Submit ad payment (multipart) → 201 |
| POST | `/api/v2/ads/{type}/{id}/payment/resubmit` | owner | Resubmit after rejection → 201 |
| GET | `/api/v2/ads/{type}/{id}/payment` | owner | Payment stage/status for a listing |
| GET | `/api/payments/methods` | user | Active payment methods |
| POST | `/api/payments` | user | General (untargeted) payment proof → 201 |
| GET | `/api/payments/my?status=` | user | Own payments (both purposes) |
| GET | `/api/payments/{id}` | owner/admin | Payment details |
| GET | `/api/payments/{id}/screenshot` | owner/admin | **NEW** — private proof image |
| GET | `/api/banner-bookings/payment-methods`, `quote`, `POST /api/banner-bookings` … | user | Banner booking flow (own module) |
| GET | `/api/banner-bookings/{id}/payment-proof` | owner, or admin with View on Banner Requests | **NEW** — banner receipt image |

### Admin (`/api/v2/admin`, admin role + page `Payments` + permission)

| Method | Route | Permission |
|---|---|---|
| GET / PUT | `/ad-payment/settings` | View / Edit |
| GET | `/ad-payment/submissions`, `/ad-payment/submissions/{id}`, `/ad-payment/history` | View |
| GET | `/ad-payment/submissions/{id}/screenshot` | View |
| PATCH | `/ad-payment/submissions/{id}/review` | View + Approve or Reject (checked per decision) |
| GET | `/payments`, `/payments/{id}`, `/payments/statuses` | View |
| GET | `/payments/{id}/screenshot` | View — **NEW** |
| PATCH | `/payments/{id}/approve` · `/reject` · `/refund` | Approve · Reject · Manage |
| GET/POST/PUT/DELETE | `/payment-methods`, `/payment-methods/{id}` | View/Create/Edit/Delete |
| PATCH | `/payment-methods/{id}/status` | Manage |
| GET | `/payment-methods/types` | View |
| GET | `/audit-logs`, `/audit-logs/{id}`, `/audit-logs/metadata` | (audit page) |
| GET | `/dashboard`, `/dashboard/analytics` | pending payment counts, latest/pending payments |
| — | `/banner-requests/{id}/approve-payment` · `/reject-payment`, `/banner-prices` | banner module |

Legacy v1 duplicates (kept, see §16): `/api/admin/payment-methods` (CRUD), `/api/admin/banner-bookings/*`.

## 8. Frontend DTOs (exact property names, camelCase in JSON)

- **PublicAdPaymentConfigDto**: `isEnabled, price, currency, instructions, paymentMethods[], updatedAt`
- **AdPaymentMethodDto**: `id, name, arabicName, type, phoneNumber, instaPayIdentifier, bankName, accountHolderName, accountNumber, iban, paymentInfo, instructions`
- **AdPaymentModuleDto**: `advertisementType, advertisementTypeKey, advertisementTypeName, categoryId, subCategoryId, apiRoute, isSubjectToPayment`
- **SubmitAdPaymentRequest** (multipart): `PaymentMethodId` (required), `Screenshot` (required file), `TransactionReference` (≤100), `Notes` (≤500), `ExpectedAmount`, `ExpectedCurrency`
- **AdPaymentSubmissionResultDto**: `paymentSubmissionId, advertisementId, advertisementType, amount, currency, paymentStatus, paymentStatusName, submittedAt, paymentMethodId, paymentMethodName, transactionReference, notes, isResubmission, previousPaymentSubmissionId, stage, stageName, statusEndpoint`
- **AdPaymentStatusDto**: `advertisementId, advertisementType, paymentsEnabled, isPaymentRequired, canSubmit, hasPayment, paymentSubmissionId, paymentStatus, paymentStatusName, amount, currency, paymentMethodId, paymentMethodName, transactionReference, submittedAt, reviewedAt, rejectionReason, reviewFeedback, currentPrice, currentCurrency, canResubmit, stage, stageName, advertisementTitle, advertisementModerationStatus, advertisementModerationStatusName, notes, submitEndpoint, resubmitEndpoint`
- **Stage values**: `NotRequired | AwaitingPayment | PaymentUnderReview | PaymentRejected | Paid`
- **PaymentMethodDto**: `id, name, arabicName, type, typeName, phoneNumber, instaPayId, bank{bankName, accountHolderName, accountNumber, iban}, instructions, isActive, displayOrder`
- **SubmitPaymentRequest** (multipart, general): `PaymentMethodId` (>0), `Amount` (1 – 1,000,000, ≤2 decimals), `Screenshot` (required), `Notes` (≤1000)
- **PaymentDetailsDto**: `id, userId, paymentMethod{id,name,arabicName,type,typeName}, amount, currency, screenshotUrl, notes, status, statusName, submittedAt, approvedAt, rejectedAt, rejectReason, reviewFeedback`
- **PaymentListItemDto**: `id, amount, currency, paymentMethod, screenshotUrl, status, statusName, submittedAt, rejectReason, reviewFeedback`
- **BannerBookingDto.paymentProofUrl** is always `/api/banner-bookings/{id}/payment-proof` (an authenticated route, not a file URL); `BannerBookingListItemDto.paymentProofUploaded` is unchanged.
- **Admin**: `AdminPaymentListItemDto` (`id, userId, userName, userPhone, userEmail, amount, currency, paymentMethodId, paymentMethodName, screenshotUrl, status, statusName, submittedAt, reviewedAt`), `AdminPaymentDetailsDto` (+ `notes, snapshot{paymentMethodName, paymentInfo, accountName, instructions, isSnapshot}, currentPaymentMethodName, reviewedBy, reviewedByName, rejectReason, reviewFeedback, bannerRequest`), `AdminAdPaymentSubmissionDto`, `AdPaymentReviewResultDto` (`payment, listingAction, listingActionName, listingModerationStatus`), `AdminAdPaymentSettingsDto` (config + `paymentMethodIds, usesAllActiveMethods, enabledAt, updatedBy, updatedByName`)
- **Requests (admin)**: `UpdateAdPaymentSettingsRequest {isEnabled, price, currency?, instructions?, paymentMethodIds?}`, `ReviewAdPaymentRequest {decision: "Approved"|"Rejected", rejectionReason?, feedback?}`, `ApprovePaymentRequest {feedback?}`, `RejectPaymentRequest {reason?, feedback?}`, `RefundPaymentRequest {reason}` (required), `CreatePaymentMethodRequest`/`UpdatePaymentMethodRequest {name, arabicName?, type, phoneNumber?, instaPayIdentifier?, bankName?, accountHolderName?, accountNumber?, iban?, instructions?, isActive, displayOrder}`, `UpdateActiveStatusRequest {isActive}`

Enums travel as **integers** in JSON (no global string enum converter).

## 9. Admin dashboard capabilities

| Capability | Endpoint | Status |
|---|---|---|
| Manage payment methods | `/api/v2/admin/payment-methods*` | ✅ |
| Manage pricing (ad price/currency/on-off/allowlist) | `/api/v2/admin/ad-payment/settings` | ✅ (one global price) |
| Banner prices | `/api/v2/admin/banner-prices/{location}` | ✅ (banner module) |
| View all payments, search, filter | `GET /api/v2/admin/payments?search=&status=&paymentMethodId=&userId=&fromDate=&toDate=&pageIndex=&pageSize=` | ✅ |
| Ad payment queue with category/module filters | `GET /api/v2/admin/ad-payment/submissions?status=&type=&categoryId=&userId=&advertisementId=&fromDate=&toDate=` | ✅ |
| View details / proof | `/payments/{id}`, `/payments/{id}/screenshot` (new), `/ad-payment/submissions/{id}/screenshot` | ✅ |
| Approve / reject | `/payments/{id}/approve|reject`, `/ad-payment/submissions/{id}/review` | ✅ |
| Refund | `/payments/{id}/refund` | ✅ (admin-initiated) |
| History | `/ad-payment/history`, `/payments` (all statuses) | ✅ |
| Statistics | `/dashboard`, `/dashboard/analytics` | ⚠️ counts + lists only |
| Audit logs | `/audit-logs` (`ApprovePayment 20, RejectPayment 21, RefundPayment 22, Create/Update/DeletePaymentMethod 23-25`, settings changes) | ✅ |

## 10. Dynamic pricing

- **Ads:** `AdPaymentSettings.Price/Currency` (admin `PUT /ad-payment/settings`, validated: 0 – 1,000,000,
  ≤2 decimals, price > 0 required to enable, 3-letter currency). Read by `GetPublicConfigAsync`; written to
  each payment as `Amount/Currency` **at submission**, so history keeps the original amount even after a
  price change. The client never supplies the amount; `ExpectedAmount` mismatch → 409.
- **Banners:** `BannerPlacement.Price` (admin), copied onto the booking at creation.
- **General payments:** amount is self-declared and **activates nothing**; it is a record for the admin, not
  a price.
- No hard-coded price exists in the code path (`AdPaymentSettings` is seeded disabled with price 0 by the
  migration). No duplicated pricing logic was found: ad pricing lives only in `AdPaymentService` /
  `AdPaymentGate`.
- Grandfathering: listings created before payments were enabled and still awaiting first approval do not pay.

## 11. Payment methods and activation

Seeded by migration (`PaymentCatalog.SeedMethods`): **Vodafone Cash** (id 1, MobileWallet), **InstaPay**
(id 2), **Bank account** (id 3). The live activation status is data, not code — read it with
`GET /api/v2/admin/payment-methods`. This audit did not query the production database.

Activation propagates automatically: every user-facing list (`/api/payments/methods`, ad config, banner
`payment-methods`) filters `IsActive` at query time; ad payments additionally intersect with the optional
allowlist (empty = all active methods). No module holds its own method list, so activating a method makes
it available to all 65 modules at once; deactivating hides it from new transactions while snapshots keep
historical rows intact.

## 12. Module coverage matrix

All 65 `ListingModuleType` values are subject to payment (`AdPaymentCatalog.Exempt` is empty; asserted by
`Every_listing_module_is_subject_to_payment`). Every sub-category form carries the `payment` section
(asserted by `Every_sub_category_form_carries_a_payment_section_for_its_module`). Integration is
identical for every module because it is implemented **once**: submit/status endpoints take `{type}`; the
gate runs inside the single `AdminAdService.ApproveAsync`.

| Category | Modules (ListingModuleType) | Pricing source | Methods | Workflow | Activation | Notifications |
|---|---|---|---|---|---|---|
| سيارات / private ads | Advertisement=1 | AdPaymentSettings | offered set | submit→review→credit | approval/republish consumes credit | ✅ |
| ورش وصنايعية | Craftsman=2, Workshop=3 | same | same | same | same | ✅ |
| رجال أعمال | Factory=4, Farm=5, Company=6, Supplier=7, WholesaleTrader=8, FruitVegetableMerchant=9 | same | same | same | same | ✅ |
| وظائف | JobRequest=10, JobOpportunity=11 | same | same | same | same | ✅ |
| حيوانات | Livestock=20 … OtherAnimal=28 (9) | same | same | same | same | ✅ |
| تحف | DecorAntique=30 … CoinStamp=34 (5) | same | same | same | same | ✅ |
| ملابس | MenClothing=35 … KidsClothing=37 | same | same | same | same | ✅ |
| تسوق أونلاين | Accessory=38 … HomemadeFood=43 (6) | same | same | same | same | ✅ |
| افرش بيتك | Furniture=50 … PlantOrnament=56 (7) | same | same | same | same | ✅ |
| عقارات | Land=60, Apartment=61, Shop=62, Building=63 | same | same | same | same | ✅ |
| بوابة الخيرات | Rescue=65, BloodRequest=66, AskConsult=67 | same | same | same | same | ✅ ⚠️ see §19 |
| مفقودات | LostItem=70, FoundItem=71 | same | same | same | same | ✅ |
| صحتي | Doctor=80 … Hospital=85 (6) | same | same | same | same | ✅ |
| مكاتب | LawyerOffice=90 … AccountantOffice=94 (5) | same | same | same | same | ✅ |
| مطاعم وكافيهات / دليل محافظتي | RestaurantAndCafe=100, CommercialShop=101, GeneralService=102, ResortHotelAmusement=103 | same | same | same | same | ✅ |
| Banner booking (not a listing) | — | BannerPlacement price | active methods | own booking states | booking approve/publish | ✅ (Banner* types) |

No module currently has a free-module business rule in code; making one free is a one-line change to
`AdPaymentCatalog.Exempt`. Whether charity/lost-and-found should be free is a **business decision**, flagged
in §19 rather than changed.

## 13. Notification matrix

All rows below are persisted in `Notifications` and pushed in real time by `INotificationService`
(SignalR hub `/hubs/notifications`; asserted end-to-end in `AdPaymentReviewWorkflowTests` via a real hub
connection). Owner notifications are sent **after** the database write commits.

| Event | Recipient | Type (int) | Deep link | Ad flow | General flow |
|---|---|---|---|---|---|
| Proof submitted / under review | payer | PaymentSubmitted 17 | `/payments/{id}` | ✅ | ✅ |
| New payment proof | admins with Payments page | AdminNewPayment 53 | `/admin/payments/{id}` | ✅ (payer, listing, resubmission flag) | ✅ (**payer name fixed**) |
| Approved (+ feedback, + "republished"/"awaiting moderation") | payer | PaymentApproved 18 | `/payments/{id}` | ✅ | ✅ (**deep link fixed**) |
| Rejected (+ optional reason, feedback) | payer | PaymentRejected 19 | `/payments/{id}` | ✅ | ✅ (**deep link fixed**) |
| Refunded (+ reason) | payer | PaymentRefunded 59 | `/payments/{id}` | ✅ | ✅ (**deep link fixed**) |
| Advertisement activated | owner | listing approval notification (moderation module) | listing | ✅ (approval message states republish) | n/a |
| Banner payment approved / rejected | booker | BannerPaymentApproved 34 / Rejected 35 | booking | — | — |
| Payment request created / instructions available | — | **NOT IMPLEMENTED** (no "request" step: the user submits proof directly; instructions come from config) | | | |
| Payment failed / cancelled / refund initiated | — | **NOT APPLICABLE / NOT IMPLEMENTED** (see §3) | | | |
| Admin: refund request / payment failure | — | **NOT APPLICABLE** | | | |

Localization: all wording is Egyptian Arabic (project rule — there is no English variant of any
notification). Duplicates: admin alerts go through `SendOnceAsync` keyed on the payment id.

## 14. Database relationships & migrations

- `Payments.UserId → AspNetUsers` **NoAction**; `Payments.ApprovedBy → AspNetUsers` **NoAction**;
  `Payments.PaymentMethodId → PaymentMethods` **Restrict** (a used method can only be deactivated).
- Indexes: `(UserId, SubmittedAt)`, `(Status, SubmittedAt)`, `PaymentMethodId`, `(TargetType, TargetId)`,
  `(Purpose, Status, SubmittedAt)`, unique filtered `UX_Payments_AdPayment_OnePending`
  (`Purpose = 6 AND Status = 1`); `PaymentMethods`: unique `Name`, `(IsActive, DisplayOrder)`.
- Money: `decimal(18,2)` on `Payments.Amount` and `AdPaymentSettings.Price`.
- Migrations: `20260728103452_intialCreate` (creates `Payments`/`PaymentMethods`; the earlier
  manual-payment migration was squashed into it), `20261103135029_AddAdvertisementPaymentSystem`,
  `20261106112437_AddPaymentReviewFeedback`.
- `dotnet ef migrations has-pending-model-changes` (run against LocalDB): **"No changes have been made to the
  model since the last migration."**
- This audit changed **no entity and needs no migration**. Nothing was run against production.

## 15. Security findings

| Severity | Finding | Status |
|---|---|---|
| High | General payment proofs written to public `/uploads/payments` | **Fixed (F1).** New files private; legacy files refused by the app and by the committed Nginx rule (needs server install, §19 R1). |
| High | Banner booking payment proofs written to public `/uploads/banner-bookings-proof` | **Fixed (F8).** Same legacy handling. |
| Medium | No rate limiting on any endpoint, including payment submission (each request stores an image) | **Open** (§19 R3) |
| Medium | Notification failure after commit returned 500 → duplicate resubmissions | **Fixed (F3)** |
| Medium | Admin could refund their own payment | **Fixed (F5)** |
| Low | Credit consumption raced with refund | **Fixed (F7)** |
| Low | General payment amount is client-declared | Accepted: it activates nothing; documented. |
| — | Verified OK: server-side ad price; ownership checks (404, no existence leak); admin role + page + per-action permission; self-review guard; magic-byte image validation (`IFileService`/`PrivateFileStore`); 256 MB request ceiling; proof responses `no-store`/`nosniff`/sandbox CSP; no secrets in payment code; no SQL string building (EF only). | |

## 16. Removed code

| Removed | Why it was safe |
|---|---|
| `IPaymentRepository.GetPagedAsync(PaymentFilterParams)` + implementation | Grep across Core/Infrastructure/MarkatPlace/Shared/Tests: no caller. The admin list uses `GetPagedForAdminAsync`. Not an endpoint, not DI-resolved by name, no reflection. |
| `IPaymentRepository.Update(Payment)` + implementation | No caller; all status changes use guarded `TransitionAsync`/`DecideAsync`. |
| `Shared/DTOs/Payments/PaymentFilterParams.cs` | Only used by the removed method; not bound by any controller action. |

Deliberately **kept**: v1 `api/admin/payment-methods` (same service as v2). It is one of seven v1
`api/admin/*` controllers that may still be called by an older dashboard build; removing it is an API
contract decision (§19 R6). `PaymentPurpose` values 1-5 are unused but are integer enum values persisted in
the database contract — removing them gains nothing.

## 17. Build and test results (actual runs)

| Step | Command | Result |
|---|---|---|
| Release build | `dotnet build MarkatPlace.slnx -c Release` | **0 errors, 5 warnings** — all pre-existing, none in changed files (CS8602 in `BuildingsController`, xUnit2000, xUnit2013, EF1002 in a test fixture, NU1903 SignalR.Common 10.0.10 advisory) |
| Payment + receipt tests | `dotnet test … --filter` (payment/receipt classes) | all passed, 0 failed, 0 skipped |
| Full suite (baseline, `HEAD` 7c983f4, clean worktree) | `dotnet test Tests/MarkatPlace.Tests -c Release --logger trx` | **3594 passed**, 0 failed, 0 skipped |
| Full suite (with all changes) | same | **3598 passed, 0 failed, 0 skipped** (4 m 27 s, LocalDB lifecycle suite ran) |
| Model check | `dotnet ef migrations has-pending-model-changes` | no pending changes |
| Mutation check | uploads block in `Program.cs` disabled → legacy-receipt test **fails**; restored → passes | the test really guards the fix |

**Test-count reconciliation (investigated, resolved).** Per-test TRX files from both runs were diffed:
+5 new tests, and `NoEnglishInResponsesTests.No_customer_facing_message_is_written_in_English` went from
1453 to 1452 rows. That theory has one row per production source file; the audit deleted one
(`Shared/DTOs/Payments/PaymentFilterParams.cs`) and added none. 3594 + 5 − 1 = 3598. No test was lost or skipped.

New tests:
- `GenericPaymentWorkflowTests.cs` — `A_general_payment_proof_is_private_and_reaches_only_its_owner_and_admins`
  (private storage, owner 200, stranger 404, anonymous 401, admin route 200, non-admin on admin route 403,
  admin alert names the payer, approval deep link); `An_admin_cannot_refund_a_payment_they_made_themselves`.
- `PaymentReceiptPrivacyTests.cs` — `A_banner_payment_receipt_is_private_and_reaches_only_its_owner_and_banner_admins`
  (real `POST /api/banner-bookings`; receipt only in the private store; owner 200 with `no-store`/`nosniff`;
  stranger 404; admin with Payments-page-only permissions 404; anonymous 401; super-admin 200; admin DTO
  route; delete removes the private file); `Legacy_public_receipts_are_refused_on_the_uploads_path_but_still_served_to_their_owner`
  (legacy banner + general receipts: `/uploads/...` 404 for anonymous and owner, case-insensitive; owner and
  admin routes 200; stranger 404; DTOs return the routes; delete removes the legacy file);
  `The_nginx_site_refuses_every_receipt_folder_before_serving_uploads_from_disk`.

Already covered by existing tests: method withdrawal guard, price validation, server-side amount (tampered
`Amount` ignored), price drift 409, ownership, duplicate pending 409, resubmission rules, concurrent/duplicate
decisions, 402 gate, credit consumption on approval/republish, grandfathering, SignalR delivery, admin
permission split, all-modules coverage.

## 18. Files modified by this audit

| File | Change |
|---|---|
| `Core/Services/PaymentService.cs` | F1 private store, F2 payer name, F3 guarded side effects, F4 deep links, F5 refund guard, F6 Arabic messages, `OpenScreenshotAsync` |
| `Core/ServicesAbstraction/IPaymentService.cs` | `OpenScreenshotAsync` |
| `Core/ServicesAbstraction/IPaymentRepository.cs` | removed dead `GetPagedAsync(PaymentFilterParams)`, `Update` |
| `Infastrucre/Presitance/Repositories/PaymentRepository.cs` | removed the same |
| `Infastrucre/Presitance/Repositories/AdPaymentRepository.cs` | F7 guarded `ConsumeCreditAsync` |
| `Core/Services/Mapping/PaymentMappingProfile.cs` | owner screenshot route for private proofs |
| `Shared/Constants/PaymentCatalog.cs` | private folder + owner/admin screenshot routes + `IsPrivateScreenshot` |
| `Infastrucre/Presantion/Controllers/PaymentsController.cs` | `GET /api/payments/{id}/screenshot` |
| `Infastrucre/Presantion/Controllers/Admin/AdminPaymentsV2Controller.cs` | `GET /api/v2/admin/payments/{id}/screenshot` |
| `Core/Services/BannerBookings/BannerBookingService.cs` | F8: receipt via `IPrivateFileStore`, route stored as URL, `OpenPaymentProofAsync`, store-aware delete and cleanup |
| `Core/ServicesAbstraction/IBannerBookingService.cs` | `OpenPaymentProofAsync` |
| `Core/Services/Mapping/BannerBookingMappingProfile.cs` | `paymentProofUrl` = authenticated route |
| `Shared/Constants/BannerBookingCatalog.cs` | `PaymentProofRoute`, `IsPrivateProof` |
| `Infastrucre/Presantion/Controllers/BannerBookingsController.cs` | `GET /api/banner-bookings/{id}/payment-proof` (owner or permitted admin) |
| `Core/ServicesAbstraction/IFileService.cs`, `Infastrucre/Presitance/Services/FileService.cs` | `Open(relativePath)` for legacy receipts; `Delete` and `Open` share one traversal-safe resolver |
| `Shared/Constants/ImageConstants.cs` | `NonPublicUploadFolders` |
| `MarkatPlace/Program.cs` | 404 for `/uploads/payments/*` and `/uploads/banner-bookings-proof/*` |
| `deploy/nginx/api.shopiklopik.com.conf` | same 404 rule above the media location |
| `docs/API.md`, `docs/BUSINESS_RULES.md`, `docs/DECISIONS.md` (D-47) | receipt routes and the decision |
| `Tests/MarkatPlace.Tests/PaymentReceiptPrivacyTests.cs` | banner + legacy receipt tests |
| `Tests/MarkatPlace.Tests/BackendAuditFixesTests.cs`, `DirectoryTestKit.cs`, `GovernorateGuideModulesTests.cs` | constructor / `IFileService.Open` updates of existing test doubles |
| `Shared/DTOs/Payments/PaymentFilterParams.cs` | deleted (dead) |
| `Tests/MarkatPlace.Tests/GenericPaymentWorkflowTests.cs` | new tests |
| `docs/PAYMENT_SYSTEM_AUDIT_AND_FRONTEND_INTEGRATION_REPORT.md` | this report |

## 19. Remaining tasks

| Pri | Task | Recommended fix |
|---|---|---|
| R1 High | Legacy receipts are still on disk under the public uploads root; Nginx serves `/uploads` from disk, so they stay reachable **until the live Nginx site gets the new rule** | Do **not** copy the committed site file over the live one (its `root` is a template path). Edit the live file and paste only the block `location ~* ^/uploads/(payments\|banner-bookings-proof)/ { return 404; }` **above** the `location ~* ^/uploads/...` media regex, then `sudo nginx -t && sudo systemctl reload nginx`. Check: an existing receipt URL answers 404 and a normal listing image still answers 200. Read-only inventory first: `sudo find <uploads root>/payments <uploads root>/banner-bookings-proof -type f` piped to `wc -l`. |
| R2 Low | Optionally move legacy receipts into the private store | Needs explicit approval (moves customer files). Not required for privacy once R1 is applied. |
| R3 Medium | No rate limiting | `AddRateLimiter` with a per-user fixed window on the three proof-upload endpoints (no new package). |
| R4 Medium | Refund of an already **consumed** ad credit leaves the listing live | Business decision: suspend/expire the listing on refund, or allow only unconsumed credits to be refunded. |
| R5 Medium | No user cancel of a pending payment | Add `Cancelled` status + `POST /api/v2/ads/{type}/{id}/payment/cancel` (owner, pending only, guarded update). |
| R6 Low | v1 `api/admin/payment-methods` duplicates v2 | Confirm the dashboard uses v2 only, then remove the v1 controller. |
| R7 Low | Statistics are counts only | Add a revenue/period endpoint if the dashboard needs it. |
| R8 Business | Charity and lost-and-found modules pay like commercial ones | Decide; exemption is `AdPaymentCatalog.Exempt`. |
| R9 Low | Per-module prices, method fees | Only if required by the business; would need schema changes. |

## 20. Frontend integration guide

### Ad publication payment (the main flow)

1. **Module & form** — `GET /api/lookups/create-ad-form/{categoryId}/{subCategoryId?}`. The response's
   `payment` section has `isSubjectToPayment`, `configEndpoint`, `submitEndpoint`, `statusEndpoint`,
   `resubmitEndpoint`, `contentType: "multipart/form-data"` and `fields`. Never hard-code these.
2. **Live price & methods** — `GET /api/v2/ad-payment/config` (authenticated, no-store):
   ```json
   { "success": true, "data": { "isEnabled": true, "price": 50, "currency": "EGP", "instructions": "…",
     "paymentMethods": [ { "id": 1, "name": "Vodafone Cash", "arabicName": "فودافون كاش", "type": 1,
       "phoneNumber": "010…", "paymentInfo": "010…", "instructions": "…" } ], "updatedAt": "…" } }
   ```
   If `isEnabled` is false, show no payment step.
3. **Create the ad** as today (creation is never blocked by payment).
4. **Show instructions** for the method the user picks (`paymentInfo`, `instructions`, bank fields).
5. **Submit proof** — `POST /api/v2/ads/{type}/{id}/payment`, multipart:
   `PaymentMethodId` (required), `Screenshot` (required image), `TransactionReference` (≤100),
   `Notes` (≤500), `ExpectedAmount` = displayed price, `ExpectedCurrency`. **Never send an amount to be
   charged** — the server uses its own price.
   - 201 → `AdPaymentSubmissionResultDto` (`stage: "PaymentUnderReview"`).
   - 400 payments off / method not offered / missing screenshot / not needed / grandfathered.
   - 404 listing not found or not yours. 409 price changed (refresh), already pending, already paid.
   - 422 validation (FluentValidation, `errors[]`).
6. **Track** — `GET /api/v2/ads/{type}/{id}/payment` → `AdPaymentStatusDto`. Drive the UI from `stage`
   and the booleans `canSubmit`, `canResubmit`; show `rejectionReason`/`reviewFeedback` when present.
   Also refresh on SignalR notifications of types 17/18/19/59 whose `referenceId` is the payment id.
7. **Rejected** → `POST …/payment/resubmit` (same body). 409 if the latest payment is not rejected.
8. **Approved** → stage `Paid`. If the ad was already approved and expired, it is republished automatically
   (`listingAction: "Republished"`); a pending ad stays pending until moderation approves it, and that
   approval consumes the credit. The owner gets the moderation notification as usual.
9. **Receipt images** — every receipt URL in a response is an **authenticated API route**:
   `screenshotUrl` → `/api/payments/{id}/screenshot` (owner) or `/api/v2/admin/payments/{id}/screenshot`
   (admin rows); banner `paymentProofUrl` → `/api/banner-bookings/{id}/payment-proof` (owner or admin with
   View on Banner Requests). Load them with the bearer token (`fetch` → `URL.createObjectURL(blob)`); a plain
   `<img src>` sends no token and gets 401. Responses are `private, no-store` — do not cache them.

### General payment (only if the product uses it)
`GET /api/payments/methods` → `POST /api/payments` (multipart `PaymentMethodId, Amount, Screenshot, Notes`)
→ `GET /api/payments/my` / `GET /api/payments/{id}` → screenshot as above. It unlocks nothing.

### Admin
- Settings: `GET/PUT /api/v2/admin/ad-payment/settings` (`{ isEnabled, price, currency, instructions, paymentMethodIds }`;
  `[]` = all active methods).
- Methods: CRUD on `/api/v2/admin/payment-methods`, toggle with `PATCH …/{id}/status {isActive}`; 409 when
  deleting a used method or withdrawing the last ad method.
- Queue: `GET /api/v2/admin/ad-payment/submissions?status=1` → review with
  `PATCH /api/v2/admin/ad-payment/submissions/{id}/review { "decision": "Approved"|"Rejected", "rejectionReason"?, "feedback"? }`
  (403 own payment, 409 already decided).
- All payments: `GET /api/v2/admin/payments`, details `/{id}`, proof `/{id}/screenshot`,
  `PATCH /{id}/approve {feedback?}`, `/{id}/reject {reason?, feedback?}`, `/{id}/refund {reason}`.

### Status codes
200/201 success · 400 business rule · 401 no/invalid token · 402 ad approval without payment · 403
permission or self-review · 404 not found / not owner · 409 conflict (price changed, duplicate, already
decided, method in use) · 413 body too large · 422 validation. Error body: the standard envelope with
`success: false`, Arabic `message`, optional `errors[]`.

## 21. Final readiness assessment

**READY FOR FRONTEND INTEGRATION.**

Evidence: every step of the requested 12-step flow maps to an existing, tested endpoint (§20); pricing and
methods are entirely server-driven; the review lifecycle is atomic and duplicate-safe; 3598/3598 tests pass
including SignalR delivery; the model has no pending migrations; every receipt is private.

**Not READY FOR PRODUCTION** because: legacy receipts stay reachable through Nginx until the new rule is
added to the live site (R1), there is no rate limiting (R3), the refund-after-activation rule is undecided
(R4), and none of these changes are deployed — production currently runs an earlier build.
