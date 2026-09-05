using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class DecorAntiqueListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public DecorAntiqueListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.DecorAntique;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.DecorAntiques
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.DecorAntique,
                Title = x.Title,
                MainImageUrl = x.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
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

public sealed class AntiqueListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public AntiqueListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Antique;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Antiques
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Antique,
                Title = x.Title,
                MainImageUrl = x.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
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

public sealed class PaintingListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public PaintingListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Painting;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Paintings
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Painting,
                Title = x.Title,
                MainImageUrl = x.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
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

public sealed class HandmadeListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public HandmadeListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Handmade;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Handmades
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Handmade,
                Title = x.Title,
                MainImageUrl = x.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
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

public sealed class CoinStampListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public CoinStampListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.CoinStamp;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.CoinStamps
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.CoinStamp,
                Title = x.Title,
                MainImageUrl = x.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
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
