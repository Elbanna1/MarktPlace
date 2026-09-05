using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Presentation.Controllers;
using Services.ReadConfigs;
using Shared.DTOs.Lookups.Read;
using Shared.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

using Shared.Constants;
namespace MarkatPlace.Extensions;

public class ReadConfigExamplesOperationFilter : IOperationFilter
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType != typeof(LookupsController) ||
            context.MethodInfo.Name != nameof(LookupsController.GetReadConfig))
            return;

        if (!operation.Responses.TryGetValue("200", out var response) ||
            !response.Content.TryGetValue("application/json", out var mediaType))
            return;

        foreach (var example in ReadConfigExamples.All)
        {
            var envelope = ApiResponse<ReadConfigDto>.Ok(
                example.Config, UserMessages.Lookups.ReadConfigLoaded);

            mediaType.Examples[example.Key] = new OpenApiExample
            {
                Summary = example.Summary,
                Value = OpenApiAnyFactory.CreateFromJson(JsonSerializer.Serialize(envelope, SerializerOptions))
            };
        }
    }
}
