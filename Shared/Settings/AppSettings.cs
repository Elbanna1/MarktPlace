using Shared.Constants;

namespace Shared.Settings;

public class AppSettings
{
    public const string SectionName = "App";

    public string? BaseUrl { get; set; }

    private string? _frontendUrl;

    public string? FrontendUrl
    {
        get => _frontendUrl;
        set => _frontendUrl = string.IsNullOrWhiteSpace(value) ? null : value.Trim().TrimEnd('/');
    }

    private string _registerPath = ReferralCatalog.DefaultRegisterPath;

    public string RegisterPath
    {
        get => _registerPath;
        set => _registerPath = string.IsNullOrWhiteSpace(value)
            ? ReferralCatalog.DefaultRegisterPath
            : value.Trim();
    }

    private string _createAdPath = FrontendRoutes.DefaultCreateAdPath;

    public string CreateAdPath
    {
        get => _createAdPath;
        set => _createAdPath = string.IsNullOrWhiteSpace(value)
            ? FrontendRoutes.DefaultCreateAdPath
            : "/" + value.Trim().Trim('/');
    }

    private string _homePath = FrontendRoutes.DefaultHomePath;

    public string HomePath
    {
        get => _homePath;
        set => _homePath = string.IsNullOrWhiteSpace(value)
            ? FrontendRoutes.DefaultHomePath
            : "/" + value.Trim().TrimStart('/');
    }

    private string _referralQueryParameter = ReferralCatalog.DefaultQueryParameter;

    public string ReferralQueryParameter
    {
        get => _referralQueryParameter;
        set => _referralQueryParameter = string.IsNullOrWhiteSpace(value)
            ? ReferralCatalog.DefaultQueryParameter
            : value.Trim();
    }
}
