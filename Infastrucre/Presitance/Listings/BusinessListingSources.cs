using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class FactoryListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public FactoryListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Factory;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Factories
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(f => ownerId == null || f.UserId == ownerId)
            .Select(f => new UserListingRow
            {
                Id = f.Id,
                OwnerId = f.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Factory,
                Title = f.Title,
                MainImageUrl = f.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = null,

                Status = f.ModerationStatus == ModerationStatus.Approved
                    ? (f.ExpireAt != null && f.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : f.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : f.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,

                ModerationStatus = f.ModerationStatus,
                RejectionReason = f.RejectionReason,
                ModerationNotes = f.ModerationNotes,
                ModeratedAt = f.ModeratedAt,
                CreatedAt = f.CreatedAt,
                PublishedAt = f.PublishedAt,
                ExpireAt = f.ExpireAt,
                SupportsRepublish = true
            });
}

public sealed class FarmListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public FarmListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Farm;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Farms
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(f => ownerId == null || f.UserId == ownerId)
            .Select(f => new UserListingRow
            {
                Id = f.Id,
                OwnerId = f.UserId,
                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Farm,
                Title = f.Title,
                MainImageUrl = f.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = null,

                Status = f.ModerationStatus == ModerationStatus.Approved
                    ? (f.ExpireAt != null && f.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : f.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : f.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,

                ModerationStatus = f.ModerationStatus,
                RejectionReason = f.RejectionReason,
                ModerationNotes = f.ModerationNotes,
                ModeratedAt = f.ModeratedAt,
                CreatedAt = f.CreatedAt,
                PublishedAt = f.PublishedAt,
                ExpireAt = f.ExpireAt,
                SupportsRepublish = true
            });
}

public sealed class CompanyListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public CompanyListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Company;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Companies
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(c => ownerId == null || c.UserId == ownerId)
            .Select(c => new UserListingRow
            {
                Id = c.Id,
                OwnerId = c.UserId,
                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Company,
                Title = c.Title,

                MainImageUrl = c.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault() ?? c.LogoUrl,
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
