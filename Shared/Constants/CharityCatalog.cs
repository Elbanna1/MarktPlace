using Shared.Enums;

namespace Shared.Constants;

public sealed record CharityLookupEntry<TEnum>(TEnum Id, string Name, string NameEn)
    where TEnum : struct, Enum;

public static class CharityCatalog
{
    public const int MaxAskConsultImages = 5;

    public const int MaxCharityImages = 5;

    public const int MaxCommentLength = 2000;

    public const double MinLatitude = -90d;

    public const double MaxLatitude = 90d;

    public const double MinLongitude = -180d;

    public const double MaxLongitude = 180d;

    public static bool IsRealLocation(double? latitude, double? longitude) =>
        latitude is { } lat && longitude is { } lng &&
        lat is >= MinLatitude and <= MaxLatitude &&
        lng is >= MinLongitude and <= MaxLongitude &&
        !(lat == 0d && lng == 0d);

    public const string RescueResponsibilityDeclaration =
        "أقر بأنني مسؤول عن هذا المنشور، وأن جميع البيانات والمعلومات التي قمت بإدخالها صحيحة، " +
        "وأوافق على نشر الاستغاثة على منصة شيبك لبيك، وأتحمل كامل المسؤولية عن محتوى المنشور.";

    public const string BloodRequestResponsibilityDeclaration =
        "أقر بأنني مسؤول عن هذا المنشور، وأن جميع البيانات والمعلومات التي قمت بإدخالها صحيحة، " +
        "وأوافق على نشر طلب فصيلة الدم على منصة شيبك لبيك، وأتحمل كامل المسؤولية عن محتوى المنشور.";

    public const string AskConsultResponsibilityDeclaration =
        "أقر بأنني مسؤول عن محتوى السؤال والمعلومات التي قمت بنشرها وأوافق على نشره على منصة شيبك لبيك.";

    public const string ResponsibilityRequiredMessage =
        "يجب الموافقة على إقرار المسؤولية قبل النشر.";

    public const bool BloodRequestsAreUrgent = true;

    public static readonly IReadOnlyList<CharityLookupEntry<BloodGroup>> BloodGroups =
        new List<CharityLookupEntry<BloodGroup>>
        {
            new(BloodGroup.APositive, "A+", "A+"),
            new(BloodGroup.ANegative, "A-", "A-"),
            new(BloodGroup.BPositive, "B+", "B+"),
            new(BloodGroup.BNegative, "B-", "B-"),
            new(BloodGroup.ABPositive, "AB+", "AB+"),
            new(BloodGroup.ABNegative, "AB-", "AB-"),
            new(BloodGroup.OPositive, "O+", "O+"),
            new(BloodGroup.ONegative, "O-", "O-")
        };

    public static readonly IReadOnlyList<CharityLookupEntry<AskConsultCategory>> AskConsultCategories =
        new List<CharityLookupEntry<AskConsultCategory>>
        {
            new(AskConsultCategory.Medical, "طبي", "Medical"),
            new(AskConsultCategory.Legal, "قانوني", "Legal"),
            new(AskConsultCategory.Cars, "سيارات", "Cars"),
            new(AskConsultCategory.RealEstate, "عقارات", "Real estate"),
            new(AskConsultCategory.Jobs, "وظائف", "Jobs"),
            new(AskConsultCategory.BusinessAndTrade, "أعمال وتجارة", "Business & trade"),
            new(AskConsultCategory.Agriculture, "زراعة", "Agriculture"),
            new(AskConsultCategory.Education, "تعليم", "Education"),
            new(AskConsultCategory.Technology, "تكنولوجيا", "Technology"),
            new(AskConsultCategory.ContractingAndFinishing, "مقاولات وتشطيبات", "Contracting & finishing"),
            new(AskConsultCategory.FamilyAndLife, "أسرة وحياة", "Family & life"),
            new(AskConsultCategory.General, "عام", "General"),
            new(AskConsultCategory.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<CharityLookupEntry<CharitySortBy>> SortOptions =
        new List<CharityLookupEntry<CharitySortBy>>
        {
            new(CharitySortBy.Newest, "الأحدث", "Newest"),
            new(CharitySortBy.Oldest, "الأقدم", "Oldest"),
            new(CharitySortBy.MostViewed, "الأكثر مشاهدة", "Most viewed"),
            new(CharitySortBy.MostLiked, "الأكثر إعجابًا", "Most liked")
        };

    public static string GetBloodGroupName(BloodGroup value) => NameOf(BloodGroups, value);

    public static string? GetBloodGroupName(BloodGroup? value) => NameOrNull(BloodGroups, value);

    public static string GetAskConsultCategoryName(AskConsultCategory value) =>
        NameOf(AskConsultCategories, value);

    public static string? GetAskConsultCategoryName(AskConsultCategory? value) =>
        NameOrNull(AskConsultCategories, value);

    private static string NameOf<TEnum>(
        IReadOnlyList<CharityLookupEntry<TEnum>> entries, TEnum value)
        where TEnum : struct, Enum =>
        entries.FirstOrDefault(entry => entry.Id.Equals(value))?.Name ?? value.ToString();

    private static string? NameOrNull<TEnum>(
        IReadOnlyList<CharityLookupEntry<TEnum>> entries, TEnum? value)
        where TEnum : struct, Enum =>
        value is null ? null : NameOf(entries, value.Value);
}
