using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.HomeFurnishing;

public class CreateHomeApplianceRequest : CreateHomeFurnishingRequestBase
{
    public HomeApplianceDeviceType DeviceType { get; set; }

    public string? OtherDeviceType { get; set; }

    public HomeApplianceBrand Brand { get; set; }

    public string? OtherBrand { get; set; }

    public List<HomeApplianceColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public HomeApplianceCondition Condition { get; set; }

    public HomeApplianceWarranty Warranty { get; set; }

    public string? WarrantyDuration { get; set; }

    public string? PowerRating { get; set; }

    public bool DeliveryAvailable { get; set; }
}

public class UpdateHomeApplianceRequest : CreateHomeApplianceRequest, IHomeFurnishingGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class HomeApplianceDetailsDto : HomeFurnishingDetailsDtoBase
{
    public HomeApplianceDeviceType DeviceType { get; set; }

    public string DeviceTypeName { get; set; } = default!;
    public string? OtherDeviceType { get; set; }

    public HomeApplianceBrand Brand { get; set; }

    public string BrandName { get; set; } = default!;
    public string? OtherBrand { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public HomeApplianceCondition Condition { get; set; }

    public string ConditionName { get; set; } = default!;

    public HomeApplianceWarranty Warranty { get; set; }

    public string WarrantyName { get; set; } = default!;

    public string? WarrantyDuration { get; set; }

    public string? PowerRating { get; set; }

    public bool DeliveryAvailable { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.HomeAppliance;
}

public class HomeApplianceListItemDto : HomeFurnishingListItemDtoBase
{
    public HomeApplianceDeviceType DeviceType { get; set; }
    public string DeviceTypeName { get; set; } = default!;
    public string? OtherDeviceType { get; set; }

    public HomeApplianceBrand Brand { get; set; }
    public string BrandName { get; set; } = default!;
    public string? OtherBrand { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public HomeApplianceCondition Condition { get; set; }
    public string ConditionName { get; set; } = default!;

    public HomeApplianceWarranty Warranty { get; set; }
    public string WarrantyName { get; set; } = default!;
    public string? WarrantyDuration { get; set; }

    public string? PowerRating { get; set; }

    public bool DeliveryAvailable { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.HomeAppliance;
}

public class HomeApplianceFilterParams : HomeFurnishingFilterParamsBase
{
    public HomeApplianceDeviceType? DeviceType { get; set; }

    public HomeApplianceBrand? Brand { get; set; }

    public HomeApplianceColor? Color { get; set; }

    public HomeApplianceCondition? Condition { get; set; }

    public HomeApplianceWarranty? Warranty { get; set; }

    public bool? DeliveryAvailable { get; set; }
}
