using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Shared.Constants;

public static class HomeApplianceCatalog
{
    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceDeviceType>> DeviceTypes =
        new List<HomeFurnishingLookupEntry<HomeApplianceDeviceType>>
        {
            new(HomeApplianceDeviceType.Refrigerator, "ثلاجة", "Refrigerator"),
            new(HomeApplianceDeviceType.DeepFreezer, "ديب فريزر", "Deep freezer"),
            new(HomeApplianceDeviceType.WashingMachine, "غسالة", "Washing machine"),
            new(HomeApplianceDeviceType.Dishwasher, "غسالة أطباق", "Dishwasher"),
            new(HomeApplianceDeviceType.Cooker, "بوتاجاز", "Cooker"),
            new(HomeApplianceDeviceType.ElectricOven, "فرن كهربائي", "Electric oven"),
            new(HomeApplianceDeviceType.Microwave, "ميكروويف", "Microwave"),
            new(HomeApplianceDeviceType.Hood, "شفاط", "Cooker hood"),
            new(HomeApplianceDeviceType.VacuumCleaner, "مكنسة كهربائية", "Vacuum cleaner"),
            new(HomeApplianceDeviceType.Fan, "مروحة", "Fan"),
            new(HomeApplianceDeviceType.AirConditioner, "تكييف", "Air conditioner"),
            new(HomeApplianceDeviceType.WaterHeater, "سخان", "Water heater"),
            new(HomeApplianceDeviceType.Kettle, "كاتيل", "Kettle"),
            new(HomeApplianceDeviceType.Blender, "خلاط", "Blender"),
            new(HomeApplianceDeviceType.Chopper, "كبة", "Chopper"),
            new(HomeApplianceDeviceType.Juicer, "عصارة", "Juicer"),
            new(HomeApplianceDeviceType.FoodProcessor, "محضر طعام", "Food processor"),
            new(HomeApplianceDeviceType.CoffeeMachine, "ماكينة قهوة", "Coffee machine"),
            new(HomeApplianceDeviceType.AirFryer, "قلاية هوائية", "Air fryer"),
            new(HomeApplianceDeviceType.Iron, "مكواة", "Iron"),
            new(HomeApplianceDeviceType.SewingMachine, "ماكينة خياطة", "Sewing machine"),
            new(HomeApplianceDeviceType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceBrand>> Brands =
        new List<HomeFurnishingLookupEntry<HomeApplianceBrand>>
        {
            new(HomeApplianceBrand.Lg, "LG", "LG"),
            new(HomeApplianceBrand.Samsung, "Samsung", "Samsung"),
            new(HomeApplianceBrand.Toshiba, "Toshiba", "Toshiba"),
            new(HomeApplianceBrand.Sharp, "Sharp", "Sharp"),
            new(HomeApplianceBrand.Fresh, "Fresh", "Fresh"),
            new(HomeApplianceBrand.Unionaire, "Unionaire", "Unionaire"),
            new(HomeApplianceBrand.Zanussi, "Zanussi", "Zanussi"),
            new(HomeApplianceBrand.Bosch, "Bosch", "Bosch"),
            new(HomeApplianceBrand.Philips, "Philips", "Philips"),
            new(HomeApplianceBrand.Moulinex, "Moulinex", "Moulinex"),
            new(HomeApplianceBrand.Tefal, "Tefal", "Tefal"),
            new(HomeApplianceBrand.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceCondition>> Conditions =
        new List<HomeFurnishingLookupEntry<HomeApplianceCondition>>
        {
            new(HomeApplianceCondition.New, "جديد", "New"),
            new(HomeApplianceCondition.UsedExcellent, "مستعمل بحالة ممتازة", "Used - excellent condition"),
            new(HomeApplianceCondition.UsedGood, "مستعمل بحالة جيدة", "Used - good condition")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceWarranty>> Warranties =
        new List<HomeFurnishingLookupEntry<HomeApplianceWarranty>>
        {
            new(HomeApplianceWarranty.Available, "يوجد ضمان", "Available"),
            new(HomeApplianceWarranty.NotAvailable, "لا يوجد ضمان", "Not available")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupEntry<HomeApplianceColor>> Colors =
        new List<HomeFurnishingLookupEntry<HomeApplianceColor>>
        {
            new(HomeApplianceColor.White, "أبيض", "White"),
            new(HomeApplianceColor.Black, "أسود", "Black"),
            new(HomeApplianceColor.Brown, "بني", "Brown"),
            new(HomeApplianceColor.Beige, "بيج", "Beige"),
            new(HomeApplianceColor.Gray, "رمادي", "Gray"),
            new(HomeApplianceColor.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> DeviceTypeOptions =
        HomeFurnishingCatalog.ToOptions(DeviceTypes);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> BrandOptions =
        HomeFurnishingCatalog.ToOptions(Brands);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ConditionOptions =
        HomeFurnishingCatalog.ToOptions(Conditions);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> WarrantyOptions =
        HomeFurnishingCatalog.ToOptions(Warranties);
    public static readonly IReadOnlyList<HomeFurnishingLookupItemDto> ColorOptions =
        HomeFurnishingCatalog.ToOptions(Colors);

    public static string GetDeviceTypeName(HomeApplianceDeviceType value) =>
        HomeFurnishingCatalog.GetName(DeviceTypes, value);
    public static string GetBrandName(HomeApplianceBrand value) =>
        HomeFurnishingCatalog.GetName(Brands, value);
    public static string GetConditionName(HomeApplianceCondition value) =>
        HomeFurnishingCatalog.GetName(Conditions, value);
    public static string GetWarrantyName(HomeApplianceWarranty value) =>
        HomeFurnishingCatalog.GetName(Warranties, value);

    public static string GetColorName(HomeApplianceColor value) =>
        HomeFurnishingCatalog.GetName(Colors, value);
    public static string GetColorNameEn(HomeApplianceColor value) =>
        HomeFurnishingCatalog.GetNameEn(Colors, value);

    public static List<HomeFurnishingLookupItemDto> SelectedColors(
        IEnumerable<HomeApplianceColor> selected) =>
        HomeFurnishingCatalog.SelectedOptions(Colors, selected);
}
