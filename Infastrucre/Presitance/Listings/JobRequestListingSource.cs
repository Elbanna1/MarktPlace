using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class JobRequestListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public JobRequestListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.JobRequest;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.JobRequests
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(j => ownerId == null || j.UserId == ownerId)
            .Select(j => new UserListingRow
            {
                Id = j.Id,
                OwnerId = j.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.JobRequest,
                Title = j.Title,
                MainImageUrl = j.ProfileImageUrl,
                Price = null,

                Status = j.ModerationStatus == ModerationStatus.Approved
                    ? (j.ExpireAt != null && j.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : j.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : j.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,

                ModerationStatus = j.ModerationStatus,
                RejectionReason = j.RejectionReason,
                ModerationNotes = j.ModerationNotes,
                ModeratedAt = j.ModeratedAt,
                CreatedAt = j.CreatedAt,
                PublishedAt = j.PublishedAt,
                ExpireAt = j.ExpireAt,
                SupportsRepublish = true
            });
}
