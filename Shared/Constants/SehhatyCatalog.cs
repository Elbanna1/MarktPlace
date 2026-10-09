using Shared.Enums;

namespace Shared.Constants;

public sealed record SehhatyLookupEntry<TEnum>(TEnum Id, string Name, string NameEn)
    where TEnum : struct, Enum;

public sealed record SehhatyModule(
    ListingModuleType Type,
    SubCategoryType SubCategory,
    string Route,
    string Key,
    string ImagesFolder,
    string EntityName,
    string Icon,
    string NameLabel,
    string NameLabelEn,
    string NamePlaceholder,
    string Section,
    string TitlePlaceholder,
    string DescriptionPlaceholder,
    string ListLabel,
    string ListLabelEn,
    string DetailsLabel,
    string DetailsLabelEn,
    string MyListingsLabel,
    string MyListingsLabelEn,
    string NotFoundMessage)
{
    public string PublicRoute => "/" + Route;
}

public static class SehhatyCatalog
{
    public const string CategoryName = "صحتي";

    public const bool ImagesRequired = true;

    public const int MaxNameLength = 150;

    public const int MaxTitleLength = 150;

    public const int MaxDescriptionLength = 4000;

    public const int MaxAddressLength = 300;

    public const int MaxGoogleMapsUrlLength = 1000;

    public const int MaxOtherSpecialtyLength = 150;

    public const int MaxEmailLength = 256;

    public const int MaxPhoneLength = 20;

    public const string AddressPlaceholder = "الشارع، العلامة المميزة…";

    public const string GoogleMapsPlaceholder = "https://maps.google.com/…";

    public const string TitlePlaceholder = "اكتب عنوانًا واضحًا ومختصرًا";

    public const string GoogleMapsUrlMessage = "لينك خرائط جوجل لازم يبدأ بـ http أو https.";

    public const string EmailMessage = "من فضلك اكتب بريد إلكتروني صحيح.";

    public const string SpecialtyLabel = "تخصص الطبيب";

    public const string OtherSpecialtyLabel = "اكتب التخصص";

    public const string OtherSpecialtyRequiredMessage = "من فضلك اكتب تخصص الطبيب لما تختار «أخرى».";

    public static readonly SehhatyModule Doctors = new(
        ListingModuleType.Doctor, SubCategoryType.Doctors, SehhatyRoutes.Doctors, "doctors",
        FileUploadConstants.DoctorsFolder, "إعلان الطبيب", "🩺",
        "اسم الطبيب", "Doctor name", "مثال: د. أحمد محمود", "بيانات الطبيب",
        TitlePlaceholder,
        "اذكر في الوصف: • معلومات إضافية • الخدمات اللي بتقدمها • مواعيد الكشف",
        "قائمة الأطباء", "Doctors list", "تفاصيل إعلان الطبيب", "Doctor details",
        "إعلانات الأطباء المضافة بواسطتي", "My doctor listings",
        "إعلان الطبيب غير موجود.");

    public static readonly SehhatyModule MedicalLaboratories = new(
        ListingModuleType.MedicalLaboratory, SubCategoryType.MedicalLaboratories,
        SehhatyRoutes.MedicalLaboratories, "medicalLaboratories",
        FileUploadConstants.MedicalLaboratoriesFolder, "إعلان معمل التحاليل", "🧪",
        "اسم المعمل", "Laboratory name", "مثال: معمل الشفاء للتحاليل الطبية", "بيانات المعمل",
        TitlePlaceholder,
        "اذكر في الوصف: • التحاليل المتاحة • مواعيد العمل • خدمة سحب العينات من البيت لو متاحة",
        "قائمة معامل التحاليل الطبية", "Medical laboratories list",
        "تفاصيل إعلان معمل التحاليل", "Medical laboratory details",
        "إعلانات معامل التحاليل المضافة بواسطتي", "My medical laboratory listings",
        "إعلان معمل التحاليل غير موجود.");

    public static readonly SehhatyModule DentalLaboratories = new(
        ListingModuleType.DentalLaboratory, SubCategoryType.DentalLaboratories,
        SehhatyRoutes.DentalLaboratories, "dentalLaboratories",
        FileUploadConstants.DentalLaboratoriesFolder, "إعلان معمل الأسنان", "🦷",
        "اسم معمل الأسنان", "Dental laboratory name", "مثال: معمل النور للأسنان", "بيانات معمل الأسنان",
        TitlePlaceholder,
        "اذكر في الوصف: • التركيبات والخدمات اللي بتقدمها • الخامات المستخدمة • مدة التسليم",
        "قائمة معامل الأسنان", "Dental laboratories list",
        "تفاصيل إعلان معمل الأسنان", "Dental laboratory details",
        "إعلانات معامل الأسنان المضافة بواسطتي", "My dental laboratory listings",
        "إعلان معمل الأسنان غير موجود.");

    public static readonly SehhatyModule RadiologyLaboratories = new(
        ListingModuleType.RadiologyLaboratory, SubCategoryType.RadiologyLaboratories,
        SehhatyRoutes.RadiologyLaboratories, "radiologyLaboratories",
        FileUploadConstants.RadiologyLaboratoriesFolder, "إعلان معمل الأشعة", "🩻",
        "اسم معمل الأشعة", "Radiology laboratory name", "مثال: مركز الأمل للأشعة", "بيانات معمل الأشعة",
        TitlePlaceholder,
        "اذكر في الوصف: • أنواع الأشعة المتاحة • مواعيد العمل • مدة تسليم النتائج",
        "قائمة معامل الأشعة", "Radiology laboratories list",
        "تفاصيل إعلان معمل الأشعة", "Radiology laboratory details",
        "إعلانات معامل الأشعة المضافة بواسطتي", "My radiology laboratory listings",
        "إعلان معمل الأشعة غير موجود.");

    public static readonly SehhatyModule MedicalCenters = new(
        ListingModuleType.MedicalCenter, SubCategoryType.MedicalCenters,
        SehhatyRoutes.MedicalCenters, "medicalCenters",
        FileUploadConstants.MedicalCentersFolder, "إعلان المركز الطبي", "🏥",
        "اسم المركز الطبي", "Medical center name", "مثال: مركز الرحمة الطبي", "بيانات المركز الطبي",
        TitlePlaceholder,
        "اذكر في الوصف: • العيادات والتخصصات المتاحة • مواعيد العمل • الخدمات الإضافية",
        "قائمة المراكز الطبية", "Medical centers list",
        "تفاصيل إعلان المركز الطبي", "Medical center details",
        "إعلانات المراكز الطبية المضافة بواسطتي", "My medical center listings",
        "إعلان المركز الطبي غير موجود.");

    public static readonly SehhatyModule Hospitals = new(
        ListingModuleType.Hospital, SubCategoryType.Hospitals,
        SehhatyRoutes.Hospitals, "hospitals",
        FileUploadConstants.HospitalsFolder, "إعلان المستشفى", "🏨",
        "اسم المستشفى", "Hospital name", "مثال: مستشفى الفيوم التخصصي", "بيانات المستشفى",
        TitlePlaceholder,
        "اذكر في الوصف: • الأقسام والتخصصات المتاحة • خدمة الطوارئ • الخدمات الإضافية",
        "قائمة المستشفيات", "Hospitals list",
        "تفاصيل إعلان المستشفى", "Hospital details",
        "إعلانات المستشفيات المضافة بواسطتي", "My hospital listings",
        "إعلان المستشفى غير موجود.");

    public static readonly IReadOnlyList<SehhatyModule> Modules =
    [
        Doctors, MedicalLaboratories, DentalLaboratories, RadiologyLaboratories, MedicalCenters, Hospitals
    ];

    public static SehhatyModule? ModuleOf(SubCategoryType subCategory) =>
        Modules.FirstOrDefault(module => module.SubCategory == subCategory);

    public static SehhatyModule For(ListingModuleType type) =>
        Modules.FirstOrDefault(module => module.Type == type)
        ?? throw new ArgumentOutOfRangeException(nameof(type), type, "Not a Sehhaty listing module.");

    public static string NameRequiredMessage(SehhatyModule module) => $"{module.NameLabel} مطلوب.";

    public static readonly IReadOnlyList<SehhatyLookupEntry<DoctorSpecialty>> Specialties =
        new List<SehhatyLookupEntry<DoctorSpecialty>>
        {
            new(DoctorSpecialty.Cardiology, "القلب والأوعية الدموية", "Cardiology"),
            new(DoctorSpecialty.Pediatrics, "طب الأطفال", "Pediatrics"),
            new(DoctorSpecialty.DermatologyAndCosmetology, "الجلدية والتجميل", "Dermatology & cosmetology"),
            new(DoctorSpecialty.Orthopedics, "العظام", "Orthopedics"),
            new(DoctorSpecialty.Neurology, "المخ والأعصاب", "Neurology"),
            new(DoctorSpecialty.ObstetricsAndGynecology, "النساء والتوليد", "Obstetrics & gynecology"),
            new(DoctorSpecialty.Ophthalmology, "طب وجراحة العيون", "Ophthalmology"),
            new(DoctorSpecialty.Dentistry, "طب وجراحة الأسنان", "Dentistry"),
            new(DoctorSpecialty.InternalMedicine, "الباطنة العامة", "Internal medicine"),
            new(DoctorSpecialty.Otolaryngology, "الأنف والأذن والحنجرة", "Ear, nose & throat"),
            new(DoctorSpecialty.Urology, "المسالك البولية", "Urology"),
            new(DoctorSpecialty.GeneralSurgery, "الجراحة العامة", "General surgery"),
            new(DoctorSpecialty.Neurosurgery, "جراحة المخ والأعصاب", "Neurosurgery"),
            new(DoctorSpecialty.Psychiatry, "الطب النفسي", "Psychiatry"),
            new(DoctorSpecialty.PhysicalTherapyAndRehabilitation, "العلاج الطبيعي والتأهيل", "Physical therapy & rehabilitation"),
            new(DoctorSpecialty.DiagnosticRadiology, "الأشعة التشخيصية", "Diagnostic radiology"),
            new(DoctorSpecialty.MedicalLaboratoryTesting, "التحاليل الطبية", "Medical laboratory testing"),
            new(DoctorSpecialty.FamilyMedicine, "طب الأسرة", "Family medicine"),
            new(DoctorSpecialty.EmergencyMedicine, "طب الطوارئ", "Emergency medicine"),
            new(DoctorSpecialty.ChestAndAllergy, "الصدر والحساسية", "Chest & allergy"),
            new(DoctorSpecialty.Nephrology, "أمراض الكلى", "Nephrology"),
            new(DoctorSpecialty.Oncology, "الأورام", "Oncology"),
            new(DoctorSpecialty.Hematology, "أمراض الدم", "Hematology"),
            new(DoctorSpecialty.RheumatologyAndImmunology, "الروماتيزم والمناعة", "Rheumatology & immunology"),
            new(DoctorSpecialty.AnesthesiaAndIntensiveCare, "التخدير والعناية المركزة", "Anesthesia & intensive care"),
            new(DoctorSpecialty.ClinicalNutrition, "التغذية العلاجية", "Clinical nutrition"),
            new(DoctorSpecialty.AndrologyAndInfertility, "أمراض الذكورة والعقم", "Andrology & infertility"),
            new(DoctorSpecialty.CardiothoracicSurgery, "جراحة القلب والصدر", "Cardiothoracic surgery"),
            new(DoctorSpecialty.PlasticSurgery, "جراحة التجميل", "Plastic surgery"),
            new(DoctorSpecialty.VascularSurgery, "جراحة الأوعية الدموية", "Vascular surgery"),
            new(DoctorSpecialty.GastroenterologyAndHepatology, "أمراض الجهاز الهضمي والكبد", "Gastroenterology & hepatology"),
            new(DoctorSpecialty.EndocrinologyAndDiabetes, "الغدد الصماء والسكري", "Endocrinology & diabetes"),
            new(DoctorSpecialty.Geriatrics, "طب الشيخوخة", "Geriatrics"),
            new(DoctorSpecialty.PainMedicine, "طب الألم", "Pain medicine"),
            new(DoctorSpecialty.SleepMedicine, "طب النوم", "Sleep medicine"),
            new(DoctorSpecialty.RadiationOncology, "طب الأورام الإشعاعي", "Radiation oncology"),
            new(DoctorSpecialty.TransfusionMedicine, "طب نقل الدم", "Transfusion medicine"),
            new(DoctorSpecialty.SportsMedicine, "الطب الرياضي", "Sports medicine"),
            new(DoctorSpecialty.SpineSurgery, "جراحة العمود الفقري", "Spine surgery"),
            new(DoctorSpecialty.PediatricSurgery, "جراحة الأطفال", "Pediatric surgery"),
            new(DoctorSpecialty.MaxillofacialSurgery, "جراحة الوجه والفكين", "Maxillofacial surgery"),
            new(DoctorSpecialty.CosmeticDentistry, "طب الأسنان التجميلي", "Cosmetic dentistry"),
            new(DoctorSpecialty.Orthodontics, "تقويم الأسنان", "Orthodontics"),
            new(DoctorSpecialty.Endodontics, "علاج الجذور والأعصاب", "Endodontics"),
            new(DoctorSpecialty.Periodontics, "أمراض اللثة", "Periodontics"),
            new(DoctorSpecialty.PediatricDentistry, "طب أسنان الأطفال", "Pediatric dentistry"),
            new(DoctorSpecialty.Neonatology, "طب حديثي الولادة", "Neonatology"),
            new(DoctorSpecialty.MedicalGenetics, "طب الوراثة", "Medical genetics"),
            new(DoctorSpecialty.NuclearMedicine, "الطب النووي", "Nuclear medicine"),
            new(DoctorSpecialty.ForensicMedicine, "الطب الشرعي", "Forensic medicine"),
            new(DoctorSpecialty.OccupationalHealth, "الصحة المهنية", "Occupational health"),
            new(DoctorSpecialty.CommunityMedicine, "طب المجتمع", "Community medicine"),
            new(DoctorSpecialty.PreventiveMedicine, "الطب الوقائي", "Preventive medicine"),
            new(DoctorSpecialty.TravelMedicine, "طب السفر", "Travel medicine"),
            new(DoctorSpecialty.ImmunologyAndAllergy, "أمراض المناعة والحساسية", "Immunology & allergy"),
            new(DoctorSpecialty.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<SehhatyLookupEntry<SehhatySortBy>> SortOptions =
        new List<SehhatyLookupEntry<SehhatySortBy>>
        {
            new(SehhatySortBy.Newest, "الأحدث", "Newest"),
            new(SehhatySortBy.Oldest, "الأقدم", "Oldest"),
            new(SehhatySortBy.MostViewed, "الأكثر مشاهدة", "Most viewed")
        };

    public static string GetSpecialtyName(DoctorSpecialty value) =>
        Specialties.FirstOrDefault(entry => entry.Id == value)?.Name ?? value.ToString();

    public static string DisplaySpecialty(DoctorSpecialty value, string? otherSpecialty) =>
        value == DoctorSpecialty.Other && !string.IsNullOrWhiteSpace(otherSpecialty)
            ? otherSpecialty.Trim()
            : GetSpecialtyName(value);

    public static bool IsHttpUrl(string? value) =>
        string.IsNullOrWhiteSpace(value) ||
        (Uri.TryCreate(value.Trim(), UriKind.Absolute, out var uri) &&
         (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps));
}
