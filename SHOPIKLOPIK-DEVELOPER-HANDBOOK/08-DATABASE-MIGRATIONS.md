# 08 — Database Migrations

[← Start Here](00-START-HERE.md) · Related: [07-DATABASE](07-DATABASE.md) ·
[26-DEPLOYMENT](26-DEPLOYMENT.md) · [31-ROLLBACK](31-ROLLBACK.md)

---

## Strategy in one paragraph

Code-first EF Core migrations live in `Infastrucre/Presitance/Migrations/` (**60** of them). They are
applied **automatically at application start-up** by `await db.Database.MigrateAsync()` in
`Program.cs`. There is no separate migration step in the deployment procedure, and no manual
`dotnet ef database update` in production.

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await IdentityDataSeeder.SeedAsync(scope.ServiceProvider, app.Configuration);
    // … demo data, Development only …
}
```

### What that implies

| Implication | Consequence |
| --- | --- |
| The first request after a deployment waits for migrations | a long migration = a slow first request |
| A failing migration **stops the application from starting** | it will never serve traffic in a half-migrated state — this is the desirable behaviour |
| Two instances starting at once can race | deploy one instance first, or apply migrations out of band |
| Rolling back code does **not** roll back the database | see [31-ROLLBACK.md](31-ROLLBACK.md) |

---

## Creating a migration

```bash
# from the repository root
dotnet ef migrations add <DescriptiveName> \
  --project Infastrucre/Presitance/Presitance.csproj \
  --startup-project MarkatPlace/MarkatPlace.csproj
```

Then, **before doing anything else**:

```bash
dotnet build          # required, or start-up reports a false PendingModelChanges
```

### ⚠️ Always read the generated migration

This is the single most damaging trap in this repository.

> **EF Core turns an unrelated drop + add pair into `RenameColumn`.**
>
> If a migration drops `Foo` and adds `Bar` on the same table, the scaffolder may decide you renamed
> `Foo` to `Bar` and emit `migrationBuilder.RenameColumn(...)`. The migration then **carries the old
> column's data into the new column**, silently reinterpreting it. Nothing fails; the data is just
> wrong.

Read every scaffolded migration and check:

- [ ] no unexpected `RenameColumn` / `RenameTable`
- [ ] no `DropColumn` you did not intend
- [ ] `Up()` and `Down()` are symmetric
- [ ] a new non-nullable column has a default, or the table is empty
- [ ] new indexes carry the `INCLUDE` columns you expect
- [ ] `HasData` changes affect only reference data

If EF got it wrong, hand-edit the migration to do the explicit thing (add the new column, copy with
SQL if needed, drop the old one).

### Naming

Existing names are inconsistent — you will find `AddLostFoundModule` and `AddAdminRolesAndPermissions`
alongside `FixBugs`, `FixBulpp`, `TheFianlDop` and `EditsWbaugs`. **Follow the good examples.** A
migration name is the only label a future maintainer has when reading `__EFMigrationsHistory`.

---

## Applying a migration manually

```bash
dotnet ef database update \
  --project Infastrucre/Presitance/Presitance.csproj \
  --startup-project MarkatPlace/MarkatPlace.csproj
```

Design-time commands can also run without the host via `AppDbContextFactory`, which reads the
environment variable **`MARKATPLACE_CONNECTION`** (not `ConnectionStrings__DefaultConnection`) and
falls back to `Server=.;Database=MarkatPlaceDb;…`.

### Generating a script instead of executing

For a production change you want to inspect first:

```bash
dotnet ef migrations script <FromMigration> <ToMigration> \
  --project Infastrucre/Presitance/Presitance.csproj \
  --startup-project MarkatPlace/MarkatPlace.csproj \
  --idempotent --output migration.sql
```

`--idempotent` produces a script safe to run against a database at any intermediate state.

---

## Reverting

```bash
# revert the database to a previous migration
dotnet ef database update <PreviousMigrationName> --project … --startup-project …

# remove the last (not yet applied) migration from the code
dotnet ef migrations remove --project … --startup-project …
```

`migrations remove` only works if the migration has **not** been applied to the database the tools
connect to.

---

## The migration history table

EF tracks applied migrations in `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`).

### `fix-migration-history.sql`

A one-off baselining script at the repository root. It exists because a database once had the schema
of the initial migration but no `__EFMigrationsHistory` row for it, so EF tried to create tables that
already existed.

What it does:

1. **Aborts** if `AspNetRoles` does not exist — i.e. refuses to run on a fresh database, where you
   should just run the migrations normally.
2. Creates `__EFMigrationsHistory` if it is missing.
3. Inserts the row for `20260728074202_inital` if it is absent.
4. Prints the resulting history, newest first.

Run it **only** against an existing deployment whose schema is genuinely ahead of its history table,
and only after taking a backup.

---

## The 60 migrations, chronologically

| Period | Migrations | What was being built |
| --- | --- | --- |
| 2026-07-28 → 07-29 | `intialCreate`, `AddClothingCategoryModules`, `AddNotificationPresentationFields`, `lastUploadUpdates` | foundations, Clothing, notifications |
| 2026-08-06 → 08-08 | `AddOnlineShoppingCategoryModules`, `AddHomeFurnishingCategoryModules`, `AddRealEstateCategoryModules`, `RebuildCarsCategoryModule`, `AddListingInteractions`, `AddBannerModule`, `AddFeedbackModule` | four more categories, interactions, banners |
| 2026-08-09 → 08-15 | `AddBannerBookingModule`, `AddListingModerationSystem`, `AddAdminDashboardV2`, `AddNotificationInterestsModule`, `AddAdminAuditLog`, `AddNotificationListingContext`, `AddListingPublicationWindow` | **moderation**, admin V2, publication window |
| 2026-08-19 → 08-25 | shopping field removals, vehicle tweaks, `AddListingRatings`, `AddReferralSystem`, real-estate advertiser name, `AddCharityModules` | ratings, referrals, Charity |
| 2026-08-28 → 09-02 | nullable car window, apartment price/m², configurable banner duration, comment `UpdatedAt`, `AddAdminRolesAndPermissions`, optional job center, five index migrations | admin permissions, performance indexes |
| 2026-09-04 → 09-05 | `AddLandSearchCoveringIndex`, `Producation` | Land search, production prep |
| 2026-09-23 | `AddListingSortIndexes`, `AddAdvertiserLookupIndexes` | sort indexes, advertiser lookup |

Reading them in order is a surprisingly good way to learn the domain.

---

## Rules for new migrations

1. **Read the scaffolded file.** Every time.
2. **Additive first.** Add a nullable column, deploy, backfill, then tighten — three deployments
   beat one outage.
3. **Never edit an applied migration.** Write a new one.
4. **Never delete a migration** that has run anywhere.
5. **`HasData` is for reference data only.** Never seed user data.
6. **Check `Down()`.** You may need it at 2 a.m.
7. **Build before you run**, or you get a false `PendingModelChanges` error.
8. **Take a backup before anything destructive** in production.

---

## Migration failure modes

| Symptom | Cause | Fix |
| --- | --- | --- |
| Start-up throws `PendingModelChanges` | stale build after `migrations add` | `dotnet build`, rerun |
| `There is already an object named 'X'` | schema exists, history row missing | baseline — see `fix-migration-history.sql` |
| `Introducing FOREIGN KEY constraint … may cause cycles or multiple cascade paths` (error 1785) | a cascade from `AspNetUsers` | use `OnDelete(DeleteBehavior.NoAction)` for the user FK |
| A migration hangs | a lock from another connection, or a long table rebuild | check `sys.dm_exec_requests`; never kill a partially applied migration without a plan |
| The app starts, but data is subtly wrong after a deploy | an unnoticed `RenameColumn` | restore from backup, write a corrective migration |
| Scaffolding produces a huge unrelated diff | model snapshot drift | verify you are on the right branch and have built; never hand-edit `AppDbContextModelSnapshot.cs` |
