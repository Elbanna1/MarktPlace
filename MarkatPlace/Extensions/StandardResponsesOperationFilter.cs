using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarkatPlace.Extensions;

public class StandardResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var envelope = context.SchemaGenerator.GenerateSchema(
            typeof(Shared.Responses.ApiResponse), context.SchemaRepository);

        AddResponse(operation, StatusCodes.Status400BadRequest, envelope);
        AddResponse(operation, StatusCodes.Status500InternalServerError, envelope);
        AddResponse(operation, StatusCodes.Status429TooManyRequests, envelope);

        if (operation.RequestBody is not null)
            AddResponse(operation, StatusCodes.Status422UnprocessableEntity, envelope);

        var allowsAnonymous = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<Microsoft.AspNetCore.Authorization.IAllowAnonymous>()
            .Any();

        var requiresAuthorization = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<Microsoft.AspNetCore.Authorization.IAuthorizeData>()
            .Any();

        if (requiresAuthorization && !allowsAnonymous)
        {
            AddResponse(operation, StatusCodes.Status401Unauthorized, envelope);
            AddResponse(operation, StatusCodes.Status403Forbidden, envelope);
        }

        if (operation.Parameters?.Any(parameter => parameter.In == ParameterLocation.Path) == true)
            AddResponse(operation, StatusCodes.Status404NotFound, envelope);
    }

    private static void AddResponse(OpenApiOperation operation, int statusCode, OpenApiSchema schema)
    {
        var key = statusCode.ToString();

        if (operation.Responses.ContainsKey(key))
            return;

        operation.Responses[key] = new OpenApiResponse
        {
            Description = string.Empty,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new() { Schema = schema }
            }
        };
    }
}
