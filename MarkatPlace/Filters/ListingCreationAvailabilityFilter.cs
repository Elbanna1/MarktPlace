using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServicesAbstraction;
using Shared.Catalog;
using Shared.Constants;
using Shared.Enums;
using Shared.Responses;

namespace MarkatPlace.Filters;

public class ListingCreationAvailabilityFilter : IAsyncActionFilter
{
    private const string LostFoundRoute = "lost-found";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (HttpMethods.IsPost(context.HttpContext.Request.Method) &&
            ResolveCreation(context) is { } creation)
        {
            var provider = context.HttpContext.RequestServices.GetService<ICatalogConfigurationProvider>();

            if (provider is not null)
            {
                var snapshot = await provider.GetSnapshotAsync(context.HttpContext.RequestAborted);

                if (!snapshot.IsVisible(creation, CatalogArea.AdCreation, DateTime.UtcNow))
                {
                    context.Result = new ObjectResult(ApiResponse.Fail(UserMessages.Lookups.CreationUnavailable))
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };

                    return;
                }
            }
        }

        await next();
    }

    private static CatalogContext? ResolveCreation(ActionExecutingContext context)
    {
        var template = context.ActionDescriptor.AttributeRouteInfo?.Template;

        if (string.IsNullOrEmpty(template))
            return null;

        var segments = template.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length != 2 || !segments[0].Equals("api", StringComparison.OrdinalIgnoreCase))
            return null;

        var subCategoryId = ReadArgument<int>(context, "SubCategoryId");

        if (ListingModuleCatalog.ModuleOfRoute(segments[1]) is { } module)
            return CatalogContext.ForModule(module, subCategoryId);

        if (segments[1].Equals(LostFoundRoute, StringComparison.OrdinalIgnoreCase) &&
            ReadArgument<PostType>(context, "PostType") is { } postType)
            return CatalogContext.ForModule(ListingModuleCatalog.ModuleOf(postType));

        return null;
    }

    private static T? ReadArgument<T>(ActionExecutingContext context, string name) where T : struct
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var value = argument.GetType()
                .GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                ?.GetValue(argument);

            if (value is T typed)
                return typed;
        }

        return null;
    }
}
