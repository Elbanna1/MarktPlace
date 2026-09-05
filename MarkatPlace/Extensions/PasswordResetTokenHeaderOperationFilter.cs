using Microsoft.OpenApi.Models;
using Shared.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarkatPlace.Extensions;

public class PasswordResetTokenHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var path = context.ApiDescription.RelativePath ?? string.Empty;

        if (!path.EndsWith("verify-reset-code", StringComparison.OrdinalIgnoreCase) &&
            !path.EndsWith("reset-password", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = AuthConstants.PasswordResetTokenHeader,
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema { Type = "string" }
        });
    }
}
