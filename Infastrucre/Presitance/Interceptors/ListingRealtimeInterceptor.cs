using System.Runtime.CompilerServices;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Persistence.Listings;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Realtime;
using Shared.Enums;

namespace Persistence.Interceptors;

public sealed class ListingRealtimeInterceptor : SaveChangesInterceptor
{
    private static readonly IReadOnlyDictionary<Type, ListingModuleType> ModulesByEntity =
        ModerationModuleRegistry.EntityTypes
            .Where(entry => entry.Value != typeof(LostFoundPost))
            .ToDictionary(entry => entry.Value, entry => entry.Key);

    private readonly ConditionalWeakTable<DbContext, List<ListingChange>> _pending = new();
    private readonly IReadOnlyList<IListingEventPublisher> _publishers;
    private readonly ILogger<ListingRealtimeInterceptor> _logger;

    public ListingRealtimeInterceptor(
        IEnumerable<IListingEventPublisher> publishers, ILogger<ListingRealtimeInterceptor> logger)
    {
        _publishers = publishers.ToList();
        _logger = logger;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        Capture(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Capture(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        PublishAsync(eventData.Context, CancellationToken.None).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await PublishAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        Discard(eventData.Context);
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        Discard(eventData.Context);
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private void Discard(DbContext? context)
    {
        if (context is not null)
            _pending.Remove(context);
    }

    private void Capture(DbContext? context)
    {
        if (context is null || _publishers.Count == 0)
            return;

        try
        {
            var now = DateTime.UtcNow;
            var changes = new List<ListingChange>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is not IModeratedListing listing ||
                    entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
                {
                    continue;
                }

                if (Describe(entry, listing, now) is { } change)
                    changes.Add(change);
            }

            if (changes.Count == 0)
            {
                _pending.Remove(context);
                return;
            }

            _pending.AddOrUpdate(context, changes);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Could not prepare the real-time listing events for this save.");
            _pending.Remove(context);
        }
    }

    private async Task PublishAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null || !_pending.TryGetValue(context, out var changes))
            return;

        _pending.Remove(context);

        foreach (var publisher in _publishers)
        {
            try
            {
                await publisher.PublishAsync(changes, cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                _logger.LogWarning(exception,
                    "Publishing {Count} real-time listing event(s) failed; the save itself succeeded.",
                    changes.Count);
            }
        }
    }

    private static ListingChange? Describe(EntityEntry entry, IModeratedListing listing, DateTime now)
    {
        if (ModuleOf(entry) is not { } module)
            return null;

        var id = Read<Guid>(entry, "Id", original: false);
        var moderation = listing.ModerationStatus;
        var previousModeration = entry.State == EntityState.Added
            ? moderation
            : Read<ModerationStatus>(entry, nameof(IModeratedListing.ModerationStatus), original: true);

        var expireAt = Read<DateTime?>(entry, "ExpireAt", original: false);
        var previousExpireAt = Read<DateTime?>(entry, "ExpireAt", original: entry.State != EntityState.Added);

        var deleted = entry.State == EntityState.Deleted || IsDeleted(entry, original: false);
        var previouslyDeleted = entry.State != EntityState.Added && IsDeleted(entry, original: true);

        var republishCount = Read<int>(entry, "RepublishCount", original: false);
        var previousRepublishCount = entry.State == EntityState.Added
            ? republishCount
            : Read<int>(entry, "RepublishCount", original: true);

        var isPublic = !deleted && IsPublic(moderation, expireAt, now);
        var wasPublic = entry.State != EntityState.Added && !previouslyDeleted &&
                        IsPublic(previousModeration, previousExpireAt, now);

        var kind = entry.State switch
        {
            EntityState.Added => ListingChangeKinds.Created,
            _ when deleted && !previouslyDeleted => ListingChangeKinds.Deleted,
            _ when moderation != previousModeration => moderation switch
            {
                ModerationStatus.Approved => ListingChangeKinds.Approved,
                ModerationStatus.Rejected => ListingChangeKinds.Rejected,
                ModerationStatus.Suspended => ListingChangeKinds.Suspended,
                _ => ListingChangeKinds.ReturnedToReview
            },
            _ when republishCount > previousRepublishCount => ListingChangeKinds.Republished,
            _ => ListingChangeKinds.Updated
        };

        var (categoryId, subCategoryId) = CategoryOf(entry, module);

        var change = new ListingChangedEventDto
        {
            Id = id,
            Type = module,
            TypeName = module.ToString(),
            Route = ListingModuleCatalog.RouteOf(module),
            CategoryId = categoryId,
            SubCategoryId = subCategoryId,
            Change = kind,
            Status = StatusOf(moderation, expireAt, deleted, now),
            ModerationStatus = moderation,
            IsPublic = isPublic,
            WasPublic = wasPublic,
            PublishedAt = Read<DateTime?>(entry, "PublishedAt", original: false),
            ExpireAt = expireAt,
            RemainingDays = ListingLifecycle.RemainingDays(expireAt, now),
            RepublishCount = republishCount,
            OccurredAt = now
        };

        return new ListingChange(listing.OwnerUserId, change);
    }

    public static bool IsPublic(ModerationStatus moderation, DateTime? expireAt, DateTime now) =>
        moderation == ModerationStatus.Approved && (expireAt is null || expireAt > now);

    public static ListingStatus StatusOf(ModerationStatus moderation, DateTime? expireAt, bool deleted, DateTime now) =>
        moderation switch
        {
            ModerationStatus.Approved when ListingLifecycle.HasExpired(expireAt, now) => ListingStatus.Expired,
            ModerationStatus.Approved => ListingStatus.Active,
            ModerationStatus.Rejected => ListingStatus.Rejected,
            ModerationStatus.Suspended => ListingStatus.Suspended,
            _ => ListingStatus.Pending
        };

    private static ListingModuleType? ModuleOf(EntityEntry entry)
    {
        if (entry.Entity is LostFoundPost post)
            return ListingModuleCatalog.ModuleOf(post.PostType);

        return ModulesByEntity.TryGetValue(entry.Entity.GetType(), out var module) ? module : null;
    }

    private static (int? CategoryId, int? SubCategoryId) CategoryOf(EntityEntry entry, ListingModuleType module)
    {
        if (entry.Metadata.FindProperty("CategoryId") is not null &&
            entry.Metadata.FindProperty("SubCategoryId") is not null &&
            entry.Property("CategoryId").CurrentValue is int categoryId &&
            entry.Property("SubCategoryId").CurrentValue is int subCategoryId)
        {
            return (categoryId, subCategoryId);
        }

        return ((int?)ListingModuleCatalog.CategoryOf(module), (int?)ListingModuleCatalog.SubCategoryOf(module));
    }

    private static bool IsDeleted(EntityEntry entry, bool original)
    {
        if (entry.Metadata.FindProperty("IsDeleted") is not null)
            return Read<bool>(entry, "IsDeleted", original);

        if (entry.Entity is Advertisement)
            return Read<AdvertisementStatus>(entry, nameof(Advertisement.Status), original) == AdvertisementStatus.Deleted;

        return false;
    }

    private static T Read<T>(EntityEntry entry, string property, bool original)
    {
        if (entry.Metadata.FindProperty(property) is null)
            return default!;

        var value = original ? entry.Property(property).OriginalValue : entry.Property(property).CurrentValue;
        return value is T typed ? typed : default!;
    }
}
