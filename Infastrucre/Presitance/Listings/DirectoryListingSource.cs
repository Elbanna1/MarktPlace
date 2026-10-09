using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public abstract class DirectoryListingSource<TListing, TImage> : IUserListingSource
    where TListing : DirectoryListing<TImage>
    where TImage : DirectoryListingImage
{
    private readonly AppDbContext _context;

    protected DirectoryListingSource(AppDbContext context)
    {
        _context = context;
    }

    public abstract ListingModuleType Type { get; }

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false)
    {
        var type = Type;

        return _context.Set<TListing>()
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,
                CategoryId = null,
                SubCategoryId = null,
                Type = type,
                Title = x.Title,
                MainImageUrl = x.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.SortOrder)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = null,
                Status = x.ModerationStatus == ModerationStatus.Approved
                    ? (x.ExpireAt != null && x.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : x.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : x.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,
                ModerationStatus = x.ModerationStatus,
                RejectionReason = x.RejectionReason,
                ModerationNotes = x.ModerationNotes,
                ModeratedAt = x.ModeratedAt,
                CreatedAt = x.CreatedAt,
                PublishedAt = x.PublishedAt,
                ExpireAt = x.ExpireAt,
                RepublishCount = x.RepublishCount,
                SupportsRepublish = true
            });
    }
}
