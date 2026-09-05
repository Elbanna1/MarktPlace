using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Shared.Constants;

public static class FurnishingCurtainCatalog
{
    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<FurnishingCurtainProductType>> ProductTypes =
        new List<HomeFurnishingLookupEntry<FurnishingCurtainProductType>>
        {
            new(FurnishingCurtainProductType.Curtains, "ستائر", "Curtains"),
            new(FurnishingCurtainProductType.Carpet, "سجاد", "Carpet"),
            new(FurnishingCurtainProductType.Moquette, "موكيت", "Moquette"),
            new(FurnishingCurtainProductType.BedSheets, "ملايات", "Bed sheets"),
            new(FurnishingCurtainProductType.Blankets, "بطاطين", "Blankets"),
            new(FurnishingCurtainProductType.Duvet, "لحاف", "Duvet"),
            new(FurnishingCurtainProductType.BedCovers, "مفارش", "Bed covers"),
            new(FurnishingCurtainProductType.Pillows, "مخدات", "Pillows"),
            new(FurnishingCurtainProductType.DecorCushions, "وسائد ديكور", "Decor cushions"),
            new(FurnishingCurtainProductType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<FurnishingCurtainSize>> Sizes =
        new List<HomeFurnishingLookupEntry<FurnishingCurtainSize>>
        {
            new(FurnishingCurtainSize.Single, "مفرد", "Single"),
            new(FurnishingCurtainSize.HalfDouble, "نصف مزدوج", "Half double"),
            new(FurnishingCurtainSize.Double, "مزدوج", "Double"),
            new(FurnishingCurtainSize.King, "كينج", "King"),
            new(FurnishingCurtainSize.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<FurnishingCurtainMaterial>> Materials =
        new List<HomeFurnishingLookupEntry<FurnishingCurtainMaterial>>
        {
            new(FurnishingCurtainMaterial.Cotton, "قطن", "Cotton"),
            new(FurnishingCurtainMaterial.Linen, "كتان", "Linen"),
            new(FurnishingCurtainMaterial.Silk, "حرير", "Silk"),
            new(FurnishingCurtainMaterial.Wool, "صوف", "Wool"),
            new(FurnishingCurtainMaterial.Polyester, "بوليستر", "Polyester"),
            new(FurnishingCurtainMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<FurnishingCurtainColor>> Colors =
        new List<HomeFurnishingLookupEntry<FurnishingCurtainColor>>
        {
            new(FurnishingCurtainColor.White, "أبيض", "White"),
            new(FurnishingCurtainColor.Black, "أسود", "Black"),
            new(FurnishingCurtainColor.Brown, "بني", "Brown"),
            new(FurnishingCurtainColor.Beige, "بيج", "Beige"),
            new(FurnishingCurtainColor.Gray, "رمادي", "Gray"),
            new(FurnishingCurtainColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ProductTypeOptions =
        HomeFurnishingCatalog.ToOptions(ProductTypes);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> SizeOptions =
        HomeFurnishingCatalog.ToOptions(Sizes);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> MaterialOptions =
        HomeFurnishingCatalog.ToOptions(Materials);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ColorOptions =
        HomeFurnishingCatalog.ToOptions(Colors);

    public static string GetProductTypeName(FurnishingCurtainProductType value) =>
        HomeFurnishingCatalog.GetName(ProductTypes, value);
    public static string GetSizeName(FurnishingCurtainSize value) =>
        HomeFurnishingCatalog.GetName(Sizes, value);
    public static string GetMaterialName(FurnishingCurtainMaterial value) =>
        HomeFurnishingCatalog.GetName(Materials, value);

    public static string GetColorName(FurnishingCurtainColor value) =>
        HomeFurnishingCatalog.GetName(Colors, value);
    public static string GetColorNameEn(FurnishingCurtainColor value) =>
        HomeFurnishingCatalog.GetNameEn(Colors, value);

    public static List<HomeFurnishingLookupItemDto> SelectedColors(
        IEnumerable<FurnishingCurtainColor> selected) =>
        HomeFurnishingCatalog.SelectedOptions(Colors, selected);
}
