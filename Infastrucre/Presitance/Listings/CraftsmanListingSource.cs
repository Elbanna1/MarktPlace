using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class CraftsmanListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public CraftsmanListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Craftsman;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Craftsmen
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(c => ownerId == null || c.UserId == ownerId)
            .Select(c => new UserListingRow
            {
                Id = c.Id,
                OwnerId = c.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Craftsman,
                Title = c.AdTitle,
                MainImageUrl = c.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = null,

                Status = c.ModerationStatus == ModerationStatus.Approved
                    ? (c.ExpireAt != null && c.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : c.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : c.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,

                ModerationStatus = c.ModerationStatus,
                RejectionReason = c.RejectionReason,
                ModerationNotes = c.ModerationNotes,
                ModeratedAt = c.ModeratedAt,
                CreatedAt = c.CreatedAt,
                PublishedAt = c.PublishedAt,
                ExpireAt = c.ExpireAt,
                SupportsRepublish = true
            });
}
