using Shared.DTOs.Clothing;
using Shared.Enums;

namespace Shared.Constants;

public static class KidsClothingCatalog
{
    public static readonly IReadOnlyList<ClothingLookupEntry<KidsClothingType>> ClothingTypes =
        new List<ClothingLookupEntry<KidsClothingType>>
        {
            new(KidsClothingType.TShirt, "تيشيرت أطفال", "Kids T-shirt"),
            new(KidsClothingType.Dress, "فستان أطفال", "Kids dress"),
            new(KidsClothingType.Trousers, "بنطلون أطفال", "Kids trousers"),
            new(KidsClothingType.Set, "طقم أطفال", "Kids set"),
            new(KidsClothingType.Jacket, "جاكيت أطفال", "Kids jacket"),
            new(KidsClothingType.Pajamas, "بيجامة أطفال", "Kids pajamas"),
            new(KidsClothingType.NewbornClothing, "ملابس مواليد", "Newborn clothing"),
            new(KidsClothingType.Underwear, "ملابس داخلية أطفال", "Kids underwear"),
            new(KidsClothingType.Sportswear, "ملابس رياضية أطفال", "Kids sportswear"),
            new(KidsClothingType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<KidsClothingBrand>> Brands =
        new List<ClothingLookupEntry<KidsClothingBrand>>
        {
            new(KidsClothingBrand.Imported, "مستورد", "Imported"),
            new(KidsClothingBrand.Local, "محلي", "Local"),
            new(KidsClothingBrand.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<KidsClothingSize>> Sizes =
        new List<ClothingLookupEntry<KidsClothingSize>>
        {
            new(KidsClothingSize.Newborn, "حديث ولادة", "Newborn"),
            new(KidsClothingSize.Months0To3, "0-3 شهور", "0-3 months"),
            new(KidsClothingSize.Months3To6, "3-6 شهور", "3-6 months"),
            new(KidsClothingSize.Months6To12, "6-12 شهر", "6-12 months"),
            new(KidsClothingSize.OneYear, "سنة", "1 year"),
            new(KidsClothingSize.TwoYears, "سنتين", "2 years"),
            new(KidsClothingSize.ThreeYears, "3 سنوات", "3 years"),
            new(KidsClothingSize.FourYears, "4 سنوات", "4 years"),
            new(KidsClothingSize.FiveYears, "5 سنوات", "5 years"),
            new(KidsClothingSize.SixYears, "6 سنوات", "6 years"),
            new(KidsClothingSize.SevenToEightYears, "7-8 سنوات", "7-8 years"),
            new(KidsClothingSize.NineToTenYears, "9-10 سنوات", "9-10 years"),
            new(KidsClothingSize.ElevenToTwelveYears, "11-12 سنة", "11-12 years"),
            new(KidsClothingSize.ThirteenToFourteenYears, "13-14 سنة", "13-14 years")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<KidsClothingColor>> Colors =
        new List<ClothingLookupEntry<KidsClothingColor>>
        {
            new(KidsClothingColor.Black, "أسود", "Black"),
            new(KidsClothingColor.White, "أبيض", "White"),
            new(KidsClothingColor.Gray, "رمادي", "Gray"),
            new(KidsClothingColor.Blue, "أزرق", "Blue"),
            new(KidsClothingColor.SkyBlue, "سماوي", "Sky blue"),
            new(KidsClothingColor.Green, "أخضر", "Green"),
            new(KidsClothingColor.Yellow, "أصفر", "Yellow"),
            new(KidsClothingColor.Red, "أحمر", "Red"),
            new(KidsClothingColor.Pink, "وردي", "Pink"),
            new(KidsClothingColor.Purple, "بنفسجي", "Purple"),
            new(KidsClothingColor.Orange, "برتقالي", "Orange"),
            new(KidsClothingColor.MultiColor, "متعدد الألوان", "Multi-color"),
            new(KidsClothingColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<KidsClothingCondition>> Conditions =
        new List<ClothingLookupEntry<KidsClothingCondition>>
        {
            new(KidsClothingCondition.New, "جديد", "New"),
            new(KidsClothingCondition.NewClearance, "جديد بتصفيات", "New (clearance)"),
            new(KidsClothingCondition.UsedExcellent, "مستعمل بحالة ممتازة", "Used - excellent condition")
        };

    public static readonly IReadOnlyList<ClothingLookupEntry<KidsClothingSellingMethod>> SellingMethods =
        new List<ClothingLookupEntry<KidsClothingSellingMethod>>
        {
            new(KidsClothingSellingMethod.Store, "محل", "Store"),
            new(KidsClothingSellingMethod.Online, "أونلاين", "Online"),
            new(KidsClothingSellingMethod.StoreAndOnline, "محل + أونلاين", "Store + online")
        };

    public static readonly IReadOnlyList<ClothingLookupItemDto> ClothingTypeOptions = ClothingCatalog.ToOptions(ClothingTypes);
    public static readonly IReadOnlyList<ClothingLookupItemDto> BrandOptions = ClothingCatalog.ToOptions(Brands);
    public static readonly IReadOnlyList<ClothingLookupItemDto> SizeOptions = ClothingCatalog.ToOptions(Sizes);
    public static readonly IReadOnlyList<ClothingLookupItemDto> ColorOptions = ClothingCatalog.ToOptions(Colors);
    public static readonly IReadOnlyList<ClothingLookupItemDto> ConditionOptions = ClothingCatalog.ToOptions(Conditions);
    public static readonly IReadOnlyList<ClothingLookupItemDto> SellingMethodOptions = ClothingCatalog.ToOptions(SellingMethods);

    public static string GetClothingTypeName(KidsClothingType value) => ClothingCatalog.GetName(ClothingTypes, value);
    public static string GetBrandName(KidsClothingBrand value) => ClothingCatalog.GetName(Brands, value);
    public static string GetConditionName(KidsClothingCondition value) => ClothingCatalog.GetName(Conditions, value);
    public static string GetSellingMethodName(KidsClothingSellingMethod value) => ClothingCatalog.GetName(SellingMethods, value);

    public static string GetSizeName(KidsClothingSize value) => ClothingCatalog.GetName(Sizes, value);
    public static string GetSizeNameEn(KidsClothingSize value) => ClothingCatalog.GetNameEn(Sizes, value);
    public static string GetColorName(KidsClothingColor value) => ClothingCatalog.GetName(Colors, value);
    public static string GetColorNameEn(KidsClothingColor value) => ClothingCatalog.GetNameEn(Colors, value);

    public static List<ClothingLookupItemDto> SelectedSizes(IEnumerable<KidsClothingSize> selected) =>
        ClothingCatalog.SelectedOptions(Sizes, selected);

    public static List<ClothingLookupItemDto> SelectedColors(IEnumerable<KidsClothingColor> selected) =>
        ClothingCatalog.SelectedOptions(Colors, selected);
}
