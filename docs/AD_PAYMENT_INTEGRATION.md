# Ad Payment — frontend integration contract

Payment is a **section inside the existing create-ad form**, not a separate page. The admin switches it on and
off from `PUT /api/v2/admin/ad-payment/settings`; the backend enforces it no matter what the client does.

## 1. How the form works

1. Open the form as today: `GET /api/lookups/create-ad-form/{categoryId}/{subCategoryId?}`. Every concrete form
   now carries a static `payment` descriptor (cached with the form):
   `isSubjectToPayment`, `advertisementType` (the `{type}` to use below), `advertisementTypeKey`,
   `configEndpoint`, `submitEndpoint`, `resubmitEndpoint`, `statusEndpoint`, `method`, `contentType`,
   `section` (`"payment"`) and `fields[]` (same `FormFieldDto` shape as the ad fields).
2. In parallel, read the **live** settings: `GET /api/v2/ad-payment/config` (authenticated, `no-store`).
   - `isEnabled: false` → render nothing; submit the ad exactly as before. Done.
   - `isEnabled: true` → render the Payment section: `price` + `currency`, `instructions`, a radio list of
     `paymentMethods` (show the chosen method's `phoneNumber` / `instaPayIdentifier` / `bankName` /
     `accountHolderName` / `accountNumber` / `iban` / `paymentInfo` / `instructions`), a transaction
     reference, the proof image and optional notes.
3. On submit (one button, two calls):
   1. `POST` the ad to its module's existing create endpoint (unchanged). Read `data.id`.
   2. `POST {submitEndpoint}` with `{advertisementType}` = `payment.advertisementType` and
      `{advertisementId}` = `data.id`, multipart, including `ExpectedAmount` = `price` and
      `ExpectedCurrency` = `currency` that the user saw.
4. If step 3.2 fails (network, 409 price changed, bad image), the ad already exists and simply **awaits
   payment**. Show the error and retry the payment call only. `GET {statusEndpoint}` always tells you where the
   ad stands.

## 2. Endpoints

All responses use the standard envelope `{ success, message, data, errors }`. Enums in JSON are numeric
(`paymentStatus`: 1 Pending, 2 Approved, 3 Rejected). `{type}` in a route accepts the `ListingModuleType`
number **or** name (`50` or `Furniture`).

### A. `GET /api/v2/ad-payment/config` — live configuration

Auth: any signed-in user (401 otherwise). `GET /api/v2/public/ad-payment/config` returns the same payload
anonymously and is kept for compatibility. Both send `Cache-Control: no-store`.

```json
{
  "isEnabled": true,
  "price": 150.0,
  "currency": "EGP",
  "instructions": "حوّل المبلغ وارفع صورة الإيصال.",
  "updatedAt": "2026-11-04T08:00:00Z",
  "paymentMethods": [
    {
      "id": 1, "name": "Vodafone Cash", "arabicName": "فودافون كاش", "type": 1,
      "phoneNumber": "01000000000", "instaPayIdentifier": null, "bankName": null,
      "accountHolderName": "Shopiklopik", "accountNumber": null, "iban": null,
      "paymentInfo": "01000000000", "instructions": "حوّل على الرقم ده"
    }
  ]
}
```

Only active methods (narrowed by the admin's `paymentMethodIds`, if set) are returned, and none while
`isEnabled` is false. No admin metadata (`updatedBy`, `enabledAt`, `paymentMethodIds`, a method's `isActive`,
display order or timestamps) is exposed.

### B. Creating the advertisement — unchanged routes, enforced server-side

The 65 module create endpoints are **unchanged** (same request, same response). What changes is what happens
after creation while payments are enabled:

| Ad state after create | Public | Admin moderation queue | Admin can approve the ad? |
| --- | --- | --- | --- |
| `Pending`, stage `AwaitingPayment` | No | Yes, row carries `payment.stage = "AwaitingPayment"` | **No — 402** |
| `Pending`, stage `PaymentUnderReview` | No | Yes, `payment.stage = "PaymentUnderReview"` | **No — 402** |
| `Pending`, stage `PaymentRejected` | No | Yes | **No — 402** |
| `Pending`, stage `Paid` (approved, unused payment) | No | Yes | Yes → published, payment consumed |

The advertisement is stored in its own table as a normal `Pending` listing. It is invisible to the public
(moderation filter) until an admin approves it, and approval is refused with **402 Payment Required** until an
approved ad payment exists. This single gate sits in `PATCH /api/v2/admin/ads/{type}/{id}/approve`, the only
code path that publishes a listing, so it covers every module, every create route, direct API calls, modified
payloads and owner republish alike. The identifiers needed for the payment are the create response's `data.id`
plus the form's `payment.advertisementType`.

### C. `POST /api/v2/ads/{type}/{id}/payment` — submit payment proof

Auth: the advertisement's owner (anyone else, or an unknown id → 404). Content type: `multipart/form-data`.

| Field | Type | Rules |
| --- | --- | --- |
| `PaymentMethodId` | int | required; must be one of the currently offered active methods, else 400 |
| `Screenshot` | file | required; a real image (checked by content, not name/MIME), max size as in `create-ad-form.upload` |
| `TransactionReference` | string | optional, ≤ 100 |
| `Notes` | string | optional, ≤ 500 |
| `ExpectedAmount` | decimal | optional but recommended; differs from the current price → **409** |
| `ExpectedCurrency` | string | optional; differs from the current currency → **409** |

`Amount`/`Currency` are never read from the request: the server copies them from the settings and freezes them
on the submission.

`201 Created`:

```json
{
  "paymentSubmissionId": "8a4f…", "advertisementId": "3c1d…", "advertisementType": 50,
  "amount": 150.0, "currency": "EGP", "paymentStatus": 1, "paymentStatusName": "قيد المراجعة",
  "submittedAt": "2026-11-04T08:01:00Z", "paymentMethodId": 1, "paymentMethodName": "فودافون كاش",
  "transactionReference": "TX-123", "notes": null, "isResubmission": false,
  "previousPaymentSubmissionId": null, "stage": "PaymentUnderReview",
  "stageName": "إثبات الدفع قيد المراجعة", "statusEndpoint": "/api/v2/ads/Furniture/3c1d…/payment"
}
```

| Status | When |
| --- | --- |
| 400 | payments disabled; the ad doesn't need a payment (window still running, grandfathered); bad method; missing/invalid image; field too long |
| 401 | not signed in |
| 404 | ad not found or not yours |
| 409 | a submission is already pending; an approved unused payment already exists; price/currency changed |
| 413 | request body too large |

### D. `GET /api/v2/ads/{type}/{id}/payment` — payment status (owner)

```json
{
  "advertisementId": "3c1d…", "advertisementType": 50, "advertisementTitle": "كنبة",
  "advertisementModerationStatus": "Pending", "advertisementModerationStatusName": "قيد المراجعة",
  "paymentsEnabled": true, "isPaymentRequired": true,
  "stage": "PaymentRejected", "stageName": "تم رفض إثبات الدفع، ارفع إثبات جديد",
  "canSubmit": true, "canResubmit": true, "hasPayment": true,
  "paymentSubmissionId": "8a4f…", "paymentStatus": 3, "paymentStatusName": "مرفوض",
  "amount": 150.0, "currency": "EGP", "paymentMethodId": 1, "paymentMethodName": "فودافون كاش",
  "transactionReference": "TX-123", "notes": null,
  "submittedAt": "2026-11-04T08:01:00Z", "reviewedAt": "2026-11-04T09:00:00Z",
  "rejectionReason": "المبلغ ناقص",
  "currentPrice": 200.0, "currentCurrency": "EGP",
  "submitEndpoint": "/api/v2/ads/Furniture/3c1d…/payment",
  "resubmitEndpoint": "/api/v2/ads/Furniture/3c1d…/payment/resubmit"
}
```

`amount`/`currency` are the **frozen** values of the latest submission; `currentPrice` is what a new
submission would cost. 401 / 404 as above. `no-store`.

`stage` values: `NotRequired`, `AwaitingPayment`, `PaymentUnderReview`, `PaymentRejected`, `Paid`.

### E. `POST /api/v2/ads/{type}/{id}/payment/resubmit` — after a rejection

Same fields, rules and response as C (`isResubmission: true`, `previousPaymentSubmissionId` set). Allowed only
when the latest submission is `Rejected` (`canResubmit: true`): **400** if there is no submission, **409** if
the latest one is `Pending` or `Approved`. A new `Pending` row is created at the **current** price. The
rejected row stays in history. (Posting to C after a rejection is equivalent and still accepted.)

### F. `GET /api/v2/ad-payment/modules` — applicability map

Auth: signed-in. One row per advertisement type: `advertisementType`, `advertisementTypeKey`,
`advertisementTypeName`, `categoryId`, `subCategoryId`, `apiRoute`, `isSubjectToPayment`.

### Admin APIs — unchanged contracts

`GET|PUT /api/v2/admin/ad-payment/settings`, `GET /submissions`, `GET /submissions/{id}`,
`GET /submissions/{id}/screenshot`, `PATCH /submissions/{id}/review`, `GET /history` keep their request and
response shapes. Review accepts `Approve`/`Approved` and `Reject`/`Rejected`; a rejection needs a reason, an
approval ignores it (empty is fine). Additive only: the public config gained `updatedAt` (the admin settings
already had it), and admin ad list / ad details rows carry an optional `payment` object (`paymentsEnabled`,
`isPaymentRequired`, `stage`, `stageName`, `paymentSubmissionId`, `paymentStatus`).

## 3. Lifecycle

```
create ad ──► Pending / AwaitingPayment
                 │ POST payment
                 ▼
          Pending / PaymentUnderReview ──admin Reject──► Pending / PaymentRejected
                 │ admin Approve                                │ POST resubmit
                 ▼                                              └──► PaymentUnderReview (again)
          Pending / Paid  (credit)
                 │ admin approves the AD (normal moderation)        admin rejects the AD
                 ▼                                                  ▼
          Approved, 30-day window, credit consumed          Rejected (credit kept; owner edits → Pending → approval uses it)
                 │ window ends
                 ▼
          Expired ──owner republish──► Pending / AwaitingPayment (new payment required)
                  ──owner pays while expired──► admin approves payment → listing renewed automatically
```

- Payment approval **does not** approve the advertisement; it only unblocks moderation. The one exception is
  an already-approved, expired ad: approving its payment renews it (the existing admin republish).
- Payment rejection notifies the owner with the reason and keeps the ad `Pending`.
- Editing a live ad sends it back to review without a new payment (its window is still running).
- Ads created **before** the admin switched payments on are grandfathered for their first approval.
- Disabling payments un-gates everything immediately; pending submissions stay reviewable.

### Republish policy

Owner republish (`POST /api/advertisements/{id}/republish?type=…`) resets the ad to `Pending` with a fresh
creation date, so **a new payment is required** while payments are enabled. The response now carries
`paymentRequired: true` and `paymentEndpoint`. An old payment cannot be reused: each approved payment is
consumed by the window it opened. Admin republish is an admin decision and is not gated (it consumes a credit
if one exists).

### Price changes mid-flow

- The amount is frozen on the submission. Changing the price never alters a pending or approved submission;
  admins review the frozen amount.
- A client that sends `ExpectedAmount`/`ExpectedCurrency` gets **409** if the price changed between showing the
  form and submitting, instead of being charged a different amount silently.
- A resubmission after rejection is charged at the price in force at resubmission time (`currentPrice` in the
  status tells the client in advance).
- Disabling a payment method doesn't affect submissions already made with it; it just stops being offered.

## 4. Advertisement types subject to Ad Payment

All 65 advertisement types are subject to Ad Payment (cars sub-categories 1–4 share the `Advertisement`
module). An exemption, if ever needed, is a one-line change in `AdPaymentCatalog.Exempt`: the form descriptor,
the approval gate, the status endpoint and `/api/v2/ad-payment/modules` all read it.

| # | `advertisementType` | Key | Name | Category | Sub-category | Ad Payment |
|---|---|---|---|---|---|---|
| 1 | 1 | `Advertisement` | الإعلان | 1 | 1, 2, 3, 4 | Yes |
| 2 | 2 | `Craftsman` | بيانات الحرفي | 2 | 6 | Yes |
| 3 | 3 | `Workshop` | بيانات الورشة | 2 | 5 | Yes |
| 4 | 4 | `Factory` | إعلان المصنع | 4 | 9 | Yes |
| 5 | 5 | `Farm` | إعلان المزرعة | 4 | 10 | Yes |
| 6 | 6 | `Company` | إعلان الشركة | 4 | 11 | Yes |
| 7 | 7 | `Supplier` | إعلان المورد | 4 | 12 | Yes |
| 8 | 8 | `WholesaleTrader` | إعلان تاجر الجملة | 4 | 13 | Yes |
| 9 | 9 | `FruitVegetableMerchant` | إعلان تاجر الخضر والفاكهة | 4 | 14 | Yes |
| 10 | 10 | `JobRequest` | طلب الوظيفة | 5 | 15 | Yes |
| 11 | 11 | `JobOpportunity` | فرصة العمل | 5 | 16 | Yes |
| 12 | 20 | `Livestock` | إعلان المواشي | 6 | 17 | Yes |
| 13 | 21 | `SheepGoat` | إعلان الأغنام والماعز | 6 | 18 | Yes |
| 14 | 22 | `Horse` | إعلان الخيول | 6 | 19 | Yes |
| 15 | 23 | `Camel` | إعلان الإبل | 6 | 20 | Yes |
| 16 | 24 | `Bird` | إعلان الطيور | 6 | 21 | Yes |
| 17 | 25 | `Pet` | إعلان الحيوانات الأليفة | 6 | 22 | Yes |
| 18 | 26 | `Fish` | إعلان الأسماك | 6 | 23 | Yes |
| 19 | 27 | `Bee` | إعلان النحل | 6 | 24 | Yes |
| 20 | 28 | `OtherAnimal` | إعلان الحيوان | 6 | 25 | Yes |
| 21 | 30 | `DecorAntique` | إعلان التحف | 7 | 26 | Yes |
| 22 | 31 | `Antique` | إعلان الأنتيكات | 7 | 27 | Yes |
| 23 | 32 | `Painting` | اللوحة الفنية | 7 | 28 | Yes |
| 24 | 33 | `Handmade` | إعلان الأعمال اليدوية | 7 | 29 | Yes |
| 25 | 34 | `CoinStamp` | إعلان العملات والطوابع | 7 | 30 | Yes |
| 26 | 35 | `MenClothing` | إعلان الملابس الرجالي | 8 | 31 | Yes |
| 27 | 36 | `WomenClothing` | إعلان الملابس الحريمي | 8 | 32 | Yes |
| 28 | 37 | `KidsClothing` | إعلان ملابس الأطفال | 8 | 33 | Yes |
| 29 | 38 | `Accessory` | إعلان الإكسسوارات | 9 | 34 | Yes |
| 30 | 39 | `Cosmetic` | إعلان مستحضرات التجميل | 9 | 35 | Yes |
| 31 | 40 | `HomeKitchen` | إعلان المنزل والمطبخ | 9 | 36 | Yes |
| 32 | 41 | `ShoppingElectronic` | إعلان الإلكترونيات | 9 | 37 | Yes |
| 33 | 42 | `GiftToy` | إعلان الهدايا والألعاب | 9 | 38 | Yes |
| 34 | 43 | `HomemadeFood` | إعلان الأكل المنزلي | 9 | 39 | Yes |
| 35 | 50 | `Furniture` | إعلان الأثاث | 10 | 40 | Yes |
| 36 | 51 | `FurnishingCurtain` | إعلان المفروشات والستائر | 10 | 41 | Yes |
| 37 | 52 | `LightingDecor` | إعلان الإضاءة والديكور | 10 | 42 | Yes |
| 38 | 53 | `KitchenTool` | إعلان أدوات المطبخ | 10 | 43 | Yes |
| 39 | 54 | `HomeAppliance` | إعلان الأجهزة الكهربائية | 10 | 44 | Yes |
| 40 | 55 | `BathroomSupply` | إعلان مستلزمات الحمام | 10 | 45 | Yes |
| 41 | 56 | `PlantOrnament` | إعلان النباتات والزينة | 10 | 46 | Yes |
| 42 | 60 | `Land` | إعلان الأرض | 11 | 47 | Yes |
| 43 | 61 | `Apartment` | إعلان الشقة | 11 | 48 | Yes |
| 44 | 62 | `Shop` | إعلان المحل | 11 | 49 | Yes |
| 45 | 63 | `Building` | إعلان المبنى | 11 | 65 | Yes |
| 46 | 65 | `Rescue` | الاستغاثة | 12 | 50 | Yes |
| 47 | 66 | `BloodRequest` | طلب فصيلة الدم | 12 | 51 | Yes |
| 48 | 67 | `AskConsult` | السؤال | 12 | 52 | Yes |
| 49 | 70 | `LostItem` | بلاغ المفقودات | 3 | 7 | Yes |
| 50 | 71 | `FoundItem` | بلاغ المعثورات | 3 | 8 | Yes |
| 51 | 80 | `Doctor` | إعلان الطبيب | 13 | 53 | Yes |
| 52 | 81 | `MedicalLaboratory` | إعلان معمل التحاليل | 13 | 54 | Yes |
| 53 | 82 | `DentalLaboratory` | إعلان معمل الأسنان | 13 | 55 | Yes |
| 54 | 83 | `RadiologyLaboratory` | إعلان معمل الأشعة | 13 | 56 | Yes |
| 55 | 84 | `MedicalCenter` | إعلان المركز الطبي | 13 | 57 | Yes |
| 56 | 85 | `Hospital` | إعلان المستشفى | 13 | 58 | Yes |
| 57 | 90 | `LawyerOffice` | إعلان مكتب المحاماة | 14 | 59 | Yes |
| 58 | 91 | `EngineeringConsultancyOffice` | إعلان المكتب الهندسي | 14 | 60 | Yes |
| 59 | 92 | `Contractor` | إعلان المقاول | 14 | 61 | Yes |
| 60 | 93 | `IndustrialConsultancyOffice` | إعلان مكتب الاستشارات الصناعية | 14 | 62 | Yes |
| 61 | 94 | `AccountantOffice` | إعلان مكتب المحاسب القانوني | 14 | 63 | Yes |
| 62 | 100 | `RestaurantAndCafe` | إعلان المطعم أو الكافيه | 15 | 64 | Yes |
| 63 | 101 | `CommercialShop` | إعلان المحل التجاري | 2 | 66 | Yes |
| 64 | 102 | `GeneralService` | إعلان الخدمة | 2 | 67 | Yes |
| 65 | 103 | `ResortHotelAmusement` | إعلان القرية السياحية أو الفندق أو الملاهي | 2 | 68 | Yes |
