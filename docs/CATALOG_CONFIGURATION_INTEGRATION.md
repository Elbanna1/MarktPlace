# Catalog configuration: payment and visibility per category, sub-category and module

The admin can switch ad payments and visibility on or off for any **category**, **sub-category** or **advertisement
module**, in bulk, with optional UTC schedules. Children inherit from their parent unless they have their own rule.
One resolver (`CatalogConfigurationSnapshot`) computes the effective value. The ad payment check (402 on approval),
the moderation queue, proof submission, the lookups, the home page and the create endpoints all read it, so no
module has its own payment or visibility code.

All responses use the standard envelope `{ success, message, data, errors }`. Enum values in responses are numeric
and come with a `...Key` or `...Name` string where it matters. **Request bodies take strings for node types, kinds
and areas** (`"Category"`/`"1"`, `"Payment"`, `"HomePage"`), parsed case-insensitively. Null properties are left
out of the JSON.

## 1. The hierarchy

```
Global (PUT /api/v2/admin/ad-payment/settings: on/off, price, currency, methods, instructions)
└─ Category            nodeType "Category"    nodeId = category id       (e.g. 10 افرش بيتك)
   └─ SubCategory      nodeType "SubCategory" nodeId = sub-category id   (e.g. 40 أثاث)
      └─ Module        nodeType "Module"      nodeId = ListingModuleType (e.g. 50 Furniture)
```

- The tree comes from the database categories (`ILookupService`), completed with `CategoryCatalog` for any inactive
  entry, so every category and sub-category in the code appears. Modules come from `ListingModuleCatalog.ModuleOf`.
- The car module `Advertisement` (1) appears under each car sub-category (1–4). A rule on `Module:1` applies to all
  four; a rule on `SubCategory:2` (Taxi) applies to taxi ads only.
- Node keys are `"{NodeType}:{NodeId}"`, e.g. `Category:10`, `SubCategory:40`, `Module:50`.

## 2. Inheritance and precedence

For an ad of module M in sub-category S of category C, the resolver walks `Module:M → SubCategory:S → Category:C →
Global`:

| Setting | Rule |
| --- | --- |
| Payment on/off | The **first** node in the chain with a rule **in effect now** decides. No such node: the global switch. |
| Price | The first in-effect rule **that has a price**, walking from the module upward; otherwise the global price. A rule can switch payments on without a price and inherit one. |
| Currency | Same as price; otherwise the global currency. |
| Visibility (per area) | The first node in the chain with an in-effect rule **for that area**. No rule: visible and enabled. |
| Protected free | Category 12 (بوابة الخيرات) and everything under it: never requires payment, whatever is configured (§5). |

A rule is **in effect** when `activationDate ≤ now < expirationDate`; a missing date means unbounded. Outside its
window a rule is ignored and the node inherits from its parent. Each node has at most one payment rule and at most
one rule per visibility area (unique index), so schedules never overlap within a node. Across levels, the lowest
node always wins. **Resetting** a node deletes its rule, and it inherits again.

`paymentRequired` = enabled **and** not protected **and** the module is subject to payment **and** price > 0.

**Grandfathering.** As before, a pending or rejected ad created before payment became required for it does not have
to pay. "Became required" is the deciding rule's `requiredSince`. It is set when the rule is switched on; if the
parent already required payment, the parent's date is kept, so a child rule never frees ads that already owed
payment. Approved and live ads are never affected by any change.

## 3. Admin endpoints

Base: `/api/v2/admin/catalog-configuration`. All need the **Admin** role plus the page permission shown. A Super
Admin passes every check.

| Method | Route | Permission | Purpose |
| --- | --- | --- | --- |
| GET | `/api/v2/admin/catalog-configuration` | Categories · View | Full hierarchy, explicit and effective configuration |
| GET | `/api/v2/admin/catalog-configuration/options` | Categories · View | Node types, areas, kinds, statuses, limits |
| GET | `/api/v2/admin/catalog-configuration/nodes/{nodeType}/{nodeId}` | Categories · View | One node (a category includes its children) |
| GET | `/api/v2/admin/catalog-configuration/effective?type=&categoryId=&subCategoryId=` | Categories · View | Effective payment and visibility for a selection, with the chain |
| GET | `/api/v2/admin/catalog-configuration/scheduled` | Categories · View | Rules with a future activation or expiration |
| GET | `/api/v2/admin/catalog-configuration/history` | Categories · View | Audit log of configuration changes (the audit-log filters apply) |
| PUT | `/api/v2/admin/catalog-configuration/payment` | **Payments · Edit** | Bulk create or update payment rules |
| PUT | `/api/v2/admin/catalog-configuration/visibility` | Categories · Edit | Bulk create or update visibility rules |
| POST | `/api/v2/admin/catalog-configuration/reset` | Payments · Edit (kind Payment) / Categories · Edit (kind Visibility) | Delete rules so nodes inherit again |

GET responses are `Cache-Control: no-store`. 401 when signed out, 403 without the permission.

### 3.1 GET hierarchy: response (abridged)

```json
{
  "generatedAt": "2026-11-06T18:00:00Z",
  "globalPayment": { "paymentEnabled": false, "paymentAmount": 0, "currency": "EGP",
                     "settingsEndpoint": "/api/v2/admin/ad-payment/settings" },
  "categories": [
    {
      "key": "Category:10", "nodeType": 1, "nodeTypeName": "Category", "nodeId": 10,
      "name": "Home Furnishing", "nameAr": "افرش بيتك", "categoryId": 10,
      "isProtectedFree": false, "isSubjectToPayment": true,
      "payment": {
        "explicit": { "configurationId": 7, "paymentEnabled": true, "paymentAmount": 70.0,
                      "requiredSince": "2026-11-06T17:59:00Z", "configurationStatus": "Active",
                      "createdAt": "2026-11-06T17:59:00Z", "updatedBy": "<admin id>" },
        "effective": { "paymentEnabled": true, "paymentRequired": true, "paymentAmount": 70.0, "currency": "EGP",
                       "requiredSince": "2026-11-06T17:59:00Z", "isProtectedFree": false,
                       "inheritanceMode": "Explicit", "sourceKey": "Category:10",
                       "priceSourceKey": "Category:10", "configurationStatus": "Active" }
      },
      "visibility": [
        { "area": 1, "areaKey": "HomePage", "areaName": "الصفحة الرئيسية",
          "isVisible": true, "isEnabled": true, "inheritanceMode": "Default" }
      ],
      "children": [
        { "key": "SubCategory:40", "nodeType": 2, "nodeId": 40, "parentKey": "Category:10", "nameAr": "أثاث",
          "payment": { "effective": { "paymentRequired": true, "inheritanceMode": "Inherited",
                                      "sourceKey": "Category:10", "parentConfigurationId": 7, "...": "..." } },
          "children": [ { "key": "Module:50", "nodeType": 3, "nodeId": 50, "moduleKey": "Furniture",
                          "parentKey": "SubCategory:40", "...": "..." } ] }
      ]
    }
  ]
}
```

- `inheritanceMode`: `Explicit` (this node's own rule decides), `Inherited` (an ancestor's rule decides; see
  `sourceKey` and `parentConfigurationId`), `Global` (payment comes from the global settings), `Default`
  (visibility with no rule anywhere means shown), `Protected` (charity).
- `configurationStatus` (effective): `Protected`, `Inherited` (no own rule), or the own rule's status `Active` /
  `Scheduled` / `Expired`.

### 3.2 PUT payment

```json
{
  "targets": [ { "nodeType": "Category", "nodeId": 10 }, { "nodeType": "Module", "nodeId": 60 } ],
  "paymentEnabled": true,
  "paymentAmount": 70.00,
  "currency": "EGP",
  "paymentActivationDate": "2026-12-01T00:00:00Z",
  "paymentExpirationDate": "2027-01-01T00:00:00Z"
}
```

- Every target gets the same rule, and the request is all-or-nothing (one `SaveChanges`). Existing rules are
  replaced field by field: an omitted `paymentAmount` or `currency` means "inherit", and omitted dates mean
  unbounded.
- `paymentEnabled: false` is an explicit **free** override (e.g. one free module inside a paid category). To go back
  to the parent's setting, use `reset`, not `false`.
- Response `CatalogBulkResultDto`: `{ created, updated, removed, items: [CatalogNodeDto] }`.

Validation (**400** unless noted):

| Case | Response |
| --- | --- |
| no targets, more than 100, unknown node type or id | 400 (`errors[]` lists the unknown keys) |
| `paymentEnabled: true` on category 12, sub-categories 50–52, modules 65–67 | 400 `"بوابة الخيرات وكل أقسامها مجانية دايماً…"` with the offending keys |
| `paymentEnabled: true` and no active payment method offered | 400 |
| `paymentEnabled: true`, no `paymentAmount`, and the inherited price is 0 | 400 |
| `paymentAmount` ≤ 0, > 1,000,000 or with more than 2 decimals | 400 |
| `currency` not 3 ASCII letters | 400 |
| a date without UTC offset (`2027-01-01T00:00:00`) | 400. Send `Z` or an offset; offsets are converted to UTC |
| activation ≥ expiration, or expiration in the past | 400 |
| two admins writing the same new node at once | 409 |

### 3.3 PUT visibility

```json
{
  "targets": [ { "nodeType": "Category", "nodeId": 10 } ],
  "areas": [ "HomePage", "CategoryListing" ],
  "isVisible": false,
  "isEnabled": true,
  "activationDate": null,
  "expirationDate": null
}
```

One rule per target × area. `isVisible: false` hides; `isEnabled: false` keeps the node shown but disables it (for
`AdCreation`: the form and the create endpoints are refused). Same date, target and limit rules as payment.

### 3.4 POST reset

```json
{ "targets": [ { "nodeType": "Module", "nodeId": 50 } ], "kind": "Payment" }
{ "targets": [ { "nodeType": "Category", "nodeId": 10 } ], "kind": "Visibility", "areas": [ "HomePage" ] }
```

`kind` is required (`Payment` | `Visibility`). For Visibility, omitting `areas` resets every area of the targets.
The response's `removed` is the number of deleted rules.

### 3.5 GET options, effective, scheduled, history

- `options`: `nodeTypes`, `areas` (`key`, `value`, `nameAr`, `description`, `enforcedByBackend`), `kinds`,
  `inheritanceOrder` `["Module","SubCategory","Category","Global"]`, `configurationStatuses`,
  `protectedFreeCategoryIds` `[12]`, `defaultCurrency`, `maxPaymentAmount`, `maxTargetsPerRequest` (100),
  `cacheSeconds` (30).
- `effective?type=Furniture` (or `type=50`, or `categoryId=10&subCategoryId=40`): `{ evaluatedAt, advertisementType,
  categoryId, subCategoryId, chain: ["Module:50","SubCategory:40","Category:10","Global"], payment, visibility[] }`.
  400 when nothing is given.
- `scheduled`: `[{ configurationId, key, kind, areaKey, name, isEnabled, isVisible, paymentAmount, currency,
  activationDate, expirationDate, configurationStatus }]`, ordered by the next date.
- `history`: the standard paginated audit log (`/api/v2/admin/audit-logs` shape) filtered to target type
  `CatalogConfiguration`. Each change is one row per target (per area for visibility), with the admin, old value,
  new value and time. Actions: `UpdateCatalogPayment` (100), `UpdateCatalogVisibility` (101),
  `ResetCatalogConfiguration` (102).

## 4. Application areas

| Area | Value | Enforced by the backend |
| --- | --- | --- |
| `HomePage` | 1 | Yes. `GET /api/home` drops sections whose `categoryId` is hidden, and drops hidden categories from `categories` |
| `CategoryListing` | 2 | Yes. `GET /api/lookups/categories` and `/categories-tree` drop hidden categories |
| `SubCategoryListing` | 3 | Yes. `GET /api/lookups/subcategories/{id}` and the sub-categories inside `categories-tree` |
| `AdCreation` | 4 | Yes. `GET /api/lookups/create-ad-form/...` returns **404**; every create `POST /api/{module}` returns **403** `"إضافة الإعلانات في القسم ده متوقفة حالياً."` |
| `Browsing` | 5 | No. The frontend reads it from the public configuration |
| `Search` | 6 | No. The frontend reads it from the public configuration |

These are the areas that exist in the backend. There is no backend concept of a navigation menu or a "featured
categories" block, so those were not invented. Hiding never deletes or unpublishes data, and existing listings stay
reachable through their own URLs. Visibility is **not** authorization: every existing authentication and
authorization check still applies, and admin endpoints always show everything.

**Caching:** the public lookup and home endpoints are cached by the existing response cache (`[HttpCache]`: lookups
300 s, home 60 s). A visibility change can take up to that long to show there. The configuration snapshot itself
is cached 30 s and dropped on every write, so the admin and public configuration endpoints, the payment check and
the create checks are current right away. No restart is needed. Schedules are evaluated at request time.

## 5. بوابة الخيرات is always free

- Identified by `CategoryType.Charity` = **category id 12**, never by its Arabic name
  (`Shared/Constants/CatalogProtection.cs`). Its descendants are found through the catalog mappings: sub-categories
  50, 51, 52 and modules `Rescue` (65), `BloodRequest` (66), `AskConsult` (67).
- `AdPaymentCatalog.IsSubjectToPayment` is false for those three modules. Their forms carry `isSubjectToPayment:
  false`, they are never in a queue payment rule, approval never returns 402, and `POST /api/v2/ads/{type}/{id}/payment`
  returns 400.
- The resolver reports `isProtectedFree: true` / `paymentRequired: false` even if the global switch or a rule says
  otherwise. `PUT …/payment` with `paymentEnabled: true` on any of them is refused with 400.
- Moderation, editing, expiry and republish of charity ads are unchanged.

## 6. Public configuration (no admin data)

| Method | Route | Auth |
| --- | --- | --- |
| GET | `/api/v2/public/catalog-configuration` | Anonymous, `no-store` |
| GET | `/api/v2/ad-payment/config?type=&categoryId=&subCategoryId=` | Signed-in, `no-store` |
| GET | `/api/v2/public/ad-payment/config?type=&categoryId=&subCategoryId=` | Anonymous, `no-store` |

`/api/v2/public/catalog-configuration`:

```json
{
  "generatedAt": "2026-11-06T18:00:00Z", "cacheSeconds": 30,
  "categories": [
    { "key": "Category:12", "nodeType": 1, "nodeId": 12, "name": "Charity", "nameAr": "بوابة الخيرات",
      "areas": [ { "area": "HomePage", "isVisible": true, "isEnabled": true } ],
      "payment": { "paymentRequired": false, "isFree": true },
      "children": [ { "key": "SubCategory:50", "...": "...", "children": [ { "key": "Module:65", "moduleKey": "Rescue" } ] } ] }
  ]
}
```

Only effective values: no rule ids, admin ids, timestamps or scheduled rules. `paymentAmount`/`currency` are present
only when `paymentRequired` is true.

`ad-payment/config` **with a selection** returns the effective payment for that form: `isEnabled` (= payment
required), `isProtected`, `scope` (`Module`/`SubCategory`/`Category`), `advertisementType`, `categoryId`,
`subCategoryId`, `price`, `currency`, `instructions` and `paymentMethods` (empty when not required). **Without a
selection** it returns the global default exactly as before, plus `anyCategoryRequiresPayment`.

## 7. Recommended frontend sequence

**Admin dashboard**
1. `GET …/catalog-configuration/options` once, to build the area checkboxes, labels and limits.
2. `GET …/catalog-configuration` to render the tree. Show `effective` values, and mark `Explicit` rules versus
   `Inherited` (link to `sourceKey`), `Protected` and `Scheduled`.
3. Multi-select nodes, then `PUT …/payment` or `PUT …/visibility` with all selected targets. Replace the shown nodes
   with `items` from the response (or re-fetch the tree).
4. "Reset to inherited" calls `POST …/reset` with the selected targets and `kind`.
5. Disable the payment toggle for nodes with `isProtectedFree: true`; the backend refuses it anyway.
6. `GET …/scheduled` and `GET …/history` for the schedule and audit tabs.

**Public site / create-ad form**
1. On load (or on navigation), `GET /api/v2/public/catalog-configuration`, and use `areas` for Browsing and Search
   (the other areas are already filtered by the backend).
2. When a create form opens: `GET /api/lookups/create-ad-form/{categoryId}/{subCategoryId}` (404 means creation is
   disabled), then `GET /api/v2/ad-payment/config?categoryId=..&subCategoryId=..` or `?type=..`. **Pass the
   selection**: without it you get the global default, which is wrong when a single category is paid.
3. If `isEnabled` is true, show the payment section exactly as described in `docs/AD_PAYMENT_INTEGRATION.md`. If
   false, submit as a free ad.

## 8. Where it lives

| Piece | File |
| --- | --- |
| Rule table | `Core/Domain/Entities/CatalogConfiguration.cs`, migration `AddCatalogConfiguration` |
| Resolver (pure) | `Shared/Catalog/CatalogConfigurationSnapshot.cs` |
| Cached snapshot | `Core/Services/Catalog/CatalogConfigurationProvider.cs` (30 s, dropped on write) |
| Admin/public logic | `Core/Services/Catalog/CatalogConfigurationService.cs` |
| Free-category rule | `Shared/Constants/CatalogProtection.cs` (+ `AdPaymentCatalog.Exempt`) |
| Payment check and queue | `Core/Services/AdPayments/AdPaymentGate.cs`, `AdminAdRepository.ApplyPaymentStage` |
| Creation check | `MarkatPlace/Filters/ListingCreationAvailabilityFilter.cs` |
| Endpoints | `AdminCatalogConfigurationController.cs`, `CatalogConfigurationController.cs`, `AdPaymentsController.cs` |
