using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Settings;

namespace Persistence.Services;

public class ReferralLinkBuilder : IReferralLinkBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppSettings _app;

    public ReferralLinkBuilder(IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> app)
    {
        _httpContextAccessor = httpContextAccessor;
        _app = app.Value;
    }

    public string Build(string referralCode)
    {
        var path = _app.RegisterPath.StartsWith('/') ? _app.RegisterPath : $"/{_app.RegisterPath}";

        var query = $"{Uri.EscapeDataString(_app.ReferralQueryParameter)}=" +
                    Uri.EscapeDataString(referralCode);

        var separator = path.Contains('?') ? "&" : "?";

        return $"{ResolveBaseUrl()}{path}{separator}{query}";
    }

    private string ResolveBaseUrl()
    {
        if (!string.IsNullOrWhiteSpace(_app.FrontendUrl))
            return _app.FrontendUrl.TrimEnd('/');

        var request = _httpContextAccessor.HttpContext?.Request;
        if (request is not null)
        {
            var pathBase = request.PathBase.HasValue ? request.PathBase.Value!.TrimEnd('/') : string.Empty;
            return $"{request.Scheme}://{request.Host.Value}{pathBase}";
        }

        return _app.BaseUrl?.TrimEnd('/') ?? string.Empty;
    }
}
