using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Presentation.Controllers;
using Services.AdForms;
using Shared.DTOs.Lookups.Forms;
using Shared.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

using Shared.Constants;
namespace MarkatPlace.Extensions;

public class CreateAdFormExamplesOperationFilter : IOperationFilter
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
            context.MethodInfo.Name != nameof(LookupsController.GetCreateAdForm))
            return;

        if (!operation.Responses.TryGetValue("200", out var response) ||
            !response.Content.TryGetValue("application/json", out var mediaType))
            return;

        foreach (var example in CreateAdFormExamples.All)
        {
            var envelope = ApiResponse<CreateAdFormDto>.Ok(
                example.Form, UserMessages.Lookups.FormLoaded);

            mediaType.Examples[example.Key] = new OpenApiExample
            {
                Summary = example.Summary,
                Value = OpenApiAnyFactory.CreateFromJson(JsonSerializer.Serialize(envelope, SerializerOptions))
            };
        }
    }
}
