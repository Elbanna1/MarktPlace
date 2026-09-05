using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Presentation.Controllers.Admin;
using ServicesAbstraction;
using Shared.Authorization;
using Shared.Constants;
using Shared.Responses;
using System.Security.Claims;

namespace MarkatPlace.Filters;

public class AdminPermissionFilter : IAsyncAuthorizationFilter
{
    private const string LegacyAdminRoutePrefix = "/api/admin";

    private readonly IAdminPermissionService _permissions;
    private readonly ILogger<AdminPermissionFilter> _logger;

    public AdminPermissionFilter(
        IAdminPermissionService permissions,
        ILogger<AdminPermissionFilter> logger)
    {
        _permissions = permissions;
        _logger = logger;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var metadata = context.ActionDescriptor.EndpointMetadata;

        if (metadata.OfType<IAllowAnonymous>().Any())
            return;

        var superAdminOnly = metadata.OfType<SuperAdminOnlyAttribute>().Any();
        var selfService = metadata.OfType<AdminSelfServiceAttribute>().Any();
        var requirement = metadata.OfType<RequireAdminPermissionAttribute>().LastOrDefault();
        var page = metadata.OfType<AdminPageAttribute>().LastOrDefault();

        var isAdminEndpoint = IsAdministrationEndpoint(context);

        if (!isAdminEndpoint && !superAdminOnly && !selfService && requirement is null)
            return;

        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cancellationToken = context.HttpContext.RequestAborted;

        if (superAdminOnly)
        {
            await ApplyAsync(context, await _permissions.AuthorizeSuperAdminAsync(userId, cancellationToken));
            return;
        }

        if (selfService)
        {
            await ApplyAsync(context, await _permissions.AuthorizeAdminAsync(userId, cancellationToken));
            return;
        }

        if (requirement is null)
        {
            _logger.LogError(
                "Administration endpoint {Route} ({Action}) declares no [RequireAdminPermission]; " +
                "refusing the request. Annotate the action, or mark it [AdminSelfService] / [SuperAdminOnly].",
                context.HttpContext.Request.Path,
                (context.ActionDescriptor as ControllerActionDescriptor)?.DisplayName);

            await ApplyAsync(context, AdminAccessResult.Denied());
            return;
        }

        var pageKey = requirement.PageKey ?? page?.PageKey;

        if (pageKey is null)
        {
            _logger.LogError(
                "Administration endpoint {Route} requires {Permission} but no [AdminPage] declares which " +
                "page it belongs to; refusing the request.",
                context.HttpContext.Request.Path, requirement.Permission);

            await ApplyAsync(context, AdminAccessResult.Denied());
            return;
        }

        await ApplyAsync(
            context,
            await _permissions.AuthorizeAsync(userId, pageKey, requirement.Permission, cancellationToken));
    }

    private static bool IsAdministrationEndpoint(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor descriptor &&
            typeof(AdminApiController).IsAssignableFrom(descriptor.ControllerTypeInfo))
            return true;

        var path = context.HttpContext.Request.Path;

        return path.StartsWithSegments(LegacyAdminRoutePrefix, StringComparison.OrdinalIgnoreCase) ||
               path.StartsWithSegments("/" + ApiVersions.AdminRoutePrefix, StringComparison.OrdinalIgnoreCase);
    }

    private static Task ApplyAsync(AuthorizationFilterContext context, AdminAccessResult result)
    {
        if (result.Allowed)
            return Task.CompletedTask;

        context.Result = new ObjectResult(ApiResponse.Fail(result.Message ?? "مالكش صلاحية توصل للمحتوى ده."))
        {
            StatusCode = result.StatusCode
        };

        return Task.CompletedTask;
    }
}
