using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.HomeFurnishing;

public class CreateFurnishingCurtainRequest : CreateHomeFurnishingRequestBase
{
    public FurnishingCurtainProductType ProductType { get; set; }

    public string? OtherProductType { get; set; }

    public FurnishingCurtainSize Size { get; set; }

    public string? OtherSize { get; set; }

    public FurnishingCurtainMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public List<FurnishingCurtainColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public bool DeliveryAvailable { get; set; }
}

public class UpdateFurnishingCurtainRequest : CreateFurnishingCurtainRequest, IHomeFurnishingGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class FurnishingCurtainDetailsDto : HomeFurnishingDetailsDtoBase
{
    public FurnishingCurtainProductType ProductType { get; set; }

    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public FurnishingCurtainSize Size { get; set; }

    public string SizeName { get; set; } = default!;
    public string? OtherSize { get; set; }

    public FurnishingCurtainMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public bool DeliveryAvailable { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.FurnishingCurtain;
}

public class FurnishingCurtainListItemDto : HomeFurnishingListItemDtoBase
{
    public FurnishingCurtainProductType ProductType { get; set; }
    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public FurnishingCurtainSize Size { get; set; }
    public string SizeName { get; set; } = default!;
    public string? OtherSize { get; set; }

    public FurnishingCurtainMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public bool DeliveryAvailable { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.FurnishingCurtain;
}

public class FurnishingCurtainFilterParams : HomeFurnishingFilterParamsBase
{
    public FurnishingCurtainProductType? ProductType { get; set; }

    public FurnishingCurtainSize? Size { get; set; }

    public FurnishingCurtainMaterial? Material { get; set; }

    public FurnishingCurtainColor? Color { get; set; }

    public bool? DeliveryAvailable { get; set; }
}
