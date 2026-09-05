using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.BannerBookings;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.BannerBookings;

public static class BannerImageRules
{
    public readonly record struct Requirement(
        BannerImageKind Kind, int Width, int Height, long MaxSizeBytes, string[] AllowedFormats)
    {
        public double AspectRatio => Height == 0 ? 0 : (double)Width / Height;
    }

    public static Requirement DesktopRequirement(BannerPlacementSetting placement) => new(
        BannerImageKind.Desktop,
        placement.DesktopWidth,
        placement.DesktopHeight,
        placement.MaxImageSizeBytes,
        BannerBookingCatalog.ParseFormats(placement.AllowedFormats));

    public static Requirement MobileRequirement(BannerPlacementSetting placement) => new(
        BannerImageKind.Mobile,
        placement.MobileWidth,
        placement.MobileHeight,
        placement.MaxImageSizeBytes,
        BannerBookingCatalog.ParseFormats(placement.AllowedFormats));

    public static ImageDimensions Validate(UploadImageModel? image, Requirement requirement)
    {
        var label = BannerBookingCatalog.GetImageKindName(requirement.Kind);

        if (image is null)
            throw new BadRequestException($"{label} مطلوبة.");

        var size = Math.Max(image.Length, image.Content.LongLength);

        if (size <= 0)
            throw new BadRequestException($"{label} فارغة.");

        var format = ImageFormatCatalog.Detect(image.Content);

        if (format is null)
            throw new BadRequestException(
                $"{label} ليست صورة صالحة. الصيغ المقبولة: {DescribeFormats(requirement.AllowedFormats)}.");

        if (!IsAllowed(format, requirement.AllowedFormats) || !BannerBookingCatalog.SupportedFormats.Contains(format))
            throw new BadRequestException(
                $"{label} بصيغة {format.Name} غير مقبولة. الصيغ المقبولة: {DescribeFormats(requirement.AllowedFormats)}.");

        if (size > requirement.MaxSizeBytes)
            throw new BadRequestException(
                $"{label} أكبر من الحد الأقصى ({ToMegabytes(requirement.MaxSizeBytes)} MB).");

        var dimensions = ImageDimensionReader.Read(image.Content)
            ?? throw new BadRequestException($"{label} تالفة أو غير مكتملة، تعذّرت قراءة أبعادها.");

        if (!RatioMatches(dimensions, requirement))
        {
            throw new BadRequestException(
                $"{label} نسبة أبعادها {FormatRatio(dimensions.Width, dimensions.Height)} " +
                $"بينما المطلوب {FormatRatio(requirement.Width, requirement.Height)} " +
                $"({requirement.Width} × {requirement.Height} px).");
        }

        if (dimensions.Width != requirement.Width || dimensions.Height != requirement.Height)
        {
            throw new BadRequestException(
                $"{label} مقاسها {dimensions} بينما المقاس المطلوب {requirement.Width} × {requirement.Height} px.");
        }

        return dimensions;
    }

    public static BannerImageSpecDto ToSpec(Requirement requirement, string? note = null) => new()
    {
        Kind = requirement.Kind,
        KindName = BannerBookingCatalog.GetImageKindName(requirement.Kind),
        Width = requirement.Width,
        Height = requirement.Height,
        Resolution = $"{requirement.Width} × {requirement.Height} px",
        AspectRatio = FormatRatio(requirement.Width, requirement.Height),
        AllowedFormats = requirement.AllowedFormats,
        AllowedFormatNames = DescribeFormats(requirement.AllowedFormats),
        MaxSizeBytes = requirement.MaxSizeBytes,
        MaxSizeMegabytes = ToMegabytes(requirement.MaxSizeBytes),
        Note = note
    };

    public static string FormatRatio(int width, int height)
    {
        if (width <= 0 || height <= 0)
            return "-";

        var ratio = (double)width / height;

        for (var denominator = 1; denominator <= 10; denominator++)
        {
            var numerator = (int)Math.Round(ratio * denominator);
            if (numerator == 0)
                continue;

            var candidate = (double)numerator / denominator;

            if (Math.Abs(candidate - ratio) / ratio <= BannerBookingCatalog.AspectRatioTolerance)
                return $"{numerator}:{denominator}";
        }

        var divisor = GreatestCommonDivisor(width, height);
        return $"{width / divisor}:{height / divisor}";
    }

    public static int ToMegabytes(long bytes) =>
        (int)Math.Ceiling(bytes / (double)(1024 * 1024));

    public static string DescribeFormats(IReadOnlyList<string> extensions) =>
        extensions.Count == 0
            ? BannerBookingCatalog.AllowedFormatNames
            : string.Join(", ", extensions.Select(extension => extension.TrimStart('.').ToUpperInvariant()));

    private static bool RatioMatches(ImageDimensions dimensions, Requirement requirement)
    {
        var required = requirement.AspectRatio;

        if (required <= 0 || dimensions.AspectRatio <= 0)
            return false;

        return Math.Abs(dimensions.AspectRatio - required) / required <= BannerBookingCatalog.AspectRatioTolerance;
    }

    private static bool IsAllowed(ImageFormatDescriptor format, IReadOnlyList<string> allowed) =>
        allowed.Count == 0 ||
        format.Extensions.Any(extension => allowed.Contains(extension, StringComparer.OrdinalIgnoreCase));

    private static int GreatestCommonDivisor(int first, int second)
    {
        while (second != 0)
            (first, second) = (second, first % second);

        return first == 0 ? 1 : first;
    }
}
