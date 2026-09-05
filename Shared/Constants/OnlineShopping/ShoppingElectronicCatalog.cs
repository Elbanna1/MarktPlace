using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Shared.Constants;

public static class ShoppingElectronicCatalog
{
    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<ShoppingElectronicSection>> Sections =
        new List<OnlineShoppingLookupEntry<ShoppingElectronicSection>>
        {
            new(ShoppingElectronicSection.Headphones, "سماعات", "Headphones"),
            new(ShoppingElectronicSection.Chargers, "شواحن", "Chargers"),
            new(ShoppingElectronicSection.Cables, "كابلات", "Cables"),
            new(ShoppingElectronicSection.PowerBanks, "باور بانك", "Power banks"),
            new(ShoppingElectronicSection.SmartWatches, "ساعات ذكية", "Smart watches"),
            new(ShoppingElectronicSection.Cases, "جرابات", "Cases"),
            new(ShoppingElectronicSection.Keyboards, "لوحات مفاتيح", "Keyboards"),
            new(ShoppingElectronicSection.Mice, "ماوس", "Mice"),
            new(ShoppingElectronicSection.SecurityCameras, "كاميرات مراقبة", "Security cameras"),
            new(ShoppingElectronicSection.LedLighting, "إضاءة LED", "LED lighting"),
            new(ShoppingElectronicSection.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<ShoppingElectronicCompatibility>> Compatibilities =
        new List<OnlineShoppingLookupEntry<ShoppingElectronicCompatibility>>
        {
            new(ShoppingElectronicCompatibility.Android, "Android", "Android"),
            new(ShoppingElectronicCompatibility.IPhone, "iPhone", "iPhone"),
            new(ShoppingElectronicCompatibility.Windows, "Windows", "Windows"),
            new(ShoppingElectronicCompatibility.AllDevices, "جميع الأجهزة", "All devices")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<ShoppingElectronicCondition>> Conditions =
        new List<OnlineShoppingLookupEntry<ShoppingElectronicCondition>>
        {
            new(ShoppingElectronicCondition.New, "جديد", "New"),
            new(ShoppingElectronicCondition.NewSealed, "جديد بتغليفه", "New (sealed)")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<ShoppingElectronicWarranty>> Warranties =
        new List<OnlineShoppingLookupEntry<ShoppingElectronicWarranty>>
        {
            new(ShoppingElectronicWarranty.Available, "متاح", "Available"),
            new(ShoppingElectronicWarranty.NotAvailable, "غير متاح", "Not available")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> SectionOptions =
        OnlineShoppingCatalog.ToOptions(Sections);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> CompatibilityOptions =
        OnlineShoppingCatalog.ToOptions(Compatibilities);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> ConditionOptions =
        OnlineShoppingCatalog.ToOptions(Conditions);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> WarrantyOptions =
        OnlineShoppingCatalog.ToOptions(Warranties);

    public static string GetSectionName(ShoppingElectronicSection value) =>
        OnlineShoppingCatalog.GetName(Sections, value);

    public static string GetCompatibilityName(ShoppingElectronicCompatibility value) =>
        OnlineShoppingCatalog.GetName(Compatibilities, value);

    public static string GetConditionName(ShoppingElectronicCondition value) =>
        OnlineShoppingCatalog.GetName(Conditions, value);

    public static string GetWarrantyName(ShoppingElectronicWarranty value) =>
        OnlineShoppingCatalog.GetName(Warranties, value);
}
