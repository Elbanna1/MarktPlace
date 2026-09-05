using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.HomeFurnishing;

public class CreateBathroomSupplyRequest : CreateHomeFurnishingRequestBase
{
    public BathroomSupplyProductType ProductType { get; set; }

    public string? OtherProductType { get; set; }

    public BathroomSupplyMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public List<BathroomSupplyColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }
}

public class UpdateBathroomSupplyRequest : CreateBathroomSupplyRequest, IHomeFurnishingGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class BathroomSupplyDetailsDto : HomeFurnishingDetailsDtoBase
{
    public BathroomSupplyProductType ProductType { get; set; }

    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public BathroomSupplyMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.BathroomSupply;
}

public class BathroomSupplyListItemDto : HomeFurnishingListItemDtoBase
{
    public BathroomSupplyProductType ProductType { get; set; }
    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public BathroomSupplyMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.BathroomSupply;
}

public class BathroomSupplyFilterParams : HomeFurnishingFilterParamsBase
{
    public BathroomSupplyProductType? ProductType { get; set; }

    public BathroomSupplyMaterial? Material { get; set; }

    public BathroomSupplyColor? Color { get; set; }
}
