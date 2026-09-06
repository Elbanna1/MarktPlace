using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Settings;

namespace Persistence.Services;

public class ReferralLinkBuilder : IReferralLinkBuilder
{
    private readonly AppSettings _app;

    public ReferralLinkBuilder(IOptions<AppSettings> app)
    {
        _app = app.Value;
    }

    public string Build(string referralCode)
    {
        var path = _app.RegisterPath.StartsWith('/') ? _app.RegisterPath : $"/{_app.RegisterPath}";

        var query = $"{Uri.EscapeDataString(_app.ReferralQueryParameter)}=" +
                    Uri.EscapeDataString(referralCode);

        var separator = path.Contains('?') ? "&" : "?";

        return $"{ResolveFrontendUrl()}{path}{separator}{query}";
    }

    private string ResolveFrontendUrl()
    {
        var frontendUrl = _app.FrontendUrl ?? _app.BaseUrl;

        if (string.IsNullOrWhiteSpace(frontendUrl))
        {
            throw new InvalidOperationException(
                $"{AppSettings.SectionName}:FrontendUrl is not configured, so an invitation link " +
                "cannot be built. Set it to the public address of the frontend site (for example " +
                "'App__FrontendUrl=https://shopiklopik.com'). The link is deliberately never derived " +
                "from the incoming request, because that would point invitees at the API host " +
                "instead of the site they are meant to register on.");
        }

        return frontendUrl.TrimEnd('/');
    }
}
