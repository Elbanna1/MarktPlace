using Shared.Enums;

namespace Shared.Constants;

public static class OfficesCatalog
{
    public const string CategoryName = "مكاتب";

    public static readonly DirectoryModule LawyerOffices = new(
        ListingModuleType.LawyerOffice, SubCategoryType.LawyerOffices, OfficesRoutes.LawyerOffices,
        "lawyerOffices", FileUploadConstants.LawyerOfficesFolder, "إعلان مكتب المحاماة", "⚖️",
        "اسم المكتب أو المحامي", "Office or lawyer name", "مثال: مكتب الأستاذ أحمد محمود للمحاماة",
        "بيانات مكتب المحاماة",
        DirectoryListingRules.TitlePlaceholder,
        "اذكر في الوصف: • مجالات التخصص (جنائي، مدني، أسرة…) • الخدمات اللي بتقدمها • مواعيد العمل",
        "قائمة مكاتب المحامين", "Lawyer offices list",
        "تفاصيل إعلان مكتب المحاماة", "Lawyer office details",
        "إعلانات مكاتب المحامين المضافة بواسطتي", "My lawyer office listings",
        "إعلان مكتب المحاماة غير موجود.",
        NameFilterIsExact: true);

    public static readonly DirectoryModule EngineeringConsultancyOffices = new(
        ListingModuleType.EngineeringConsultancyOffice, SubCategoryType.EngineeringConsultancyOffices,
        OfficesRoutes.EngineeringConsultancyOffices,
        "engineeringConsultancyOffices", FileUploadConstants.EngineeringConsultancyOfficesFolder,
        "إعلان المكتب الهندسي", "📐",
        "اسم المكتب الهندسي", "Engineering office name", "مثال: المكتب الهندسي للتصميمات والإشراف",
        "بيانات المكتب الهندسي",
        DirectoryListingRules.TitlePlaceholder,
        "اذكر في الوصف: • الخدمات (تصميم، إشراف، تراخيص…) • التخصصات الهندسية • أهم الأعمال السابقة",
        "قائمة المكاتب الاستشارية الهندسية", "Engineering consultancy offices list",
        "تفاصيل إعلان المكتب الهندسي", "Engineering office details",
        "إعلانات المكاتب الهندسية المضافة بواسطتي", "My engineering office listings",
        "إعلان المكتب الهندسي غير موجود.",
        NameFilterIsExact: true);

    public static readonly DirectoryModule Contractors = new(
        ListingModuleType.Contractor, SubCategoryType.Contractors, OfficesRoutes.Contractors,
        "contractors", FileUploadConstants.ContractorsFolder, "إعلان المقاول", "🏗️",
        "اسم المكتب أو المقاول", "Office or contractor name", "مثال: مكتب الأمل للمقاولات",
        "بيانات المقاول",
        DirectoryListingRules.TitlePlaceholder,
        "اذكر في الوصف: • أعمال الهدم والبناء اللي بتنفذها • المعدات والعمالة المتاحة • أهم المشاريع السابقة",
        "قائمة المقاولين (هدم وبناء)", "Demolition & construction contractors list",
        "تفاصيل إعلان المقاول", "Contractor details",
        "إعلانات المقاولين المضافة بواسطتي", "My contractor listings",
        "إعلان المقاول غير موجود.",
        NameFilterIsExact: true);

    public static readonly DirectoryModule IndustrialConsultancyOffices = new(
        ListingModuleType.IndustrialConsultancyOffice, SubCategoryType.IndustrialConsultancyOffices,
        OfficesRoutes.IndustrialConsultancyOffices,
        "industrialConsultancyOffices", FileUploadConstants.IndustrialConsultancyOfficesFolder,
        "إعلان مكتب الاستشارات الصناعية", "🏭",
        "اسم مكتب الاستشارة الصناعية", "Industrial consultancy office name", "مثال: مكتب النهضة للاستشارات الصناعية",
        "بيانات مكتب الاستشارات الصناعية",
        DirectoryListingRules.TitlePlaceholder,
        "اذكر في الوصف: • مجالات الاستشارة (دراسات جدوى، تراخيص، خطوط إنتاج…) • القطاعات اللي بتخدمها • الخبرات السابقة",
        "قائمة المكاتب الاستشارية الصناعية", "Industrial consultancy offices list",
        "تفاصيل إعلان مكتب الاستشارات الصناعية", "Industrial consultancy office details",
        "إعلانات مكاتب الاستشارات الصناعية المضافة بواسطتي", "My industrial consultancy listings",
        "إعلان مكتب الاستشارات الصناعية غير موجود.",
        NameFilterIsExact: true);

    public static readonly DirectoryModule AccountantOffices = new(
        ListingModuleType.AccountantOffice, SubCategoryType.AccountantOffices, OfficesRoutes.AccountantOffices,
        "accountantOffices", FileUploadConstants.AccountantOfficesFolder, "إعلان مكتب المحاسب القانوني", "🧮",
        "اسم مكتب المحاسب القانوني أو اسم المحاسب القانوني", "Accountant office or accountant name",
        "مثال: مكتب الأستاذ محمد علي للمحاسبة والمراجعة",
        "بيانات مكتب المحاسب القانوني",
        DirectoryListingRules.TitlePlaceholder,
        "اذكر في الوصف: • الخدمات (مراجعة، ضرائب، تأسيس شركات…) • نوع العملاء • مواعيد العمل",
        "قائمة مكاتب المحاسبين القانونيين", "Certified public accountant offices list",
        "تفاصيل إعلان مكتب المحاسب القانوني", "Accountant office details",
        "إعلانات مكاتب المحاسبين المضافة بواسطتي", "My accountant office listings",
        "إعلان مكتب المحاسب القانوني غير موجود.",
        NameFilterIsExact: true);

    public static readonly IReadOnlyList<DirectoryModule> Modules =
    [
        LawyerOffices, EngineeringConsultancyOffices, Contractors, IndustrialConsultancyOffices, AccountantOffices
    ];

    public static DirectoryModule? ModuleOf(SubCategoryType subCategory) =>
        Modules.FirstOrDefault(module => module.SubCategory == subCategory);
}
