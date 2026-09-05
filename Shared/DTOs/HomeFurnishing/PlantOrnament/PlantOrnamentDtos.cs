using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.HomeFurnishing;

public class CreatePlantOrnamentRequest : CreateHomeFurnishingRequestBase
{
    public PlantOrnamentProductType ProductType { get; set; }

    public string? OtherProductType { get; set; }

    public PlantOrnamentSuitableFor SuitableFor { get; set; }

    public decimal? Height { get; set; }
}

public class UpdatePlantOrnamentRequest : CreatePlantOrnamentRequest, IHomeFurnishingGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class PlantOrnamentDetailsDto : HomeFurnishingDetailsDtoBase
{
    public PlantOrnamentProductType ProductType { get; set; }

    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public PlantOrnamentSuitableFor SuitableFor { get; set; }

    public string SuitableForName { get; set; } = default!;

    public decimal? Height { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.PlantOrnament;
}

public class PlantOrnamentListItemDto : HomeFurnishingListItemDtoBase
{
    public PlantOrnamentProductType ProductType { get; set; }
    public string ProductTypeName { get; set; } = default!;
    public string? OtherProductType { get; set; }

    public PlantOrnamentSuitableFor SuitableFor { get; set; }
    public string SuitableForName { get; set; } = default!;

    public decimal? Height { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.PlantOrnament;
}

public class PlantOrnamentFilterParams : HomeFurnishingFilterParamsBase
{
    public PlantOrnamentProductType? ProductType { get; set; }

    public PlantOrnamentSuitableFor? SuitableFor { get; set; }

    public decimal? HeightFrom { get; set; }

    public decimal? HeightTo { get; set; }
}
