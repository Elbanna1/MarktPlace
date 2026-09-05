using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class WorkshopListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public WorkshopListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Workshop;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Workshops
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(w => ownerId == null || w.UserId == ownerId)
            .Select(w => new UserListingRow
            {
                Id = w.Id,
                OwnerId = w.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Workshop,
                Title = w.AdTitle,
                MainImageUrl = w.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = null,

                Status = w.ModerationStatus == ModerationStatus.Approved
                    ? (w.ExpireAt != null && w.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : w.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : w.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,

                ModerationStatus = w.ModerationStatus,
                RejectionReason = w.RejectionReason,
                ModerationNotes = w.ModerationNotes,
                ModeratedAt = w.ModeratedAt,
                CreatedAt = w.CreatedAt,
                PublishedAt = w.PublishedAt,
                ExpireAt = w.ExpireAt,
                SupportsRepublish = true
            });
}
