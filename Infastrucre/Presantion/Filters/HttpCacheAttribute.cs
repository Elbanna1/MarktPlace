using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using JsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Presentation.Filters;

[AttributeUsage(AttributeTargets.Method)]
public sealed class HttpCacheAttribute : Attribute, IAsyncResultFilter
{
    public int MaxAgeSeconds { get; init; }

    public bool Revalidate { get; init; }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (TryGetPayload(context, out var payload))
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var scope = request.Headers.ContainsKey(HeaderNames.Authorization) ? "private" : "public";

            response.Headers.CacheControl = Revalidate
                ? $"{scope}, no-cache"
                : $"{scope}, max-age={MaxAgeSeconds}";

            var entityTag = ComputeEntityTag(context.HttpContext.RequestServices, payload);

            if (entityTag is not null)
            {
                response.Headers.ETag = entityTag;

                if (IsNoneMatch(request.Headers.IfNoneMatch, entityTag))
                    context.Result = new StatusCodeResult(StatusCodes.Status304NotModified);
            }
        }

        await next();
    }

    private static bool TryGetPayload(
        ResultExecutingContext context, [NotNullWhen(true)] out object? payload)
    {
        payload = null;

        var method = context.HttpContext.Request.Method;

        if (!HttpMethods.IsGet(method) && !HttpMethods.IsHead(method))
            return false;

        if (context.Result is not ObjectResult { Value: { } value } result)
            return false;

        if (result.StatusCode is not (null or StatusCodes.Status200OK))
            return false;

        payload = value;
        return true;
    }

    private static string? ComputeEntityTag(IServiceProvider services, object payload)
    {
        try
        {
            var options = services.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;
            var body = JsonSerializer.SerializeToUtf8Bytes(payload, payload.GetType(), options);
            var digest = Convert.ToHexString(SHA256.HashData(body))[..32].ToLowerInvariant();

            return $"W/\"{digest}\"";
        }
        catch (Exception exception)
        {
            services.GetService<ILogger<HttpCacheAttribute>>()?.LogDebug(
                exception, "Computing an ETag failed; serving the response without one.");

            return null;
        }
    }

    private static bool IsNoneMatch(StringValues header, string entityTag)
    {
        var current = Opaque(entityTag);

        foreach (var value in header)
        {
            if (string.IsNullOrEmpty(value))
                continue;

            foreach (var candidate in value.Split(','))
            {
                var trimmed = candidate.Trim();

                if (trimmed == "*")
                    return true;

                if (string.Equals(Opaque(trimmed), current, StringComparison.Ordinal))
                    return true;
            }
        }

        return false;
    }

    private static string Opaque(string entityTag) =>
        entityTag.StartsWith("W/", StringComparison.Ordinal)
            ? entityTag[2..].Trim()
            : entityTag;
}
