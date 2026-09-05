using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class WholesaleTraderListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public WholesaleTraderListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.WholesaleTrader;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.WholesaleTraders
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(t => ownerId == null || t.UserId == ownerId)
            .Select(t => new UserListingRow
            {
                Id = t.Id,
                OwnerId = t.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.WholesaleTrader,
                Title = t.Title,
                MainImageUrl = t.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = null,

                Status = t.ModerationStatus == ModerationStatus.Approved
                    ? (t.ExpireAt != null && t.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : t.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : t.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,

                ModerationStatus = t.ModerationStatus,
                RejectionReason = t.RejectionReason,
                ModerationNotes = t.ModerationNotes,
                ModeratedAt = t.ModeratedAt,
                CreatedAt = t.CreatedAt,
                PublishedAt = t.PublishedAt,
                ExpireAt = t.ExpireAt,
                SupportsRepublish = true
            });
}
