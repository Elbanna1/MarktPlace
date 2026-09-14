namespace Shared.Settings;

public class SwaggerAuthSettings
{
    public const string SectionName = "SwaggerAuth";

    public const string Scheme = "Basic";

    public const string Realm = "Swagger";

    public const string PathPrefix = "/swagger";

    private string? _username;

    public string? Username
    {
        get => _username;
        set => _username = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private string? _password;

    public string? Password
    {
        get => _password;
        set => _password = string.IsNullOrEmpty(value) ? null : value;
    }

    public bool IsConfigured => _username is not null && _password is not null;
}
