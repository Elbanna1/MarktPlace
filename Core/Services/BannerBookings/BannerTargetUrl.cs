using Shared.Constants;
using Shared.Exceptions;

namespace Services.BannerBookings;

public static class BannerTargetUrl
{
    public readonly record struct Target(string Url, bool IsInternal);

    public static bool IsValid(string? url) => TryParse(url, out _);

    public static Target Parse(string? url) =>
        TryParse(url, out var target)
            ? target
            : throw new BadRequestException(
                "رابط الإعلان غير صحيح. استخدم رابطًا خارجيًا يبدأ بـ https:// أو مسارًا داخل التطبيق يبدأ بـ /.");

    private static bool TryParse(string? url, out Target target)
    {
        target = default;

        if (string.IsNullOrWhiteSpace(url))
            return false;

        var trimmed = url.Trim();

        if (trimmed.Length > BannerBookingCatalog.MaxTargetUrlLength)
            return false;

        if (trimmed.Any(character => char.IsControl(character) || char.IsWhiteSpace(character)))
            return false;

        if (trimmed.StartsWith('/'))
        {
            if (trimmed.StartsWith("//", StringComparison.Ordinal))
                return false;

            if (trimmed.Contains("..", StringComparison.Ordinal))
                return false;

            target = new Target(trimmed, IsInternal: true);
            return true;
        }

        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out var uri))
            return false;

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            return false;

        if (string.IsNullOrWhiteSpace(uri.Host) || !uri.Host.Contains('.', StringComparison.Ordinal))
            return false;

        target = new Target(uri.AbsoluteUri, IsInternal: false);
        return true;
    }
}
