using Shared.Enums;

namespace Shared.Constants;

public sealed record GeneralServiceTypeEntry(GeneralServiceType Id, string Name, string NameEn);

public sealed record GeneralServiceSubtypeEntry(
    GeneralServiceSubtype Id, GeneralServiceType ServiceType, string Name, string NameEn);

public static class GovernorateGuideCatalog
{
    public const string AddressPlaceholder = "اكتب العنوان بالتفصيل، الشارع والعلامة المميزة";

    public const string GoogleMapsPlaceholder = "https://maps.google.com/...";

    public const string CommercialShopsName = "محلات تجارية";

    public const string GeneralServicesName = "خدمات عامة";

    public const string ResortsHotelsAndAmusementName = "قرى سياحية وفنادق وملاهي";

    public const string ActivityTypeLabel = "نوع النشاط";

    public const string OtherActivityTypeLabel = "حدد نوع النشاط";

    public const int MaxOtherActivityTypeLength = 150;

    public const string ServiceTypeLabel = "نوع الخدمة";

    public const string FixedServiceSubtypeField = "FixedServiceSubtype";

    public const string MobileServiceSubtypeField = "MobileServiceSubtype";

    public const string FixedServiceSubtypeLabel = "نوع الخدمة الثابتة";

    public const string MobileServiceSubtypeLabel = "نوع الخدمة المتنقلة";

    public static readonly DirectoryModule CommercialShops = new(
        ListingModuleType.CommercialShop, SubCategoryType.CommercialShops,
        GovernorateGuideRoutes.CommercialShops, "commercialShops",
        FileUploadConstants.CommercialShopsFolder, "إعلان المحل التجاري", "🏬",
        "اسم المحل", "Shop name", "اكتب اسم المحل",
        "بيانات المحل",
        "اكتب عنوان الإعلان",
        "اكتب وصف المحل ونشاطه وتفاصيله",
        "قائمة المحلات التجارية", "Commercial shops list",
        "تفاصيل إعلان المحل التجاري", "Commercial shop details",
        "إعلانات المحلات التجارية المضافة بواسطتي", "My commercial shop listings",
        "إعلان المحل التجاري غير موجود.",
        NameFilterIsExact: true,
        AdSection: "بيانات المحل",
        AddressPlaceholder: AddressPlaceholder,
        GoogleMapsPlaceholder: GoogleMapsPlaceholder);

    public static readonly DirectoryModule GeneralServices = new(
        ListingModuleType.GeneralService, SubCategoryType.GeneralServices,
        GovernorateGuideRoutes.GeneralServices, "generalServices",
        FileUploadConstants.GeneralServicesFolder, "إعلان الخدمة", "🛠️",
        "اسم المعلن", "Advertiser name", "اكتب اسم المعلن",
        "بيانات الخدمة",
        "اكتب عنوان الإعلان",
        "اكتب وصف الخدمة وتفاصيلها",
        "قائمة الخدمات العامة", "General services list",
        "تفاصيل إعلان الخدمة", "General service details",
        "إعلانات الخدمات العامة المضافة بواسطتي", "My general service listings",
        "إعلان الخدمة غير موجود.",
        NameFilterIsExact: true,
        AdSection: "بيانات الإعلان",
        AddressPlaceholder: AddressPlaceholder,
        GoogleMapsPlaceholder: GoogleMapsPlaceholder);

    public static readonly DirectoryModule ResortsHotelsAndAmusement = new(
        ListingModuleType.ResortHotelAmusement, SubCategoryType.ResortsHotelsAndAmusement,
        GovernorateGuideRoutes.ResortsHotelsAndAmusement, "resortsHotelsAndAmusement",
        FileUploadConstants.ResortsHotelsAndAmusementFolder, "إعلان القرية السياحية أو الفندق أو الملاهي", "🏨",
        "اسم النشاط / اسم المعلن", "Business / advertiser name", "اكتب اسم النشاط أو اسم المعلن",
        "بيانات النشاط",
        "اكتب عنوان الإعلان",
        "اكتب وصف المكان وخدماته وتفاصيله",
        "قائمة القرى السياحية والفنادق والملاهي", "Resorts, hotels & amusement list",
        "تفاصيل إعلان القرية السياحية أو الفندق أو الملاهي", "Resort, hotel or amusement details",
        "إعلانات القرى السياحية والفنادق والملاهي المضافة بواسطتي", "My resort, hotel & amusement listings",
        "إعلان القرية السياحية أو الفندق أو الملاهي غير موجود.",
        NameFilterIsExact: true,
        AdSection: "بيانات الإعلان",
        AddressPlaceholder: AddressPlaceholder,
        GoogleMapsPlaceholder: GoogleMapsPlaceholder);

    public static readonly IReadOnlyList<DirectoryModule> All =
        [CommercialShops, GeneralServices, ResortsHotelsAndAmusement];

    public static readonly IReadOnlyList<GeneralServiceTypeEntry> ServiceTypes =
    [
        new(GeneralServiceType.Fixed, "خدمات ثابتة", "Fixed services"),
        new(GeneralServiceType.Mobile, "خدمات متنقلة", "Mobile services")
    ];

    public static readonly IReadOnlyList<GeneralServiceSubtypeEntry> ServiceSubtypes =
    [
        new(GeneralServiceSubtype.EpoxyAndFiberglass, GeneralServiceType.Fixed, "الإيبوكسي والفايبر جلاس", "Epoxy & fiberglass"),
        new(GeneralServiceSubtype.MobileRepair, GeneralServiceType.Fixed, "صيانة موبايلات", "Mobile phone repair"),
        new(GeneralServiceSubtype.OrientalAndArabicSweets, GeneralServiceType.Fixed, "حلواني شرقي وعربي", "Oriental & Arabic sweets"),
        new(GeneralServiceSubtype.WeddingPhotographyHalls, GeneralServiceType.Fixed, "صالات تصوير أفراح", "Wedding photography halls"),
        new(GeneralServiceSubtype.PrivateSchools, GeneralServiceType.Fixed, "مدارس خاصة", "Private schools"),
        new(GeneralServiceSubtype.AdvertisingAndMarketing, GeneralServiceType.Fixed, "دعاية وإعلان", "Advertising & marketing"),
        new(GeneralServiceSubtype.SolarEnergy, GeneralServiceType.Fixed, "طاقة شمسية", "Solar energy"),

        new(GeneralServiceSubtype.WeddingChef, GeneralServiceType.Mobile, "شيف أفراح", "Wedding chef"),
        new(GeneralServiceSubtype.WeddingCars, GeneralServiceType.Mobile, "سيارات زفاف", "Wedding cars"),
        new(GeneralServiceSubtype.BridalFurnishingAndCabinets, GeneralServiceType.Mobile, "فرش العروسة والنيش", "Bridal furnishing & cabinets"),
        new(GeneralServiceSubtype.GroupTransport, GeneralServiceType.Mobile, "نقل جماعي", "Group transport"),
        new(GeneralServiceSubtype.PrivateRides, GeneralServiceType.Mobile, "مشاوير خاصة", "Private rides"),
        new(GeneralServiceSubtype.SchoolTransport, GeneralServiceType.Mobile, "نقل مدرسي", "School transport"),
        new(GeneralServiceSubtype.GoodsAndFurnitureMoving, GeneralServiceType.Mobile, "نقل بضائع وأثاث", "Goods & furniture moving"),
        new(GeneralServiceSubtype.TowTruck, GeneralServiceType.Mobile, "ونش سيارات", "Car tow truck"),
        new(GeneralServiceSubtype.DeliveryServices, GeneralServiceType.Mobile, "دليفري وتوصيل", "Delivery")
    ];

    public static DirectoryModule? ModuleOf(SubCategoryType subCategory) =>
        All.FirstOrDefault(module => module.SubCategory == subCategory);

    public static IEnumerable<GeneralServiceSubtypeEntry> SubtypesOf(GeneralServiceType type) =>
        ServiceSubtypes.Where(entry => entry.ServiceType == type);

    public static bool BelongsTo(GeneralServiceSubtype? subtype, GeneralServiceType? type) =>
        subtype is { } value && type is { } parent &&
        ServiceSubtypes.Any(entry => entry.Id == value && entry.ServiceType == parent);

    public static GeneralServiceType? ServiceTypeOf(GeneralServiceSubtype subtype) =>
        ServiceSubtypes.FirstOrDefault(entry => entry.Id == subtype)?.ServiceType;

    public static string? GetServiceTypeName(GeneralServiceType? type) =>
        ServiceTypes.FirstOrDefault(entry => entry.Id == type)?.Name;

    public static string? GetServiceSubtypeName(GeneralServiceSubtype? subtype) =>
        ServiceSubtypes.FirstOrDefault(entry => entry.Id == subtype)?.Name;

    public static string? GetActivityTypeName(ShopSuitableActivity? activity) =>
        ShopCatalog.GetSuitableActivityName(activity);
}
