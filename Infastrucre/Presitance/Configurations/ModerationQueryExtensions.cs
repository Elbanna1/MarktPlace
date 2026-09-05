using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace Persistence.Configurations;

public static class ModerationQueryExtensions
{
    public static IQueryable<TEntity> IncludingUnmoderated<TEntity>(this IQueryable<TEntity> query)
        where TEntity : class, IModeratedListing =>
        query.IgnoreQueryFilters([ModerationModelConfiguration.FilterName]);

    public static IQueryable<TEntity> IncludingUnmoderatedIf<TEntity>(
        this IQueryable<TEntity> query, bool include)
        where TEntity : class, IModeratedListing =>
        include ? query.IncludingUnmoderated() : query;

    public static IQueryable<TEntity> VisibleToViewer<TEntity>(
        this IQueryable<TEntity> query, bool includeUnmoderated, string? viewerUserId)
        where TEntity : class, IModeratedListing, IExpiringListing
    {
        if (includeUnmoderated)
            return query.IncludingUnmoderated();

        if (string.IsNullOrEmpty(viewerUserId))
            return query;

        return query
            .IncludingUnmoderated()
            .Where(x =>
                (x.ModerationStatus == ModerationStatus.Approved &&
                 (x.ExpireAt == null || x.ExpireAt > DateTime.UtcNow))
                || EF.Property<string>(x, OwnerUserIdProperty) == viewerUserId);
    }

    public const string OwnerUserIdProperty = "UserId";

    public static IQueryable<TChild> IncludingUnmoderatedParent<TChild>(this IQueryable<TChild> query)
        where TChild : class =>
        query.IgnoreQueryFilters([ModerationModelConfiguration.FilterName]);
}
