using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.HomeFurnishing;

public class CreateKitchenToolRequest : CreateHomeFurnishingRequestBase
{
    public KitchenToolProductType ProductType { get; set; }

    public string? OtherProductType { get; set; }

    public KitchenToolMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public List<KitchenToolColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }
}

public class UpdateKitchenToolRequest : CreateKitchenToolRequest, IHomeFurnishingGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class KitchenToolDetailsDto : HomeFurnishingDetailsDtoBase
{
    public KitchenToolProductType ProductType { get; set; }

    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public KitchenToolMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.KitchenTool;
}

public class KitchenToolListItemDto : HomeFurnishingListItemDtoBase
{
    public KitchenToolProductType ProductType { get; set; }
    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public KitchenToolMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.KitchenTool;
}

public class KitchenToolFilterParams : HomeFurnishingFilterParamsBase
{
    public KitchenToolProductType? ProductType { get; set; }

    public KitchenToolMaterial? Material { get; set; }

    public KitchenToolColor? Color { get; set; }
}
