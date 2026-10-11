using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class Building : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string AdvertiserName { get; set; } = default!;

    public RealEstateListingType ListingType { get; set; }

    public BuildingType BuildingType { get; set; }

    public decimal? Price { get; set; }

    public decimal? PricePerMeter { get; set; }

    public decimal Area { get; set; }

    public ApartmentFurnishedStatus FurnishedStatus { get; set; }

    public ApartmentFinishingType FinishingType { get; set; }

    public ApartmentPropertyAge PropertyAge { get; set; }

    public ApartmentDirection Direction { get; set; }

    public ApartmentViewType? ViewType { get; set; }

    public decimal? GardenArea { get; set; }

    public int? FloorsCount { get; set; }

    public int? BathroomsCount { get; set; }

    public ApartmentReceptionPieces? ReceptionPieces { get; set; }

    public int? RoomsCount { get; set; }

    public bool? HasSwimmingPool { get; set; }

    public int? ResidentialUnitsCount { get; set; }

    public bool? HasIndependentEntrance { get; set; }

    public int? ApartmentsCount { get; set; }

    public int? ShopsCount { get; set; }

    public bool? HasElevator { get; set; }

    public bool? HasGarage { get; set; }

    public ApartmentLegalStatus LegalStatus { get; set; }

    public string? LicenseNumber { get; set; }

    public DateTime? LicenseIssueDate { get; set; }

    public DateTime? LicenseExpiryDate { get; set; }

    public string? LicenseIssuer { get; set; }

    public ApartmentReconciliationForm? ReconciliationForm { get; set; }

    public ApartmentOwnershipDocument OwnershipDocument { get; set; }

    public bool? IsRegistered { get; set; }

    public bool? HasViolations { get; set; }

    public string? ViolationDetails { get; set; }

    public string Governorate { get; set; } = default!;

    public string Center { get; set; } = default!;

    public RealEstateProject? Project { get; set; }

    public string? OtherProject { get; set; }

    public string? District { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public bool Negotiable { get; set; }

    public string? Notes { get; set; }

    public ApartmentPaymentMethod? PaymentMethod { get; set; }

    public decimal? DownPayment { get; set; }

    public decimal? InstallmentAmount { get; set; }

    public string? InstallmentPeriod { get; set; }

    public ApartmentInstallmentProvider? InstallmentProvider { get; set; }

    public bool? HasMaintenanceDeposit { get; set; }

    public decimal? MaintenanceDepositAmount { get; set; }

    public decimal? MonthlyFees { get; set; }

    public ApartmentRentType? RentType { get; set; }

    public decimal? RentValue { get; set; }

    public decimal? SecurityDeposit { get; set; }

    public decimal? RentDownPayment { get; set; }

    public int? MinimumRentPeriod { get; set; }

    public DateTime? AvailableFrom { get; set; }

    public DateTime? AvailableTo { get; set; }

    public string? OwnerConditions { get; set; }

    public ApartmentExchangeWith? ExchangeWith { get; set; }

    public bool? AcceptsDifferencePayment { get; set; }

    public decimal? DifferenceAmount { get; set; }

    public string? ExchangeDetails { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsPremium { get; set; }

    public bool IsUrgent { get; set; }

    public int ViewCount { get; set; }

    public string? VideoPath { get; set; }

    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<BuildingImage> Images { get; set; } = new List<BuildingImage>();

    public ICollection<BuildingFeatureSelection> Features { get; set; } =
        new List<BuildingFeatureSelection>();

    public ICollection<BuildingRentInclusionSelection> RentInclusions { get; set; } =
        new List<BuildingRentInclusionSelection>();

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

public class BuildingImage : IOrderedListingImage
{
    public Guid Id { get; set; }

    public Guid BuildingId { get; set; }
    public Building Building { get; set; } = default!;

    public string FileName { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class BuildingFeatureSelection
{
    public Guid BuildingId { get; set; }
    public Building Building { get; set; } = default!;

    public ApartmentFeature Feature { get; set; }
}

public class BuildingRentInclusionSelection
{
    public Guid BuildingId { get; set; }
    public Building Building { get; set; } = default!;

    public ApartmentRentInclusion Inclusion { get; set; }
}
