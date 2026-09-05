using Shared.Enums;

namespace Shared.Constants;

public static class WorkshopCraftsmenCatalog
{
    public static readonly IReadOnlyDictionary<WorkshopType, string> WorkshopTypeNames =
        new Dictionary<WorkshopType, string>
        {
            [WorkshopType.Carpentry] = "ورشة نجارة",
            [WorkshopType.Blacksmithing] = "ورشة حدادة",
            [WorkshopType.Aluminum] = "ورشة ألوميتال",
            [WorkshopType.Glass] = "ورشة زجاج",
            [WorkshopType.MarbleAndGranite] = "ورشة رخام وجرانيت",
            [WorkshopType.Furniture] = "ورشة موبيليا",
            [WorkshopType.Kitchens] = "ورشة مطابخ",
            [WorkshopType.Upholstery] = "ورشة تنجيد",
            [WorkshopType.Lathing] = "ورشة خراطة",
            [WorkshopType.Welding] = "ورشة لحام",
            [WorkshopType.CarRepair] = "ورشة سيارات",
            [WorkshopType.CarBodywork] = "ورشة سمكرة",
            [WorkshopType.CarPainting] = "ورشة دهان سيارات",
            [WorkshopType.CarElectrical] = "ورشة كهرباء سيارات",
            [WorkshopType.Motor] = "ورشة موتور",
            [WorkshopType.Motorcycles] = "ورشة دراجات نارية",
            [WorkshopType.ElectricalApplianceRepair] = "ورشة إصلاح أجهزة كهربائية",
            [WorkshopType.AirConditioningAndRefrigeration] = "ورشة تكييف وتبريد",
            [WorkshopType.MetalManufacturing] = "ورشة تصنيع معدني",
            [WorkshopType.PlasticManufacturing] = "ورشة تصنيع بلاستيك",
            [WorkshopType.Clothing] = "ورشة ملابس",
            [WorkshopType.Shoes] = "ورشة أحذية",
            [WorkshopType.Leather] = "ورشة جلود",
            [WorkshopType.Embroidery] = "ورشة تطريز",
            [WorkshopType.Printing] = "ورشة طباعة",
            [WorkshopType.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<ExperienceLevel, string> ExperienceLevelNames =
        new Dictionary<ExperienceLevel, string>
        {
            [ExperienceLevel.LessThanOneYear] = "أقل من سنة",
            [ExperienceLevel.OneToThreeYears] = "1-3 سنوات",
            [ExperienceLevel.ThreeToFiveYears] = "3-5 سنوات",
            [ExperienceLevel.FiveToTenYears] = "5-10 سنوات",
            [ExperienceLevel.MoreThanTenYears] = "أكثر من 10 سنوات"
        };

    public readonly record struct SpecializationEntry(CraftsmanSpecialization Value, string Group, string Name);

    public static readonly IReadOnlyList<SpecializationEntry> Specializations = new List<SpecializationEntry>
    {
        new(CraftsmanSpecialization.FurnitureCarpenter, "النجارة", "نجار أثاث"),
        new(CraftsmanSpecialization.DoorCarpenter, "النجارة", "نجار أبواب"),
        new(CraftsmanSpecialization.KitchenCarpenter, "النجارة", "نجار مطابخ"),

        new(CraftsmanSpecialization.Plumber, "السباكة", "سباك"),
        new(CraftsmanSpecialization.PlumbingInstallation, "السباكة", "تأسيس سباكة"),
        new(CraftsmanSpecialization.PlumbingMaintenance, "السباكة", "صيانة سباكة"),

        new(CraftsmanSpecialization.HomeElectrician, "الكهرباء", "كهربائي منازل"),
        new(CraftsmanSpecialization.IndustrialElectrician, "الكهرباء", "كهربائي صناعي"),

        new(CraftsmanSpecialization.Painter, "الدهانات", "نقاش"),
        new(CraftsmanSpecialization.DecorativePainting, "الدهانات", "دهانات ديكورية"),

        new(CraftsmanSpecialization.Ceramic, "الأرضيات", "سيراميك"),
        new(CraftsmanSpecialization.Porcelain, "الأرضيات", "بورسلين"),
        new(CraftsmanSpecialization.Marble, "الأرضيات", "رخام"),
        new(CraftsmanSpecialization.Granite, "الأرضيات", "جرانيت"),
        new(CraftsmanSpecialization.Parquet, "الأرضيات", "باركيه"),

        new(CraftsmanSpecialization.GypsumBoard, "الجبس", "جبس بورد"),
        new(CraftsmanSpecialization.GypsumDecoration, "الجبس", "ديكورات جبس"),

        new(CraftsmanSpecialization.Blacksmith, "الحدادة", "حداد"),
        new(CraftsmanSpecialization.CrystalBlacksmith, "الحدادة", "حداد كريتال"),

        new(CraftsmanSpecialization.Aluminum, "الألوميتال", "ألوميتال"),

        new(CraftsmanSpecialization.Glass, "الزجاج", "زجاج"),

        new(CraftsmanSpecialization.ElectricWelding, "اللحام", "لحام كهرباء"),
        new(CraftsmanSpecialization.ArgonWelding, "اللحام", "لحام أرجون"),

        new(CraftsmanSpecialization.AirConditioningTechnician, "التكييف", "فني تكييف"),
        new(CraftsmanSpecialization.RefrigerationTechnician, "التكييف", "فني تبريد"),

        new(CraftsmanSpecialization.WashingMachineMaintenance, "الأجهزة المنزلية", "صيانة غسالات"),
        new(CraftsmanSpecialization.RefrigeratorMaintenance, "الأجهزة المنزلية", "صيانة ثلاجات"),
        new(CraftsmanSpecialization.StoveMaintenance, "الأجهزة المنزلية", "صيانة بوتاجازات"),
        new(CraftsmanSpecialization.WaterHeaterMaintenance, "الأجهزة المنزلية", "صيانة سخانات"),

        new(CraftsmanSpecialization.KitchenInstallation, "المطابخ", "تركيب مطابخ"),

        new(CraftsmanSpecialization.CurtainInstallation, "الستائر", "تركيب ستائر"),

        new(CraftsmanSpecialization.SolarEnergyInstallation, "الطاقة الشمسية", "تركيب طاقة شمسية"),

        new(CraftsmanSpecialization.HomeCleaning, "التنظيف", "تنظيف منازل"),
        new(CraftsmanSpecialization.CompanyCleaning, "التنظيف", "تنظيف شركات"),

        new(CraftsmanSpecialization.FurnitureAssembly, "نقل الأثاث", "فك وتركيب أثاث"),
        new(CraftsmanSpecialization.FurnitureMoving, "نقل الأثاث", "نقل أثاث"),

        new(CraftsmanSpecialization.Other, "أخرى", "أخرى")
    };

    private static readonly IReadOnlyDictionary<CraftsmanSpecialization, string> SpecializationNames =
        Specializations.ToDictionary(s => s.Value, s => s.Name);

    public static string GetWorkshopTypeName(WorkshopType type) =>
        WorkshopTypeNames.TryGetValue(type, out var name) ? name : string.Empty;

    public static string GetExperienceLevelName(ExperienceLevel level) =>
        ExperienceLevelNames.TryGetValue(level, out var name) ? name : string.Empty;

    public static string GetSpecializationName(CraftsmanSpecialization specialization) =>
        SpecializationNames.TryGetValue(specialization, out var name) ? name : string.Empty;
}
