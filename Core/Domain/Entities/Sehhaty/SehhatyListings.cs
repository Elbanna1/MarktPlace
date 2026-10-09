using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public abstract class SehhatyListing : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMapsUrl { get; set; }

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public int ViewCount { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public DateTime? FirstPublishedAt { get; set; }

    public int RepublishCount { get; set; }

    public ModerationStatus ModerationStatus { get; set; } = ModerationStatus.Pending;

    public DateTime? ModeratedAt { get; set; }

    public string? ModeratedBy { get; set; }

    public ListingRejectionReason? RejectionReason { get; set; }

    public string? ModerationNotes { get; set; }

    [NotMapped]
    public string OwnerUserId => UserId;

    [NotMapped]
    public string ListingTitle => Title;
}

public abstract class SehhatyListing<TImage> : SehhatyListing
    where TImage : SehhatyListingImage
{
    public ICollection<TImage> Images { get; set; } = new List<TImage>();
}

public abstract class SehhatyListingImage : IOrderedListingImage
{
    public Guid Id { get; set; }

    public Guid ListingId { get; set; }

    public string FileName { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Doctor : SehhatyListing<DoctorImage>
{
    public DoctorSpecialty Specialty { get; set; }

    public string? OtherSpecialty { get; set; }
}

public class DoctorImage : SehhatyListingImage
{
    public Doctor Listing { get; set; } = default!;
}

public class MedicalLaboratory : SehhatyListing<MedicalLaboratoryImage>;

public class MedicalLaboratoryImage : SehhatyListingImage
{
    public MedicalLaboratory Listing { get; set; } = default!;
}

public class DentalLaboratory : SehhatyListing<DentalLaboratoryImage>;

public class DentalLaboratoryImage : SehhatyListingImage
{
    public DentalLaboratory Listing { get; set; } = default!;
}

public class RadiologyLaboratory : SehhatyListing<RadiologyLaboratoryImage>;

public class RadiologyLaboratoryImage : SehhatyListingImage
{
    public RadiologyLaboratory Listing { get; set; } = default!;
}

public class MedicalCenter : SehhatyListing<MedicalCenterImage>;

public class MedicalCenterImage : SehhatyListingImage
{
    public MedicalCenter Listing { get; set; } = default!;
}

public class Hospital : SehhatyListing<HospitalImage>;

public class HospitalImage : SehhatyListingImage
{
    public Hospital Listing { get; set; } = default!;
}
