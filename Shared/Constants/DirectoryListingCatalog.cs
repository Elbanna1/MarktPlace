using Shared.Enums;

namespace Shared.Constants;

public sealed record DirectoryModule(
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
    string NotFoundMessage,
    bool ImagesRequired = true,
    bool NameFilterIsExact = false)
{
    public string PublicRoute => "/" + Route;

    public CategoryType Category => ListingModuleCatalog.CategoryOf(SubCategory)
        ?? throw new InvalidOperationException($"{SubCategory} has no parent category.");
}

public sealed record DirectorySortOption(DirectorySortBy Id, string Name, string NameEn);

public static class DirectoryListingRules
{
    public const int MaxNameLength = 150;

    public const int MaxTitleLength = 150;

    public const int MaxDescriptionLength = 4000;

    public const int MaxAddressLength = 300;

    public const int MaxGoogleMapsUrlLength = 1000;

    public const int MaxEmailLength = 256;

    public const int MaxPhoneLength = 20;

    public const string AddressPlaceholder = "الشارع، العلامة المميزة…";

    public const string GoogleMapsPlaceholder = "https://maps.google.com/…";

    public const string TitlePlaceholder = "اكتب عنوانًا واضحًا ومختصرًا";

    public const string GoogleMapsUrlMessage = "لينك خرائط جوجل لازم يبدأ بـ http أو https.";

    public const string EmailMessage = "من فضلك اكتب بريد إلكتروني صحيح.";

    public const string ImagesRequiredMessage = "لازم ترفع صورة واحدة على الأقل.";

    public static string NameRequiredMessage(DirectoryModule module) => $"{module.NameLabel} مطلوب.";

    public static readonly IReadOnlyList<DirectorySortOption> SortOptions =
    [
        new(DirectorySortBy.Newest, "الأحدث", "Newest"),
        new(DirectorySortBy.Oldest, "الأقدم", "Oldest"),
        new(DirectorySortBy.MostViewed, "الأكثر مشاهدة", "Most viewed")
    ];

    public static bool IsHttpUrl(string? value) =>
        string.IsNullOrWhiteSpace(value) ||
        (Uri.TryCreate(value.Trim(), UriKind.Absolute, out var uri) &&
         (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps));
}
