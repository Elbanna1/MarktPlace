using Shared.Enums;

namespace Shared.Constants;

public static class WholesaleTraderCatalog
{
    public readonly record struct Entry(WholesaleTradeType Value, string Name, string NameEn);

    public static readonly IReadOnlyList<Entry> TradeTypes = new List<Entry>
    {
        new(WholesaleTradeType.Food, "مواد غذائية", "Food"),
        new(WholesaleTradeType.Agriculture, "زراعة", "Agriculture"),
        new(WholesaleTradeType.Fashion, "أزياء", "Fashion"),
        new(WholesaleTradeType.HomeSupplies, "مستلزمات منزلية", "Home Supplies"),
        new(WholesaleTradeType.BuildingMaterials, "مواد بناء", "Building Materials"),
        new(WholesaleTradeType.Automotive, "سيارات", "Automotive"),
        new(WholesaleTradeType.AgricultureAndLivestock, "زراعة وثروة حيوانية", "Agriculture & Livestock"),
        new(WholesaleTradeType.Other, "أخرى", "Other")
    };

    public static readonly IReadOnlyDictionary<WholesaleSaleType, string> SaleTypeNames =
        new Dictionary<WholesaleSaleType, string>
        {
            [WholesaleSaleType.Wholesale] = "جملة",
            [WholesaleSaleType.WholesaleAndRetail] = "جملة وقطاعي"
        };

    public static readonly IReadOnlyDictionary<WholesaleSaleType, string> SaleTypeNamesEn =
        new Dictionary<WholesaleSaleType, string>
        {
            [WholesaleSaleType.Wholesale] = "Wholesale",
            [WholesaleSaleType.WholesaleAndRetail] = "Wholesale & Retail"
        };

    private static readonly IReadOnlyDictionary<WholesaleTradeType, Entry> TradeTypesByValue =
        TradeTypes.ToDictionary(entry => entry.Value);

    public static string GetTradeTypeName(WholesaleTradeType value) =>
        TradeTypesByValue.TryGetValue(value, out var entry) ? entry.Name : string.Empty;

    public static string GetSaleTypeName(WholesaleSaleType value) =>
        SaleTypeNames.TryGetValue(value, out var name) ? name : string.Empty;
}
