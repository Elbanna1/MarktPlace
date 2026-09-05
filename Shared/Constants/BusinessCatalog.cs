using Shared.Enums;

namespace Shared.Constants;

public static class BusinessCatalog
{
    public const string FarmGroupPlant = "نباتية";
    public const string FarmGroupAnimal = "حيوانية";
    public const string FarmGroupAnimalProduction = "إنتاج حيواني";
    public const string FarmGroupAquatic = "مزارع مائية";
    public const string FarmGroupOther = "أخرى";

    public static readonly IReadOnlyList<GroupedEntry<FarmType>> FarmTypes =
        new List<GroupedEntry<FarmType>>
        {
            new(FarmType.Vegetables, FarmGroupPlant, FarmGroupPlant, "مزرعة خضروات"),
            new(FarmType.Fruits, FarmGroupPlant, FarmGroupPlant, "مزرعة فواكه"),
            new(FarmType.Citrus, FarmGroupPlant, FarmGroupPlant, "مزرعة موالح"),
            new(FarmType.Palms, FarmGroupPlant, FarmGroupPlant, "مزرعة نخيل"),
            new(FarmType.Olives, FarmGroupPlant, FarmGroupPlant, "مزرعة زيتون"),
            new(FarmType.MedicinalAndAromaticHerbs, FarmGroupPlant, FarmGroupPlant, "مزرعة أعشاب طبية وعطرية"),
            new(FarmType.FlowersAndOrnamentalPlants, FarmGroupPlant, FarmGroupPlant, "مزرعة زهور ونباتات زينة"),
            new(FarmType.FieldCrops, FarmGroupPlant, FarmGroupPlant, "مزرعة محاصيل حقلية"),
            new(FarmType.Greenhouse, FarmGroupPlant, FarmGroupPlant, "صوبة زراعية"),

            new(FarmType.Poultry, FarmGroupAnimal, FarmGroupAnimal, "مزرعة دواجن"),
            new(FarmType.Cattle, FarmGroupAnimal, FarmGroupAnimal, "مزرعة مواشي"),
            new(FarmType.Sheep, FarmGroupAnimal, FarmGroupAnimal, "مزرعة أغنام"),
            new(FarmType.Goats, FarmGroupAnimal, FarmGroupAnimal, "مزرعة ماعز"),
            new(FarmType.Camels, FarmGroupAnimal, FarmGroupAnimal, "مزرعة جمال"),
            new(FarmType.Rabbits, FarmGroupAnimal, FarmGroupAnimal, "مزرعة أرانب"),

            new(FarmType.Dairy, FarmGroupAnimalProduction, FarmGroupAnimalProduction, "مزرعة ألبان"),
            new(FarmType.Eggs, FarmGroupAnimalProduction, FarmGroupAnimalProduction, "مزرعة بيض"),
            new(FarmType.CalfFattening, FarmGroupAnimalProduction, FarmGroupAnimalProduction, "تسمين عجول"),
            new(FarmType.PoultryFattening, FarmGroupAnimalProduction, FarmGroupAnimalProduction, "تسمين دواجن"),

            new(FarmType.Fish, FarmGroupAquatic, FarmGroupAquatic, "مزرعة أسماك"),
            new(FarmType.Shrimp, FarmGroupAquatic, FarmGroupAquatic, "مزرعة جمبري"),

            new(FarmType.Other, FarmGroupOther, FarmGroupOther, "أخرى")
        };

    public const string CompanyGroupConstruction = "مقاولات";
    public const string CompanyGroupLogistics = "نقل ولوجستيات";
    public const string CompanyGroupCommercial = "تجارية";
    public const string CompanyGroupTechnology = "تكنولوجيا";
    public const string CompanyGroupMarketing = "تسويق ودعاية";
    public const string CompanyGroupFinancial = "مالية ومحاسبة";
    public const string CompanyGroupLegal = "قانونية";
    public const string CompanyGroupEngineering = "هندسية";
    public const string CompanyGroupMedical = "طبية";
    public const string CompanyGroupEducation = "تعليم وتدريب";
    public const string CompanyGroupCleaning = "نظافة وصيانة";
    public const string CompanyGroupOther = "أخرى";

    public static readonly IReadOnlyList<GroupedEntry<CompanyField>> CompanyFields =
        new List<GroupedEntry<CompanyField>>
        {
            new(CompanyField.GeneralContracting, CompanyGroupConstruction, "مقاولات", "مقاولات عامة"),
            new(CompanyField.Finishing, CompanyGroupConstruction, "مقاولات", "تشطيبات"),
            new(CompanyField.Insulation, CompanyGroupConstruction, "مقاولات", "أعمال العزل"),
            new(CompanyField.Demolition, CompanyGroupConstruction, "مقاولات", "أعمال الهدم"),
            new(CompanyField.Excavation, CompanyGroupConstruction, "مقاولات", "أعمال الحفر"),
            new(CompanyField.Concrete, CompanyGroupConstruction, "مقاولات", "أعمال الخرسانة"),

            new(CompanyField.DomesticShipping, CompanyGroupLogistics, "نقل ولوجستيات", "شحن داخلي"),
            new(CompanyField.InternationalShipping, CompanyGroupLogistics, "نقل ولوجستيات", "شحن دولي"),
            new(CompanyField.FurnitureMoving, CompanyGroupLogistics, "نقل ولوجستيات", "نقل أثاث"),
            new(CompanyField.Warehousing, CompanyGroupLogistics, "نقل ولوجستيات", "تخزين"),

            new(CompanyField.Import, CompanyGroupCommercial, "تجارية", "استيراد"),
            new(CompanyField.Export, CompanyGroupCommercial, "تجارية", "تصدير"),
            new(CompanyField.ImportAndExport, CompanyGroupCommercial, "تجارية", "استيراد وتصدير"),
            new(CompanyField.Distribution, CompanyGroupCommercial, "تجارية", "توزيع"),

            new(CompanyField.Software, CompanyGroupTechnology, "تكنولوجيا", "برمجيات"),
            new(CompanyField.WebsiteDevelopment, CompanyGroupTechnology, "تكنولوجيا", "تطوير مواقع إلكترونية"),
            new(CompanyField.MobileApplications, CompanyGroupTechnology, "تكنولوجيا", "تطبيقات الموبايل"),
            new(CompanyField.Networking, CompanyGroupTechnology, "تكنولوجيا", "شبكات"),
            new(CompanyField.CCTV, CompanyGroupTechnology, "تكنولوجيا", "كاميرات مراقبة"),
            new(CompanyField.SecuritySystems, CompanyGroupTechnology, "تكنولوجيا", "أنظمة أمنية"),

            new(CompanyField.Advertising, CompanyGroupMarketing, "تسويق ودعاية", "إعلانات"),
            new(CompanyField.GraphicDesign, CompanyGroupMarketing, "تسويق ودعاية", "تصميم جرافيك"),
            new(CompanyField.Printing, CompanyGroupMarketing, "تسويق ودعاية", "طباعة"),
            new(CompanyField.Photography, CompanyGroupMarketing, "تسويق ودعاية", "تصوير"),
            new(CompanyField.DigitalMarketing, CompanyGroupMarketing, "تسويق ودعاية", "تسويق إلكتروني"),

            new(CompanyField.Accounting, CompanyGroupFinancial, "مالية ومحاسبة", "محاسبة"),
            new(CompanyField.Auditing, CompanyGroupFinancial, "مالية ومحاسبة", "مراجعة حسابات"),
            new(CompanyField.TaxConsulting, CompanyGroupFinancial, "مالية ومحاسبة", "استشارات ضريبية"),

            new(CompanyField.LawFirm, CompanyGroupLegal, "قانونية", "مكتب محاماة"),
            new(CompanyField.LegalConsulting, CompanyGroupLegal, "قانونية", "استشارات قانونية"),

            new(CompanyField.EngineeringOffice, CompanyGroupEngineering, "هندسية", "مكتب هندسي"),
            new(CompanyField.Architecture, CompanyGroupEngineering, "هندسية", "تصميم معماري"),
            new(CompanyField.EngineeringSupervision, CompanyGroupEngineering, "هندسية", "إشراف هندسي"),
            new(CompanyField.Surveying, CompanyGroupEngineering, "هندسية", "أعمال المساحة"),

            new(CompanyField.MedicalSupplies, CompanyGroupMedical, "طبية", "مستلزمات طبية"),
            new(CompanyField.MedicalServices, CompanyGroupMedical, "طبية", "خدمات طبية"),

            new(CompanyField.Training, CompanyGroupEducation, "تعليم وتدريب", "تدريب"),
            new(CompanyField.Courses, CompanyGroupEducation, "تعليم وتدريب", "دورات تدريبية"),
            new(CompanyField.Nurseries, CompanyGroupEducation, "تعليم وتدريب", "حضانات"),
            new(CompanyField.EducationalCenters, CompanyGroupEducation, "تعليم وتدريب", "مراكز تعليمية"),

            new(CompanyField.CleaningCompanies, CompanyGroupCleaning, "نظافة وصيانة", "شركات نظافة"),
            new(CompanyField.PestControl, CompanyGroupCleaning, "نظافة وصيانة", "مكافحة حشرات"),
            new(CompanyField.GeneralMaintenance, CompanyGroupCleaning, "نظافة وصيانة", "صيانة عامة"),

            new(CompanyField.Other, CompanyGroupOther, "أخرى", "أخرى")
        };

    public static readonly IReadOnlyDictionary<ProductionSpecialty, string> ProductionSpecialtyNames =
        new Dictionary<ProductionSpecialty, string>
        {
            [ProductionSpecialty.Food] = "مواد غذائية",
            [ProductionSpecialty.Beverages] = "مشروبات",
            [ProductionSpecialty.Dairy] = "ألبان ومنتجاتها",
            [ProductionSpecialty.BakeryAndSweets] = "مخابز وحلويات",
            [ProductionSpecialty.Packaging] = "تعبئة وتغليف",
            [ProductionSpecialty.Plastic] = "بلاستيك",
            [ProductionSpecialty.PaperAndCardboard] = "ورق وكرتون",
            [ProductionSpecialty.Printing] = "طباعة",
            [ProductionSpecialty.SpinningAndWeaving] = "غزل ونسيج",
            [ProductionSpecialty.ReadymadeClothes] = "ملابس جاهزة",
            [ProductionSpecialty.Shoes] = "أحذية",
            [ProductionSpecialty.Leather] = "جلود",
            [ProductionSpecialty.Furniture] = "أثاث",
            [ProductionSpecialty.Wood] = "أخشاب",
            [ProductionSpecialty.Aluminum] = "ألومنيوم",
            [ProductionSpecialty.IronAndSteel] = "حديد وصلب",
            [ProductionSpecialty.MetalWorking] = "تشغيل معادن",
            [ProductionSpecialty.MachineryAndEquipment] = "ماكينات ومعدات",
            [ProductionSpecialty.ElectricalAppliances] = "أجهزة كهربائية",
            [ProductionSpecialty.Electronics] = "إلكترونيات",
            [ProductionSpecialty.CablesAndWires] = "كابلات وأسلاك",
            [ProductionSpecialty.SanitaryWare] = "أدوات صحية",
            [ProductionSpecialty.Ceramic] = "سيراميك",
            [ProductionSpecialty.MarbleAndGranite] = "رخام وجرانيت",
            [ProductionSpecialty.Glass] = "زجاج",
            [ProductionSpecialty.CementAndBuildingMaterials] = "أسمنت ومواد بناء",
            [ProductionSpecialty.Paints] = "دهانات",
            [ProductionSpecialty.Chemicals] = "كيماويات",
            [ProductionSpecialty.Detergents] = "منظفات",
            [ProductionSpecialty.Cosmetics] = "مستحضرات تجميل",
            [ProductionSpecialty.Pharmaceuticals] = "أدوية",
            [ProductionSpecialty.MedicalSupplies] = "مستلزمات طبية",
            [ProductionSpecialty.Fertilizers] = "أسمدة",
            [ProductionSpecialty.Pesticides] = "مبيدات",
            [ProductionSpecialty.AnimalFeed] = "أعلاف",
            [ProductionSpecialty.AgriculturalProducts] = "منتجات زراعية",
            [ProductionSpecialty.Recycling] = "إعادة تدوير",
            [ProductionSpecialty.CarSpareParts] = "قطع غيار السيارات",
            [ProductionSpecialty.Other] = "أخرى"
        };

    public const int ProductionSpecialtyLastId = (int)ProductionSpecialty.Other;

    public static readonly IReadOnlyDictionary<FarmType, string> FarmTypeNames =
        FarmTypes.ToDictionary(entry => entry.Value, entry => entry.Name);

    public static readonly IReadOnlyDictionary<AvailabilitySeason, string> AvailabilitySeasonNames =
        new Dictionary<AvailabilitySeason, string>
        {
            [AvailabilitySeason.AvailableNow] = "متوفر الآن",
            [AvailabilitySeason.AllYear] = "طوال العام",
            [AvailabilitySeason.Seasonal] = "موسمي"
        };

    public static readonly IReadOnlyDictionary<FarmingMethod, string> FarmingMethodNames =
        new Dictionary<FarmingMethod, string>
        {
            [FarmingMethod.Organic] = "عضوي",
            [FarmingMethod.Conventional] = "تقليدي",
            [FarmingMethod.Mixed] = "مختلط",
            [FarmingMethod.NotSpecified] = "غير محدد"
        };

    public static readonly IReadOnlyDictionary<CompanyField, string> CompanyFieldNames =
        CompanyFields.ToDictionary(entry => entry.Value, entry => entry.Name);

    public static readonly IReadOnlyDictionary<SaleType, string> SaleTypeNames =
        new Dictionary<SaleType, string>
        {
            [SaleType.Retail] = "قطاعي",
            [SaleType.Wholesale] = "جملة",
            [SaleType.Both] = "قطاعي وجملة"
        };

    public readonly record struct GroupedEntry<TValue>(TValue Value, string Group, string GroupAr, string Name)
        where TValue : struct, Enum;

    public const string SupplierGroupIndustrial = "موردو الصناعة";
    public const string SupplierGroupAgriculture = "موردو الزراعة";
    public const string SupplierGroupLivestock = "موردو الثروة الحيوانية";
    public const string SupplierGroupConstruction = "موردو مواد البناء";
    public const string SupplierGroupBusinessRetail = "موردو التجارة والتجزئة";
    public const string SupplierGroupOther = "أخرى";

    public static readonly IReadOnlyList<GroupedEntry<SupplierType>> SupplierTypes =
        new List<GroupedEntry<SupplierType>>
        {
            new(SupplierType.RawMaterials, SupplierGroupIndustrial, "موردو الصناعة", "مواد خام"),
            new(SupplierType.Chemicals, SupplierGroupIndustrial, "موردو الصناعة", "كيماويات"),
            new(SupplierType.Plastic, SupplierGroupIndustrial, "موردو الصناعة", "بلاستيك"),
            new(SupplierType.PaperAndCardboard, SupplierGroupIndustrial, "موردو الصناعة", "ورق وكرتون"),
            new(SupplierType.Packaging, SupplierGroupIndustrial, "موردو الصناعة", "مواد تعبئة وتغليف"),
            new(SupplierType.IronAndSteel, SupplierGroupIndustrial, "موردو الصناعة", "حديد وصلب"),
            new(SupplierType.Aluminum, SupplierGroupIndustrial, "موردو الصناعة", "ألومنيوم"),
            new(SupplierType.MachineryAndEquipment, SupplierGroupIndustrial, "موردو الصناعة", "ماكينات ومعدات"),
            new(SupplierType.IndustrialSpareParts, SupplierGroupIndustrial, "موردو الصناعة", "قطع غيار صناعية"),

            new(SupplierType.SeedsAndSeedlings, SupplierGroupAgriculture, "موردو الزراعة", "بذور وشتلات"),
            new(SupplierType.Fertilizers, SupplierGroupAgriculture, "موردو الزراعة", "أسمدة"),
            new(SupplierType.Pesticides, SupplierGroupAgriculture, "موردو الزراعة", "مبيدات"),
            new(SupplierType.IrrigationSupplies, SupplierGroupAgriculture, "موردو الزراعة", "مستلزمات ري"),
            new(SupplierType.AgriculturalEquipment, SupplierGroupAgriculture, "موردو الزراعة", "معدات زراعية"),
            new(SupplierType.Greenhouses, SupplierGroupAgriculture, "موردو الزراعة", "صوب زراعية"),

            new(SupplierType.AnimalFeed, SupplierGroupLivestock, "موردو الثروة الحيوانية", "أعلاف"),
            new(SupplierType.VeterinaryMedicine, SupplierGroupLivestock, "موردو الثروة الحيوانية", "أدوية بيطرية"),
            new(SupplierType.PoultrySupplies, SupplierGroupLivestock, "موردو الثروة الحيوانية", "مستلزمات دواجن"),
            new(SupplierType.CattleSupplies, SupplierGroupLivestock, "موردو الثروة الحيوانية", "مستلزمات مواشي"),
            new(SupplierType.FishFarmingSupplies, SupplierGroupLivestock, "موردو الثروة الحيوانية", "مستلزمات استزراع سمكي"),

            new(SupplierType.CementAndBuildingMaterials, SupplierGroupConstruction, "موردو مواد البناء", "أسمنت ومواد بناء"),
            new(SupplierType.BricksSandAndGravel, SupplierGroupConstruction, "موردو مواد البناء", "طوب ورمل وزلط"),
            new(SupplierType.ReinforcementSteel, SupplierGroupConstruction, "موردو مواد البناء", "حديد تسليح"),
            new(SupplierType.Ceramic, SupplierGroupConstruction, "موردو مواد البناء", "سيراميك"),
            new(SupplierType.MarbleAndGranite, SupplierGroupConstruction, "موردو مواد البناء", "رخام وجرانيت"),
            new(SupplierType.Glass, SupplierGroupConstruction, "موردو مواد البناء", "زجاج"),
            new(SupplierType.Paints, SupplierGroupConstruction, "موردو مواد البناء", "دهانات"),
            new(SupplierType.SanitaryWare, SupplierGroupConstruction, "موردو مواد البناء", "أدوات صحية"),
            new(SupplierType.CablesAndWires, SupplierGroupConstruction, "موردو مواد البناء", "كابلات وأسلاك"),
            new(SupplierType.Wood, SupplierGroupConstruction, "موردو مواد البناء", "أخشاب"),

            new(SupplierType.FoodProducts, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "مواد غذائية"),
            new(SupplierType.Beverages, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "مشروبات"),
            new(SupplierType.Dairy, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "ألبان ومنتجاتها"),
            new(SupplierType.BakeryAndSweets, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "مخابز وحلويات"),
            new(SupplierType.Detergents, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "منظفات"),
            new(SupplierType.Cosmetics, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "مستحضرات تجميل"),
            new(SupplierType.Pharmaceuticals, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "أدوية"),
            new(SupplierType.MedicalSupplies, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "مستلزمات طبية"),
            new(SupplierType.ReadymadeClothes, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "ملابس جاهزة"),
            new(SupplierType.Shoes, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "أحذية"),
            new(SupplierType.Leather, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "جلود"),
            new(SupplierType.SpinningAndWeaving, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "غزل ونسيج"),
            new(SupplierType.Furniture, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "أثاث"),
            new(SupplierType.ElectricalAppliances, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "أجهزة كهربائية"),
            new(SupplierType.Electronics, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "إلكترونيات"),
            new(SupplierType.OfficeSupplies, SupplierGroupBusinessRetail, "موردو التجارة والتجزئة", "مستلزمات مكتبية"),

            new(SupplierType.Other, SupplierGroupOther, "أخرى", "أخرى")
        };

    public const string TradeGroupFood = "المواد الغذائية";
    public const string TradeGroupAgriculturalProducts = "المنتجات الزراعية";
    public const string TradeGroupFashion = "الأزياء";
    public const string TradeGroupHomeProducts = "المنتجات المنزلية";
    public const string TradeGroupBuildingMaterials = "مواد البناء";
    public const string TradeGroupAutomotive = "السيارات";
    public const string TradeGroupAgricultureLivestock = "الزراعة والثروة الحيوانية";
    public const string TradeGroupOther = "أخرى";

    public static readonly IReadOnlyList<GroupedEntry<TradeType>> TradeTypes =
        new List<GroupedEntry<TradeType>>
        {
            new(TradeType.FoodProducts, TradeGroupFood, "المواد الغذائية", "مواد غذائية"),
            new(TradeType.Beverages, TradeGroupFood, "المواد الغذائية", "مشروبات"),
            new(TradeType.Dairy, TradeGroupFood, "المواد الغذائية", "ألبان ومنتجاتها"),
            new(TradeType.BakeryAndSweets, TradeGroupFood, "المواد الغذائية", "مخابز وحلويات"),
            new(TradeType.MeatAndPoultry, TradeGroupFood, "المواد الغذائية", "لحوم ودواجن"),
            new(TradeType.Fish, TradeGroupFood, "المواد الغذائية", "أسماك"),
            new(TradeType.GrainsAndLegumes, TradeGroupFood, "المواد الغذائية", "حبوب وبقوليات"),
            new(TradeType.SpicesAndHerbs, TradeGroupFood, "المواد الغذائية", "توابل وأعشاب"),

            new(TradeType.Vegetables, TradeGroupAgriculturalProducts, "المنتجات الزراعية", "خضروات"),
            new(TradeType.Fruits, TradeGroupAgriculturalProducts, "المنتجات الزراعية", "فواكه"),
            new(TradeType.Citrus, TradeGroupAgriculturalProducts, "المنتجات الزراعية", "موالح"),
            new(TradeType.Dates, TradeGroupAgriculturalProducts, "المنتجات الزراعية", "تمور"),
            new(TradeType.Olives, TradeGroupAgriculturalProducts, "المنتجات الزراعية", "زيتون"),
            new(TradeType.Herbs, TradeGroupAgriculturalProducts, "المنتجات الزراعية", "أعشاب"),
            new(TradeType.Flowers, TradeGroupAgriculturalProducts, "المنتجات الزراعية", "زهور"),
            new(TradeType.FieldCrops, TradeGroupAgriculturalProducts, "المنتجات الزراعية", "محاصيل حقلية"),

            new(TradeType.ReadymadeClothes, TradeGroupFashion, "الأزياء", "ملابس جاهزة"),
            new(TradeType.Shoes, TradeGroupFashion, "الأزياء", "أحذية"),
            new(TradeType.Leather, TradeGroupFashion, "الأزياء", "جلود"),
            new(TradeType.SpinningAndWeaving, TradeGroupFashion, "الأزياء", "غزل ونسيج"),

            new(TradeType.Furniture, TradeGroupHomeProducts, "المنتجات المنزلية", "أثاث"),
            new(TradeType.ElectricalAppliances, TradeGroupHomeProducts, "المنتجات المنزلية", "أجهزة كهربائية"),
            new(TradeType.Electronics, TradeGroupHomeProducts, "المنتجات المنزلية", "إلكترونيات"),
            new(TradeType.HouseholdTools, TradeGroupHomeProducts, "المنتجات المنزلية", "أدوات منزلية"),
            new(TradeType.Detergents, TradeGroupHomeProducts, "المنتجات المنزلية", "منظفات"),
            new(TradeType.Cosmetics, TradeGroupHomeProducts, "المنتجات المنزلية", "مستحضرات تجميل"),

            new(TradeType.CementAndBuildingMaterials, TradeGroupBuildingMaterials, "مواد البناء", "أسمنت ومواد بناء"),
            new(TradeType.IronAndSteel, TradeGroupBuildingMaterials, "مواد البناء", "حديد وصلب"),
            new(TradeType.Aluminum, TradeGroupBuildingMaterials, "مواد البناء", "ألومنيوم"),
            new(TradeType.Ceramic, TradeGroupBuildingMaterials, "مواد البناء", "سيراميك"),
            new(TradeType.MarbleAndGranite, TradeGroupBuildingMaterials, "مواد البناء", "رخام وجرانيت"),
            new(TradeType.Glass, TradeGroupBuildingMaterials, "مواد البناء", "زجاج"),
            new(TradeType.Paints, TradeGroupBuildingMaterials, "مواد البناء", "دهانات"),
            new(TradeType.SanitaryWare, TradeGroupBuildingMaterials, "مواد البناء", "أدوات صحية"),
            new(TradeType.Wood, TradeGroupBuildingMaterials, "مواد البناء", "أخشاب"),

            new(TradeType.SpareParts, TradeGroupAutomotive, "السيارات", "قطع غيار"),
            new(TradeType.Tyres, TradeGroupAutomotive, "السيارات", "إطارات"),
            new(TradeType.OilsAndLubricants, TradeGroupAutomotive, "السيارات", "زيوت وشحوم"),
            new(TradeType.Batteries, TradeGroupAutomotive, "السيارات", "بطاريات"),

            new(TradeType.AnimalFeed, TradeGroupAgricultureLivestock, "الزراعة والثروة الحيوانية", "أعلاف"),
            new(TradeType.Fertilizers, TradeGroupAgricultureLivestock, "الزراعة والثروة الحيوانية", "أسمدة"),
            new(TradeType.Pesticides, TradeGroupAgricultureLivestock, "الزراعة والثروة الحيوانية", "مبيدات"),
            new(TradeType.SeedsAndSeedlings, TradeGroupAgricultureLivestock, "الزراعة والثروة الحيوانية", "بذور وشتلات"),
            new(TradeType.LivestockAndPoultry, TradeGroupAgricultureLivestock, "الزراعة والثروة الحيوانية", "مواشي ودواجن"),

            new(TradeType.Other, TradeGroupOther, "أخرى", "أخرى")
        };

    private static readonly IReadOnlyDictionary<SupplierType, string> SupplierTypeNames =
        SupplierTypes.ToDictionary(entry => entry.Value, entry => entry.Name);

    private static readonly IReadOnlyDictionary<TradeType, string> TradeTypeNames =
        TradeTypes.ToDictionary(entry => entry.Value, entry => entry.Name);

    public static string GetProductionSpecialtyName(ProductionSpecialty value) =>
        ProductionSpecialtyNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static string GetFarmTypeGroup(FarmType value) =>
        FarmTypes.FirstOrDefault(entry => entry.Value == value).Group ?? string.Empty;

    public static string GetFarmTypeName(FarmType value) =>
        FarmTypeNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static string GetAvailabilitySeasonName(AvailabilitySeason value) =>
        AvailabilitySeasonNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static string GetFarmingMethodName(FarmingMethod value) =>
        FarmingMethodNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static string GetCompanyFieldGroup(CompanyField value) =>
        CompanyFields.FirstOrDefault(entry => entry.Value == value).Group ?? string.Empty;

    public static string GetCompanyFieldName(CompanyField value) =>
        CompanyFieldNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static string GetSupplierTypeName(SupplierType value) =>
        SupplierTypeNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static string GetTradeTypeName(TradeType value) =>
        TradeTypeNames.TryGetValue(value, out var name) ? name : string.Empty;

    public static string GetSaleTypeName(SaleType value) =>
        SaleTypeNames.TryGetValue(value, out var name) ? name : string.Empty;
}
