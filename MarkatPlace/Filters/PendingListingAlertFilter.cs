using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;

namespace MarkatPlace.Filters;

public class PendingListingAlertFilter : IAsyncActionFilter
{
    private readonly ILogger<PendingListingAlertFilter> _logger;

    public PendingListingAlertFilter(ILogger<PendingListingAlertFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executed = await next();

        if (executed.Exception is not null && !executed.ExceptionHandled)
            return;

        if (!HttpMethods.IsPost(context.HttpContext.Request.Method))
            return;

        var statusCode = (executed.Result as IStatusCodeActionResult)?.StatusCode
            ?? executed.HttpContext.Response.StatusCode;

        if (statusCode != StatusCodes.Status201Created)
            return;

        if (!TryReadCreated(executed.Result, out var listingId, out var title, out var postType))
            return;

        if (!TryResolveModule(context, postType, out var module))
            return;

        try
        {
            var alerts = context.HttpContext.RequestServices.GetRequiredService<IAdminAlertService>();

            await alerts.NotifyPendingListingAsync(
                module, listingId, title, cycleStartedUtc: null, context.HttpContext.RequestAborted);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to alert the administrators about the new {Module} listing {ListingId}.",
                module, listingId);
        }
    }

    private static bool TryResolveModule(
        ActionExecutingContext context, PostType? postType, out ListingModuleType module)
    {
        module = default;

        var template = context.ActionDescriptor.AttributeRouteInfo?.Template;

        if (string.IsNullOrEmpty(template))
            return false;

        var segments = template.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length != 2 || !segments[0].Equals("api", StringComparison.OrdinalIgnoreCase))
            return false;

        if (ListingModuleCatalog.ModuleOfRoute(segments[1]) is { } resolved)
        {
            module = resolved;
            return true;
        }

        if (segments[1].Equals(LostFoundRoute, StringComparison.OrdinalIgnoreCase) &&
            postType is { } type)
        {
            module = ListingModuleCatalog.ModuleOf(type);
            return true;
        }

        return false;
    }

    private const string LostFoundRoute = "lost-found";

    private static bool TryReadCreated(
        IActionResult? result, out Guid listingId, out string? title, out PostType? postType)
    {
        listingId = default;
        title = null;
        postType = null;

        if (result is not ObjectResult { Value: { } envelope })
            return false;

        var data = envelope.GetType()
            .GetProperty("Data", BindingFlags.Public | BindingFlags.Instance)
            ?.GetValue(envelope);

        if (data is null)
            return false;

        var type = data.GetType();

        if (type.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)?.GetValue(data)
            is not Guid id)
        {
            return false;
        }

        listingId = id;

        foreach (var name in new[] { "Title", "AdTitle", "ItemName", "Name", "ProductName" })
        {
            if (type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance)?.GetValue(data)
                is string value && !string.IsNullOrWhiteSpace(value))
            {
                title = value;
                break;
            }
        }

        if (type.GetProperty("PostType", BindingFlags.Public | BindingFlags.Instance)?.GetValue(data)
            is PostType resolvedPostType)
        {
            postType = resolvedPostType;
        }

        return true;
    }
}
