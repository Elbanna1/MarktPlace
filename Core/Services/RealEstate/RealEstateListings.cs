using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.RealEstate;

internal static class RealEstateListings
{
    public const int DefaultStripSize = 8;

    public const int MaxStripSize = 24;

    public const int MaxSuggestions = 10;

    public const int MinSuggestionLength = 2;

    public static int StripSize(int requested) =>
        requested <= 0 ? DefaultStripSize : Math.Min(requested, MaxStripSize);

    public static int SuggestionCount(int requested) =>
        requested <= 0 ? MaxSuggestions : Math.Min(requested, MaxSuggestions);

    public static string ShareUrl(IFileService fileService, string route, Guid id) =>
        fileService.BuildPublicUrl($"/{route.Trim('/')}/{id}");

    public static T? Keep<T>(bool conditionHolds, T? value) where T : struct =>
        conditionHolds ? value : null;

    public static string? Keep(bool conditionHolds, string? value) =>
        conditionHolds && !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;

    public static IEnumerable<T> Keep<T>(bool conditionHolds, IEnumerable<T> values) =>
        conditionHolds ? values : Enumerable.Empty<T>();

    public static string? Trimmed(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public static void EnsureHasImages(int imageCount)
    {
        if (imageCount == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");
    }

    public static string Governorate => LocationConstants.Governorate;

    public static bool AllowsProject(string? center) => RealEstateCatalog.IsProjectCenter(center);

    public static bool IsSale(RealEstateListingType listingType) =>
        listingType == RealEstateListingType.Sale;

    public static bool IsRent(RealEstateListingType listingType) =>
        listingType == RealEstateListingType.Rent;

    public static bool IsExchange(RealEstateListingType listingType) =>
        listingType == RealEstateListingType.Exchange;
}
