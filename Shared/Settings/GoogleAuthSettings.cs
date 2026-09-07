namespace Shared.Settings;

public class GoogleAuthSettings
{
    public const string SectionName = "GoogleAuth";

    private string? _clientId;

    public string? ClientId
    {
        get => _clientId;
        set => _clientId = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private string? _clientSecret;

    public string? ClientSecret
    {
        get => _clientSecret;
        set => _clientSecret = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private string _redirectUri = Shared.Constants.GoogleAuthCatalog.PostMessageRedirectUri;

    public string RedirectUri
    {
        get => _redirectUri;
        set => _redirectUri = string.IsNullOrWhiteSpace(value)
            ? Shared.Constants.GoogleAuthCatalog.PostMessageRedirectUri
            : value.Trim();
    }

    public bool RequireVerifiedEmail { get; set; } = true;

    public bool LinkVerifiedEmailToExistingAccount { get; set; } = true;

    public bool IsConfigured => !string.IsNullOrWhiteSpace(ClientId);

    public bool CanExchangeAuthorizationCode => IsConfigured && !string.IsNullOrWhiteSpace(ClientSecret);
}
