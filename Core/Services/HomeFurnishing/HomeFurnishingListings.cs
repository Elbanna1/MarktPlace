using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.HomeFurnishing;
using Shared.Exceptions;

namespace Services.HomeFurnishing;

internal static class HomeFurnishingListings
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

    public static string? OtherWhen(bool isOther, string? value) =>
        isOther && !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;

    public static string? Trimmed(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public static void EnsureHasImages(int imageCount)
    {
        if (imageCount == 0)
            throw new BadRequestException("لازم ترفع صورة واحدة على الأقل.");
    }

    public static string Governorate => LocationConstants.Governorate;
}
