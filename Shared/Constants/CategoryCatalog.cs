using Shared.Enums;

namespace Shared.Constants;

public sealed record CategoryEntry(CategoryType Id, string Name, string NameAr, bool OpensFormDirectly = false);

public sealed record SubCategoryEntry(SubCategoryType Id, CategoryType Category, string Name, string NameAr);

public static class CategoryCatalog
{
    public static readonly IReadOnlyList<CategoryEntry> Categories =
    [
        new(CategoryType.Cars, "Cars", "سيارات"),
        new(CategoryType.WorkshopsAndCraftsmen, "Workshops & Craftsmen", "دليل محافظتي"),
        new(CategoryType.LostAndFound, "Lost & Found", "المفقودات"),
        new(CategoryType.Business, "Business", "رجال أعمال"),
        new(CategoryType.Jobs, "Jobs", "الوظائف"),
        new(CategoryType.Animals, "Animals", "الحيوانات"),
        new(CategoryType.Antiques, "Antiques", "التحف والأنتيكات"),
        new(CategoryType.Clothing, "Clothing", "الملابس"),
        new(CategoryType.OnlineShopping, "Online Shopping", "التسوق أونلاين"),
        new(CategoryType.HomeFurnishing, "Home Furnishing", "افرش بيتك"),
        new(CategoryType.RealEstate, "Real Estate", "عقارات"),
        new(CategoryType.Charity, "Charity", "بوابة الخيرات"),
        new(CategoryType.Sehhaty, "Sehhaty", SehhatyCatalog.CategoryName),
        new(CategoryType.Offices, "Offices", OfficesCatalog.CategoryName),
        new(CategoryType.RestaurantsAndCafes, "Restaurants & Cafes", RestaurantsAndCafesCatalog.CategoryName,
            OpensFormDirectly: true)
    ];

    public static readonly IReadOnlyList<SubCategoryEntry> SubCategories =
    [
        new(SubCategoryType.Private, CategoryType.Cars, "Private", "ملاكي"),
        new(SubCategoryType.Taxi, CategoryType.Cars, "Taxi", "أجرة"),
        new(SubCategoryType.Motorcycles, CategoryType.Cars, "Motorcycles", "موتوسيكلات"),
        new(SubCategoryType.HeavyEquipment, CategoryType.Cars, "Heavy Equipment", "لوادر ومعدات ثقيلة"),

        new(SubCategoryType.Workshops, CategoryType.WorkshopsAndCraftsmen, "Workshops", "الورش"),
        new(SubCategoryType.Craftsmen, CategoryType.WorkshopsAndCraftsmen, "Craftsmen", "الحرفيين"),
        new(SubCategoryType.CommercialShops, CategoryType.WorkshopsAndCraftsmen, "Commercial Shops", GovernorateGuideCatalog.CommercialShopsName),
        new(SubCategoryType.GeneralServices, CategoryType.WorkshopsAndCraftsmen, "General Services", GovernorateGuideCatalog.GeneralServicesName),
        new(SubCategoryType.ResortsHotelsAndAmusement, CategoryType.WorkshopsAndCraftsmen, "Resorts, Hotels & Amusement", GovernorateGuideCatalog.ResortsHotelsAndAmusementName),

        new(SubCategoryType.LostItems, CategoryType.LostAndFound, "Lost", "ضايع مني"),
        new(SubCategoryType.FoundItems, CategoryType.LostAndFound, "Found", "لقيت"),

        new(SubCategoryType.Factories, CategoryType.Business, "Factories", "المصانع"),
        new(SubCategoryType.Farms, CategoryType.Business, "Farms", "مزارع ومحاصيل"),
        new(SubCategoryType.Companies, CategoryType.Business, "Companies", "الشركات"),
        new(SubCategoryType.Suppliers, CategoryType.Business, "Suppliers", "الموردون"),
        new(SubCategoryType.WholesaleTraders, CategoryType.Business, "Wholesale Traders", "تجار الجملة"),
        new(SubCategoryType.FruitAndVegetableTraders, CategoryType.Business, "Fruit & Vegetable Traders", "تجار خضر وفاكهة"),

        new(SubCategoryType.JobRequests, CategoryType.Jobs, "Job Requests", "طلبات عمل"),
        new(SubCategoryType.JobOpportunities, CategoryType.Jobs, "Job Opportunities", "فرص عمل"),

        new(SubCategoryType.Livestock, CategoryType.Animals, "Livestock", "المواشي"),
        new(SubCategoryType.SheepAndGoats, CategoryType.Animals, "Sheep & Goats", "الأغنام والماعز"),
        new(SubCategoryType.Horses, CategoryType.Animals, "Horses", "الخيول"),
        new(SubCategoryType.Camels, CategoryType.Animals, "Camels", "الإبل"),
        new(SubCategoryType.Birds, CategoryType.Animals, "Birds", "الطيور"),
        new(SubCategoryType.Pets, CategoryType.Animals, "Pets", "الحيوانات الأليفة"),
        new(SubCategoryType.Fish, CategoryType.Animals, "Fish", "الأسماك"),
        new(SubCategoryType.Bees, CategoryType.Animals, "Bees", "النحل"),
        new(SubCategoryType.OtherAnimals, CategoryType.Animals, "Other Animals", "حيوانات أخرى"),

        new(SubCategoryType.DecorAntiques, CategoryType.Antiques, "Antiques Decor", "تحف"),
        new(SubCategoryType.Antiques, CategoryType.Antiques, "Antiques", "أنتيكات"),
        new(SubCategoryType.Paintings, CategoryType.Antiques, "Paintings", "لوحات فنية"),
        new(SubCategoryType.Handmade, CategoryType.Antiques, "Handmade", "أعمال يدوية"),
        new(SubCategoryType.CoinsAndStamps, CategoryType.Antiques, "Coins & Stamps", "عملات وطوابع"),

        new(SubCategoryType.MenClothing, CategoryType.Clothing, "Men Clothing", "ملابس رجالي"),
        new(SubCategoryType.WomenClothing, CategoryType.Clothing, "Women Clothing", "ملابس حريمي"),
        new(SubCategoryType.KidsClothing, CategoryType.Clothing, "Kids Clothing", "ملابس أطفال"),

        new(SubCategoryType.Accessories, CategoryType.OnlineShopping, "Accessories", "إكسسوارات"),
        new(SubCategoryType.Cosmetics, CategoryType.OnlineShopping, "Cosmetics", "مستحضرات التجميل"),
        new(SubCategoryType.HomeAndKitchen, CategoryType.OnlineShopping, "Home & Kitchen", "المنزل والمطبخ"),
        new(SubCategoryType.ShoppingElectronics, CategoryType.OnlineShopping, "Electronics", "إلكترونيات"),
        new(SubCategoryType.GiftsAndToys, CategoryType.OnlineShopping, "Gifts & Toys", "هدايا وألعاب"),
        new(SubCategoryType.HomemadeFood, CategoryType.OnlineShopping, "Homemade Food", "أكل منزلي"),

        new(SubCategoryType.Furniture, CategoryType.HomeFurnishing, "Furniture", "أثاث"),
        new(SubCategoryType.FurnishingsAndCurtains, CategoryType.HomeFurnishing, "Furnishings & Curtains", "مفروشات وستائر"),
        new(SubCategoryType.LightingAndDecor, CategoryType.HomeFurnishing, "Lighting & Decor", "إضاءة وديكور"),
        new(SubCategoryType.KitchenTools, CategoryType.HomeFurnishing, "Kitchen Tools", "أدوات المطبخ"),
        new(SubCategoryType.HomeAppliances, CategoryType.HomeFurnishing, "Home Appliances", "أجهزة كهربائية منزلية"),
        new(SubCategoryType.BathroomSupplies, CategoryType.HomeFurnishing, "Bathroom Supplies", "مستلزمات الحمام"),
        new(SubCategoryType.PlantsAndOrnaments, CategoryType.HomeFurnishing, "Plants & Ornaments", "نباتات وزينة"),

        new(SubCategoryType.Lands, CategoryType.RealEstate, "Lands", "أراضي"),
        new(SubCategoryType.Apartments, CategoryType.RealEstate, "Apartments", "شقق"),
        new(SubCategoryType.Shops, CategoryType.RealEstate, "Shops", "محلات"),
        new(SubCategoryType.Buildings, CategoryType.RealEstate, "Buildings", "مباني"),

        new(SubCategoryType.Rescues, CategoryType.Charity, "Rescues", "الاستغاثة"),
        new(SubCategoryType.BloodRequests, CategoryType.Charity, "Blood Requests", "فصائل الدم"),
        new(SubCategoryType.AskConsults, CategoryType.Charity, "Ask & Consult", "اسأل واستشير"),

        new(SubCategoryType.Doctors, CategoryType.Sehhaty, "Doctors", "الأطباء"),
        new(SubCategoryType.MedicalLaboratories, CategoryType.Sehhaty, "Medical Laboratories", "معامل التحاليل الطبية"),
        new(SubCategoryType.DentalLaboratories, CategoryType.Sehhaty, "Dental Laboratories", "معامل الأسنان"),
        new(SubCategoryType.RadiologyLaboratories, CategoryType.Sehhaty, "Radiology Laboratories", "معامل الأشعة"),
        new(SubCategoryType.MedicalCenters, CategoryType.Sehhaty, "Medical Centers", "مراكز طبية"),
        new(SubCategoryType.Hospitals, CategoryType.Sehhaty, "Hospitals", "مستشفيات"),

        new(SubCategoryType.LawyerOffices, CategoryType.Offices, "Lawyer Offices", "مكاتب محامين"),
        new(SubCategoryType.EngineeringConsultancyOffices, CategoryType.Offices, "Engineering Consultancy Offices", "مكاتب استشارية هندسية"),
        new(SubCategoryType.Contractors, CategoryType.Offices, "Demolition & Construction Contractors", "مقاولين (هدم وبناء)"),
        new(SubCategoryType.IndustrialConsultancyOffices, CategoryType.Offices, "Industrial Consultancy Offices", "مكاتب استشارية صناعية"),
        new(SubCategoryType.AccountantOffices, CategoryType.Offices, "Certified Public Accountant Offices", "مكاتب محاسبين قانونيين"),

        new(SubCategoryType.RestaurantsAndCafes, CategoryType.RestaurantsAndCafes, "Restaurants & Cafes", RestaurantsAndCafesCatalog.CategoryName)
    ];

    public static CategoryEntry? FindCategory(CategoryType category) =>
        Categories.FirstOrDefault(entry => entry.Id == category);

    public static SubCategoryEntry? FindSubCategory(SubCategoryType subCategory) =>
        SubCategories.FirstOrDefault(entry => entry.Id == subCategory);

    public static bool OpensFormDirectly(int categoryId) =>
        Categories.Any(entry => (int)entry.Id == categoryId && entry.OpensFormDirectly);

    public static int? DirectSubCategoryOf(int categoryId)
    {
        if (!OpensFormDirectly(categoryId))
            return null;

        var subCategories = SubCategories.Where(entry => (int)entry.Category == categoryId).ToList();

        return subCategories.Count == 1 ? (int)subCategories[0].Id : null;
    }
}
