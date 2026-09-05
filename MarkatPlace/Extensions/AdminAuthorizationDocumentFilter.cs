using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Shared.Authorization;
using Shared.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarkatPlace.Extensions;

public class AdminAuthorizationOperationFilter : IOperationFilter
{
    public const string ExtensionName = "x-admin-authorization";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

        var superAdminOnly = metadata.OfType<SuperAdminOnlyAttribute>().Any();
        var selfService = metadata.OfType<AdminSelfServiceAttribute>().Any();
        var requirement = metadata.OfType<RequireAdminPermissionAttribute>().LastOrDefault();
        var page = metadata.OfType<AdminPageAttribute>().LastOrDefault();

        if (!superAdminOnly && !selfService && requirement is null)
            return;

        var rule = superAdminOnly
            ? "Super Admin only"
            : selfService
                ? "Any active Admin — no page permission required"
                : Describe(requirement!, page);

        operation.Extensions[ExtensionName] = new OpenApiString(rule);
    }

    private static string Describe(RequireAdminPermissionAttribute requirement, AdminPageAttribute? page)
    {
        var pageKey = requirement.PageKey ?? page?.PageKey ?? "unknown";
        var definition = AdminPageCatalog.Find(pageKey);
        var pageName = definition is null ? pageKey : definition.Name;

        return $"Admin with permission: {pageKey} → {requirement.Permission}  ({pageName})";
    }
}

public class AdminAuthorizationDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var pathItem in swaggerDoc.Paths.Values)
        {
            foreach (var operation in pathItem.Operations.Values)
            {
                if (operation.Extensions.TryGetValue(
                        AdminAuthorizationOperationFilter.ExtensionName, out var value) &&
                    value is OpenApiString rule)
                {
                    operation.Summary = "🔒 " + rule.Value;
                }
            }
        }
    }
}
