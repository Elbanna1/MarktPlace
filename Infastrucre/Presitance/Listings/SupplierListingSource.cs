using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public sealed class SupplierListingSource : IUserListingSource
{
    private readonly AppDbContext _context;

    public SupplierListingSource(AppDbContext context)
    {
        _context = context;
    }

    public ListingModuleType Type => ListingModuleType.Supplier;

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false) =>
        _context.Suppliers
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(s => ownerId == null || s.UserId == ownerId)
            .Select(s => new UserListingRow
            {
                Id = s.Id,
                OwnerId = s.UserId,

                CategoryId = null,
                SubCategoryId = null,
                Type = ListingModuleType.Supplier,
                Title = s.Title,
                MainImageUrl = s.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                Price = null,

                Status = s.ModerationStatus == ModerationStatus.Approved
                    ? (s.ExpireAt != null && s.ExpireAt <= utcNow
                        ? ListingStatus.Expired
                        : ListingStatus.Active)
                    : s.ModerationStatus == ModerationStatus.Rejected
                        ? ListingStatus.Rejected
                        : s.ModerationStatus == ModerationStatus.Suspended
                            ? ListingStatus.Suspended
                            : ListingStatus.Pending,

                ModerationStatus = s.ModerationStatus,
                RejectionReason = s.RejectionReason,
                ModerationNotes = s.ModerationNotes,
                ModeratedAt = s.ModeratedAt,
                CreatedAt = s.CreatedAt,
                PublishedAt = s.PublishedAt,
                ExpireAt = s.ExpireAt,
                SupportsRepublish = true
            });
}
