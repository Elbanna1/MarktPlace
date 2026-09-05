using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Shared.Constants;

public static class CosmeticCatalog
{
    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<CosmeticSection>> Sections =
        new List<OnlineShoppingLookupEntry<CosmeticSection>>
        {
            new(CosmeticSection.Makeup, "مكياج", "Makeup"),
            new(CosmeticSection.SkinCare, "عناية بالبشرة", "Skin care"),
            new(CosmeticSection.HairCare, "عناية بالشعر", "Hair care"),
            new(CosmeticSection.Perfumes, "عطور", "Perfumes"),
            new(CosmeticSection.Lenses, "عدسات", "Contact lenses"),
            new(CosmeticSection.NaturalProducts, "منتجات طبيعية", "Natural products"),
            new(CosmeticSection.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<CosmeticSuitableFor>> SuitableFor =
        new List<OnlineShoppingLookupEntry<CosmeticSuitableFor>>
        {
            new(CosmeticSuitableFor.Women, "نسائي", "Women"),
            new(CosmeticSuitableFor.Men, "رجالي", "Men"),
            new(CosmeticSuitableFor.Kids, "أطفال", "Kids"),
            new(CosmeticSuitableFor.Everyone, "للجميع", "Everyone")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> SectionOptions =
        OnlineShoppingCatalog.ToOptions(Sections);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> SuitableForOptions =
        OnlineShoppingCatalog.ToOptions(SuitableFor);

    public static string GetSectionName(CosmeticSection value) =>
        OnlineShoppingCatalog.GetName(Sections, value);

    public static string GetSuitableForName(CosmeticSuitableFor value) =>
        OnlineShoppingCatalog.GetName(SuitableFor, value);
}
