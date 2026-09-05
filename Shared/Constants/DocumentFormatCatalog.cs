namespace Shared.Constants;

public sealed record UploadFormatDescriptor(
    string Name,
    string CanonicalExtension,
    string CanonicalContentType,
    IReadOnlyList<string> Extensions,
    IReadOnlyList<string> ContentTypes);

public static class DocumentFormatCatalog
{
    public static readonly UploadFormatDescriptor Pdf = new(
        "PDF", ".pdf", "application/pdf",
        [".pdf"],
        ["application/pdf", "application/x-pdf", "application/acrobat", "text/pdf"]);

    public static readonly UploadFormatDescriptor Doc = new(
        "DOC", ".doc", "application/msword",
        [".doc"],
        ["application/msword", "application/vnd.ms-word", "application/doc", "application/x-msword"]);

    public static readonly UploadFormatDescriptor Docx = new(
        "DOCX", ".docx",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [".docx"],
        ["application/vnd.openxmlformats-officedocument.wordprocessingml.document"]);

    public static readonly IReadOnlyList<UploadFormatDescriptor> All = [Pdf, Docx, Doc];

    public static readonly string[] AllExtensions =
        All.SelectMany(format => format.Extensions).Distinct().OrderBy(extension => extension).ToArray();

    public static readonly string[] AllContentTypes =
        All.SelectMany(format => format.ContentTypes).Distinct().OrderBy(contentType => contentType).ToArray();

    public static readonly string DisplayNames = string.Join(", ", All.Select(format => format.Name));

    public static UploadFormatDescriptor? Detect(ReadOnlySpan<byte> content)
    {
        if (StartsWith(content, "%PDF-"u8))
            return Pdf;

        if (StartsWith(content, [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1]))
            return Doc;

        if (IsZip(content) && LooksLikeOfficeOpenXml(content))
            return Docx;

        return null;
    }

    private static bool IsZip(ReadOnlySpan<byte> content) =>
        StartsWith(content, [0x50, 0x4B, 0x03, 0x04]) ||
        StartsWith(content, [0x50, 0x4B, 0x05, 0x06]) ||
        StartsWith(content, [0x50, 0x4B, 0x07, 0x08]);

    private static bool LooksLikeOfficeOpenXml(ReadOnlySpan<byte> content)
    {
        var window = content.Length > 8192 ? content[..8192] : content;

        return Contains(window, "[Content_Types].xml"u8) || Contains(window, "word/"u8);
    }

    private static bool Contains(ReadOnlySpan<byte> haystack, ReadOnlySpan<byte> needle) =>
        haystack.IndexOf(needle) >= 0;

    private static bool StartsWith(ReadOnlySpan<byte> content, ReadOnlySpan<byte> signature) =>
        content.Length >= signature.Length && content[..signature.Length].SequenceEqual(signature);
}
