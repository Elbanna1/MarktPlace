using Shared.Enums;

namespace Shared.Constants;

public static class SupplierCatalog
{
    public readonly record struct Entry(
        SupplierSpecialization Value, string Group, string GroupAr, string Name, string NameEn);

    public const string GroupIndustrial = "موردو الصناعة";
    public const string GroupAgricultural = "موردو الزراعة";
    public const string GroupLivestock = "موردو الثروة الحيوانية";
    public const string GroupConstruction = "موردو مواد البناء";
    public const string GroupBusinessSupplies = "مستلزمات الأنشطة التجارية";
    public const string GroupOther = "أخرى";

    private const string GroupIndustrialAr = "موردو الصناعة";
    private const string GroupAgriculturalAr = "موردو الزراعة";
    private const string GroupLivestockAr = "موردو الثروة الحيوانية";
    private const string GroupConstructionAr = "موردو مواد البناء";
    private const string GroupBusinessSuppliesAr = "مستلزمات الأنشطة التجارية";
    private const string GroupOtherAr = "أخرى";

    public static readonly IReadOnlyList<Entry> Specializations = new List<Entry>
    {
        new(SupplierSpecialization.PlasticRawMaterials, GroupIndustrial, GroupIndustrialAr, "خامات بلاستيك", "Plastic Raw Materials"),
        new(SupplierSpecialization.ChemicalRawMaterials, GroupIndustrial, GroupIndustrialAr, "خامات كيماوية", "Chemical Raw Materials"),
        new(SupplierSpecialization.FoodRawMaterials, GroupIndustrial, GroupIndustrialAr, "خامات غذائية", "Food Raw Materials"),
        new(SupplierSpecialization.PackagingMaterials, GroupIndustrial, GroupIndustrialAr, "مواد تعبئة وتغليف", "Packaging Materials"),
        new(SupplierSpecialization.Textiles, GroupIndustrial, GroupIndustrialAr, "منسوجات", "Textiles"),
        new(SupplierSpecialization.Leather, GroupIndustrial, GroupIndustrialAr, "جلود", "Leather"),
        new(SupplierSpecialization.Wood, GroupIndustrial, GroupIndustrialAr, "أخشاب", "Wood"),
        new(SupplierSpecialization.Steel, GroupIndustrial, GroupIndustrialAr, "حديد وصلب", "Steel"),
        new(SupplierSpecialization.Aluminum, GroupIndustrial, GroupIndustrialAr, "ألومنيوم", "Aluminum"),
        new(SupplierSpecialization.Glass, GroupIndustrial, GroupIndustrialAr, "زجاج", "Glass"),
        new(SupplierSpecialization.Paint, GroupIndustrial, GroupIndustrialAr, "دهانات", "Paint"),
        new(SupplierSpecialization.BuildingMaterials, GroupIndustrial, GroupIndustrialAr, "مواد بناء", "Building Materials"),
        new(SupplierSpecialization.MarbleAndGranite, GroupIndustrial, GroupIndustrialAr, "رخام وجرانيت", "Marble & Granite"),
        new(SupplierSpecialization.FactorySupplies, GroupIndustrial, GroupIndustrialAr, "مستلزمات مصانع", "Factory Supplies"),
        new(SupplierSpecialization.MachineSpareParts, GroupIndustrial, GroupIndustrialAr, "قطع غيار ماكينات", "Machine Spare Parts"),
        new(SupplierSpecialization.IndustrialEquipment, GroupIndustrial, GroupIndustrialAr, "معدات صناعية", "Industrial Equipment"),

        new(SupplierSpecialization.Seeds, GroupAgricultural, GroupAgriculturalAr, "بذور", "Seeds"),
        new(SupplierSpecialization.Seedlings, GroupAgricultural, GroupAgriculturalAr, "شتلات", "Seedlings"),
        new(SupplierSpecialization.Fertilizers, GroupAgricultural, GroupAgriculturalAr, "أسمدة", "Fertilizers"),
        new(SupplierSpecialization.Pesticides, GroupAgricultural, GroupAgriculturalAr, "مبيدات", "Pesticides"),
        new(SupplierSpecialization.AnimalFeed, GroupAgricultural, GroupAgriculturalAr, "أعلاف", "Animal Feed"),
        new(SupplierSpecialization.AgriculturalEquipment, GroupAgricultural, GroupAgriculturalAr, "معدات زراعية", "Agricultural Equipment"),
        new(SupplierSpecialization.IrrigationSystems, GroupAgricultural, GroupAgriculturalAr, "أنظمة ري", "Irrigation Systems"),
        new(SupplierSpecialization.Greenhouses, GroupAgricultural, GroupAgriculturalAr, "صوب زراعية", "Greenhouses"),
        new(SupplierSpecialization.FarmSupplies, GroupAgricultural, GroupAgriculturalAr, "مستلزمات مزارع", "Farm Supplies"),

        new(SupplierSpecialization.LivestockAnimalFeed, GroupLivestock, GroupLivestockAr, "أعلاف", "Animal Feed"),
        new(SupplierSpecialization.VeterinaryMedicines, GroupLivestock, GroupLivestockAr, "أدوية بيطرية", "Veterinary Medicines"),
        new(SupplierSpecialization.PoultrySupplies, GroupLivestock, GroupLivestockAr, "مستلزمات دواجن", "Poultry Supplies"),
        new(SupplierSpecialization.LivestockFarmSupplies, GroupLivestock, GroupLivestockAr, "مستلزمات مزارع", "Farm Supplies"),

        new(SupplierSpecialization.Cement, GroupConstruction, GroupConstructionAr, "أسمنت", "Cement"),
        new(SupplierSpecialization.ConstructionSteel, GroupConstruction, GroupConstructionAr, "حديد تسليح", "Steel"),
        new(SupplierSpecialization.Bricks, GroupConstruction, GroupConstructionAr, "طوب", "Bricks"),
        new(SupplierSpecialization.SanitaryWare, GroupConstruction, GroupConstructionAr, "أدوات صحية", "Sanitary Ware"),
        new(SupplierSpecialization.ElectricalMaterials, GroupConstruction, GroupConstructionAr, "مواد كهربائية", "Electrical Materials"),
        new(SupplierSpecialization.ConstructionEquipment, GroupConstruction, GroupConstructionAr, "معدات بناء", "Construction Equipment"),

        new(SupplierSpecialization.RestaurantSupplies, GroupBusinessSupplies, GroupBusinessSuppliesAr, "مستلزمات مطاعم", "Restaurant Supplies"),
        new(SupplierSpecialization.CafeSupplies, GroupBusinessSupplies, GroupBusinessSuppliesAr, "مستلزمات كافيهات", "Cafe Supplies"),
        new(SupplierSpecialization.SupermarketSupplies, GroupBusinessSupplies, GroupBusinessSuppliesAr, "مستلزمات سوبر ماركت", "Supermarket Supplies"),
        new(SupplierSpecialization.OfficeSupplies, GroupBusinessSupplies, GroupBusinessSuppliesAr, "مستلزمات مكتبية", "Office Supplies"),
        new(SupplierSpecialization.CompanySupplies, GroupBusinessSupplies, GroupBusinessSuppliesAr, "مستلزمات شركات", "Company Supplies"),

        new(SupplierSpecialization.Other, GroupOther, GroupOtherAr, "أخرى", "Other")
    };

    private static readonly IReadOnlyDictionary<SupplierSpecialization, Entry> ByValue =
        Specializations.ToDictionary(entry => entry.Value);

    public static string GetName(SupplierSpecialization value) =>
        ByValue.TryGetValue(value, out var entry) ? entry.Name : string.Empty;

    public static string GetGroup(SupplierSpecialization value) =>
        ByValue.TryGetValue(value, out var entry) ? entry.Group : string.Empty;

    public static string GetGroupAr(SupplierSpecialization value) =>
        ByValue.TryGetValue(value, out var entry) ? entry.GroupAr : string.Empty;
}
