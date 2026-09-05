using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Shared.Constants;

namespace MarkatPlace.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.UseAllOfForInheritance();
            options.SelectSubTypesUsing(baseType =>
                baseType.GetCustomAttributes(typeof(JsonDerivedTypeAttribute), inherit: false)
                    .Cast<JsonDerivedTypeAttribute>()
                    .Select(attribute => attribute.DerivedType)
                    .ToList());

            options.SelectDiscriminatorNameUsing(baseType =>
                baseType.GetCustomAttributes(typeof(JsonPolymorphicAttribute), inherit: false)
                    .Cast<JsonPolymorphicAttribute>()
                    .FirstOrDefault()?.TypeDiscriminatorPropertyName);

            options.SelectDiscriminatorValueUsing(subType =>
            {
                for (var ancestor = subType.BaseType; ancestor is not null; ancestor = ancestor.BaseType)
                {
                    var match = ancestor
                        .GetCustomAttributes(typeof(JsonDerivedTypeAttribute), inherit: false)
                        .Cast<JsonDerivedTypeAttribute>()
                        .FirstOrDefault(attribute => attribute.DerivedType == subType);

                    if (match is not null)
                        return match.TypeDiscriminator?.ToString();
                }

                return null;
            });

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MarkatPlace V1",
                Version = "v1",

                Description = "The public marketplace API: authentication, profile, advertisements, " +
                              "workshops, craftsmen, lost & found, the home page, banners, payments, " +
                              "notifications and shared lookups. Administration is not here — it is " +
                              "published separately as \"MarkatPlace Admin V2\"."
            });

            options.SwaggerDoc(ApiVersions.AdminV2, new OpenApiInfo
            {
                Title = "MarkatPlace Admin V2",
                Version = ApiVersions.AdminV2,
                Description =
                    "The Admin Dashboard API. Every endpoint is under /api/v2/admin and requires the " +
                    "Admin role: anonymous callers get 401 and signed-in non-administrators get 403. " +
                    "Nothing public appears here, and nothing here appears in \"MarkatPlace V1\".\n\n" +
                    "AUTHORIZATION. Beyond the role, every operation costs a permission on a Dashboard " +
                    "page, and every operation states its own rule in its summary (and in the " +
                    "machine-readable x-admin-authorization extension):\n" +
                    "- \"Super Admin only\" - administrator management. Gated by the SuperAdmin " +
                    "role, read from the identity store rather than from the caller's token, and " +
                    "reachable through no page permission at all.\n" +
                    "- \"Admin with permission: {page} -> {Permission}\" - the caller must hold " +
                    "that permission on that page. Checked against the database on every request, so " +
                    "a permission granted or revoked by the Super Admin takes effect immediately.\n" +
                    "- \"Any active Admin\" - self-service (GET me/permissions, the " +
                    "administrator's own notifications). No page grant needed; a disabled " +
                    "administrator is still refused.\n\n" +
                    "GET /api/v2/admin/permissions/pages lists every assignable page and the " +
                    "permissions valid on it. GET /api/v2/admin/me/permissions returns what the " +
                    "signed-in administrator holds."
            });

            options.DocInclusionPredicate((documentName, apiDescription) =>
            {
                var isAdmin = string.Equals(
                    apiDescription.GroupName, ApiVersions.AdminV2, StringComparison.Ordinal);

                return documentName == ApiVersions.AdminV2 ? isAdmin : !isAdmin;
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the JWT token. Example: \"Bearer {your token}\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });

            options.OperationFilter<PasswordResetTokenHeaderOperationFilter>();

            options.OperationFilter<CreateAdFormExamplesOperationFilter>();

            options.OperationFilter<ReadConfigExamplesOperationFilter>();

            options.OperationFilter<BusinessModuleExamplesOperationFilter>();

            options.OperationFilter<JobModuleExamplesOperationFilter>();

            options.OperationFilter<AnimalModuleExamplesOperationFilter>();

            options.OperationFilter<AntiqueModuleExamplesOperationFilter>();

            options.OperationFilter<ClothingModuleExamplesOperationFilter>();

            options.OperationFilter<StandardResponsesOperationFilter>();

            options.OperationFilter<AdminAuthorizationOperationFilter>();

            options.DocumentFilter<MinimalSwaggerDocumentFilter>();

            options.DocumentFilter<AdminAuthorizationDocumentFilter>();
        });

        return services;
    }
}
