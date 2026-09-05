using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class AdvertisementListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public AdvertisementListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Advertisement;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Advertisements
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(a => (ownerId == null || a.OwnerId == ownerId)
                        && a.Status != AdvertisementStatus.Deleted
                        && a.DeletedAt == null)
            .Select(a => new UserListingRow
            {
                Id = a.Id,
                OwnerId = a.OwnerId,

                CategoryId = a.CategoryId,
                SubCategoryId = a.SubCategoryId,
                Type = ListingModuleType.Advertisement,
                Title = a.Title,

                MainImageUrl = a.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = a.Price,

                Status = a.ModerationStatus == ModerationStatus.Rejected
                    ? ListingStatus.Rejected
                    : a.ModerationStatus == ModerationStatus.Suspended
                        ? ListingStatus.Suspended
                        : a.ModerationStatus == ModerationStatus.Pending
                            ? ListingStatus.Pending
                            : a.Status == AdvertisementStatus.Pending
                                ? ListingStatus.Pending
                                : a.Status == AdvertisementStatus.Expired || a.ExpireAt <= utcNow
                                    ? ListingStatus.Expired
                                    : ListingStatus.Active,

                ModerationStatus = a.ModerationStatus,
                RejectionReason = a.RejectionReason,
                ModerationNotes = a.ModerationNotes,
                ModeratedAt = a.ModeratedAt,
                CreatedAt = a.CreatedAt,
                PublishedAt = a.PublishedAt,
                ExpireAt = a.ExpireAt,
                SupportsRepublish = true
            });
}
