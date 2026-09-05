using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Presentation.Controllers;
using Services.ClothingModules;
using Shared.Constants;
using Shared.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarkatPlace.Extensions;

public class ClothingModuleExamplesOperationFilter : IOperationFilter
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

        if (declaringType == typeof(MenClothingController))
        {
            (payload, statusCode) = method switch
            {
                nameof(MenClothingController.Create) => (Envelope(
                    ClothingModuleExamples.MenClothingDetails(), UserMessages.Listings.Created), "201"),
                nameof(MenClothingController.GetById) => (Envelope(
                    ClothingModuleExamples.MenClothingDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(MenClothingController.Update) => (Envelope(
                    ClothingModuleExamples.MenClothingDetails(), UserMessages.Listings.Updated), "200"),
                nameof(MenClothingController.GetList) => (Envelope(
                    Page(ClothingModuleExamples.MenClothingListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(MenClothingController.GetClothingTypes) => (Envelope(
                    MenClothingCatalog.ClothingTypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(MenClothingController.GetBrands) => (Envelope(
                    MenClothingCatalog.BrandOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(MenClothingController.GetSizes) => (Envelope(
                    MenClothingCatalog.SizeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(MenClothingController.GetColors) => (Envelope(
                    MenClothingCatalog.ColorOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(MenClothingController.GetConditions) => (Envelope(
                    MenClothingCatalog.ConditionOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(MenClothingController.GetSellingMethods) => (Envelope(
                    MenClothingCatalog.SellingMethodOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(MenClothingController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(WomenClothingController))
        {
            (payload, statusCode) = method switch
            {
                nameof(WomenClothingController.Create) => (Envelope(
                    ClothingModuleExamples.WomenClothingDetails(), UserMessages.Listings.Created), "201"),
                nameof(WomenClothingController.GetById) => (Envelope(
                    ClothingModuleExamples.WomenClothingDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(WomenClothingController.Update) => (Envelope(
                    ClothingModuleExamples.WomenClothingDetails(), UserMessages.Listings.Updated), "200"),
                nameof(WomenClothingController.GetList) => (Envelope(
                    Page(ClothingModuleExamples.WomenClothingListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(WomenClothingController.GetClothingTypes) => (Envelope(
                    WomenClothingCatalog.ClothingTypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(WomenClothingController.GetBrands) => (Envelope(
                    WomenClothingCatalog.BrandOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(WomenClothingController.GetSizes) => (Envelope(
                    WomenClothingCatalog.SizeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(WomenClothingController.GetColors) => (Envelope(
                    WomenClothingCatalog.ColorOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(WomenClothingController.GetConditions) => (Envelope(
                    WomenClothingCatalog.ConditionOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(WomenClothingController.GetSellingMethods) => (Envelope(
                    WomenClothingCatalog.SellingMethodOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(WomenClothingController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(KidsClothingController))
        {
            (payload, statusCode) = method switch
            {
                nameof(KidsClothingController.Create) => (Envelope(
                    ClothingModuleExamples.KidsClothingDetails(), UserMessages.Listings.Created), "201"),
                nameof(KidsClothingController.GetById) => (Envelope(
                    ClothingModuleExamples.KidsClothingDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(KidsClothingController.Update) => (Envelope(
                    ClothingModuleExamples.KidsClothingDetails(), UserMessages.Listings.Updated), "200"),
                nameof(KidsClothingController.GetList) => (Envelope(
                    Page(ClothingModuleExamples.KidsClothingListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(KidsClothingController.GetClothingTypes) => (Envelope(
                    KidsClothingCatalog.ClothingTypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(KidsClothingController.GetBrands) => (Envelope(
                    KidsClothingCatalog.BrandOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(KidsClothingController.GetSizes) => (Envelope(
                    KidsClothingCatalog.SizeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(KidsClothingController.GetColors) => (Envelope(
                    KidsClothingCatalog.ColorOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(KidsClothingController.GetConditions) => (Envelope(
                    KidsClothingCatalog.ConditionOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(KidsClothingController.GetSellingMethods) => (Envelope(
                    KidsClothingCatalog.SellingMethodOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(KidsClothingController.Delete) => (
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
