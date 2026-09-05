using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Shared.Constants;

public static class AccessoryCatalog
{
    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<AccessoryType>> AccessoryTypes =
        new List<OnlineShoppingLookupEntry<AccessoryType>>
        {
            new(AccessoryType.Watches, "ساعات", "Watches"),
            new(AccessoryType.Glasses, "نظارات", "Glasses"),
            new(AccessoryType.Wallets, "محافظ", "Wallets"),
            new(AccessoryType.Belts, "أحزمة", "Belts"),
            new(AccessoryType.Bags, "حقائب", "Bags"),
            new(AccessoryType.Chains, "سلاسل", "Chains"),
            new(AccessoryType.Rings, "خواتم", "Rings"),
            new(AccessoryType.Bracelets, "أساور", "Bracelets"),
            new(AccessoryType.Earrings, "أقراط", "Earrings"),
            new(AccessoryType.HairAccessories, "إكسسوارات شعر", "Hair accessories"),
            new(AccessoryType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<AccessoryCategory>> Categories =
        new List<OnlineShoppingLookupEntry<AccessoryCategory>>
        {
            new(AccessoryCategory.Men, "رجالي", "Men"),
            new(AccessoryCategory.Women, "حريمي", "Women"),
            new(AccessoryCategory.Kids, "أطفال", "Kids"),
            new(AccessoryCategory.Everyone, "للجميع", "Everyone")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<AccessoryMaterial>> Materials =
        new List<OnlineShoppingLookupEntry<AccessoryMaterial>>
        {
            new(AccessoryMaterial.Silver, "فضة", "Silver"),
            new(AccessoryMaterial.Stainless, "ستانلس", "Stainless steel"),
            new(AccessoryMaterial.Leather, "جلد", "Leather"),
            new(AccessoryMaterial.Fabric, "قماش", "Fabric"),
            new(AccessoryMaterial.Beads, "خرز", "Beads"),
            new(AccessoryMaterial.Metal, "معدن", "Metal"),
            new(AccessoryMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<AccessoryColor>> Colors =
        new List<OnlineShoppingLookupEntry<AccessoryColor>>
        {
            new(AccessoryColor.Black, "أسود", "Black"),
            new(AccessoryColor.White, "أبيض", "White"),
            new(AccessoryColor.Silver, "فضي", "Silver"),
            new(AccessoryColor.Gold, "ذهبي", "Gold"),
            new(AccessoryColor.Brown, "بني", "Brown"),
            new(AccessoryColor.Red, "أحمر", "Red"),
            new(AccessoryColor.Blue, "أزرق", "Blue"),
            new(AccessoryColor.Pink, "وردي", "Pink"),
            new(AccessoryColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> AccessoryTypeOptions =
        OnlineShoppingCatalog.ToOptions(AccessoryTypes);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> CategoryOptions =
        OnlineShoppingCatalog.ToOptions(Categories);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> MaterialOptions =
        OnlineShoppingCatalog.ToOptions(Materials);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> ColorOptions =
        OnlineShoppingCatalog.ToOptions(Colors);

    public static string GetAccessoryTypeName(AccessoryType value) =>
        OnlineShoppingCatalog.GetName(AccessoryTypes, value);

    public static string GetCategoryName(AccessoryCategory value) =>
        OnlineShoppingCatalog.GetName(Categories, value);

    public static string GetMaterialName(AccessoryMaterial value) =>
        OnlineShoppingCatalog.GetName(Materials, value);

    public static string GetColorName(AccessoryColor value) =>
        OnlineShoppingCatalog.GetName(Colors, value);

    public static string GetColorNameEn(AccessoryColor value) =>
        OnlineShoppingCatalog.GetNameEn(Colors, value);

    public static List<OnlineShoppingLookupItemDto> SelectedColors(IEnumerable<AccessoryColor> selected) =>
        OnlineShoppingCatalog.SelectedOptions(Colors, selected);
}
