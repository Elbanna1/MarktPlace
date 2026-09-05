using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.RealEstate;

public class CreateLandRequest : CreateRealEstateRequestBase
{
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

    public List<LandUtility> Utilities { get; set; } = new();

    public LandRentType? RentType { get; set; }

    public decimal? RentValue { get; set; }

    public decimal? SecurityDeposit { get; set; }

    public decimal? DownPayment { get; set; }

    public LandMinimumRentPeriod? MinimumRentPeriod { get; set; }

    public DateTime? AvailableFrom { get; set; }

    public DateTime? AvailableTo { get; set; }

    public List<LandRentInclusion> RentInclusions { get; set; } = new();

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
}

public class UpdateLandRequest : CreateLandRequest, IRealEstateGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class LandDetailsDto : RealEstateDetailsDtoBase
{
    public LandType LandType { get; set; }

    public string LandTypeName { get; set; } = default!;
    public string? OtherLandType { get; set; }

    public LandAreaUnit? AreaUnit { get; set; }

    public string? AreaUnitName { get; set; }

    public decimal Area { get; set; }
    public decimal? PricePerMeter { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? FacadeLength { get; set; }

    public LandFacadesCount? FacadesCount { get; set; }

    public string? FacadesCountName { get; set; }

    public LandDirection? Direction { get; set; }

    public string? DirectionName { get; set; }

    public decimal? StreetWidth { get; set; }

    public LandRoadType? RoadType { get; set; }

    public string? RoadTypeName { get; set; }

    public bool? InsideBuildingCordon { get; set; }
    public bool? IsBuildable { get; set; }
    public decimal? AllowedBuildingRatio { get; set; }
    public int? AllowedFloorsCount { get; set; }

    public LandLegalStatus? LegalStatus { get; set; }

    public string? LegalStatusName { get; set; }

    public string? LicenseNumber { get; set; }
    public DateTime? LicenseIssueDate { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public string? LicenseIssuer { get; set; }

    public LandReconciliationForm? ReconciliationForm { get; set; }

    public string? ReconciliationFormName { get; set; }
    public string? OtherReconciliationForm { get; set; }

    public LandOwnershipDocument? OwnershipDocument { get; set; }

    public string? OwnershipDocumentName { get; set; }
    public string? OtherOwnershipDocument { get; set; }

    public bool? HasSurveyPlan { get; set; }
    public bool? HasViolations { get; set; }
    public string? ViolationDetails { get; set; }

    public List<RealEstateLookupItemDto> Utilities { get; set; } = new();

    public LandRentType? RentType { get; set; }

    public string? RentTypeName { get; set; }

    public decimal? RentValue { get; set; }
    public decimal? SecurityDeposit { get; set; }
    public decimal? DownPayment { get; set; }

    public LandMinimumRentPeriod? MinimumRentPeriod { get; set; }

    public string? MinimumRentPeriodName { get; set; }

    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }

    public List<RealEstateLookupItemDto> RentInclusions { get; set; } = new();

    public LandContractDuration? ContractDuration { get; set; }

    public string? ContractDurationName { get; set; }

    public string? OwnerConditions { get; set; }

    public LandExchangeWith? ExchangeWith { get; set; }

    public string? ExchangeWithName { get; set; }
    public string? OtherExchangeWith { get; set; }

    public bool? AcceptsDifferencePayment { get; set; }
    public decimal? DifferenceAmount { get; set; }
    public bool? ExchangeInSameGovernorateOnly { get; set; }
    public string? ExchangeDetails { get; set; }

    public bool? IsCurrentlyCultivated { get; set; }
    public string? CurrentCropType { get; set; }
    public decimal? CultivatedFeddans { get; set; }

    public LandHarvestSeason? HarvestSeason { get; set; }

    public string? HarvestSeasonName { get; set; }

    public LandSoilType? SoilType { get; set; }

    public string? SoilTypeName { get; set; }

    public LandIrrigationSource? IrrigationSource { get; set; }

    public string? IrrigationSourceName { get; set; }
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

    public string? QualityCertificateName { get; set; }

    public bool? HasGate { get; set; }
    public bool? IsLeveledForBuilding { get; set; }
    public bool? HasFoundations { get; set; }

    public bool? HasExistingBuilding { get; set; }

    public LandExistingBuildingType? ExistingBuildingType { get; set; }

    public string? ExistingBuildingTypeName { get; set; }

    public LandBuildingCompletionRatio? BuildingCompletionRatio { get; set; }

    public string? BuildingCompletionRatioName { get; set; }

    public int? CurrentFloorsCount { get; set; }
    public bool? CanAddFloors { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.Land;
}

public class LandListItemDto : RealEstateListItemDtoBase
{
    public LandType LandType { get; set; }
    public string LandTypeName { get; set; } = default!;
    public string? OtherLandType { get; set; }

    public LandAreaUnit? AreaUnit { get; set; }
    public string? AreaUnitName { get; set; }

    public decimal Area { get; set; }
    public decimal? PricePerMeter { get; set; }
    public decimal? TotalPrice { get; set; }

    public LandFacadesCount? FacadesCount { get; set; }
    public string? FacadesCountName { get; set; }

    public LandDirection? Direction { get; set; }
    public string? DirectionName { get; set; }

    public LandRoadType? RoadType { get; set; }
    public string? RoadTypeName { get; set; }

    public bool? InsideBuildingCordon { get; set; }
    public bool? IsBuildable { get; set; }

    public LandLegalStatus? LegalStatus { get; set; }
    public string? LegalStatusName { get; set; }

    public LandOwnershipDocument? OwnershipDocument { get; set; }
    public string? OwnershipDocumentName { get; set; }

    public List<RealEstateLookupItemDto> Utilities { get; set; } = new();

    public bool? IsCurrentlyCultivated { get; set; }

    public LandIrrigationSource? IrrigationSource { get; set; }
    public string? IrrigationSourceName { get; set; }

    public bool? IsOrganic { get; set; }
    public bool? HasWell { get; set; }
    public bool? HasIrrigationNetwork { get; set; }
    public bool? HasFence { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.Land;
}

public class LandFilterParams : RealEstateFilterParamsBase
{
    public LandType? LandType { get; set; }

    public decimal? PricePerMeterFrom { get; set; }

    public decimal? PricePerMeterTo { get; set; }

    public decimal? AreaFrom { get; set; }

    public decimal? AreaTo { get; set; }

    public LandAreaUnit? AreaUnit { get; set; }

    public bool? InsideBuildingCordon { get; set; }

    public bool? IsBuildable { get; set; }

    public LandLegalStatus? LegalStatus { get; set; }

    public LandOwnershipDocument? OwnershipDocument { get; set; }

    public LandRoadType? RoadType { get; set; }

    public LandFacadesCount? FacadesCount { get; set; }

    public LandDirection? Direction { get; set; }

    public bool? IsCurrentlyCultivated { get; set; }

    public LandIrrigationSource? IrrigationSource { get; set; }

    public bool? IsOrganic { get; set; }

    public bool? HasWell { get; set; }

    public bool? HasIrrigationNetwork { get; set; }

    public bool? HasFence { get; set; }
}
