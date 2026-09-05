using System.Text.Json;
using Shared.Exceptions;
using Shared.Responses;

namespace MarkatPlace.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

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

        context.Response.Clear();
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
