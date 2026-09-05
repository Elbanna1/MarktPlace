using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Shared.Constants;

public static class HandmadeCatalog
{
    public static readonly IReadOnlyList<AntiqueLookupEntry<HandmadeType>> Types =
        new List<AntiqueLookupEntry<HandmadeType>>
        {
            new(HandmadeType.Crochet, "كروشيه", "Crochet"),
            new(HandmadeType.Macrame, "مكرمية", "Macrame"),
            new(HandmadeType.Resin, "ريزن", "Resin"),
            new(HandmadeType.Candles, "شموع", "Candles"),
            new(HandmadeType.NaturalSoap, "صابون طبيعي", "Natural soap"),
            new(HandmadeType.Embroidery, "تطريز", "Embroidery"),
            new(HandmadeType.Pottery, "فخار", "Pottery"),
            new(HandmadeType.WoodenProducts, "منتجات خشبية", "Wooden products"),
            new(HandmadeType.LeatherProducts, "منتجات جلدية", "Leather products"),
            new(HandmadeType.Accessories, "إكسسوارات", "Accessories"),
            new(HandmadeType.HomeDecor, "ديكور منزلي", "Home decor"),
            new(HandmadeType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupEntry<HandmadeColor>> Colors =
        new List<AntiqueLookupEntry<HandmadeColor>>
        {
            new(HandmadeColor.White, "أبيض", "White"),
            new(HandmadeColor.Black, "أسود", "Black"),
            new(HandmadeColor.Red, "أحمر", "Red"),
            new(HandmadeColor.Blue, "أزرق", "Blue"),
            new(HandmadeColor.Green, "أخضر", "Green"),
            new(HandmadeColor.Brown, "بني", "Brown"),
            new(HandmadeColor.Beige, "بيج", "Beige"),
            new(HandmadeColor.Gold, "ذهبي", "Gold"),
            new(HandmadeColor.Silver, "فضي", "Silver"),
            new(HandmadeColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AntiqueLookupItemDto> TypeOptions = AntiqueCatalog.ToOptions(Types);
    public static readonly IReadOnlyList<AntiqueLookupItemDto> ColorOptions = AntiqueCatalog.ToOptions(Colors);

    public static string GetTypeName(HandmadeType value) => AntiqueCatalog.GetName(Types, value);
    public static string GetColorName(HandmadeColor value) => AntiqueCatalog.GetName(Colors, value);
}
