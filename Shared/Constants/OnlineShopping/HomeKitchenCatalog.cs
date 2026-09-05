using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Shared.Constants;

public static class HomeKitchenCatalog
{
    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<HomeKitchenSection>> Sections =
        new List<OnlineShoppingLookupEntry<HomeKitchenSection>>
        {
            new(HomeKitchenSection.KitchenTools, "أدوات مطبخ", "Kitchen tools"),
            new(HomeKitchenSection.Cookware, "أواني", "Cookware"),
            new(HomeKitchenSection.SmallKitchenAppliances, "أجهزة مطبخ صغيرة", "Small kitchen appliances"),
            new(HomeKitchenSection.Decor, "ديكور", "Decor"),
            new(HomeKitchenSection.Furnishings, "مفروشات", "Furnishings"),
            new(HomeKitchenSection.StorageAndOrganization, "تخزين وتنظيم", "Storage & organization"),
            new(HomeKitchenSection.CleaningTools, "أدوات تنظيف", "Cleaning tools"),
            new(HomeKitchenSection.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<HomeKitchenMaterial>> Materials =
        new List<OnlineShoppingLookupEntry<HomeKitchenMaterial>>
        {
            new(HomeKitchenMaterial.Plastic, "بلاستيك", "Plastic"),
            new(HomeKitchenMaterial.Wood, "خشب", "Wood"),
            new(HomeKitchenMaterial.Glass, "زجاج", "Glass"),
            new(HomeKitchenMaterial.Metal, "معدن", "Metal"),
            new(HomeKitchenMaterial.Stainless, "ستانلس", "Stainless steel"),
            new(HomeKitchenMaterial.Ceramic, "سيراميك", "Ceramic"),
            new(HomeKitchenMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<HomeKitchenColor>> Colors =
        new List<OnlineShoppingLookupEntry<HomeKitchenColor>>
        {
            new(HomeKitchenColor.White, "أبيض", "White"),
            new(HomeKitchenColor.Black, "أسود", "Black"),
            new(HomeKitchenColor.Gray, "رمادي", "Gray"),
            new(HomeKitchenColor.Brown, "بني", "Brown"),
            new(HomeKitchenColor.Beige, "بيج", "Beige"),
            new(HomeKitchenColor.Silver, "فضي", "Silver"),
            new(HomeKitchenColor.Gold, "ذهبي", "Gold"),
            new(HomeKitchenColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> SectionOptions =
        OnlineShoppingCatalog.ToOptions(Sections);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> MaterialOptions =
        OnlineShoppingCatalog.ToOptions(Materials);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> ColorOptions =
        OnlineShoppingCatalog.ToOptions(Colors);

    public static string GetSectionName(HomeKitchenSection value) =>
        OnlineShoppingCatalog.GetName(Sections, value);

    public static string GetMaterialName(HomeKitchenMaterial value) =>
        OnlineShoppingCatalog.GetName(Materials, value);

    public static string GetColorName(HomeKitchenColor value) =>
        OnlineShoppingCatalog.GetName(Colors, value);

    public static string GetColorNameEn(HomeKitchenColor value) =>
        OnlineShoppingCatalog.GetNameEn(Colors, value);

    public static List<OnlineShoppingLookupItemDto> SelectedColors(IEnumerable<HomeKitchenColor> selected) =>
        OnlineShoppingCatalog.SelectedOptions(Colors, selected);
}
