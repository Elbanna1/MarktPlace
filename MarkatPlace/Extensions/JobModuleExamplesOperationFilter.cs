using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Presentation.Controllers;
using Services.JobModules;
using Shared.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

using Shared.Constants;
namespace MarkatPlace.Extensions;

public class JobModuleExamplesOperationFilter : IOperationFilter
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var declaringType = context.MethodInfo.DeclaringType;
        var method = context.MethodInfo.Name;

        object? payload = null;
        var statusCode = "200";

        if (declaringType == typeof(JobRequestsController))
        {
            (payload, statusCode) = method switch
            {
                nameof(JobRequestsController.Create) => (Envelope(
                    JobModuleExamples.JobRequestDetails(), UserMessages.Listings.Created), "201"),
                nameof(JobRequestsController.GetById) => (Envelope(
                    JobModuleExamples.JobRequestDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(JobRequestsController.Update) => (Envelope(
                    JobModuleExamples.JobRequestDetails(), UserMessages.Listings.Updated), "200"),
                nameof(JobRequestsController.GetList) => (Envelope(
                    Page(JobModuleExamples.JobRequestListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(JobRequestsController.GetJobFields) => (Envelope(
                    JobModuleExamples.JobFields(), UserMessages.Lookups.Loaded), "200"),
                nameof(JobRequestsController.GetExperienceLevels) => (Envelope(
                    JobModuleExamples.ExperienceLevels(), UserMessages.Lookups.Loaded), "200"),
                nameof(JobRequestsController.GetEducationLevels) => (Envelope(
                    JobModuleExamples.EducationLevels(), UserMessages.Lookups.Loaded), "200"),
                nameof(JobRequestsController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(JobOpportunitiesController))
        {
            (payload, statusCode) = method switch
            {
                nameof(JobOpportunitiesController.Create) => (Envelope(
                    JobModuleExamples.JobOpportunityDetails(), UserMessages.Listings.Created), "201"),
                nameof(JobOpportunitiesController.GetById) => (Envelope(
                    JobModuleExamples.JobOpportunityDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(JobOpportunitiesController.Update) => (Envelope(
                    JobModuleExamples.JobOpportunityDetails(), UserMessages.Listings.Updated), "200"),
                nameof(JobOpportunitiesController.GetList) => (Envelope(
                    Page(JobModuleExamples.JobOpportunityListItem()),
                    UserMessages.Listings.Loaded), "200"),
                nameof(JobOpportunitiesController.GetJobFields) => (Envelope(
                    JobModuleExamples.JobFields(), UserMessages.Lookups.Loaded), "200"),
                nameof(JobOpportunitiesController.GetExperienceLevels) => (Envelope(
                    JobModuleExamples.ExperienceLevels(), UserMessages.Lookups.Loaded), "200"),
                nameof(JobOpportunitiesController.GetWorkTypes) => (Envelope(
                    JobModuleExamples.WorkTypes(), UserMessages.Lookups.Loaded), "200"),
                nameof(JobOpportunitiesController.GetSalaryTypes) => (Envelope(
                    JobModuleExamples.SalaryTypes(), UserMessages.Lookups.Loaded), "200"),
                nameof(JobOpportunitiesController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }

        if (payload is null)
            return;

        if (!operation.Responses.TryGetValue(statusCode, out var response) ||
            !response.Content.TryGetValue("application/json", out var mediaType))
            return;

        mediaType.Example = OpenApiAnyFactory.CreateFromJson(
            JsonSerializer.Serialize(payload, SerializerOptions));
    }

    private static object Envelope<T>(T data, string message) => ApiResponse<T>.Ok(data, message);

    private static PaginatedResult<T> Page<T>(T item) =>
        new(new[] { item }, totalCount: 1, pageIndex: 1, pageSize: 10);
}
