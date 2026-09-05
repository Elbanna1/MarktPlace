using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.HomeFurnishing;

public class CreateFurnitureRequest : CreateHomeFurnishingRequestBase
{
    public FurnitureType FurnitureType { get; set; }

    public string? OtherFurnitureType { get; set; }

    public FurnitureMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public List<FurnitureColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public FurnitureCondition Condition { get; set; }

    public decimal Length { get; set; }

    public decimal Width { get; set; }

    public decimal Height { get; set; }

    public bool CanBeDisassembled { get; set; }

    public bool DeliveryAvailable { get; set; }
}

public class UpdateFurnitureRequest : CreateFurnitureRequest, IHomeFurnishingGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();

    public bool RemoveVideo { get; set; }
}

public class FurnitureDetailsDto : HomeFurnishingDetailsDtoBase
{
    public FurnitureType FurnitureType { get; set; }

    public string FurnitureTypeName { get; set; } = default!;
    public string? OtherFurnitureType { get; set; }

    public FurnitureMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public FurnitureCondition Condition { get; set; }

    public string ConditionName { get; set; } = default!;

    public decimal Length { get; set; }

    public decimal Width { get; set; }

    public decimal Height { get; set; }

    public bool CanBeDisassembled { get; set; }
    public bool DeliveryAvailable { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.Furniture;
}

public class FurnitureListItemDto : HomeFurnishingListItemDtoBase
{
    public FurnitureType FurnitureType { get; set; }
    public string FurnitureTypeName { get; set; } = default!;
    public string? OtherFurnitureType { get; set; }

    public FurnitureMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<HomeFurnishingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public FurnitureCondition Condition { get; set; }
    public string ConditionName { get; set; } = default!;

    public decimal Length { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }

    public bool CanBeDisassembled { get; set; }
    public bool DeliveryAvailable { get; set; }

    [JsonIgnore]
    public override ListingModuleType StatsListingType => ListingModuleType.Furniture;
}

public class FurnitureFilterParams : HomeFurnishingFilterParamsBase
{
    public FurnitureType? FurnitureType { get; set; }

    public FurnitureMaterial? Material { get; set; }

    public FurnitureColor? Color { get; set; }

    public FurnitureCondition? Condition { get; set; }

    public bool? DeliveryAvailable { get; set; }

    public bool? CanBeDisassembled { get; set; }
}
