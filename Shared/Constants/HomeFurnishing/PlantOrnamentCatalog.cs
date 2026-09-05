using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Shared.Constants;

public static class PlantOrnamentCatalog
{
    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<PlantOrnamentProductType>> ProductTypes =
        new List<HomeFurnishingLookupEntry<PlantOrnamentProductType>>
        {
            new(PlantOrnamentProductType.NaturalPlant, "نبات طبيعي", "Natural plant"),
            new(PlantOrnamentProductType.ArtificialPlant, "نبات صناعي", "Artificial plant"),
            new(PlantOrnamentProductType.Pot, "أصيص", "Pot"),
            new(PlantOrnamentProductType.Flowers, "زهور", "Flowers"),
            new(PlantOrnamentProductType.OrnamentalTree, "شجرة زينة", "Ornamental tree"),
            new(PlantOrnamentProductType.DecorFountain, "نافورة ديكور", "Decorative fountain"),
            new(PlantOrnamentProductType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<PlantOrnamentSuitableFor>> SuitableFors =
        new List<HomeFurnishingLookupEntry<PlantOrnamentSuitableFor>>
        {
            new(PlantOrnamentSuitableFor.Indoor, "داخلي", "Indoor"),
            new(PlantOrnamentSuitableFor.Outdoor, "خارجي", "Outdoor"),
            new(PlantOrnamentSuitableFor.Both, "داخلي وخارجي", "Indoor & outdoor")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ProductTypeOptions =
        HomeFurnishingCatalog.ToOptions(ProductTypes);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> SuitableForOptions =
        HomeFurnishingCatalog.ToOptions(SuitableFors);

    public static string GetProductTypeName(PlantOrnamentProductType value) =>
        HomeFurnishingCatalog.GetName(ProductTypes, value);
    public static string GetSuitableForName(PlantOrnamentSuitableFor value) =>
        HomeFurnishingCatalog.GetName(SuitableFors, value);
}
