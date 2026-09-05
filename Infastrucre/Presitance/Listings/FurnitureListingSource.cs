using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class FurnitureListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public FurnitureListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Furniture;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Furnitures
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Furniture,
                Title = x.Title,
                MainImageUrl = x.Images
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = x.Price,

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
                SupportsRepublish = true
            });
}
