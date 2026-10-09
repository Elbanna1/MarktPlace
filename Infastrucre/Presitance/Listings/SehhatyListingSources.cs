using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Data;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Listings;

public abstract class SehhatyListingSource<TListing, TImage> : IUserListingSource
    where TListing : SehhatyListing<TImage>
    where TImage : SehhatyListingImage
{
    private readonly AppDbContext _context;

    protected SehhatyListingSource(AppDbContext context)
    {
        _context = context;
    }

    public abstract ListingModuleType Type { get; }

    public IQueryable<UserListingRow> Query(string? ownerId, DateTime utcNow, bool includeUnmoderated = false)
    {
        var type = Type;

        return _context.Set<TListing>()
            .AsNoTracking()
            .IncludingUnmoderatedIf(ownerId is not null || includeUnmoderated)
            .Where(x => ownerId == null || x.UserId == ownerId)
            .Select(x => new UserListingRow
            {
                Id = x.Id,
                OwnerId = x.UserId,
                CategoryId = null,
                SubCategoryId = null,
                Type = type,
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
}

public sealed class DoctorListingSource(AppDbContext context)
    : SehhatyListingSource<Doctor, DoctorImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.Doctor;
}

public sealed class MedicalLaboratoryListingSource(AppDbContext context)
    : SehhatyListingSource<MedicalLaboratory, MedicalLaboratoryImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.MedicalLaboratory;
}

public sealed class DentalLaboratoryListingSource(AppDbContext context)
    : SehhatyListingSource<DentalLaboratory, DentalLaboratoryImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.DentalLaboratory;
}

public sealed class RadiologyLaboratoryListingSource(AppDbContext context)
    : SehhatyListingSource<RadiologyLaboratory, RadiologyLaboratoryImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.RadiologyLaboratory;
}

public sealed class MedicalCenterListingSource(AppDbContext context)
    : SehhatyListingSource<MedicalCenter, MedicalCenterImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.MedicalCenter;
}

public sealed class HospitalListingSource(AppDbContext context)
    : SehhatyListingSource<Hospital, HospitalImage>(context)
{
    public override ListingModuleType Type => ListingModuleType.Hospital;
}
