using System.Text.Json.Serialization;
using Shared.Enums;

namespace Shared.DTOs.RealEstate;

public class CreateBuildingRequest : CreateRealEstateRequestBase
{
    public BuildingType? BuildingType { get; set; }

    public decimal? Price { get; set; }

    public decimal? PricePerMeter { get; set; }

    public decimal? Area { get; set; }

    public ApartmentFurnishedStatus? FurnishedStatus { get; set; }

    public ApartmentFinishingType? FinishingType { get; set; }

    public ApartmentPropertyAge? PropertyAge { get; set; }

    public ApartmentDirection? Direction { get; set; }

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

    public ApartmentLegalStatus? LegalStatus { get; set; }

    public string? LicenseNumber { get; set; }

    public DateTime? LicenseIssueDate { get; set; }

    public DateTime? LicenseExpiryDate { get; set; }

    public string? LicenseIssuer { get; set; }

    public ApartmentReconciliationForm? ReconciliationForm { get; set; }

    public ApartmentOwnershipDocument? OwnershipDocument { get; set; }

    public bool? IsRegistered { get; set; }

    public bool? HasViolations { get; set; }

    public string? ViolationDetails { get; set; }

    public List<ApartmentFeature> Features { get; set; } = new();

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

    public List<ApartmentRentInclusion> RentInclusions { get; set; } = new();

    public string? OwnerConditions { get; set; }

    public ApartmentExchangeWith? ExchangeWith { get; set; }

    public bool? AcceptsDifferencePayment { get; set; }

    public decimal? DifferenceAmount { get; set; }

    public string? ExchangeDetails { get; set; }
}

public class UpdateBuildingRequest : CreateBuildingRequest, IRealEstateGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class BuildingDetailsDto : RealEstateDetailsDtoBase
{
    public BuildingType? BuildingType { get; set; }
    public string? BuildingTypeName { get; set; }

    public decimal? Price { get; set; }
    public decimal? PricePerMeter { get; set; }
    public decimal? Area { get; set; }

    public ApartmentFurnishedStatus? FurnishedStatus { get; set; }
    public string? FurnishedStatusName { get; set; }

    public ApartmentFinishingType? FinishingType { get; set; }
    public string? FinishingTypeName { get; set; }

    public ApartmentPropertyAge? PropertyAge { get; set; }
    public string? PropertyAgeName { get; set; }

    public ApartmentDirection? Direction { get; set; }
    public string? DirectionName { get; set; }

    public ApartmentViewType? ViewType { get; set; }
    public string? ViewTypeName { get; set; }

    public decimal? GardenArea { get; set; }
    public int? FloorsCount { get; set; }
    public int? BathroomsCount { get; set; }

    public ApartmentReceptionPieces? ReceptionPieces { get; set; }
    public string? ReceptionPiecesName { get; set; }

    public int? RoomsCount { get; set; }
    public bool? HasSwimmingPool { get; set; }

    public int? ResidentialUnitsCount { get; set; }
    public bool? HasIndependentEntrance { get; set; }

    public int? ApartmentsCount { get; set; }
    public int? ShopsCount { get; set; }
    public bool? HasElevator { get; set; }
    public bool? HasGarage { get; set; }

    public ApartmentLegalStatus? LegalStatus { get; set; }
    public string? LegalStatusName { get; set; }

    public string? LicenseNumber { get; set; }
    public DateTime? LicenseIssueDate { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public string? LicenseIssuer { get; set; }

    public ApartmentReconciliationForm? ReconciliationForm { get; set; }
    public string? ReconciliationFormName { get; set; }

    public ApartmentOwnershipDocument? OwnershipDocument { get; set; }
    public string? OwnershipDocumentName { get; set; }

    public bool? IsRegistered { get; set; }
    public bool? HasViolations { get; set; }
    public string? ViolationDetails { get; set; }

    public List<RealEstateLookupItemDto> Features { get; set; } = new();

    public ApartmentPaymentMethod? PaymentMethod { get; set; }
    public string? PaymentMethodName { get; set; }

    public decimal? DownPayment { get; set; }
    public decimal? InstallmentAmount { get; set; }
    public string? InstallmentPeriod { get; set; }

    public ApartmentInstallmentProvider? InstallmentProvider { get; set; }
    public string? InstallmentProviderName { get; set; }

    public bool? HasMaintenanceDeposit { get; set; }
    public decimal? MaintenanceDepositAmount { get; set; }
    public decimal? MonthlyFees { get; set; }

    public ApartmentRentType? RentType { get; set; }
    public string? RentTypeName { get; set; }

    public decimal? RentValue { get; set; }
    public decimal? SecurityDeposit { get; set; }
    public decimal? RentDownPayment { get; set; }
    public int? MinimumRentPeriod { get; set; }
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }

    public List<RealEstateLookupItemDto> RentInclusions { get; set; } = new();

    public string? OwnerConditions { get; set; }

    public ApartmentExchangeWith? ExchangeWith { get; set; }
    public string? ExchangeWithName { get; set; }

    public bool? AcceptsDifferencePayment { get; set; }
    public decimal? DifferenceAmount { get; set; }
    public string? ExchangeDetails { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.Building;
}

public class BuildingListItemDto : RealEstateListItemDtoBase
{
    public BuildingType? BuildingType { get; set; }
    public string? BuildingTypeName { get; set; }

    public decimal? Price { get; set; }
    public decimal? PricePerMeter { get; set; }
    public decimal? RentValue { get; set; }
    public decimal? Area { get; set; }

    public int? FloorsCount { get; set; }
    public int? RoomsCount { get; set; }
    public int? BathroomsCount { get; set; }
    public int? ResidentialUnitsCount { get; set; }
    public int? ApartmentsCount { get; set; }
    public int? ShopsCount { get; set; }

    public ApartmentFurnishedStatus? FurnishedStatus { get; set; }
    public string? FurnishedStatusName { get; set; }

    public ApartmentFinishingType? FinishingType { get; set; }
    public string? FinishingTypeName { get; set; }

    public ApartmentPropertyAge? PropertyAge { get; set; }
    public string? PropertyAgeName { get; set; }

    public ApartmentLegalStatus? LegalStatus { get; set; }
    public string? LegalStatusName { get; set; }

    public ApartmentOwnershipDocument? OwnershipDocument { get; set; }
    public string? OwnershipDocumentName { get; set; }

    public List<RealEstateLookupItemDto> Features { get; set; } = new();

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.Building;
}

public class BuildingFilterParams : RealEstateFilterParamsBase
{
    public string? AdvertiserName { get; set; }

    public BuildingType? BuildingType { get; set; }

    public decimal? AreaFrom { get; set; }

    public decimal? AreaTo { get; set; }

    public int? FloorsCount { get; set; }

    public int? RoomsCount { get; set; }

    public int? BathroomsCount { get; set; }

    public ApartmentFinishingType? FinishingType { get; set; }

    public ApartmentFurnishedStatus? FurnishedStatus { get; set; }

    public ApartmentPropertyAge? PropertyAge { get; set; }

    public bool? HasElevator { get; set; }

    public bool? HasGarage { get; set; }

    public bool? HasSwimmingPool { get; set; }

    public bool? HasGarden { get; set; }

    public ApartmentLegalStatus? LegalStatus { get; set; }

    public ApartmentOwnershipDocument? OwnershipDocument { get; set; }
}
