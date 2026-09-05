using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class LostItemListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public LostItemListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.LostItem;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        LostFoundListingQuery.Build(_context, PostType.Lost, ListingModuleType.LostItem, ownerId, utcNow, includeUnmoderated);
}

public sealed class FoundItemListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public FoundItemListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.FoundItem;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        LostFoundListingQuery.Build(_context, PostType.Found, ListingModuleType.FoundItem, ownerId, utcNow, includeUnmoderated);
}

internal static class LostFoundListingQuery
{
    public static IQueryable<UserListingRow> Build(
        AppDbContext context, PostType postType, ListingModuleType module, string? ownerId,
        DateTime utcNow, bool includeUnmoderated) =>
        context.LostFoundPosts
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(p => p.PostType == postType)
            .Where(p => ownerId == null || p.UserId == ownerId)
            .Select(p => new UserListingRow
            {
                Id = p.Id,
                OwnerId = p.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = module,
                Title = p.ItemName,
                MainImageUrl = p.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = null,

                Status = p.ModerationStatus == ModerationStatus.Approved
                    ? (p.ExpireAt != null && p.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : p.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : p.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,

                ModerationStatus = p.ModerationStatus,
                RejectionReason = p.RejectionReason,
                ModerationNotes = p.ModerationNotes,
                ModeratedAt = p.ModeratedAt,
                CreatedAt = p.CreatedAt,
                PublishedAt = p.PublishedAt,
                ExpireAt = p.ExpireAt,
                SupportsRepublish = true
            });
}
