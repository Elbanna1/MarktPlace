using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Shared.Constants;

public static class DecorAntiqueCatalog
{
    public static readonly IReadOnlyList<AntiqueLookupEntry<DecorAntiqueItemType>> ItemTypes =
        new List<AntiqueLookupEntry<DecorAntiqueItemType>>
        {
            new(DecorAntiqueItemType.Vase, "فازة", "Vase"),
            new(DecorAntiqueItemType.Statue, "تمثال", "Statue"),
            new(DecorAntiqueItemType.Candlestick, "شمعدان", "Candlestick"),
            new(DecorAntiqueItemType.Mirror, "مرآة", "Mirror"),
            new(DecorAntiqueItemType.Clock, "ساعة", "Clock"),
            new(DecorAntiqueItemType.Chandelier, "نجفة", "Chandelier"),
            new(DecorAntiqueItemType.WoodenBox, "صندوق خشبي", "Wooden box"),
            new(DecorAntiqueItemType.WallDecor, "ديكور جداري", "Wall decor"),
            new(DecorAntiqueItemType.FlowerPot, "مزهرية", "Flower pot"),
            new(DecorAntiqueItemType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<DecorAntiqueMaterial>> Materials =
        new List<AntiqueLookupEntry<DecorAntiqueMaterial>>
        {
            new(DecorAntiqueMaterial.Wood, "خشب", "Wood"),
            new(DecorAntiqueMaterial.Copper, "نحاس", "Copper"),
            new(DecorAntiqueMaterial.Bronze, "برونز", "Bronze"),
            new(DecorAntiqueMaterial.Glass, "زجاج", "Glass"),
            new(DecorAntiqueMaterial.Crystal, "كريستال", "Crystal"),
            new(DecorAntiqueMaterial.Marble, "رخام", "Marble"),
            new(DecorAntiqueMaterial.Ceramic, "سيراميك", "Ceramic"),
            new(DecorAntiqueMaterial.Porcelain, "خزف", "Porcelain"),
            new(DecorAntiqueMaterial.Stone, "حجر", "Stone"),
            new(DecorAntiqueMaterial.Resin, "راتنج", "Resin"),
            new(DecorAntiqueMaterial.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<DecorAntiqueCondition>> Conditions =
        new List<AntiqueLookupEntry<DecorAntiqueCondition>>
        {
            new(DecorAntiqueCondition.New, "جديدة", "New"),
            new(DecorAntiqueCondition.Excellent, "ممتازة", "Excellent"),
            new(DecorAntiqueCondition.VeryGood, "جيدة جدًا", "Very good"),
            new(DecorAntiqueCondition.Good, "جيدة", "Good"),
            new(DecorAntiqueCondition.NeedsRestoration, "تحتاج ترميم", "Needs restoration")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<DecorAntiqueOriginality>> Originalities =
        new List<AntiqueLookupEntry<DecorAntiqueOriginality>>
        {
            new(DecorAntiqueOriginality.Original, "أصلية", "Original"),
            new(DecorAntiqueOriginality.Replica, "نسخة", "Replica")
        };

    public static readonly IReadOnlyList<AntiqueLookupItemDto> ItemTypeOptions = AntiqueCatalog.ToOptions(ItemTypes);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> MaterialOptions = AntiqueCatalog.ToOptions(Materials);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> ConditionOptions = AntiqueCatalog.ToOptions(Conditions);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> OriginalityOptions = AntiqueCatalog.ToOptions(Originalities);

    public static string GetItemTypeName(DecorAntiqueItemType value) => AntiqueCatalog.GetName(ItemTypes, value);
    public static string GetMaterialName(DecorAntiqueMaterial value) => AntiqueCatalog.GetName(Materials, value);
    public static string GetConditionName(DecorAntiqueCondition value) => AntiqueCatalog.GetName(Conditions, value);
    public static string GetOriginalityName(DecorAntiqueOriginality value) => AntiqueCatalog.GetName(Originalities, value);
}
