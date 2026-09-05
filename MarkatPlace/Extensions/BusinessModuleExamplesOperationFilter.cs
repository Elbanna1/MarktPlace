using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Presentation.Controllers;
using Services.BusinessModules;
using Shared.DTOs.FruitVegetableMerchants;
using Shared.DTOs.Suppliers;
using Shared.DTOs.WholesaleTraders;
using Shared.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

using Shared.Constants;
namespace MarkatPlace.Extensions;

public class BusinessModuleExamplesOperationFilter : IOperationFilter
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

        if (declaringType == typeof(SuppliersController))
        {
            (payload, statusCode) = method switch
            {
                nameof(SuppliersController.Create) => (Envelope(
                    BusinessModuleExamples.SupplierDetails(), UserMessages.Listings.Created), "201"),
                nameof(SuppliersController.GetById) => (Envelope(
                    BusinessModuleExamples.SupplierDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(SuppliersController.Update) => (Envelope(
                    BusinessModuleExamples.SupplierDetails(), UserMessages.Listings.Updated), "200"),
                nameof(SuppliersController.GetList) => (Envelope(
                    Page(BusinessModuleExamples.SupplierListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(SuppliersController.GetTypes) => (Envelope(
                    BusinessModuleExamples.SupplierTypes(), UserMessages.Lookups.Loaded), "200"),
                nameof(SuppliersController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(WholesaleTradersController))
        {
            (payload, statusCode) = method switch
            {
                nameof(WholesaleTradersController.Create) => (Envelope(
                    BusinessModuleExamples.WholesaleTraderDetails(), UserMessages.Listings.Created), "201"),
                nameof(WholesaleTradersController.GetById) => (Envelope(
                    BusinessModuleExamples.WholesaleTraderDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(WholesaleTradersController.Update) => (Envelope(
                    BusinessModuleExamples.WholesaleTraderDetails(), UserMessages.Listings.Updated), "200"),
                nameof(WholesaleTradersController.GetList) => (Envelope(
                    Page(BusinessModuleExamples.WholesaleTraderListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(WholesaleTradersController.GetTradeTypes) => (Envelope(
                    BusinessModuleExamples.WholesaleTradeTypes(), UserMessages.Lookups.Loaded), "200"),
                nameof(WholesaleTradersController.GetSaleTypes) => (Envelope(
                    BusinessModuleExamples.WholesaleSaleTypes(), UserMessages.Lookups.Loaded), "200"),
                nameof(WholesaleTradersController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(FruitVegetableMerchantsController))
        {
            (payload, statusCode) = method switch
            {
                nameof(FruitVegetableMerchantsController.Create) => (Envelope(
                    BusinessModuleExamples.MerchantDetails(), UserMessages.Listings.Created), "201"),
                nameof(FruitVegetableMerchantsController.GetById) => (Envelope(
                    BusinessModuleExamples.MerchantDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(FruitVegetableMerchantsController.Update) => (Envelope(
                    BusinessModuleExamples.MerchantDetails(), UserMessages.Listings.Updated), "200"),
                nameof(FruitVegetableMerchantsController.GetList) => (Envelope(
                    Page(BusinessModuleExamples.MerchantListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(FruitVegetableMerchantsController.GetSaleTypes) => (Envelope(
                    BusinessModuleExamples.MerchantSaleTypes(), UserMessages.Lookups.Loaded), "200"),
                nameof(FruitVegetableMerchantsController.Delete) => (
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
