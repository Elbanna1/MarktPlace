using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class LandListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public LandListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Land;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Lands
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Land,
                Title = x.Title,
                MainImageUrl = x.Images
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = x.TotalPrice,

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

public sealed class ApartmentListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public ApartmentListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Apartment;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Apartments
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Apartment,
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

public sealed class ShopListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public ShopListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Shop;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Shops
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Shop,
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
