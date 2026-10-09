# Frontend changes for real-time listings and images

[← README](../README.md) · Related: [API](API.md) · [DEPLOYMENT](DEPLOYMENT.md) · [DECISIONS § D-40](DECISIONS.md#d-40--listing-state-changes-are-pushed-from-one-savechanges-interceptor)

The frontend lives in a separate repository. These are the changes it needs; the backend side is
already implemented and verified (see D-40). Every snippet below was written against the deployed
bundle (`useNotificationHub` = `cr()`, `<Image>` = `Image-*.js`), so names may differ in source.

---

## 1. `<Image>` — the false «لا توجد صورة»

**Root cause.** The component computes its state **once**:

```js
const [state, setState] = useState(() => src ? (loaded.has(src) ? "loaded" : "loading") : "error");
```

`src` is often `undefined` on the first render (list data still arriving, or the card read a field
before the response was mapped). The component is then stuck in `"error"` and renders «لا توجد صورة»
until it is remounted — i.e. until the page is refreshed. A single transient failure is permanent for
the same reason.

**Fix.** Re-derive the state whenever `src` changes, distinguish "no URL yet" from "failed", and retry
a failed load once:

```jsx
function Image({ src, alt = "", ...props }) {
  const [state, setState] = useState(() => initialState(src));
  const [attempt, setAttempt] = useState(0);

  useEffect(() => { setState(initialState(src)); setAttempt(0); }, [src]);

  const onError = () => {
    if (attempt === 0) { setAttempt(1); return; }          // one retry (cache-busted below)
    console.warn("[Image] failed to load", src);           // diagnosable, not hidden
    setState("error");
  };

  const url = attempt === 0 || !src ? src : `${src}${src.includes("?") ? "&" : "?"}retry=1`;
  // render: "loading" → shimmer, "loaded" → <img>, "error" → «لا توجد صورة» ONLY when the
  // listing genuinely has no image (pass hasImages from the caller) — otherwise «تعذّر تحميل الصورة».
}

function initialState(src) {
  return src ? (loaded.has(src) ? "loaded" : "loading") : "pending";   // "pending" ≠ "error"
}
```

Callers should render the empty state only when `images.length === 0` **and** the listing data has
loaded — never because the first render had no URL yet.

> Production note: until the uploads folder is restored on the server (DEPLOYMENT § Uploads
> directory), every existing image URL answers 404, so the corrected component will show «تعذّر تحميل
> الصورة» for those listings. That is the truthful message; it is not a frontend bug.

---

## 2. Real-time listings — reuse the existing hub connection

The app already opens `/hubs/notifications` (SignalR, automatic reconnect). Keep **one** connection
and add three handlers next to `ReceiveNotification` / `UnreadCountChanged`:

| Event | Who receives it | Payload |
| --- | --- | --- |
| `ListingChanged` | the listing's owner + every admin | `{ id, type, typeName, route, categoryId, subCategoryId, change, status, moderationStatus, isPublic, wasPublic, publishedAt, expireAt, remainingDays, republishCount, occurredAt }` |
| `PublicListingChanged` | everyone connected, **including anonymous visitors** | `{ id, type, route, categoryId, subCategoryId, change, isPublic, occurredAt }` |
| `ModerationQueueChanged` | admins | `{ id, type, change, moderationStatus, occurredAt }` |

`change` ∈ `created, updated, approved, rejected, suspended, returnedToReview, republished, expired,
deleted`. Events are sent only after the database save succeeded.

```js
// inside the existing hook, after the ReceiveNotification handler
connection.on("ListingChanged", e => {
  queryClient.invalidateQueries({ queryKey: ["my-listings"] });              // owner pages + status badges
  queryClient.invalidateQueries({ queryKey: ["profile-statistics"] });
  queryClient.invalidateQueries({ queryKey: ["listing", e.route, e.id] });   // details page
  if (isAdmin) queryClient.invalidateQueries({ queryKey: ["admin-ads"] });
});

connection.on("PublicListingChanged", e => {
  // only the feeds that can contain this listing: its module list, its category strip, the home page
  queryClient.invalidateQueries({ queryKey: ["listings", e.route] });
  queryClient.invalidateQueries({ queryKey: ["category", e.categoryId] });
  queryClient.invalidateQueries({ queryKey: ["home-feed"] });
  if (!e.isPublic) queryClient.removeQueries({ queryKey: ["listing", e.route, e.id] });
});

connection.on("ModerationQueueChanged", () => {
  queryClient.invalidateQueries({ queryKey: ["admin-ads"] });
  queryClient.invalidateQueries({ queryKey: ["admin-pending-count"] });
});

connection.onreconnected(() => {
  // events missed while disconnected: refetch what is on screen, once
  queryClient.invalidateQueries({ refetchType: "active" });
});
```

Use the app's real query keys; invalidate, don't `setQueryData`, so pagination and filters are
recomputed by the server (and images arrive with the refetched item).

**Connect anonymously too.** The hub now accepts connections without a token; anonymous connections
receive only `PublicListingChanged`. Today the hook returns early when there is no token — open the
connection for everyone and pass the token only when there is one. Recreate the connection when the
token changes (login/logout) so user-scoped events follow the new identity, and keep the existing
cleanup (`off` + `stop`) to avoid duplicate handlers.

**Fallback** when the connection is down: keep `refetchOnWindowFocus`, and use a modest
`refetchInterval` (e.g. 60 s) on the owner's my-listings and the admin queue **only while the socket is
disconnected**.

---

## 3. Notification clicks

Every listing notification — including the expiry notices — now carries `listingType`, `categoryId`
and `subCategoryId`. Resolve the target from `(categoryId, subCategoryId, referenceId)` (the existing
`notificationTarget` "pair" path) rather than from `deepLink`, whose path (`/restaurants-and-cafes/{id}`)
is an API-style route, not an app route.
