using Shared.Settings;

namespace MarkatPlace.Extensions;

public static class CorsExtensions
{
    public const string PolicyName = "MarkatPlaceCors";

    public static IServiceCollection AddApiCors(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var settings = configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>()
            ?? new CorsSettings();

        foreach (var origin in settings.AllowedOrigins)
        {
            if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new InvalidOperationException(
                    $"{CorsSettings.SectionName}:AllowedOrigins carries '{origin}', which is not an " +
                    "absolute http or https origin. An entry must be a scheme, host and optional port " +
                    "with no path, for example 'https://shopiklopik.com' or 'http://localhost:5173'.");
            }
        }

        var allowLoopback = environment.IsDevelopment() && settings.AllowLocalhostInDevelopment;

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod();

                if (settings.AllowAnyOrigin || !settings.HasAllowedOrigins)
                {
                    policy.AllowAnyOrigin();
                    return;
                }

                policy.WithOrigins([.. settings.AllowedOrigins]).AllowCredentials();

                if (allowLoopback)
                    policy.SetIsOriginAllowed(IsAllowed(settings));
            });
        });

        return services;
    }

    private static Func<string, bool> IsAllowed(CorsSettings settings) =>
        origin =>
            settings.AllowedOrigins.Contains(origin.TrimEnd('/'), StringComparer.OrdinalIgnoreCase) ||
            IsLoopback(origin);

    private static bool IsLoopback(string origin) =>
        Uri.TryCreate(origin, UriKind.Absolute, out var uri) && uri.IsLoopback;
}
