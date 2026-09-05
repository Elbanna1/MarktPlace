using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class FruitVegetableMerchantListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public FruitVegetableMerchantListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.FruitVegetableMerchant;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.FruitVegetableMerchants
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(m => ownerId == null || m.UserId == ownerId)
            .Select(m => new UserListingRow
            {
                Id = m.Id,
                OwnerId = m.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.FruitVegetableMerchant,
                Title = m.Title,
                MainImageUrl = m.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = null,

                Status = m.ModerationStatus == ModerationStatus.Approved
                    ? (m.ExpireAt != null && m.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : m.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : m.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,

                ModerationStatus = m.ModerationStatus,
                RejectionReason = m.RejectionReason,
                ModerationNotes = m.ModerationNotes,
                ModeratedAt = m.ModeratedAt,
                CreatedAt = m.CreatedAt,
                PublishedAt = m.PublishedAt,
                ExpireAt = m.ExpireAt,
                SupportsRepublish = true
            });
}
