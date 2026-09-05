using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class Land : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string AdvertiserName { get; set; } = default!;

    public RealEstateListingType ListingType { get; set; }

    public LandType LandType { get; set; }

    public string? OtherLandType { get; set; }

    public LandAreaUnit? AreaUnit { get; set; }

    public decimal Area { get; set; }

    public decimal? PricePerMeter { get; set; }

    public decimal? TotalPrice { get; set; }

    public decimal? Length { get; set; }

    public decimal? Width { get; set; }

    public decimal? FacadeLength { get; set; }

    public LandFacadesCount? FacadesCount { get; set; }

    public LandDirection? Direction { get; set; }

    public decimal? StreetWidth { get; set; }

    public LandRoadType? RoadType { get; set; }

    public bool? InsideBuildingCordon { get; set; }

    public bool? IsBuildable { get; set; }

    public decimal? AllowedBuildingRatio { get; set; }

    public int? AllowedFloorsCount { get; set; }

    public LandLegalStatus? LegalStatus { get; set; }

    public string? LicenseNumber { get; set; }

    public DateTime? LicenseIssueDate { get; set; }

    public DateTime? LicenseExpiryDate { get; set; }

    public string? LicenseIssuer { get; set; }

    public LandReconciliationForm? ReconciliationForm { get; set; }

    public string? OtherReconciliationForm { get; set; }

    public LandOwnershipDocument? OwnershipDocument { get; set; }

    public string? OtherOwnershipDocument { get; set; }

    public bool? HasSurveyPlan { get; set; }

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

    public LandRentType? RentType { get; set; }

    public decimal? RentValue { get; set; }

    public decimal? SecurityDeposit { get; set; }

    public decimal? DownPayment { get; set; }

    public LandMinimumRentPeriod? MinimumRentPeriod { get; set; }

    public DateTime? AvailableFrom { get; set; }

    public DateTime? AvailableTo { get; set; }

    public LandContractDuration? ContractDuration { get; set; }

    public string? OwnerConditions { get; set; }

    public LandExchangeWith? ExchangeWith { get; set; }

    public string? OtherExchangeWith { get; set; }

    public bool? AcceptsDifferencePayment { get; set; }

    public decimal? DifferenceAmount { get; set; }

    public bool? ExchangeInSameGovernorateOnly { get; set; }

    public string? ExchangeDetails { get; set; }

    public bool? IsCurrentlyCultivated { get; set; }

    public string? CurrentCropType { get; set; }

    public decimal? CultivatedFeddans { get; set; }

    public LandHarvestSeason? HarvestSeason { get; set; }

    public LandSoilType? SoilType { get; set; }

    public LandIrrigationSource? IrrigationSource { get; set; }

    public string? OtherIrrigationSource { get; set; }

    public bool? HasWell { get; set; }

    public bool? HasIrrigationMachine { get; set; }

    public bool? HasIrrigationNetwork { get; set; }

    public bool? HasTrees { get; set; }

    public string? TreeType { get; set; }

    public int? TreesCount { get; set; }

    public string? TreesAge { get; set; }

    public bool? HasFarmHouse { get; set; }

    public bool? HasRestHouse { get; set; }

    public bool? HasStorage { get; set; }

    public bool? HasFence { get; set; }

    public bool? HasResidentWorkers { get; set; }

    public bool? IsOrganic { get; set; }

    public bool? UsesChemicalFertilizers { get; set; }

    public bool? HasQualityCertificate { get; set; }

    public LandQualityCertificate? QualityCertificate { get; set; }

    public bool? HasGate { get; set; }

    public bool? IsLeveledForBuilding { get; set; }

    public bool? HasFoundations { get; set; }

    public bool? HasExistingBuilding { get; set; }

    public LandExistingBuildingType? ExistingBuildingType { get; set; }

    public LandBuildingCompletionRatio? BuildingCompletionRatio { get; set; }

    public int? CurrentFloorsCount { get; set; }

    public bool? CanAddFloors { get; set; }

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

    public ICollection<LandImage> Images { get; set; } = new List<LandImage>();

    public ICollection<LandUtilitySelection> Utilities { get; set; } = new List<LandUtilitySelection>();

    public ICollection<LandRentInclusionSelection> RentInclusions { get; set; } =
        new List<LandRentInclusionSelection>();

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

public class LandImage : IOrderedListingImage
{
    public Guid Id { get; set; }

    public Guid LandId { get; set; }
    public Land Land { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class LandUtilitySelection
{
    public Guid LandId { get; set; }
    public Land Land { get; set; } = default!;

    public LandUtility Utility { get; set; }
}

public class LandRentInclusionSelection
{
    public Guid LandId { get; set; }
    public Land Land { get; set; } = default!;

    public LandRentInclusion Inclusion { get; set; }
}
