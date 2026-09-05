using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Constants;
using Shared.Responses;

namespace MarkatPlace.Extensions;

public static class RateLimitingExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static IServiceCollection AddApiRateLimiting(
        this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("RateLimiting");

        var enabled = section.GetValue("Enabled", true);
        var permitPerMinute = section.GetValue("PermitPerMinute", 600);
        var authPermitPerMinute = section.GetValue("AuthPermitPerMinute", 20);

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
                    JsonSerializer.Serialize(response, SerializerOptions), cancellationToken);
            };

            if (!enabled)
            {
                options.AddPolicy(RateLimitPolicies.Auth, _ => RateLimitPartition.GetNoLimiter("disabled"));
                return;
            }

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ResolveClientKey(context),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = permitPerMinute,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    }));

            options.AddPolicy(RateLimitPolicies.Auth, context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ResolveClientKey(context),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = authPermitPerMinute,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    }));
        });

        return services;
    }

    private static string ResolveClientKey(HttpContext context)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
            return $"user:{userId}";

        var ip = context.Connection.RemoteIpAddress?.ToString();
        return string.IsNullOrEmpty(ip) ? "anonymous" : $"ip:{ip}";
    }
}
