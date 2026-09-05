using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.Responses;
using Shared.Settings;

namespace MarkatPlace.Extensions;

public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings are not configured.");

        if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) ||
            Encoding.UTF8.GetByteCount(jwtSettings.SecretKey) < 32)
        {
            throw new InvalidOperationException(
                "JwtSettings:SecretKey is missing or too short. Provide at least a 32-character secret " +
                "(via environment variable 'JwtSettings__SecretKey' in production).");
        }

        if (!environment.IsDevelopment() && IsKnownInsecureKey(jwtSettings.SecretKey))
        {
            throw new InvalidOperationException(
                "JwtSettings:SecretKey is a placeholder or a value that has been published in source " +
                "control, so it cannot be treated as a secret. Generate a new random secret of at " +
                "least 32 characters and supply it as the environment variable " +
                "'JwtSettings__SecretKey'. Note that changing it invalidates every access and " +
                "refresh token already issued: users will have to sign in again.");
        }

        if (jwtSettings.AccessTokenExpirationDays <= 0)
        {
            throw new InvalidOperationException(
                "JwtSettings:AccessTokenExpirationDays must be greater than zero. Note the setting is " +
                "expressed in DAYS and replaces the former 'AccessTokenExpirationMinutes' key.");
        }

        if (jwtSettings.RefreshTokenExpirationDays <= jwtSettings.AccessTokenExpirationDays)
        {
            throw new InvalidOperationException(
                $"JwtSettings:RefreshTokenExpirationDays ({jwtSettings.RefreshTokenExpirationDays}) must be " +
                $"greater than JwtSettings:AccessTokenExpirationDays ({jwtSettings.AccessTokenExpirationDays}).");
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;

            options.RequireHttpsMetadata = !environment.IsDevelopment();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = ctx =>
                {
                    var accessToken = ctx.Request.Query["access_token"];
                    var path = ctx.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        ctx.Token = accessToken;
                    return Task.CompletedTask;
                },
                OnChallenge = async ctx =>
                {
                    ctx.HandleResponse();
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    ctx.Response.ContentType = "application/json";
                    var response = ApiResponse.Fail("لازم تسجل دخولك عشان توصل للمحتوى ده.");
                    await ctx.Response.WriteAsync(JsonSerializer.Serialize(response,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                },
                OnForbidden = async ctx =>
                {
                    ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                    ctx.Response.ContentType = "application/json";
                    var response = ApiResponse.Fail("مالكش صلاحية توصل للمحتوى ده.");
                    await ctx.Response.WriteAsync(JsonSerializer.Serialize(response,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                }
            };
        });

        services.AddAuthorization();

        return services;
    }

    private static bool IsKnownInsecureKey(string secretKey)
    {
        string[] compromisedFragments =
        [
            "CHANGE-THIS-TO-A-LONG-RANDOM-SECRET",
            "9f83bd21e7c04a1c",
            "your-secret-key",
            "secretsecretsecret",
            "0123456789"
        ];

        return compromisedFragments.Any(fragment =>
            secretKey.Contains(fragment, StringComparison.OrdinalIgnoreCase));
    }
}
