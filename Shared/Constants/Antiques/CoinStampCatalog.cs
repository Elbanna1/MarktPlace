using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Shared.Constants;

public static class CoinStampCatalog
{
    public static readonly IReadOnlyList<AntiqueLookupEntry<CoinStampItemType>> ItemTypes =
        new List<AntiqueLookupEntry<CoinStampItemType>>
        {
            new(CoinStampItemType.Coin, "عملة معدنية", "Coin"),
            new(CoinStampItemType.Banknote, "عملة ورقية", "Banknote"),
            new(CoinStampItemType.PostageStamp, "طابع بريد", "Postage stamp"),
            new(CoinStampItemType.CoinCollection, "مجموعة عملات", "Coin collection"),
            new(CoinStampItemType.StampCollection, "مجموعة طوابع", "Stamp collection"),
            new(CoinStampItemType.CommemorativeMedal, "ميدالية تذكارية", "Commemorative medal"),
            new(CoinStampItemType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<CoinStampMetal>> Metals =
        new List<AntiqueLookupEntry<CoinStampMetal>>
        {
            new(CoinStampMetal.Gold, "ذهب", "Gold"),
            new(CoinStampMetal.Silver, "فضة", "Silver"),
            new(CoinStampMetal.Copper, "نحاس", "Copper"),
            new(CoinStampMetal.Nickel, "نيكل", "Nickel"),
            new(CoinStampMetal.Bronze, "برونز", "Bronze"),
            new(CoinStampMetal.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<CoinStampCondition>> Conditions =
        new List<AntiqueLookupEntry<CoinStampCondition>>
        {
            new(CoinStampCondition.Uncirculated, "UNC", "UNC"),
            new(CoinStampCondition.Excellent, "ممتازة", "Excellent"),
            new(CoinStampCondition.VeryGood, "جيدة جدًا", "Very good"),
            new(CoinStampCondition.Good, "جيدة", "Good")
        };

    public static readonly IReadOnlyList<AntiqueLookupItemDto> ItemTypeOptions = AntiqueCatalog.ToOptions(ItemTypes);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> MetalOptions = AntiqueCatalog.ToOptions(Metals);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> ConditionOptions = AntiqueCatalog.ToOptions(Conditions);

    public static string GetItemTypeName(CoinStampItemType value) => AntiqueCatalog.GetName(ItemTypes, value);
    public static string GetMetalName(CoinStampMetal value) => AntiqueCatalog.GetName(Metals, value);
    public static string GetConditionName(CoinStampCondition value) => AntiqueCatalog.GetName(Conditions, value);
}
