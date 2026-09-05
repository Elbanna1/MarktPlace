using Shared.Constants;

namespace Shared.Settings;

public class AppSettings
{
    public const string SectionName = "App";

    public string? BaseUrl { get; set; }

    public string? FrontendUrl { get; set; }

    private string _registerPath = ReferralCatalog.DefaultRegisterPath;

    public string RegisterPath
    {
        get => _registerPath;
        set => _registerPath = string.IsNullOrWhiteSpace(value)
            ? ReferralCatalog.DefaultRegisterPath
            : value.Trim();
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
