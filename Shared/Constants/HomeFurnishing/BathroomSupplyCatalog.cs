using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Shared.Constants;

public static class BathroomSupplyCatalog
{
    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<BathroomSupplyProductType>> ProductTypes =
        new List<HomeFurnishingLookupEntry<BathroomSupplyProductType>>
        {
            new(BathroomSupplyProductType.BathroomUnit, "وحدة حمام", "Bathroom unit"),
            new(BathroomSupplyProductType.Mirror, "مرآة", "Mirror"),
            new(BathroomSupplyProductType.Mixer, "خلاط", "Mixer tap"),
            new(BathroomSupplyProductType.Shower, "دش", "Shower"),
            new(BathroomSupplyProductType.TowelHolder, "حامل مناشف", "Towel holder"),
            new(BathroomSupplyProductType.ShowerCurtain, "ستارة حمام", "Shower curtain"),
            new(BathroomSupplyProductType.Shelves, "رفوف", "Shelves"),
            new(BathroomSupplyProductType.LaundryBasket, "سلة غسيل", "Laundry basket"),
            new(BathroomSupplyProductType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<BathroomSupplyMaterial>> Materials =
        new List<HomeFurnishingLookupEntry<BathroomSupplyMaterial>>
        {
            new(BathroomSupplyMaterial.Stainless, "ستانلس", "Stainless steel"),
            new(BathroomSupplyMaterial.Plastic, "بلاستيك", "Plastic"),
            new(BathroomSupplyMaterial.Glass, "زجاج", "Glass"),
            new(BathroomSupplyMaterial.Wood, "خشب", "Wood"),
            new(BathroomSupplyMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<BathroomSupplyColor>> Colors =
        new List<HomeFurnishingLookupEntry<BathroomSupplyColor>>
        {
            new(BathroomSupplyColor.White, "أبيض", "White"),
            new(BathroomSupplyColor.Black, "أسود", "Black"),
            new(BathroomSupplyColor.Brown, "بني", "Brown"),
            new(BathroomSupplyColor.Beige, "بيج", "Beige"),
            new(BathroomSupplyColor.Gray, "رمادي", "Gray"),
            new(BathroomSupplyColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ProductTypeOptions =
        HomeFurnishingCatalog.ToOptions(ProductTypes);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> MaterialOptions =
        HomeFurnishingCatalog.ToOptions(Materials);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ColorOptions =
        HomeFurnishingCatalog.ToOptions(Colors);

    public static string GetProductTypeName(BathroomSupplyProductType value) =>
        HomeFurnishingCatalog.GetName(ProductTypes, value);
    public static string GetMaterialName(BathroomSupplyMaterial value) =>
        HomeFurnishingCatalog.GetName(Materials, value);

    public static string GetColorName(BathroomSupplyColor value) =>
        HomeFurnishingCatalog.GetName(Colors, value);
    public static string GetColorNameEn(BathroomSupplyColor value) =>
        HomeFurnishingCatalog.GetNameEn(Colors, value);

    public static List<HomeFurnishingLookupItemDto> SelectedColors(
        IEnumerable<BathroomSupplyColor> selected) =>
        HomeFurnishingCatalog.SelectedOptions(Colors, selected);
}
