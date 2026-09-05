namespace Shared.Settings;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string Issuer { get; set; } = default!;

    public string Audience { get; set; } = default!;

    public string SecretKey { get; set; } = default!;

    public int AccessTokenExpirationDays { get; set; }

    public int RefreshTokenExpirationDays { get; set; }
}
