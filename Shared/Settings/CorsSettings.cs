namespace Shared.Settings;

public class CorsSettings
{
    public const string SectionName = "Cors";

    private IReadOnlyList<string> _allowedOrigins = [];

    public IReadOnlyList<string> AllowedOrigins
    {
        get => _allowedOrigins;
        set => _allowedOrigins = Normalize(value);
    }

    public bool AllowAnyOrigin { get; set; }

    public bool AllowLocalhostInDevelopment { get; set; } = true;

    public bool HasAllowedOrigins => AllowedOrigins.Count > 0;

    private static IReadOnlyList<string> Normalize(IReadOnlyList<string>? origins) =>
        origins is null
            ? []
            : origins
                .Where(origin => !string.IsNullOrWhiteSpace(origin))
                .Select(origin => origin.Trim().TrimEnd('/'))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
}
