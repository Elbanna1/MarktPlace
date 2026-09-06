using Shared.Settings;

namespace MarkatPlace.Extensions;

public static class AppUrlSettingsExtensions
{
    public static IServiceCollection AddAppUrlSettings(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var app = configuration.GetSection(AppSettings.SectionName).Get<AppSettings>() ?? new AppSettings();

        if (string.IsNullOrWhiteSpace(app.FrontendUrl))
        {
            if (environment.IsDevelopment())
                return services;

            throw new InvalidOperationException(
                $"{AppSettings.SectionName}:FrontendUrl is not configured. Every invitation link is " +
                "built from it, so without it invitees would be sent to the API host instead of the " +
                "site. Set it to the public address of the frontend, for example the environment " +
                "variable 'App__FrontendUrl=https://shopiklopik.com'.");
        }

        if (!Uri.TryCreate(app.FrontendUrl, UriKind.Absolute, out var frontendUri) ||
            (frontendUri.Scheme != Uri.UriSchemeHttp && frontendUri.Scheme != Uri.UriSchemeHttps))
        {
            throw new InvalidOperationException(
                $"{AppSettings.SectionName}:FrontendUrl ('{app.FrontendUrl}') is not an absolute http " +
                "or https URL. Invitation links are built by appending the register path to it, so it " +
                "must carry a scheme and a host, for example 'https://shopiklopik.com'.");
        }

        if (!environment.IsDevelopment() && frontendUri.IsLoopback)
        {
            throw new InvalidOperationException(
                $"{AppSettings.SectionName}:FrontendUrl ('{app.FrontendUrl}') points at localhost. " +
                "Invitation links carrying that address are unusable outside the server. Set it to " +
                "the public address of the frontend, for example 'https://shopiklopik.com'.");
        }

        return services;
    }
}
