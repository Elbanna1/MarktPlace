using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Constants;
using Shared.Responses;
using Shared.Settings;

namespace MarkatPlace.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddApiRateLimiting(
        this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new RateLimitingSettings();

        configuration.GetSection(RateLimitingSettings.SectionName).Bind(settings);

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                }

                if (context.HttpContext.Response.HasStarted)
                    return;

                context.HttpContext.Response.ContentType = "application/json";

                var response = ApiResponse.Fail(UserMessages.Errors.TooManyRequests);

                await context.HttpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(response, ApiJson.SerializerOptions), cancellationToken);
            };

            if (!settings.Enabled)
            {
                options.AddPolicy(RateLimitPolicies.Auth, _ => RateLimitPartition.GetNoLimiter("disabled"));
                return;
            }

            // A signed-in caller is partitioned by account and gets the authenticated budget;
            // everyone else is partitioned by address and gets the anonymous one. That is what
            // keeps one noisy neighbour behind a shared address — CGNAT, a campus, an office —
            // from spending a signed-in user's budget.
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                Partition(context, settings.Authenticated, settings.Anonymous));

            options.AddPolicy(RateLimitPolicies.Auth, context =>
                Partition(context, settings.Auth, settings.Auth, prefix: "auth:"));
        });

        return services;
    }

    private static RateLimitPartition<string> Partition(
        HttpContext context,
        RateLimitWindowSettings authenticated,
        RateLimitWindowSettings anonymous,
        string prefix = "")
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (key, limits) = string.IsNullOrEmpty(userId)
            ? ($"{prefix}{ClientAddressKey(context)}", anonymous)
            : ($"{prefix}user:{userId}", authenticated);

        return RateLimitPartition.GetSlidingWindowLimiter(key, _ => new SlidingWindowRateLimiterOptions
        {
            PermitLimit = limits.PermitPerWindow,
            Window = limits.Window,
            SegmentsPerWindow = limits.SegmentsPerWindow,
            QueueLimit = limits.QueueLimit
        });
    }

    // UseForwardedHeaders has already rewritten RemoteIpAddress from X-Forwarded-For by the time
    // the limiter runs, so this is the client's address and not Nginx's. If the forwarded-header
    // configuration ever stops trusting the proxy, every caller collapses onto one partition —
    // see the ForwardedHeaders notes in appsettings.Production.json.
    private static string ClientAddressKey(HttpContext context)
    {
        var address = context.Connection.RemoteIpAddress;

        return address is null ? "anonymous" : $"ip:{address}";
    }
}
