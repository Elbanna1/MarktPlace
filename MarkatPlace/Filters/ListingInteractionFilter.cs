using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;
using Shared.Exceptions;
using System.Security.Claims;

namespace MarkatPlace.Filters;

public class ListingInteractionFilter : IAsyncActionFilter
{
    private readonly ILogger<ListingInteractionFilter> _logger;

    public ListingInteractionFilter(ILogger<ListingInteractionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executed = await next();

        if (executed.Exception is not null && !executed.ExceptionHandled)
            return;

        if (!IsSuccess(executed))
            return;

        if (!TryResolveListing(context, out var module, out var listingId))
            return;

        var method = context.HttpContext.Request.Method;

        try
        {
            var interactions = context.HttpContext.RequestServices
                .GetRequiredService<IListingInteractionService>();

            if (HttpMethods.IsGet(method))
            {
                await interactions.RecordViewAsync(
                    module, listingId,
                    context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    context.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    context.HttpContext.RequestAborted);
            }
            else if (HttpMethods.IsDelete(method))
            {
                await interactions.PurgeListingAsync(
                    module, listingId, context.HttpContext.RequestAborted);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (NotFoundException)
        {
            _logger.LogDebug(
                "No public listing to record an interaction against for {Method} {Module}/{ListingId}.",
                method, module, listingId);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Listing interaction bookkeeping failed for {Method} {Module}/{ListingId}.",
                method, module, listingId);
        }
    }

    private static bool IsSuccess(ActionExecutedContext executed)
    {
        var statusCode = (executed.Result as IStatusCodeActionResult)?.StatusCode
            ?? executed.HttpContext.Response.StatusCode;

        return statusCode is >= 200 and < 300;
    }

    private static bool TryResolveListing(
        ActionExecutingContext context, out ListingModuleType module, out Guid listingId)
    {
        module = default;
        listingId = default;

        var template = context.ActionDescriptor.AttributeRouteInfo?.Template;

        if (string.IsNullOrEmpty(template))
            return false;

        var segments = template.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length != 3 ||
            !segments[0].Equals("api", StringComparison.OrdinalIgnoreCase) ||
            !segments[2].StartsWith("{id", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (ListingModuleCatalog.ModuleOfRoute(segments[1]) is not { } resolved)
            return false;

        if (!context.RouteData.Values.TryGetValue("id", out var raw) ||
            !Guid.TryParse(raw?.ToString(), out listingId))
        {
            return false;
        }

        module = resolved;
        return true;
    }
}
