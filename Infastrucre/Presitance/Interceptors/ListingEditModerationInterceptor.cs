using System.Collections.Concurrent;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Persistence.Listings;
using ServicesAbstraction;
using Shared.Enums;

namespace Persistence.Interceptors;

public sealed class ListingEditModerationInterceptor : SaveChangesInterceptor
{
    private static readonly HashSet<string> ContributorKeys = new(StringComparer.Ordinal)
    {
        "UserId", "ViewerUserId", "ReviewerUserId", "ReporterUserId"
    };

    private static readonly ConcurrentDictionary<IModel, IReadOnlyDictionary<IEntityType, ChildLink?>> ChildLinks =
        new();

    private readonly IListingEditReviewQueue _queue;
    private readonly IAdminActionContext _actor;
    private readonly ILogger<ListingEditModerationInterceptor> _logger;

    public ListingEditModerationInterceptor(
        IListingEditReviewQueue queue,
        IAdminActionContext actor,
        ILogger<ListingEditModerationInterceptor> logger)
    {
        _queue = queue;
        _actor = actor;
        _logger = logger;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        Apply(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Apply(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void Apply(DbContext? context)
    {
        if (context is null)
            return;

        if (_actor.IsAdmin)
            return;

        try
        {
            var entries = context.ChangeTracker.Entries().ToList();

            if (entries.Count == 0)
                return;

            var links = LinksFor(context.Model);

            var touched = new HashSet<IModeratedListing>();

            foreach (var entry in entries)
            {
                if (entry.Entity is IModeratedListing listing)
                {
                    if (entry.State == EntityState.Modified && HasContentChange(entry))
                        touched.Add(listing);

                    continue;
                }

                if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
                    continue;

                if (!links.TryGetValue(entry.Metadata, out var link) || link is null)
                    continue;

                if (entry.State == EntityState.Modified && !HasContentChange(entry))
                    continue;

                if (Parent(entry, link) is { } parent)
                    touched.Add(parent);
            }

            foreach (var listing in touched)
                Queue(context, listing);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                "Deciding whether an edit needs re-review failed; the save was left untouched.");
        }
    }

    private static bool HasContentChange(EntityEntry entry)
    {
        foreach (var property in entry.Properties)
        {
            if (ListingEditReview.SystemOwnedProperties.Contains(property.Metadata.Name))
                continue;

            if (!Equals(property.OriginalValue, property.CurrentValue))
                return true;
        }

        return false;
    }

    private static IModeratedListing? Parent(EntityEntry entry, ChildLink link)
    {
        if (link.Navigation is { } navigationName &&
            entry.Metadata.FindNavigation(navigationName) is not null &&
            entry.Reference(navigationName).CurrentValue is IModeratedListing loaded)
        {
            return loaded;
        }

        var keyValues = new object?[link.ForeignKeyProperties.Count];

        for (var i = 0; i < keyValues.Length; i++)
        {
            var property = entry.Property(link.ForeignKeyProperties[i]);

            keyValues[i] = entry.State == EntityState.Deleted
                ? property.OriginalValue ?? property.CurrentValue
                : property.CurrentValue ?? property.OriginalValue;

            if (keyValues[i] is null)
                return null;
        }

        var found = entry.Context.ChangeTracker
            .Entries()
            .FirstOrDefault(candidate =>
                candidate.Metadata.ClrType == link.PrincipalClrType &&
                MatchesKey(candidate, link.PrincipalKeyProperties, keyValues));

        return found?.Entity as IModeratedListing;
    }

    private static bool MatchesKey(
        EntityEntry candidate, IReadOnlyList<string> keyProperties, IReadOnlyList<object?> values)
    {
        for (var i = 0; i < keyProperties.Count; i++)
        {
            if (!Equals(candidate.Property(keyProperties[i]).CurrentValue, values[i]))
                return false;
        }

        return true;
    }

    private void Queue(DbContext context, IModeratedListing listing)
    {
        if (!ListingEditReview.SendBackToReview(listing))
            return;

        if (ModuleOf(listing) is not { } module)
            return;

        var id = context.Entry(listing).Property<Guid>("Id").CurrentValue;

        _queue.Enqueue(new ListingEditReviewEntry(
            module, id, listing.OwnerUserId, listing.ListingTitle, DateTime.UtcNow));

        _logger.LogInformation(
            "{Module} {ListingId} was edited by its owner and returned to the review queue.",
            module, id);
    }

    private static ListingModuleType? ModuleOf(IModeratedListing listing)
    {
        if (listing is LostFoundPost post)
            return post.PostType == PostType.Found ? ListingModuleType.FoundItem : ListingModuleType.LostItem;

        var clrType = listing.GetType();

        foreach (var pair in ModerationModuleRegistry.EntityTypes)
        {
            if (pair.Value == clrType)
                return pair.Key;
        }

        return null;
    }

    private sealed record ChildLink(
        Type PrincipalClrType,
        string? Navigation,
        IReadOnlyList<string> ForeignKeyProperties,
        IReadOnlyList<string> PrincipalKeyProperties);

    private static IReadOnlyDictionary<IEntityType, ChildLink?> LinksFor(IModel model) =>
        ChildLinks.GetOrAdd(model, Classify);

    private static IReadOnlyDictionary<IEntityType, ChildLink?> Classify(IModel model)
    {
        var map = new Dictionary<IEntityType, ChildLink?>();

        foreach (var entityType in model.GetEntityTypes())
        {
            if (typeof(IModeratedListing).IsAssignableFrom(entityType.ClrType))
            {
                map[entityType] = null;
                continue;
            }

            map[entityType] = LinkOf(entityType);
        }

        return map;
    }

    private static ChildLink? LinkOf(IEntityType entityType)
    {
        if (entityType.GetProperties().Any(p => ContributorKeys.Contains(p.Name)))
            return null;

        if (entityType.GetForeignKeys().Any(fk => fk.PrincipalEntityType.ClrType == typeof(ApplicationUser)))
            return null;

        foreach (var foreignKey in entityType.GetForeignKeys())
        {
            if (!typeof(IModeratedListing).IsAssignableFrom(foreignKey.PrincipalEntityType.ClrType))
                continue;

            return new ChildLink(
                foreignKey.PrincipalEntityType.ClrType,
                foreignKey.DependentToPrincipal?.Name,
                foreignKey.Properties.Select(p => p.Name).ToList(),
                foreignKey.PrincipalKey.Properties.Select(p => p.Name).ToList());
        }

        return null;
    }
}
