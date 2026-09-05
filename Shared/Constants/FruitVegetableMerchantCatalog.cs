using Shared.Enums;

namespace Shared.Constants;

public static class FruitVegetableMerchantCatalog
{
    public static readonly IReadOnlyDictionary<MerchantSaleType, string> SaleTypeNames =
        new Dictionary<MerchantSaleType, string>
        {
            [MerchantSaleType.Retail] = "قطاعي",
            [MerchantSaleType.Wholesale] = "جملة",
            [MerchantSaleType.Both] = "قطاعي وجملة"
        };

    public static readonly IReadOnlyDictionary<MerchantSaleType, string> SaleTypeNamesEn =
        new Dictionary<MerchantSaleType, string>
        {
            [MerchantSaleType.Retail] = "Retail",
            [MerchantSaleType.Wholesale] = "Wholesale",
            [MerchantSaleType.Both] = "Both"
        };

    public static string GetSaleTypeName(MerchantSaleType value) =>
        SaleTypeNames.TryGetValue(value, out var name) ? name : string.Empty;
}
