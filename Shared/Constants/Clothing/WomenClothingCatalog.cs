using Shared.DTOs.Clothing;
using Shared.Enums;

namespace Shared.Constants;

public static class WomenClothingCatalog
{
    public static readonly IReadOnlyList<ClothingLookupEntry<WomenClothingType>> ClothingTypes =
        new List<ClothingLookupEntry<WomenClothingType>>
        {
            new(WomenClothingType.Dress, "فستان", "Dress"),
            new(WomenClothingType.Abaya, "عباية", "Abaya"),
            new(WomenClothingType.ScarvesAndHijab, "طرح وحجاب", "Scarves & hijab"),
            new(WomenClothingType.Blouse, "بلوزة", "Blouse"),
            new(WomenClothingType.Trousers, "بنطلون", "Trousers"),
            new(WomenClothingType.Skirt, "جيبة", "Skirt"),
            new(WomenClothingType.Set, "طقم", "Set"),
            new(WomenClothingType.Jacket, "جاكيت", "Jacket"),
            new(WomenClothingType.Underwear, "ملابس داخلية", "Underwear"),
            new(WomenClothingType.Sportswear, "ملابس رياضية", "Sportswear"),
            new(WomenClothingType.Pajamas, "بيجامات", "Pajamas"),
            new(WomenClothingType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<WomenClothingBrand>> Brands =
        new List<ClothingLookupEntry<WomenClothingBrand>>
        {
            new(WomenClothingBrand.Imported, "مستورد", "Imported"),
            new(WomenClothingBrand.Local, "محلي", "Local"),
            new(WomenClothingBrand.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<WomenClothingSize>> Sizes =
        new List<ClothingLookupEntry<WomenClothingSize>>
        {
            new(WomenClothingSize.XS, "XS", "XS"),
            new(WomenClothingSize.S, "S", "S"),
            new(WomenClothingSize.M, "M", "M"),
            new(WomenClothingSize.L, "L", "L"),
            new(WomenClothingSize.XL, "XL", "XL"),
            new(WomenClothingSize.XXL, "XXL", "XXL"),
            new(WomenClothingSize.XXXL, "XXXL", "XXXL")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<WomenClothingColor>> Colors =
        new List<ClothingLookupEntry<WomenClothingColor>>
        {
            new(WomenClothingColor.Black, "أسود", "Black"),
            new(WomenClothingColor.White, "أبيض", "White"),
            new(WomenClothingColor.Gray, "رمادي", "Gray"),
            new(WomenClothingColor.Navy, "كحلي", "Navy"),
            new(WomenClothingColor.Blue, "أزرق", "Blue"),
            new(WomenClothingColor.SkyBlue, "سماوي", "Sky blue"),
            new(WomenClothingColor.Green, "أخضر", "Green"),
            new(WomenClothingColor.Olive, "زيتي", "Olive"),
            new(WomenClothingColor.Brown, "بني", "Brown"),
            new(WomenClothingColor.Beige, "بيج", "Beige"),
            new(WomenClothingColor.Red, "أحمر", "Red"),
            new(WomenClothingColor.Maroon, "نبيتي", "Maroon"),
            new(WomenClothingColor.Orange, "برتقالي", "Orange"),
            new(WomenClothingColor.Yellow, "أصفر", "Yellow"),
            new(WomenClothingColor.Pink, "وردي", "Pink"),
            new(WomenClothingColor.Purple, "بنفسجي", "Purple"),
            new(WomenClothingColor.MultiColor, "متعدد الألوان", "Multi-color"),
            new(WomenClothingColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<WomenClothingCondition>> Conditions =
        new List<ClothingLookupEntry<WomenClothingCondition>>
        {
            new(WomenClothingCondition.New, "جديد", "New"),
            new(WomenClothingCondition.NewClearance, "جديد بتصفيات", "New (clearance)"),
            new(WomenClothingCondition.UsedExcellent, "مستعمل بحالة ممتازة", "Used - excellent condition")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<WomenClothingSellingMethod>> SellingMethods =
        new List<ClothingLookupEntry<WomenClothingSellingMethod>>
        {
            new(WomenClothingSellingMethod.Store, "محل", "Store"),
            new(WomenClothingSellingMethod.Online, "أونلاين", "Online"),
            new(WomenClothingSellingMethod.StoreAndOnline, "محل + أونلاين", "Store + online")
        };

    public static readonly IReadOnlyList<ClothingLookupItemDto> ClothingTypeOptions = ClothingCatalog.ToOptions(ClothingTypes);
    public static readonly IReadOnlyList<ClothingLookupItemDto> BrandOptions = ClothingCatalog.ToOptions(Brands);
    public static readonly IReadOnlyList<ClothingLookupItemDto> SizeOptions = ClothingCatalog.ToOptions(Sizes);
    public static readonly IReadOnlyList<ClothingLookupItemDto> ColorOptions = ClothingCatalog.ToOptions(Colors);
    public static readonly IReadOnlyList<ClothingLookupItemDto> ConditionOptions = ClothingCatalog.ToOptions(Conditions);
    public static readonly IReadOnlyList<ClothingLookupItemDto> SellingMethodOptions = ClothingCatalog.ToOptions(SellingMethods);

    public static string GetClothingTypeName(WomenClothingType value) => ClothingCatalog.GetName(ClothingTypes, value);
    public static string GetBrandName(WomenClothingBrand value) => ClothingCatalog.GetName(Brands, value);
    public static string GetConditionName(WomenClothingCondition value) => ClothingCatalog.GetName(Conditions, value);
    public static string GetSellingMethodName(WomenClothingSellingMethod value) => ClothingCatalog.GetName(SellingMethods, value);

    public static string GetSizeName(WomenClothingSize value) => ClothingCatalog.GetName(Sizes, value);
    public static string GetSizeNameEn(WomenClothingSize value) => ClothingCatalog.GetNameEn(Sizes, value);
    public static string GetColorName(WomenClothingColor value) => ClothingCatalog.GetName(Colors, value);
    public static string GetColorNameEn(WomenClothingColor value) => ClothingCatalog.GetNameEn(Colors, value);

    public static List<ClothingLookupItemDto> SelectedSizes(IEnumerable<WomenClothingSize> selected) =>
        ClothingCatalog.SelectedOptions(Sizes, selected);

    public static List<ClothingLookupItemDto> SelectedColors(IEnumerable<WomenClothingColor> selected) =>
        ClothingCatalog.SelectedOptions(Colors, selected);
}
