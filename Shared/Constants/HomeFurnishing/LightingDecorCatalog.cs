using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Shared.Constants;

public static class LightingDecorCatalog
{
    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<LightingDecorProductType>> ProductTypes =
        new List<HomeFurnishingLookupEntry<LightingDecorProductType>>
        {
            new(LightingDecorProductType.Chandelier, "نجفة", "Chandelier"),
            new(LightingDecorProductType.TableLamp, "أباجورة", "Table lamp"),
            new(LightingDecorProductType.Spotlight, "سبوت", "Spotlight"),
            new(LightingDecorProductType.LedStrip, "شريط LED", "LED strip"),
            new(LightingDecorProductType.WallClock, "ساعة حائط", "Wall clock"),
            new(LightingDecorProductType.Mirror, "مرآة", "Mirror"),
            new(LightingDecorProductType.Painting, "لوحة", "Painting"),
            new(LightingDecorProductType.Vase, "فازة", "Vase"),
            new(LightingDecorProductType.Candlestick, "شمعدان", "Candlestick"),
            new(LightingDecorProductType.Shelves, "رفوف", "Shelves"),
            new(LightingDecorProductType.WallDecor, "ديكور حائط", "Wall decor"),
            new(LightingDecorProductType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<LightingDecorMaterial>> Materials =
        new List<HomeFurnishingLookupEntry<LightingDecorMaterial>>
        {
            new(LightingDecorMaterial.Glass, "زجاج", "Glass"),
            new(LightingDecorMaterial.Crystal, "كريستال", "Crystal"),
            new(LightingDecorMaterial.Metal, "معدن", "Metal"),
            new(LightingDecorMaterial.Wood, "خشب", "Wood"),
            new(LightingDecorMaterial.Plastic, "بلاستيك", "Plastic"),
            new(LightingDecorMaterial.Ceramic, "سيراميك", "Ceramic"),
            new(LightingDecorMaterial.Fabric, "قماش", "Fabric"),
            new(LightingDecorMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<LightingDecorColor>> Colors =
        new List<HomeFurnishingLookupEntry<LightingDecorColor>>
        {
            new(LightingDecorColor.White, "أبيض", "White"),
            new(LightingDecorColor.Black, "أسود", "Black"),
            new(LightingDecorColor.Brown, "بني", "Brown"),
            new(LightingDecorColor.Beige, "بيج", "Beige"),
            new(LightingDecorColor.Gray, "رمادي", "Gray"),
            new(LightingDecorColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<LightingDecorLightType>> LightTypes =
        new List<HomeFurnishingLookupEntry<LightingDecorLightType>>
        {
            new(LightingDecorLightType.White, "أبيض", "White"),
            new(LightingDecorLightType.Yellow, "أصفر", "Yellow"),
            new(LightingDecorLightType.Rgb, "RGB", "RGB")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ProductTypeOptions =
        HomeFurnishingCatalog.ToOptions(ProductTypes);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> MaterialOptions =
        HomeFurnishingCatalog.ToOptions(Materials);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ColorOptions =
        HomeFurnishingCatalog.ToOptions(Colors);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> LightTypeOptions =
        HomeFurnishingCatalog.ToOptions(LightTypes);

    public static string GetProductTypeName(LightingDecorProductType value) =>
        HomeFurnishingCatalog.GetName(ProductTypes, value);
    public static string GetMaterialName(LightingDecorMaterial value) =>
        HomeFurnishingCatalog.GetName(Materials, value);

    public static string? GetLightTypeName(LightingDecorLightType? value) =>
        HomeFurnishingCatalog.GetNameOrNull(LightTypes, value);

    public static string GetColorName(LightingDecorColor value) =>
        HomeFurnishingCatalog.GetName(Colors, value);
    public static string GetColorNameEn(LightingDecorColor value) =>
        HomeFurnishingCatalog.GetNameEn(Colors, value);

    public static List<HomeFurnishingLookupItemDto> SelectedColors(
        IEnumerable<LightingDecorColor> selected) =>
        HomeFurnishingCatalog.SelectedOptions(Colors, selected);
}
