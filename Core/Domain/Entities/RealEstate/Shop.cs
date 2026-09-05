using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class Shop : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string AdvertiserName { get; set; } = default!;

    public RealEstateListingType ListingType { get; set; }

    public ShopSuitableActivity? SuitableActivity { get; set; }

    public string? OtherSuitableActivity { get; set; }

    public decimal? Price { get; set; }

    public decimal? Area { get; set; }

    public ShopFloorType? FloorType { get; set; }

    public decimal? CeilingHeight { get; set; }

    public decimal? FacadeWidth { get; set; }

    public ShopFacadesCount? FacadesCount { get; set; }

    public ShopFacadeDirection? FacadeDirection { get; set; }

    public ShopFinishingType? FinishingType { get; set; }

    public ShopPropertyAge? PropertyAge { get; set; }

    public bool? HasBathroom { get; set; }

    public int? BathroomsCount { get; set; }

    public bool? HasStorage { get; set; }

    public decimal? StorageArea { get; set; }

    public bool? HasGlassFacade { get; set; }

    public bool? SuitableForRestaurantOrCafe { get; set; }

    public bool? HasExtractorFan { get; set; }

    public bool? HasPrivateEntrance { get; set; }

    public ShopEntrancesCount? EntrancesCount { get; set; }

    public ShopLegalStatus? LegalStatus { get; set; }

    public string? LicenseNumber { get; set; }

    public ShopLicenseType? LicenseType { get; set; }

    public string? LicenseIssuer { get; set; }

    public DateTime? LicenseIssueDate { get; set; }

    public DateTime? LicenseExpiryDate { get; set; }

    public ShopReconciliationForm? ReconciliationForm { get; set; }

    public string? OtherReconciliationForm { get; set; }

    public ShopOwnershipDocument? OwnershipDocument { get; set; }

    public string? OtherOwnershipDocument { get; set; }

    public bool? IsRegistered { get; set; }

    public bool? WasPreviouslyOperating { get; set; }

    public string? PreviousActivity { get; set; }

    public string? PreviousOperatingPeriod { get; set; }

    public string? VacancyReason { get; set; }

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

    public ShopPaymentMethod? PaymentMethod { get; set; }

    public decimal? DownPayment { get; set; }

    public string? InstallmentPeriod { get; set; }

    public decimal? InstallmentAmount { get; set; }

    public ShopInstallmentProvider? InstallmentProvider { get; set; }

    public ShopRentType? RentType { get; set; }

    public decimal? RentValue { get; set; }

    public decimal? SecurityDeposit { get; set; }

    public decimal? RentDownPayment { get; set; }

    public int? MinimumRentPeriod { get; set; }

    public DateTime? AvailableFrom { get; set; }

    public DateTime? AvailableTo { get; set; }

    public bool? AllowsActivityChange { get; set; }

    public string? OwnerConditions { get; set; }

    public ShopExchangeWith? ExchangeWith { get; set; }

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

    public ICollection<ShopImage> Images { get; set; } = new List<ShopImage>();

    public ICollection<ShopUtilitySelection> Utilities { get; set; } = new List<ShopUtilitySelection>();

    public ICollection<ShopRentInclusionSelection> RentInclusions { get; set; } =
        new List<ShopRentInclusionSelection>();

    public ICollection<ShopRentSuitableActivitySelection> RentSuitableActivities { get; set; } =
        new List<ShopRentSuitableActivitySelection>();

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

public class ShopImage : IOrderedListingImage
{
    public Guid Id { get; set; }

    public Guid ShopId { get; set; }
    public Shop Shop { get; set; } = default!;

    public string FileName { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ShopUtilitySelection
{
    public Guid ShopId { get; set; }
    public Shop Shop { get; set; } = default!;

    public ShopUtility Utility { get; set; }
}

public class ShopRentInclusionSelection
{
    public Guid ShopId { get; set; }
    public Shop Shop { get; set; } = default!;

    public ShopRentInclusion Inclusion { get; set; }
}

public class ShopRentSuitableActivitySelection
{
    public Guid ShopId { get; set; }
    public Shop Shop { get; set; } = default!;

    public ShopRentSuitableActivity Activity { get; set; }
}
