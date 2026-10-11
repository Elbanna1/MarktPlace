using Shared.Enums;

namespace Shared.Constants;

public static class WorkshopCraftsmenCatalog
{
    public static readonly IReadOnlyDictionary<WorkshopType, string> WorkshopTypeNames =
        new Dictionary<WorkshopType, string>
        {
            [WorkshopType.Carpentry] = "ورشة باب وشباك",
            [WorkshopType.Blacksmithing] = "حداد",
            [WorkshopType.Aluminum] = "ورشة ألوميتال",
            [WorkshopType.Glass] = "ورشة زجاج",
            [WorkshopType.MarbleAndGranite] = "ورشة رخام وجرانيت",
            [WorkshopType.Furniture] = "نجار موبيليا",
            [WorkshopType.Kitchens] = "ورشة ألوميتال ومطابخ",
            [WorkshopType.Upholstery] = "ورشة تنجيد إفرنجي",
            [WorkshopType.TraditionalUpholstery] = "ورشة تنجيد بلدي",
            [WorkshopType.Lathing] = "ورشة خراطة",
            [WorkshopType.Welding] = "ورشة لحام",
            [WorkshopType.CarRepair] = "مراكز صيانة",
            [WorkshopType.CarBodywork] = "ورشة سمكرة",
            [WorkshopType.CarPainting] = "دوكو سيارات",
            [WorkshopType.CarElectrical] = "ورشة كهرباء سيارات",
            [WorkshopType.Motor] = "ميكانيكا سيارات",
            [WorkshopType.Motorcycles] = "ورشة دراجات نارية",
            [WorkshopType.ElectricalApplianceRepair] = "صيانة أجهزة منزلية",
            [WorkshopType.AirConditioningAndRefrigeration] = "ورشة تكييف وتبريد",
            [WorkshopType.MetalManufacturing] = "لحام أرجون ومعادن",
            [WorkshopType.PlasticManufacturing] = "ورشة تصنيع بلاستيك",
            [WorkshopType.CncEngraving] = "الحفر بالكمبيوتر CNC",
            [WorkshopType.Clothing] = "مشغل ملابس",
            [WorkshopType.Shoes] = "ورشة أحذية",
            [WorkshopType.Leather] = "ورشة جلود",
            [WorkshopType.Embroidery] = "ورشة تطريز",
            [WorkshopType.Curtains] = "ورشة ستائر",
            [WorkshopType.Printing] = "مطابع ومكتبات",
            [WorkshopType.Other] = "أخرى"
        };

    public static readonly IReadOnlySet<WorkshopType> RetiredWorkshopTypes = new HashSet<WorkshopType>
    {
        WorkshopType.Aluminum,
        WorkshopType.PlasticManufacturing,
        WorkshopType.Embroidery
    };

    public static readonly IReadOnlySet<WorkshopType> InactiveWorkshopTypeRows = new HashSet<WorkshopType>
    {
        WorkshopType.PlasticManufacturing,
        WorkshopType.Embroidery
    };

    public static readonly IReadOnlyList<int> RetiredWorkshopTypeIds =
        RetiredWorkshopTypes.Select(t => (int)t).Order().ToList();

    public const string RetiredChoiceMessage = "الاختيار ده مبقاش متاح للإعلانات الجديدة، من فضلك اختار نوع تاني.";

    public static readonly IReadOnlyDictionary<ExperienceLevel, string> ExperienceLevelNames =
        new Dictionary<ExperienceLevel, string>
        {
            [ExperienceLevel.LessThanOneYear] = "أقل من سنة",
            [ExperienceLevel.OneToThreeYears] = "1-3 سنوات",
            [ExperienceLevel.ThreeToFiveYears] = "3-5 سنوات",
            [ExperienceLevel.FiveToTenYears] = "5-10 سنوات",
            [ExperienceLevel.MoreThanTenYears] = "أكثر من 10 سنوات"
        };

    public readonly record struct SpecializationEntry(
        CraftsmanSpecialization Value, string Group, string Name, bool IsActive = true);

    public static readonly IReadOnlyList<SpecializationEntry> Specializations = new List<SpecializationEntry>
    {
        new(CraftsmanSpecialization.FurnitureCarpenter, "النجارة", "نجار أثاث"),
        new(CraftsmanSpecialization.DoorCarpenter, "النجارة", "نجار أبواب"),
        new(CraftsmanSpecialization.KitchenCarpenter, "النجارة", "نجار مطابخ"),

        new(CraftsmanSpecialization.Plumber, "السباكة", "سباكة"),
        new(CraftsmanSpecialization.PlumbingInstallation, "السباكة", "تأسيس سباكة", IsActive: false),
        new(CraftsmanSpecialization.PlumbingMaintenance, "السباكة", "صيانة سباكة", IsActive: false),

        new(CraftsmanSpecialization.HomeElectrician, "الكهرباء", "كهربائي منازل و3 فاز"),
        new(CraftsmanSpecialization.IndustrialElectrician, "الكهرباء", "كهربائي صناعي", IsActive: false),

        new(CraftsmanSpecialization.Painter, "الدهانات", "نقاش"),
        new(CraftsmanSpecialization.DecorativePainting, "الدهانات", "دهانات ديكورية", IsActive: false),

        new(CraftsmanSpecialization.Ceramic, "الأرضيات", "سيراميك وبورسلين"),
        new(CraftsmanSpecialization.Porcelain, "الأرضيات", "بورسلين", IsActive: false),
        new(CraftsmanSpecialization.Marble, "الأرضيات", "رخام وجرانيت"),
        new(CraftsmanSpecialization.Granite, "الأرضيات", "جرانيت", IsActive: false),
        new(CraftsmanSpecialization.Parquet, "الأرضيات", "باركيه"),
        new(CraftsmanSpecialization.Epoxy, "الأرضيات", "إيبوكسي"),

        new(CraftsmanSpecialization.GypsumBoard, "الجبس", "جبس بورد وبلدي"),
        new(CraftsmanSpecialization.GypsumDecoration, "الجبس", "ديكورات جبس"),

        new(CraftsmanSpecialization.Blacksmith, "الحدادة", "حداد"),
        new(CraftsmanSpecialization.CrystalBlacksmith, "الحدادة", "حداد كريتال", IsActive: false),

        new(CraftsmanSpecialization.Aluminum, "الألوميتال", "ألوميتال"),

        new(CraftsmanSpecialization.Glass, "الزجاج", "زجاج"),

        new(CraftsmanSpecialization.ElectricWelding, "اللحام", "لحام كهرباء", IsActive: false),
        new(CraftsmanSpecialization.ArgonWelding, "اللحام", "لحام أرجون"),

        new(CraftsmanSpecialization.AirConditioningTechnician, "التكييف", "تكييف وتبريد"),
        new(CraftsmanSpecialization.RefrigerationTechnician, "التكييف", "فني تبريد", IsActive: false),

        new(CraftsmanSpecialization.WashingMachineMaintenance, "الأجهزة المنزلية", "صيانة غسالات", IsActive: false),
        new(CraftsmanSpecialization.RefrigeratorMaintenance, "الأجهزة المنزلية", "صيانة ثلاجات", IsActive: false),
        new(CraftsmanSpecialization.StoveMaintenance, "الأجهزة المنزلية", "صيانة بوتاجازات"),
        new(CraftsmanSpecialization.WaterHeaterMaintenance, "الأجهزة المنزلية", "صيانة سخانات", IsActive: false),

        new(CraftsmanSpecialization.KitchenInstallation, "المطابخ", "تركيب مطابخ"),

        new(CraftsmanSpecialization.CurtainInstallation, "الستائر", "تركيب ستائر"),

        new(CraftsmanSpecialization.SolarEnergyInstallation, "الطاقة الشمسية", "تركيب طاقة شمسية", IsActive: false),

        new(CraftsmanSpecialization.HomeCleaning, "التنظيف", "تنظيف منازل"),
        new(CraftsmanSpecialization.CompanyCleaning, "التنظيف", "تنظيف شركات"),

        new(CraftsmanSpecialization.FurnitureAssembly, "نقل الأثاث", "فك وتركيب أثاث"),
        new(CraftsmanSpecialization.FurnitureMoving, "نقل الأثاث", "نقل أثاث"),

        new(CraftsmanSpecialization.Pneumatics, "النيوماتيك", "نيوماتيك"),

        new(CraftsmanSpecialization.SafesAndLocksmith, "الخزائن والمفاتيح", "فني خزائن ومفاتيح"),

        new(CraftsmanSpecialization.Other, "أخرى", "أخرى")
    };

    public static readonly IReadOnlyDictionary<CraftsmanSpecialization, CraftsmanSpecialization> MergedSpecializations =
        new Dictionary<CraftsmanSpecialization, CraftsmanSpecialization>
        {
            [CraftsmanSpecialization.PlumbingInstallation] = CraftsmanSpecialization.Plumber,
            [CraftsmanSpecialization.PlumbingMaintenance] = CraftsmanSpecialization.Plumber,
            [CraftsmanSpecialization.IndustrialElectrician] = CraftsmanSpecialization.HomeElectrician,
            [CraftsmanSpecialization.Porcelain] = CraftsmanSpecialization.Ceramic,
            [CraftsmanSpecialization.Granite] = CraftsmanSpecialization.Marble,
            [CraftsmanSpecialization.RefrigerationTechnician] = CraftsmanSpecialization.AirConditioningTechnician
        };

    private static readonly IReadOnlySet<CraftsmanSpecialization> ActiveSpecializations =
        Specializations.Where(s => s.IsActive).Select(s => s.Value).ToHashSet();

    public static bool IsActiveWorkshopType(WorkshopType type) =>
        WorkshopTypeNames.ContainsKey(type) && !RetiredWorkshopTypes.Contains(type);

    public static CraftsmanSpecialization CanonicalSpecialization(CraftsmanSpecialization specialization) =>
        MergedSpecializations.TryGetValue(specialization, out var target) ? target : specialization;

    public static bool IsActiveSpecialization(CraftsmanSpecialization specialization) =>
        ActiveSpecializations.Contains(CanonicalSpecialization(specialization));

    private static readonly IReadOnlyDictionary<CraftsmanSpecialization, string> SpecializationNames =
        Specializations.ToDictionary(s => s.Value, s => s.Name);

    public static string GetWorkshopTypeName(WorkshopType type) =>
        WorkshopTypeNames.TryGetValue(type, out var name) ? name : string.Empty;

    public static string GetExperienceLevelName(ExperienceLevel level) =>
        ExperienceLevelNames.TryGetValue(level, out var name) ? name : string.Empty;

    public static string GetSpecializationName(CraftsmanSpecialization specialization) =>
        SpecializationNames.TryGetValue(specialization, out var name) ? name : string.Empty;
}
