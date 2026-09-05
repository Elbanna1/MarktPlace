using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.RealEstate;

public class CreateShopRequest : CreateRealEstateRequestBase
{
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

    public List<ShopUtility> Utilities { get; set; } = new();

    public bool? WasPreviouslyOperating { get; set; }

    public string? PreviousActivity { get; set; }

    public string? PreviousOperatingPeriod { get; set; }

    public string? VacancyReason { get; set; }

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

    public List<ShopRentInclusion> RentInclusions { get; set; } = new();

    public bool? AllowsActivityChange { get; set; }

    public string? OwnerConditions { get; set; }

    public ShopExchangeWith? ExchangeWith { get; set; }

    public bool? AcceptsDifferencePayment { get; set; }

    public decimal? DifferenceAmount { get; set; }

    public string? ExchangeDetails { get; set; }
}

public class UpdateShopRequest : CreateShopRequest, IRealEstateGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class ShopDetailsDto : RealEstateDetailsDtoBase
{
    public ShopSuitableActivity? SuitableActivity { get; set; }

    public string? SuitableActivityName { get; set; }
    public string? OtherSuitableActivity { get; set; }

    public decimal? Price { get; set; }
    public decimal? Area { get; set; }

    public ShopFloorType? FloorType { get; set; }

    public string? FloorTypeName { get; set; }

    public decimal? CeilingHeight { get; set; }
    public decimal? FacadeWidth { get; set; }

    public ShopFacadesCount? FacadesCount { get; set; }

    public string? FacadesCountName { get; set; }

    public ShopFacadeDirection? FacadeDirection { get; set; }

    public string? FacadeDirectionName { get; set; }

    public ShopFinishingType? FinishingType { get; set; }

    public string? FinishingTypeName { get; set; }

    public ShopPropertyAge? PropertyAge { get; set; }

    public string? PropertyAgeName { get; set; }

    public bool? HasBathroom { get; set; }
    public int? BathroomsCount { get; set; }
    public bool? HasStorage { get; set; }
    public decimal? StorageArea { get; set; }
    public bool? HasGlassFacade { get; set; }
    public bool? SuitableForRestaurantOrCafe { get; set; }
    public bool? HasExtractorFan { get; set; }
    public bool? HasPrivateEntrance { get; set; }

    public ShopEntrancesCount? EntrancesCount { get; set; }

    public string? EntrancesCountName { get; set; }

    public ShopLegalStatus? LegalStatus { get; set; }

    public string? LegalStatusName { get; set; }

    public string? LicenseNumber { get; set; }

    public ShopLicenseType? LicenseType { get; set; }

    public string? LicenseTypeName { get; set; }

    public string? LicenseIssuer { get; set; }
    public DateTime? LicenseIssueDate { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }

    public ShopReconciliationForm? ReconciliationForm { get; set; }

    public string? ReconciliationFormName { get; set; }
    public string? OtherReconciliationForm { get; set; }

    public ShopOwnershipDocument? OwnershipDocument { get; set; }

    public string? OwnershipDocumentName { get; set; }
    public string? OtherOwnershipDocument { get; set; }

    public bool? IsRegistered { get; set; }

    public List<RealEstateLookupItemDto> Utilities { get; set; } = new();

    public bool? WasPreviouslyOperating { get; set; }
    public string? PreviousActivity { get; set; }
    public string? PreviousOperatingPeriod { get; set; }
    public string? VacancyReason { get; set; }

    public ShopPaymentMethod? PaymentMethod { get; set; }

    public string? PaymentMethodName { get; set; }

    public decimal? DownPayment { get; set; }
    public string? InstallmentPeriod { get; set; }
    public decimal? InstallmentAmount { get; set; }

    public ShopInstallmentProvider? InstallmentProvider { get; set; }

    public string? InstallmentProviderName { get; set; }

    public ShopRentType? RentType { get; set; }

    public string? RentTypeName { get; set; }

    public decimal? RentValue { get; set; }
    public decimal? SecurityDeposit { get; set; }
    public decimal? RentDownPayment { get; set; }
    public int? MinimumRentPeriod { get; set; }
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }

    public List<RealEstateLookupItemDto> RentInclusions { get; set; } = new();

    public List<RealEstateLookupItemDto> RentSuitableActivities { get; set; } = new();

    public bool? AllowsActivityChange { get; set; }
    public string? OwnerConditions { get; set; }

    public ShopExchangeWith? ExchangeWith { get; set; }

    public string? ExchangeWithName { get; set; }

    public bool? AcceptsDifferencePayment { get; set; }
    public decimal? DifferenceAmount { get; set; }
    public string? ExchangeDetails { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.Shop;
}

public class ShopListItemDto : RealEstateListItemDtoBase
{
    public ShopSuitableActivity? SuitableActivity { get; set; }
    public string? SuitableActivityName { get; set; }
    public string? OtherSuitableActivity { get; set; }

    public decimal? Price { get; set; }
    public decimal? Area { get; set; }

    public ShopFloorType? FloorType { get; set; }
    public string? FloorTypeName { get; set; }

    public ShopFinishingType? FinishingType { get; set; }
    public string? FinishingTypeName { get; set; }

    public ShopFacadesCount? FacadesCount { get; set; }
    public string? FacadesCountName { get; set; }

    public ShopPropertyAge? PropertyAge { get; set; }
    public string? PropertyAgeName { get; set; }

    public bool? HasStorage { get; set; }
    public bool? HasBathroom { get; set; }

    public ShopLegalStatus? LegalStatus { get; set; }
    public string? LegalStatusName { get; set; }

    public ShopLicenseType? LicenseType { get; set; }
    public string? LicenseTypeName { get; set; }

    public ShopOwnershipDocument? OwnershipDocument { get; set; }
    public string? OwnershipDocumentName { get; set; }

    public List<RealEstateLookupItemDto> Utilities { get; set; } = new();

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.Shop;
}

public class ShopFilterParams : RealEstateFilterParamsBase
{
    public ShopSuitableActivity? SuitableActivity { get; set; }

    public decimal? AreaFrom { get; set; }

    public decimal? AreaTo { get; set; }

    public ShopFloorType? FloorType { get; set; }

    public ShopFinishingType? FinishingType { get; set; }

    public ShopFacadesCount? FacadesCount { get; set; }

    public bool? HasStorage { get; set; }

    public bool? HasBathroom { get; set; }

    public bool? IsLicensed { get; set; }

    public bool? IsReconciliation { get; set; }

    public ShopLicenseType? LicenseType { get; set; }

    public ShopOwnershipDocument? OwnershipDocument { get; set; }

    public bool? HasParking { get; set; }

    public bool? HasAirConditioning { get; set; }

    public bool? HasNaturalGas { get; set; }

    public bool? HasSurveillanceCameras { get; set; }
}
