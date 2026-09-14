using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Shared.Settings;

namespace MarkatPlace.Middleware;

public class SwaggerBasicAuthMiddleware
{
    private static readonly PathString SwaggerRoot = new(SwaggerAuthSettings.PathPrefix);

    private static readonly string Challenge =
        $"{SwaggerAuthSettings.Scheme} realm=\"{SwaggerAuthSettings.Realm}\"";

    private static readonly string HeaderPrefix = SwaggerAuthSettings.Scheme + " ";

    private readonly RequestDelegate _next;
    private readonly SwaggerAuthSettings _settings;
    private readonly bool _enforced;
    private readonly bool _requiresHttps;

    public SwaggerBasicAuthMiddleware(
        RequestDelegate next,
        IOptions<SwaggerAuthSettings> settings,
        IHostEnvironment environment)
    {
        _next = next;
        _settings = settings.Value;

        var isDevelopment = environment.IsDevelopment();

        _enforced = !isDevelopment || _settings.IsConfigured;
        _requiresHttps = !isDevelopment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_enforced ||
            !context.Request.Path.StartsWithSegments(SwaggerRoot, StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        if (_requiresHttps && !context.Request.IsHttps)
        {
            Redirect(context);
            return;
        }

        if (!_settings.IsConfigured || !IsAuthorised(context.Request))
        {
            Refuse(context);
            return;
        }

        await _next(context);
    }

    private static void Redirect(HttpContext context)
    {
        var request = context.Request;

        context.Response.Headers.CacheControl = "no-store";
        context.Response.Redirect(
            $"https://{request.Host.Host}{request.PathBase}{request.Path}{request.QueryString}");
    }

    private static void Refuse(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers.WWWAuthenticate = Challenge;
        context.Response.Headers.CacheControl = "no-store";
    }

    private bool IsAuthorised(HttpRequest request)
    {
        var header = request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(header) ||
            !header.StartsWith(HeaderPrefix, StringComparison.OrdinalIgnoreCase))
            return false;

        byte[] decoded;

        try
        {
            decoded = Convert.FromBase64String(header[HeaderPrefix.Length..].Trim());
        }
        catch (FormatException)
        {
            return false;
        }

        var pair = Encoding.UTF8.GetString(decoded);
        var separator = pair.IndexOf(':');

        if (separator < 0)
            return false;

        var username = Matches(pair[..separator], _settings.Username);
        var password = Matches(pair[(separator + 1)..], _settings.Password);

        return username & password;
    }

    private static bool Matches(string candidate, string? expected) =>
        expected is not null &&
        CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(candidate), Encoding.UTF8.GetBytes(expected));
}
