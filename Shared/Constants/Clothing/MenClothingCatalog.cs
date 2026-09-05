using Shared.DTOs.Clothing;
using Shared.Enums;

namespace Shared.Constants;

public static class MenClothingCatalog
{
    public static readonly IReadOnlyList<ClothingLookupEntry<MenClothingType>> ClothingTypes =
        new List<ClothingLookupEntry<MenClothingType>>
        {
            new(MenClothingType.TShirt, "تيشيرت", "T-shirt"),
            new(MenClothingType.Shirt, "قميص", "Shirt"),
            new(MenClothingType.Trousers, "بنطلون", "Trousers"),
            new(MenClothingType.Jeans, "جينز", "Jeans"),
            new(MenClothingType.Suit, "بدلة", "Suit"),
            new(MenClothingType.Jacket, "جاكيت", "Jacket"),
            new(MenClothingType.Sweatshirt, "سويت شيرت", "Sweatshirt"),
            new(MenClothingType.Underwear, "ملابس داخلية", "Underwear"),
            new(MenClothingType.Sportswear, "ملابس رياضية", "Sportswear"),
            new(MenClothingType.Pajamas, "بيجامات", "Pajamas"),
            new(MenClothingType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<MenClothingBrand>> Brands =
        new List<ClothingLookupEntry<MenClothingBrand>>
        {
            new(MenClothingBrand.Imported, "مستورد", "Imported"),
            new(MenClothingBrand.Local, "محلي", "Local"),
            new(MenClothingBrand.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<MenClothingSize>> Sizes =
        new List<ClothingLookupEntry<MenClothingSize>>
        {
            new(MenClothingSize.XS, "XS", "XS"),
            new(MenClothingSize.S, "S", "S"),
            new(MenClothingSize.M, "M", "M"),
            new(MenClothingSize.L, "L", "L"),
            new(MenClothingSize.XL, "XL", "XL"),
            new(MenClothingSize.XXL, "XXL", "XXL"),
            new(MenClothingSize.XXXL, "XXXL", "XXXL"),
            new(MenClothingSize.XXXXL, "XXXXL", "XXXXL")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<MenClothingColor>> Colors =
        new List<ClothingLookupEntry<MenClothingColor>>
        {
            new(MenClothingColor.Black, "أسود", "Black"),
            new(MenClothingColor.White, "أبيض", "White"),
            new(MenClothingColor.Gray, "رمادي", "Gray"),
            new(MenClothingColor.Navy, "كحلي", "Navy"),
            new(MenClothingColor.Blue, "أزرق", "Blue"),
            new(MenClothingColor.SkyBlue, "سماوي", "Sky blue"),
            new(MenClothingColor.Green, "أخضر", "Green"),
            new(MenClothingColor.Olive, "زيتي", "Olive"),
            new(MenClothingColor.Brown, "بني", "Brown"),
            new(MenClothingColor.Beige, "بيج", "Beige"),
            new(MenClothingColor.Red, "أحمر", "Red"),
            new(MenClothingColor.Maroon, "نبيتي", "Maroon"),
            new(MenClothingColor.Orange, "برتقالي", "Orange"),
            new(MenClothingColor.Yellow, "أصفر", "Yellow"),
            new(MenClothingColor.MultiColor, "متعدد الألوان", "Multi-color"),
            new(MenClothingColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<MenClothingCondition>> Conditions =
        new List<ClothingLookupEntry<MenClothingCondition>>
        {
            new(MenClothingCondition.New, "جديد", "New"),
            new(MenClothingCondition.NewClearance, "جديد بتصفيات", "New (clearance)"),
            new(MenClothingCondition.UsedExcellent, "مستعمل بحالة ممتازة", "Used - excellent condition")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<MenClothingSellingMethod>> SellingMethods =
        new List<ClothingLookupEntry<MenClothingSellingMethod>>
        {
            new(MenClothingSellingMethod.Store, "محل", "Store"),
            new(MenClothingSellingMethod.Online, "أونلاين", "Online"),
            new(MenClothingSellingMethod.StoreAndOnline, "محل + أونلاين", "Store + online")
        };

    public static readonly IReadOnlyList<ClothingLookupItemDto> ClothingTypeOptions = ClothingCatalog.ToOptions(ClothingTypes);
    public static readonly IReadOnlyList<ClothingLookupItemDto> BrandOptions = ClothingCatalog.ToOptions(Brands);
    public static readonly IReadOnlyList<ClothingLookupItemDto> SizeOptions = ClothingCatalog.ToOptions(Sizes);
    public static readonly IReadOnlyList<ClothingLookupItemDto> ColorOptions = ClothingCatalog.ToOptions(Colors);
    public static readonly IReadOnlyList<ClothingLookupItemDto> ConditionOptions = ClothingCatalog.ToOptions(Conditions);
    public static readonly IReadOnlyList<ClothingLookupItemDto> SellingMethodOptions = ClothingCatalog.ToOptions(SellingMethods);

    public static string GetClothingTypeName(MenClothingType value) => ClothingCatalog.GetName(ClothingTypes, value);
    public static string GetBrandName(MenClothingBrand value) => ClothingCatalog.GetName(Brands, value);
    public static string GetConditionName(MenClothingCondition value) => ClothingCatalog.GetName(Conditions, value);
    public static string GetSellingMethodName(MenClothingSellingMethod value) => ClothingCatalog.GetName(SellingMethods, value);

    public static string GetSizeName(MenClothingSize value) => ClothingCatalog.GetName(Sizes, value);
    public static string GetSizeNameEn(MenClothingSize value) => ClothingCatalog.GetNameEn(Sizes, value);
    public static string GetColorName(MenClothingColor value) => ClothingCatalog.GetName(Colors, value);
    public static string GetColorNameEn(MenClothingColor value) => ClothingCatalog.GetNameEn(Colors, value);

    public static List<ClothingLookupItemDto> SelectedSizes(IEnumerable<MenClothingSize> selected) =>
        ClothingCatalog.SelectedOptions(Sizes, selected);

    public static List<ClothingLookupItemDto> SelectedColors(IEnumerable<MenClothingColor> selected) =>
        ClothingCatalog.SelectedOptions(Colors, selected);
}
