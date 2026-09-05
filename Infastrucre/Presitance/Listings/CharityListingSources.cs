using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class RescueListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public RescueListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Rescue;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Rescues
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,
                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Rescue,
                Title = x.RescuerName,
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
                SupportsRepublish = true
            });
}

public sealed class BloodRequestListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public BloodRequestListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.BloodRequest;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.BloodRequests
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,
                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.BloodRequest,
                Title = x.RequesterName,
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
                SupportsRepublish = true
            });
}

public sealed class AskConsultListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public AskConsultListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.AskConsult;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.AskConsults
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,
                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.AskConsult,
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
                SupportsRepublish = true
            });
}
