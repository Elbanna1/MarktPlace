using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Presentation.Controllers;
using Services.AntiqueModules;
using Shared.Constants;
using Shared.DTOs.Antiques;
using Shared.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarkatPlace.Extensions;

public class AntiqueModuleExamplesOperationFilter : IOperationFilter
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

        if (declaringType == typeof(DecorAntiquesController))
        {
            (payload, statusCode) = method switch
            {
                nameof(DecorAntiquesController.Create) => (Envelope(
                    AntiqueModuleExamples.DecorAntiqueDetails(),
                    UserMessages.Listings.Created), "201"),
                nameof(DecorAntiquesController.GetById) => (Envelope(
                    AntiqueModuleExamples.DecorAntiqueDetails(),
                    UserMessages.Listings.DetailsLoaded), "200"),
                nameof(DecorAntiquesController.Update) => (Envelope(
                    AntiqueModuleExamples.DecorAntiqueDetails(),
                    UserMessages.Listings.Updated), "200"),
                nameof(DecorAntiquesController.GetList) => (Envelope(
                    Page(AntiqueModuleExamples.DecorAntiqueListItem()),
                    UserMessages.Listings.Loaded), "200"),
                nameof(DecorAntiquesController.GetItemTypes) => (Envelope(
                    DecorAntiqueCatalog.ItemTypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(DecorAntiquesController.GetMaterials) => (Envelope(
                    DecorAntiqueCatalog.MaterialOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(DecorAntiquesController.GetConditions) => (Envelope(
                    DecorAntiqueCatalog.ConditionOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(DecorAntiquesController.GetOriginalities) => (Envelope(
                    DecorAntiqueCatalog.OriginalityOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(DecorAntiquesController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(AntiquesController))
        {
            (payload, statusCode) = method switch
            {
                nameof(AntiquesController.Create) => (Envelope(
                    AntiqueModuleExamples.AntiqueDetails(), UserMessages.Listings.Created), "201"),
                nameof(AntiquesController.GetById) => (Envelope(
                    AntiqueModuleExamples.AntiqueDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(AntiquesController.Update) => (Envelope(
                    AntiqueModuleExamples.AntiqueDetails(), UserMessages.Listings.Updated), "200"),
                nameof(AntiquesController.GetList) => (Envelope(
                    Page(AntiqueModuleExamples.AntiqueListItem()),
                    UserMessages.Listings.Loaded), "200"),
                nameof(AntiquesController.GetTypes) => (Envelope(
                    AntiqueModuleCatalog.TypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(AntiquesController.GetMaterials) => (Envelope(
                    AntiqueModuleCatalog.MaterialOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(AntiquesController.GetConditions) => (Envelope(
                    AntiqueModuleCatalog.ConditionOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(AntiquesController.GetWorkingStatuses) => (Envelope(
                    AntiqueModuleCatalog.WorkingStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(AntiquesController.GetOriginalities) => (Envelope(
                    AntiqueModuleCatalog.OriginalityOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(AntiquesController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(PaintingsController))
        {
            (payload, statusCode) = method switch
            {
                nameof(PaintingsController.Create) => (Envelope(
                    AntiqueModuleExamples.PaintingDetails(), UserMessages.Listings.Created), "201"),
                nameof(PaintingsController.GetById) => (Envelope(
                    AntiqueModuleExamples.PaintingDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(PaintingsController.Update) => (Envelope(
                    AntiqueModuleExamples.PaintingDetails(), UserMessages.Listings.Updated), "200"),
                nameof(PaintingsController.GetList) => (Envelope(
                    Page(AntiqueModuleExamples.PaintingListItem()),
                    UserMessages.Listings.Loaded), "200"),
                nameof(PaintingsController.GetTypes) => (Envelope(
                    PaintingCatalog.TypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PaintingsController.GetMaterials) => (Envelope(
                    PaintingCatalog.MaterialOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PaintingsController.GetOriginalities) => (Envelope(
                    PaintingCatalog.OriginalityOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PaintingsController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(HandmadeController))
        {
            (payload, statusCode) = method switch
            {
                nameof(HandmadeController.Create) => (Envelope(
                    AntiqueModuleExamples.HandmadeDetails(), UserMessages.Listings.Created), "201"),
                nameof(HandmadeController.GetById) => (Envelope(
                    AntiqueModuleExamples.HandmadeDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(HandmadeController.Update) => (Envelope(
                    AntiqueModuleExamples.HandmadeDetails(), UserMessages.Listings.Updated), "200"),
                nameof(HandmadeController.GetList) => (Envelope(
                    Page(AntiqueModuleExamples.HandmadeListItem()),
                    UserMessages.Listings.Loaded), "200"),
                nameof(HandmadeController.GetTypes) => (Envelope(
                    HandmadeCatalog.TypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(HandmadeController.GetColors) => (Envelope(
                    HandmadeCatalog.ColorOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(HandmadeController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(CoinsStampsController))
        {
            (payload, statusCode) = method switch
            {
                nameof(CoinsStampsController.Create) => (Envelope(
                    AntiqueModuleExamples.CoinStampDetails(),
                    UserMessages.Listings.Created), "201"),
                nameof(CoinsStampsController.GetById) => (Envelope(
                    AntiqueModuleExamples.CoinStampDetails(),
                    UserMessages.Listings.DetailsLoaded), "200"),
                nameof(CoinsStampsController.Update) => (Envelope(
                    AntiqueModuleExamples.CoinStampDetails(),
                    UserMessages.Listings.Updated), "200"),
                nameof(CoinsStampsController.GetList) => (Envelope(
                    Page(AntiqueModuleExamples.CoinStampListItem()),
                    UserMessages.Listings.Loaded), "200"),
                nameof(CoinsStampsController.GetItemTypes) => (Envelope(
                    CoinStampCatalog.ItemTypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(CoinsStampsController.GetMetals) => (Envelope(
                    CoinStampCatalog.MetalOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(CoinsStampsController.GetConditions) => (Envelope(
                    CoinStampCatalog.ConditionOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(CoinsStampsController.Delete) => (
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
