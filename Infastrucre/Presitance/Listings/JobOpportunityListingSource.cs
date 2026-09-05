using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class JobOpportunityListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public JobOpportunityListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.JobOpportunity;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.JobOpportunities
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(j => ownerId == null || j.UserId == ownerId)
            .Select(j => new UserListingRow
            {
                Id = j.Id,
                OwnerId = j.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.JobOpportunity,
                Title = j.Title,
                MainImageUrl = j.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault() ?? j.LogoUrl,
                Price = j.Salary,

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
