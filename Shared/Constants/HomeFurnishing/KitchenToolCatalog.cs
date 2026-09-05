using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Shared.Constants;

public static class KitchenToolCatalog
{
    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<KitchenToolProductType>> ProductTypes =
        new List<HomeFurnishingLookupEntry<KitchenToolProductType>>
        {
            new(KitchenToolProductType.PotSets, "أطقم حلل", "Pot sets"),
            new(KitchenToolProductType.Pans, "مقالي", "Pans"),
            new(KitchenToolProductType.Trays, "صواني", "Trays"),
            new(KitchenToolProductType.Plates, "أطباق", "Plates"),
            new(KitchenToolProductType.Cups, "أكواب", "Cups"),
            new(KitchenToolProductType.Cutlery, "ملاعق وشوك", "Cutlery"),
            new(KitchenToolProductType.Knives, "سكاكين", "Knives"),
            new(KitchenToolProductType.Jars, "برطمانات", "Jars"),
            new(KitchenToolProductType.StorageTools, "أدوات تخزين", "Storage tools"),
            new(KitchenToolProductType.ServingTools, "أدوات تقديم", "Serving tools"),
            new(KitchenToolProductType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<KitchenToolMaterial>> Materials =
        new List<HomeFurnishingLookupEntry<KitchenToolMaterial>>
        {
            new(KitchenToolMaterial.Stainless, "ستانلس", "Stainless steel"),
            new(KitchenToolMaterial.Granite, "جرانيت", "Granite"),
            new(KitchenToolMaterial.Teflon, "تيفال", "Teflon"),
            new(KitchenToolMaterial.Aluminum, "ألومنيوم", "Aluminium"),
            new(KitchenToolMaterial.Glass, "زجاج", "Glass"),
            new(KitchenToolMaterial.Silicone, "سيليكون", "Silicone"),
            new(KitchenToolMaterial.Plastic, "بلاستيك", "Plastic"),
            new(KitchenToolMaterial.Wood, "خشب", "Wood"),
            new(KitchenToolMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<KitchenToolColor>> Colors =
        new List<HomeFurnishingLookupEntry<KitchenToolColor>>
        {
            new(KitchenToolColor.White, "أبيض", "White"),
            new(KitchenToolColor.Black, "أسود", "Black"),
            new(KitchenToolColor.Brown, "بني", "Brown"),
            new(KitchenToolColor.Beige, "بيج", "Beige"),
            new(KitchenToolColor.Gray, "رمادي", "Gray"),
            new(KitchenToolColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ProductTypeOptions =
        HomeFurnishingCatalog.ToOptions(ProductTypes);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> MaterialOptions =
        HomeFurnishingCatalog.ToOptions(Materials);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ColorOptions =
        HomeFurnishingCatalog.ToOptions(Colors);

    public static string GetProductTypeName(KitchenToolProductType value) =>
        HomeFurnishingCatalog.GetName(ProductTypes, value);
    public static string GetMaterialName(KitchenToolMaterial value) =>
        HomeFurnishingCatalog.GetName(Materials, value);

    public static string GetColorName(KitchenToolColor value) =>
        HomeFurnishingCatalog.GetName(Colors, value);
    public static string GetColorNameEn(KitchenToolColor value) =>
        HomeFurnishingCatalog.GetNameEn(Colors, value);

    public static List<HomeFurnishingLookupItemDto> SelectedColors(
        IEnumerable<KitchenToolColor> selected) =>
        HomeFurnishingCatalog.SelectedOptions(Colors, selected);
}
