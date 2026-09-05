namespace Shared.Constants;

public static class VideoFormatCatalog
{
    public static readonly UploadFormatDescriptor Mp4 = new(
        "MP4", ".mp4", "video/mp4",
        [".mp4", ".m4v"],
        ["video/mp4", "video/x-m4v", "application/mp4"]);

    public static readonly UploadFormatDescriptor Mov = new(
        "MOV", ".mov", "video/quicktime",
        [".mov", ".qt"],
        ["video/quicktime", "video/x-quicktime"]);

    public static readonly IReadOnlyList<UploadFormatDescriptor> All = [Mp4, Mov];

    public static readonly string[] AllExtensions =
        All.SelectMany(format => format.Extensions).Distinct().OrderBy(extension => extension).ToArray();

    public static readonly string[] AllContentTypes =
        All.SelectMany(format => format.ContentTypes).Distinct().OrderBy(contentType => contentType).ToArray();

    public static readonly string DisplayNames = string.Join(", ", All.Select(format => format.Name));

    public static UploadFormatDescriptor? Detect(ReadOnlySpan<byte> content)
    {
        if (content.Length < 12 || !content.Slice(4, 4).SequenceEqual("ftyp"u8))
            return null;

        var brand = content.Slice(8, 4);

        if (brand.SequenceEqual("qt  "u8))
            return Mov;

        if (brand.SequenceEqual("isom"u8) || brand.SequenceEqual("iso2"u8) ||
            brand.SequenceEqual("iso4"u8) || brand.SequenceEqual("iso5"u8) ||
            brand.SequenceEqual("iso6"u8) || brand.SequenceEqual("mp41"u8) ||
            brand.SequenceEqual("mp42"u8) || brand.SequenceEqual("avc1"u8) ||
            brand.SequenceEqual("dash"u8) || brand.SequenceEqual("mmp4"u8) ||
            brand.SequenceEqual("M4V "u8) || brand.SequenceEqual("M4VP"u8))
        {
            return Mp4;
        }

        return null;
    }
}
