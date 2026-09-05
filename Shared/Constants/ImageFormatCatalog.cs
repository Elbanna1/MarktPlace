namespace Shared.Constants;

public sealed record ImageFormatDescriptor(
    string Name,
    string CanonicalExtension,
    string CanonicalContentType,
    IReadOnlyList<string> Extensions,
    IReadOnlyList<string> ContentTypes);

public static class ImageFormatCatalog
{
    public static readonly ImageFormatDescriptor Jpeg = new(
        "JPEG", ".jpg", "image/jpeg",
        [".jpg", ".jpeg", ".jpe", ".jfif", ".jfi", ".pjpeg"],
        ["image/jpeg", "image/jpg", "image/pjpeg", "image/x-jpeg"]);

    public static readonly ImageFormatDescriptor Png = new(
        "PNG", ".png", "image/png",
        [".png"],
        ["image/png", "image/x-png", "image/apng"]);

    public static readonly ImageFormatDescriptor Webp = new(
        "WebP", ".webp", "image/webp",
        [".webp"],
        ["image/webp"]);

    public static readonly ImageFormatDescriptor Gif = new(
        "GIF", ".gif", "image/gif",
        [".gif"],
        ["image/gif"]);

    public static readonly ImageFormatDescriptor Bmp = new(
        "BMP", ".bmp", "image/bmp",
        [".bmp", ".dib"],
        ["image/bmp", "image/x-bmp", "image/x-ms-bmp", "image/x-windows-bmp"]);

    public static readonly ImageFormatDescriptor Tiff = new(
        "TIFF", ".tiff", "image/tiff",
        [".tif", ".tiff"],
        ["image/tiff", "image/x-tiff", "image/tif"]);

    public static readonly ImageFormatDescriptor Icon = new(
        "ICO", ".ico", "image/x-icon",
        [".ico", ".cur"],
        ["image/x-icon", "image/vnd.microsoft.icon", "image/ico", "image/icon"]);

    public static readonly ImageFormatDescriptor Heif = new(
        "HEIF/HEIC", ".heic", "image/heic",
        [".heic", ".heif", ".hif"],
        ["image/heic", "image/heif", "image/heic-sequence", "image/heif-sequence"]);

    public static readonly ImageFormatDescriptor Avif = new(
        "AVIF", ".avif", "image/avif",
        [".avif", ".avifs"],
        ["image/avif", "image/avif-sequence"]);

    public static readonly IReadOnlyList<ImageFormatDescriptor> All =
        [Jpeg, Png, Webp, Gif, Bmp, Tiff, Icon, Heif, Avif];

    public static readonly string[] AllExtensions =
        All.SelectMany(format => format.Extensions).Distinct().OrderBy(extension => extension).ToArray();

    public static readonly string[] AllContentTypes =
        All.SelectMany(format => format.ContentTypes).Distinct().OrderBy(contentType => contentType).ToArray();

    public static readonly string DisplayNames = string.Join(", ", All.Select(format => format.Name));

    public static ImageFormatDescriptor? Detect(ReadOnlySpan<byte> content)
    {
        if (StartsWith(content, [0xFF, 0xD8, 0xFF]))
            return Jpeg;

        if (StartsWith(content, [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]))
            return Png;

        if (StartsWith(content, "GIF87a"u8) || StartsWith(content, "GIF89a"u8))
            return Gif;

        if (content.Length >= 12 && StartsWith(content, "RIFF"u8) && content.Slice(8, 4).SequenceEqual("WEBP"u8))
            return Webp;

        if (StartsWith(content, [0x49, 0x49, 0x2A, 0x00]) || StartsWith(content, [0x4D, 0x4D, 0x00, 0x2A]))
            return Tiff;

        if (content.Length >= 6 &&
            content[0] == 0x00 && content[1] == 0x00 &&
            (content[2] == 0x01 || content[2] == 0x02) && content[3] == 0x00 &&
            (content[4] | content[5]) != 0)
        {
            return Icon;
        }

        if (content.Length >= 12 && content.Slice(4, 4).SequenceEqual("ftyp"u8))
        {
            var brand = content.Slice(8, 4);

            if (brand.SequenceEqual("avif"u8) || brand.SequenceEqual("avis"u8))
                return Avif;

            if (brand.SequenceEqual("heic"u8) || brand.SequenceEqual("heix"u8) ||
                brand.SequenceEqual("heim"u8) || brand.SequenceEqual("heis"u8) ||
                brand.SequenceEqual("hevc"u8) || brand.SequenceEqual("hevx"u8) ||
                brand.SequenceEqual("mif1"u8) || brand.SequenceEqual("msf1"u8))
            {
                return Heif;
            }
        }

        if (content.Length >= 26 && StartsWith(content, "BM"u8))
            return Bmp;

        return null;
    }

    public static bool LooksExecutable(ReadOnlySpan<byte> content)
    {
        if (StartsWith(content, "MZ"u8) ||
            StartsWith(content, [0x7F, 0x45, 0x4C, 0x46]) ||
            StartsWith(content, [0xFE, 0xED, 0xFA, 0xCE]) ||
            StartsWith(content, [0xFE, 0xED, 0xFA, 0xCF]) ||
            StartsWith(content, [0xCE, 0xFA, 0xED, 0xFE]) ||
            StartsWith(content, [0xCF, 0xFA, 0xED, 0xFE]) ||
            StartsWith(content, [0xCA, 0xFE, 0xBA, 0xBE]))
        {
            return true;
        }

        if (StartsWith(content, "#!"u8) ||
            StartsWith(content, "PK"u8) ||
            StartsWith(content, "Rar!"u8) ||
            StartsWith(content, [0xD0, 0xCF, 0x11, 0xE0]))
        {
            return true;
        }

        return StartsWithText(content, "<?php") ||
               StartsWithText(content, "<script") ||
               StartsWithText(content, "<html") ||
               StartsWithText(content, "<!doctype") ||
               StartsWithText(content, "<svg") ||
               StartsWithText(content, "<?xml");
    }

    private static bool StartsWith(ReadOnlySpan<byte> content, ReadOnlySpan<byte> signature) =>
        content.Length >= signature.Length && content[..signature.Length].SequenceEqual(signature);

    private static bool StartsWithText(ReadOnlySpan<byte> content, string prefix)
    {
        if (StartsWith(content, [0xEF, 0xBB, 0xBF]))
            content = content[3..];

        while (content.Length > 0 && content[0] is 0x20 or 0x09 or 0x0A or 0x0D)
            content = content[1..];

        if (content.Length < prefix.Length)
            return false;

        for (var index = 0; index < prefix.Length; index++)
        {
            var actual = (char)content[index];
            if (char.ToLowerInvariant(actual) != prefix[index])
                return false;
        }

        return true;
    }
}
