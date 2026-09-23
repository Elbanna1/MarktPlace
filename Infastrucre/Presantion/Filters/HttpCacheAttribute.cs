using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using JsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Presentation.Filters;

public enum HttpCacheScope
{
    ViewerSpecific = 0,

    Shared = 1
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class HttpCacheAttribute : Attribute, IAsyncResultFilter
{
    public int MaxAgeSeconds { get; init; }

    public bool Revalidate { get; init; }

    public HttpCacheScope Scope { get; init; } = HttpCacheScope.ViewerSpecific;

    private static readonly string[] EveryQueryKey = ["*"];

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.HttpContext.Features.Get<IResponseCachingFeature>() is { } responseCaching)
            responseCaching.VaryByQueryKeys = EveryQueryKey;

        if (TryGetPayload(context, out var payload))
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            var shared = Scope == HttpCacheScope.Shared;

            var directive = shared || !request.Headers.ContainsKey(HeaderNames.Authorization)
                ? "public"
                : "private";

            response.Headers.CacheControl = Revalidate
                ? $"{directive}, no-cache"
                : $"{directive}, max-age={MaxAgeSeconds}";

            AppendVary(response, HeaderNames.AcceptEncoding);

            AppendVary(response, HeaderNames.Origin);

            if (!shared)
                AppendVary(response, HeaderNames.Authorization);

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

    private static void AppendVary(HttpResponse response, string header)
    {
        var current = response.Headers.Vary;

        foreach (var value in current)
        {
            if (value is null)
                continue;

            foreach (var candidate in value.Split(','))
            {
                if (string.Equals(candidate.Trim(), header, StringComparison.OrdinalIgnoreCase))
                    return;
            }
        }

        response.Headers.Vary = StringValues.Concat(current, header);
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
