using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Shared.Constants;

public static class GiftToyCatalog
{
    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<GiftToyType>> Types =
        new List<OnlineShoppingLookupEntry<GiftToyType>>
        {
            new(GiftToyType.KidsToys, "ألعاب أطفال", "Kids toys"),
            new(GiftToyType.GiftBoxes, "بوكس هدايا", "Gift boxes"),
            new(GiftToyType.Flowers, "ورد", "Flowers"),
            new(GiftToyType.Chocolate, "شوكولاتة", "Chocolate"),
            new(GiftToyType.Mugs, "مجات", "Mugs"),
            new(GiftToyType.Keychains, "ميداليات", "Keychains"),
            new(GiftToyType.Chains, "سلاسل", "Chains"),
            new(GiftToyType.EducationalToys, "ألعاب تعليمية", "Educational toys"),
            new(GiftToyType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupEntry<GiftToySuitableFor>> SuitableFor =
        new List<OnlineShoppingLookupEntry<GiftToySuitableFor>>
        {
            new(GiftToySuitableFor.Kids, "أطفال", "Kids"),
            new(GiftToySuitableFor.Girls, "بنات", "Girls"),
            new(GiftToySuitableFor.YoungAdults, "شباب", "Young adults"),
            new(GiftToySuitableFor.Everyone, "للجميع", "Everyone")
        };

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> TypeOptions =
        OnlineShoppingCatalog.ToOptions(Types);

    public static readonly IReadOnlyList<OnlineShoppingLookupItemDto> SuitableForOptions =
        OnlineShoppingCatalog.ToOptions(SuitableFor);

    public static string GetTypeName(GiftToyType value) =>
        OnlineShoppingCatalog.GetName(Types, value);

    public static string GetSuitableForName(GiftToySuitableFor value) =>
        OnlineShoppingCatalog.GetName(SuitableFor, value);
}
