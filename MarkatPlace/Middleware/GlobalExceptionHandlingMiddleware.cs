using System.Text.Json;
using Shared.Constants;
using Shared.Exceptions;
using Shared.Responses;

namespace MarkatPlace.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private static readonly string[] PreservedHeaders =
    [
        "Access-Control-Allow-Origin",
        "Access-Control-Allow-Credentials",
        "Access-Control-Allow-Headers",
        "Access-Control-Allow-Methods",
        "Access-Control-Expose-Headers",
        "Access-Control-Max-Age",
        "Vary"
    ];

    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            _logger.LogWarning(ex, "Handled application exception: {Message}", ex.Message);
            await WriteResponseAsync(context, ex.StatusCode, ex.Message, ex.Errors);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogInformation(
                "Request {Method} {Path} was cancelled by the client.",
                context.Request.Method, context.Request.Path);

            if (!context.Response.HasStarted)
                context.Response.StatusCode = StatusCodesExtra.ClientClosedRequest;
        }
        catch (BadHttpRequestException ex)
        {
            var statusCode = ex.StatusCode == StatusCodes.Status413PayloadTooLarge
                ? StatusCodes.Status413PayloadTooLarge
                : StatusCodes.Status400BadRequest;

            _logger.LogWarning(
                "Rejected {Method} {Path} at the protocol level with {StatusCode}: {Message}",
                context.Request.Method, context.Request.Path, statusCode, ex.Message);

            await WriteResponseAsync(context, statusCode, DescribeProtocolFailure(statusCode), null);
        }
        catch (InvalidDataException ex)
        {
            _logger.LogWarning(
                "Rejected the multipart body of {Method} {Path}: {Message}",
                context.Request.Method, context.Request.Path, ex.Message);

            await WriteResponseAsync(
                context,
                StatusCodes.Status413PayloadTooLarge,
                DescribeProtocolFailure(StatusCodes.Status413PayloadTooLarge),
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await WriteResponseAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "حصل خطأ غير متوقع. من فضلك حاول تاني بعد شوية.",
                null);
        }
    }

    private static string DescribeProtocolFailure(int statusCode) =>
        statusCode == StatusCodes.Status413PayloadTooLarge
            ? $"{UserMessages.Errors.RequestTooLarge} " +
              $"أقصى حجم للطلب الواحد {FileUploadConstants.MaxRequestBodySizeMegabytes} ميجابايت."
            : UserMessages.Errors.MalformedRequest;

    private async Task WriteResponseAsync(
        HttpContext context,
        int statusCode,
        string message,
        IReadOnlyList<string>? errors)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogWarning(
                "Cannot write the error response for {Method} {Path}: the response had already started.",
                context.Request.Method, context.Request.Path);
            return;
        }

        var preserved = new List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>>();

        foreach (var name in PreservedHeaders)
        {
            if (context.Response.Headers.TryGetValue(name, out var value))
                preserved.Add(new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>(name, value));
        }

        context.Response.Clear();

        foreach (var header in preserved)
            context.Response.Headers[header.Key] = header.Value;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = ApiResponse.Fail(message, errors);

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, SerializerOptions));
    }
}

internal static class StatusCodesExtra
{
    public const int ClientClosedRequest = 499;
}
