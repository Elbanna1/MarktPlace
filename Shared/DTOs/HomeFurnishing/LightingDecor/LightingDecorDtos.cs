using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.HomeFurnishing;

public class CreateLightingDecorRequest : CreateHomeFurnishingRequestBase
{
    public LightingDecorProductType ProductType { get; set; }

    public string? OtherProductType { get; set; }

    public LightingDecorMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public List<LightingDecorColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public LightingDecorLightType? LightType { get; set; }
}

public class UpdateLightingDecorRequest : CreateLightingDecorRequest, IHomeFurnishingGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class LightingDecorDetailsDto : HomeFurnishingDetailsDtoBase
{
    public LightingDecorProductType ProductType { get; set; }

    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public LightingDecorMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public LightingDecorLightType? LightType { get; set; }

    public string? LightTypeName { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.LightingDecor;
}

public class LightingDecorListItemDto : HomeFurnishingListItemDtoBase
{
    public LightingDecorProductType ProductType { get; set; }
    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public LightingDecorMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public LightingDecorLightType? LightType { get; set; }
    public string? LightTypeName { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.LightingDecor;
}

public class LightingDecorFilterParams : HomeFurnishingFilterParamsBase
{
    public LightingDecorProductType? ProductType { get; set; }

    public LightingDecorMaterial? Material { get; set; }

    public LightingDecorColor? Color { get; set; }

    public LightingDecorLightType? LightType { get; set; }
}
