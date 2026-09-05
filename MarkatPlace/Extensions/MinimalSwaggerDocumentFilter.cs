using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarkatPlace.Extensions;

public class MinimalSwaggerDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var tag in swaggerDoc.Tags ?? new List<OpenApiTag>())
        {
            tag.Description = null;
            tag.ExternalDocs = null;
        }

        foreach (var pathItem in swaggerDoc.Paths.Values)
        {
            pathItem.Description = null;
            pathItem.Summary = null;

            foreach (var operation in pathItem.Operations.Values)
            {
                operation.Summary = null;
                operation.Description = null;
                operation.ExternalDocs = null;

                foreach (var parameter in operation.Parameters ?? new List<OpenApiParameter>())
                    parameter.Description = null;

                if (operation.RequestBody is not null)
                    operation.RequestBody.Description = null;

                foreach (var response in operation.Responses.Values)
                    response.Description = string.Empty;
            }
        }
    }
}
