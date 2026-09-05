# Performance

[← README](../README.md) · Related: [DATABASE](DATABASE.md) · [TESTING](TESTING.md) · [DECISIONS](DECISIONS.md)

Every figure here was **measured**, not estimated. The measurement conditions are stated so they can
be reproduced or challenged.

> **Measurement conditions.** An isolated SQL Server LocalDB seeded to **200,047 أراضي listings**,
> 600,095 images, 600,032 multi-select rows, ownership spread over **5,000 accounts** (~40 listings
> each), of which ~43,000 are inside their publication window. p50/p95 over 15 warm requests.
> `X-Sql-Count` read from the response header.

---

## How to measure

```bash
export Diagnostics__SqlCounter=true
```

Then read the response headers:

| Header | Meaning |
| --- | --- |
| `X-Sql-Count` | Database round trips |
| `X-Sql-Ms` | Time in SQL |
| `X-Elapsed-Ms` | Total request time |

> The interceptor is **only constructed when the switch is on** — it is absent from the pipeline in
> an ordinary run.

**The real N+1 test is that cost does not grow with page size or image count.** A constant round-trip
count is not a defect.

For logical reads, go to the database directly:

```sql
SET STATISTICS IO ON;
SELECT …;
```

---

## Current profile

| Endpoint | SQL | p50 | p95 |
| --- | ---: | ---: | ---: |
| Land feed — page 1 (anonymous) | 9 | 40 ms | 52 ms |
| Land feed — page 1 (signed in) | 12 | 41 ms | 42 ms |
| Land feed — deep page 500 | 9 | 39 ms | 49 ms |
| Land feed — deep page 5,000 | 9 | 39 ms | 42 ms |
| Land feed — deep page 9,999 | 9 | 39 ms | 42 ms |
| Land feed — filter by landType | 9 | 51 ms | 54 ms |
| Land feed — sort priceAsc | 9 | 69 ms | 70 ms |
| Land feed — substring search | 9 | 270 ms | 274 ms |
| Land price statistics | 1 | 14 ms | 14 ms |
| Land recently-added | 7 | 5 ms | 6 ms |
| Land lookup | 1 | 1.6 ms | 1.7 ms |
| Land details (anonymous) | 13 | 6 ms | 7 ms |
| Land details (signed in) | 16 | 8 ms | 9 ms |
| Land details (admin) | 15 | 7 ms | 8 ms |
| Land similar | 9 | 31 ms | 34 ms |
| Land related | 9 | 205 ms | 237 ms |
| Profile my-listings | 2 | 26 ms | 47 ms |
| Notifications list | 2 | 2 ms | 2 ms |
| Admin dashboard | 33 | 108 ms | 119 ms |
| Home page | 1 | 1.8 ms | 2 ms |
| create-ad-form (Land) | 2 | 3 ms | 4 ms |

**Deep pagination is flat** — page 1, page 500 and page 9,999 all cost ~39 ms and 9 round trips. That
is the key-first paging helper doing its job.

---

## Key-first paging

**Problem.** A list query is filtered, sorted, paged with `OFFSET/FETCH`, then given its `Include`s.
Two or more collections force `AsSplitQuery`, and EF then **repeats the whole filtered, sorted, paged
sub-query in every split statement**. The expensive part of a page is the sort — and the sort was
being paid once per `Include`.

**Solution.** `PagedListingQuery.ToPageAsync` asks for the page's **keys** first — one narrow,
index-friendly, sorted query — then loads those rows and their collections **by key**.

> Measured on 65k apartments with three collections: the root statement cost **10,177 logical reads
> and 88 ms**, and each collection statement re-sorted the same 65k rows for another ~95 ms.
> Afterwards: **3 logical reads** for the root and **2 per collection**.

It cannot change what is returned: the second query is built from the same `IQueryable`, so every
filter — including the global moderation and soft-delete filters — is still in force.

Used by **all 48** module list endpoints.

---

## Discovery strips

`similar` and `related` had the same problem and had been left out of the fix — an unindexable sort
over every listing in the module, paid three or four times to return eight rows.

`PagedListingQuery.ToStripAsync` (a thin wrapper over `ToPageAsync` — a strip is page one) now serves
them in the nine repositories that had this shape: Apartment, BathroomSupply, FurnishingCurtain,
Furniture, HomeAppliance, KitchenTool, Land, LightingDecor, Shop.

| Endpoint | Before | After | Change |
| --- | ---: | ---: | ---: |
| `GET /api/lands/{id}/related` | 1,330 ms p50 · 1,865 p95 | 354 ms · 395 | **3.8×** |
| `GET /api/lands/{id}/similar` | 656 ms p50 · 789 p95 | 167 ms · 180 | **3.9×** |
| `GET /api/lands` *(control, unchanged code)* | 65.5 ms | 59.6 ms | — |

### One change was measured and reverted

`recently-added` was rewritten the same way and got **slower** — 8.7 ms → 10.3 ms. Its
`ORDER BY CreatedAt DESC` is already served by an index, so there is no sort to save and the extra
keyed round trip is pure cost. It was reverted.

> **The rule, written in the helper's own documentation:** use `ToStripAsync` only where the ordering
> is a computed expression (`ABS(price - price)`) or a predicate (`UserId == owner`) the database
> cannot seek. Where an index already serves the `ORDER BY`, the single-query form wins.

---

## The visibility + ordering index

Every list endpoint of every windowed module asks two questions in one breath: which rows are
publicly visible, and which twenty come first in the module's default order. Neither was answerable
from the indexes that existed — practically every row is approved and inside its window, so a
single-column index on either narrows nothing.

Declared **once**, in `ModerationModelConfiguration`, for all modules:

```
(ModerationStatus, [IsPremium, IsFeatured,] CreatedAt, Id)  INCLUDE (IsDeleted, ExpireAt)
```

- Leading with the moderation decision makes it an **equality seek**.
- Following with the module's own sort columns, **descending, in the repository's order**, means the
  page is read straight off the index in order and stops after twenty rows at any depth.
- Carrying the soft-delete flag and the window as included columns lets the visibility test and the
  `COUNT` be answered without touching the table.

> Measured on a 65k-row module table: page 1 fell from **1,892 logical reads and 97 ms** to **3 reads
> and under a millisecond**; page 500 to 58 reads; the `COUNT` from 793 reads to 353.

Two details that look optional and are not:

- **The `Id` tie-breaker must be in the key.** A nonclustered index carries the clustering key
  ascending; the lists ask for it descending. That single mismatch puts the sort back — 353 reads and
  39 ms versus 3 reads and under a millisecond.
- **The soft-delete flag must be carried.** Without it the database still opens every row to answer
  "and not deleted" — measured identically to having no index at all (4,312 reads versus 292).

---

## The Land search covering index

`IX_Lands_Search` — migration `AddLandSearchCoveringIndex`:

```
(ModerationStatus, IsDeleted, ExpireAt)
INCLUDE (Title, Description, District, Address, OtherLandType, OtherProject,
         CreatedAt, IsPremium, IsFeatured, TotalPrice)
```

**Why it helps.** البحث is a substring match, so `LIKE N'%term%'` can never seek. What an index can
still do is decide which rows the scan visits and how wide each is — and that was the whole cost. The
search predicate reads six columns that lived in no index, so SQL Server scanned the **197 MB**
clustered index — every one of a listing's hundred-odd columns, for all 200,000 rows, live or long
expired.

| Endpoint | Before | After | Change |
| --- | ---: | ---: | ---: |
| `price-statistics` | 115.7 ms | 14.1 ms | **8.2×** |
| Sort `priceDesc` | 181.9 ms | 64.7 ms | **2.8×** |
| Sort `priceAsc` | 166.8 ms | 68.8 ms | **2.4×** |
| Broad search (`أرض`) | 121.5 ms | 31.4 ms | **3.9×** |
| Narrower search (`مميزة`) | 364.7 ms | 293.4 ms | 1.24× |
| Narrow search (`رقم 12345`) | 812.7 ms | 737.8 ms | 1.10× |
| **Logical reads (search `COUNT`)** | **25,204** | **1,625** | **15.5×** |

Index size: **58 MB** against a 197 MB clustered index.

### ⚠️ The trap, measured twice

The index only works if **every** searched column is in the `INCLUDE`. Leaving out one —
`OtherLandType` — was tested, and the optimiser **abandoned the index entirely** and went back to the
25,204-read clustered scan, for no benefit at all.

That is why this cannot live in the shared `ApplyCommonIndexes` helper: each module searches its own
"أخرى" free-text columns, so the covering set is genuinely per module.

**It is applied to أراضي only**, where it is measured — deliberately **not** rolled out blindly to the
other modules. Each costs roughly 58 MB per 200,000 rows; that is a storage decision to be made per
module, with a measurement.

---

## The owner-visibility rule costs nothing

`VisibleToViewer` adds an OR-ed ownership branch to the by-id read. It was verified not to change the
query shape:

| Metric | Before | After |
| --- | ---: | ---: |
| Details round trips (anonymous / signed-in / admin) | 13 / 16 / 15 | **13 / 16 / 15** |
| Logical reads on the by-id read | 3 | **3** |

Still a clustered PK seek; ownership is a residual predicate on the single row already found. No
ownership pre-check, no second round trip.

---

## Cross-module statistics

`ListingStatsFilter` fills `views`, `averageRating`, `ratingsCount` and `isFavorite` into every
listing DTO in the response. It is a **global filter** rather than per-module code because these
numbers live in cross-module tables — joining them per service would be 48 copies of one fetch, and
the card would eventually disagree with the details page.

Cost: **one batched query per response**, regardless of page size. A signed-in non-owner costs one
extra query for the views-visibility rule; anonymous and admin cost none.

---

## Other decisions

| Decision | Detail |
| --- | --- |
| **Projection** | List DTOs carry only what a card shows. A field the card does not show does not belong on the list DTO |
| **Tracking** | `AsNoTracking()` on every read; tracked only for writes |
| **Split queries** | `AsSplitQuery()` whenever more than one collection is included |
| **Caching** | `IMemoryCache` with `SizeLimit = 512` for lookup reference data; every entry declares size 1, so the bound is "at most N lists". Admin toggles must invalidate `ILookupCache` |
| **Compression** | Brotli + Gzip, `CompressionLevel.Fastest` |
| **Static files** | Uploads served `immutable` with `max-age=31536000` — names are GUIDs and content never changes |
| **Response caching** | Middleware registered; opt-in per endpoint via `[ResponseCache]` |

---

## Remaining bottlenecks

Known, measured, and not yet fixed.

| Bottleneck | Cost | What would fix it |
| --- | ---: | --- |
| A narrow search term still scans every live listing | 738 ms | Full-text search. **Not verifiable in the environment inspected** — `SERVERPROPERTY('IsFullTextInstalled')` returned 0 and no Arabic word breaker (LCID 1025) was registered. Check the production host before planning it |
| `related` sorts on a computed predicate over an `OR` | 205 ms | Splitting it into two seekable top-N queries plus `(UserId, CreatedAt)` and `(Center, CreatedAt)` indexes. Provably equivalent but **unmeasured** — left for a pass that can verify it |
| Admin dashboard issues 33 round trips | 108 ms | Acceptable: a constant, not an N+1, and only administrators pay it |
| The covering index exists only for أراضي | — | The same pattern per module, after a storage check |

### Index hygiene

Roughly **1,266 indexes** exist across the database, many single-column on `bit` flags (`HasFence`,
`IsOrganic`, `Negotiable`) with almost no selectivity. They cost write throughput and storage and are
unlikely to be chosen by any plan.

> **Do not drop them speculatively.** Review against `sys.dm_db_index_usage_stats` **on production**
> first — usage on a development database proves nothing.

---

## Rules for performance work

1. **Measure before and after, on the same data**, with the same warm-up.
2. **Do not claim an optimisation you have not demonstrated.** One change in this project was
   reverted because the measurement did not support it.
3. **A constant round-trip count is not a defect.** Only growth with page size or collection size is.
4. **Do not create indexes blindly** — an index that does not cover every predicate can be ignored
   entirely, buying nothing and costing every write.
5. **Production SQL is a remote host**, so round trips dominate. `X-Sql-Count` is the metric that
   matters most.
