using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Presentation.Controllers;
using Services.AnimalModules;
using Shared.Constants;
using Shared.DTOs.Animals;
using Shared.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarkatPlace.Extensions;

public class AnimalModuleExamplesOperationFilter : IOperationFilter
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

        if (declaringType == typeof(LivestockController))
        {
            (payload, statusCode) = method switch
            {
                nameof(LivestockController.Create) => (Envelope(
                    AnimalModuleExamples.LivestockDetails(), UserMessages.Listings.Created), "201"),
                nameof(LivestockController.GetById) => (Envelope(
                    AnimalModuleExamples.LivestockDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(LivestockController.Update) => (Envelope(
                    AnimalModuleExamples.LivestockDetails(), UserMessages.Listings.Updated), "200"),
                nameof(LivestockController.GetList) => (Envelope(
                    Page(AnimalModuleExamples.LivestockListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(LivestockController.GetBreeds) => (Envelope(
                    LivestockCatalog.BreedOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(LivestockController.GetPurposes) => (Envelope(
                    LivestockCatalog.PurposeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(LivestockController.GetAges) => (Envelope(
                    LivestockCatalog.AgeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(LivestockController.GetGenders) => (Envelope(
                    LivestockCatalog.GenderOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(LivestockController.GetHealthStatuses) => (Envelope(
                    LivestockCatalog.HealthStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(LivestockController.GetVaccinations) => (Envelope(
                    LivestockCatalog.VaccinationOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(LivestockController.GetProductions) => (Envelope(
                    LivestockCatalog.ProductionOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(LivestockController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(SheepGoatsController))
        {
            (payload, statusCode) = method switch
            {
                nameof(SheepGoatsController.Create) => (Envelope(
                    AnimalModuleExamples.SheepGoatDetails(), UserMessages.Listings.Created), "201"),
                nameof(SheepGoatsController.GetById) => (Envelope(
                    AnimalModuleExamples.SheepGoatDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(SheepGoatsController.Update) => (Envelope(
                    AnimalModuleExamples.SheepGoatDetails(), UserMessages.Listings.Updated), "200"),
                nameof(SheepGoatsController.GetList) => (Envelope(
                    Page(AnimalModuleExamples.SheepGoatListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(SheepGoatsController.GetBreeds) => (Envelope(
                    SheepGoatCatalog.BreedOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(SheepGoatsController.GetPurposes) => (Envelope(
                    SheepGoatCatalog.PurposeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(SheepGoatsController.GetAges) => (Envelope(
                    SheepGoatCatalog.AgeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(SheepGoatsController.GetGenders) => (Envelope(
                    SheepGoatCatalog.GenderOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(SheepGoatsController.GetHealthStatuses) => (Envelope(
                    SheepGoatCatalog.HealthStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(SheepGoatsController.GetVaccinations) => (Envelope(
                    SheepGoatCatalog.VaccinationOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(SheepGoatsController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(HorsesController))
        {
            (payload, statusCode) = method switch
            {
                nameof(HorsesController.Create) => (Envelope(
                    AnimalModuleExamples.HorseDetails(), UserMessages.Listings.Created), "201"),
                nameof(HorsesController.GetById) => (Envelope(
                    AnimalModuleExamples.HorseDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(HorsesController.Update) => (Envelope(
                    AnimalModuleExamples.HorseDetails(), UserMessages.Listings.Updated), "200"),
                nameof(HorsesController.GetList) => (Envelope(
                    Page(AnimalModuleExamples.HorseListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(HorsesController.GetBreeds) => (Envelope(
                    HorseCatalog.BreedOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(HorsesController.GetPurposes) => (Envelope(
                    HorseCatalog.PurposeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(HorsesController.GetAges) => (Envelope(
                    HorseCatalog.AgeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(HorsesController.GetGenders) => (Envelope(
                    HorseCatalog.GenderOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(HorsesController.GetHealthStatuses) => (Envelope(
                    HorseCatalog.HealthStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(HorsesController.GetTrainingLevels) => (Envelope(
                    HorseCatalog.TrainingLevelOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(HorsesController.GetVaccinations) => (Envelope(
                    HorseCatalog.VaccinationOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(HorsesController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(CamelsController))
        {
            (payload, statusCode) = method switch
            {
                nameof(CamelsController.Create) => (Envelope(
                    AnimalModuleExamples.CamelDetails(), UserMessages.Listings.Created), "201"),
                nameof(CamelsController.GetById) => (Envelope(
                    AnimalModuleExamples.CamelDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(CamelsController.Update) => (Envelope(
                    AnimalModuleExamples.CamelDetails(), UserMessages.Listings.Updated), "200"),
                nameof(CamelsController.GetList) => (Envelope(
                    Page(AnimalModuleExamples.CamelListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(CamelsController.GetBreeds) => (Envelope(
                    CamelCatalog.BreedOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(CamelsController.GetPurposes) => (Envelope(
                    CamelCatalog.PurposeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(CamelsController.GetAges) => (Envelope(
                    CamelCatalog.AgeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(CamelsController.GetGenders) => (Envelope(
                    CamelCatalog.GenderOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(CamelsController.GetHealthStatuses) => (Envelope(
                    CamelCatalog.HealthStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(CamelsController.GetVaccinations) => (Envelope(
                    CamelCatalog.VaccinationOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(CamelsController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(BirdsController))
        {
            (payload, statusCode) = method switch
            {
                nameof(BirdsController.Create) => (Envelope(
                    AnimalModuleExamples.BirdDetails(), UserMessages.Listings.Created), "201"),
                nameof(BirdsController.GetById) => (Envelope(
                    AnimalModuleExamples.BirdDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(BirdsController.Update) => (Envelope(
                    AnimalModuleExamples.BirdDetails(), UserMessages.Listings.Updated), "200"),
                nameof(BirdsController.GetList) => (Envelope(
                    Page(AnimalModuleExamples.BirdListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(BirdsController.GetTypes) => (Envelope(
                    BirdCatalog.TypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BirdsController.GetPurposes) => (Envelope(
                    BirdCatalog.PurposeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BirdsController.GetAges) => (Envelope(
                    BirdCatalog.AgeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BirdsController.GetGenders) => (Envelope(
                    BirdCatalog.GenderOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BirdsController.GetHealthStatuses) => (Envelope(
                    BirdCatalog.HealthStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BirdsController.GetVaccinations) => (Envelope(
                    BirdCatalog.VaccinationOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BirdsController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(PetsController))
        {
            (payload, statusCode) = method switch
            {
                nameof(PetsController.Create) => (Envelope(
                    AnimalModuleExamples.PetDetails(), UserMessages.Listings.Created), "201"),
                nameof(PetsController.GetById) => (Envelope(
                    AnimalModuleExamples.PetDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(PetsController.Update) => (Envelope(
                    AnimalModuleExamples.PetDetails(), UserMessages.Listings.Updated), "200"),
                nameof(PetsController.GetList) => (Envelope(
                    Page(AnimalModuleExamples.PetListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(PetsController.GetBreeds) => (Envelope(
                    PetCatalog.BreedOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PetsController.GetPurposes) => (Envelope(
                    PetCatalog.PurposeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PetsController.GetAges) => (Envelope(
                    PetCatalog.AgeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PetsController.GetGenders) => (Envelope(
                    PetCatalog.GenderOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PetsController.GetHealthStatuses) => (Envelope(
                    PetCatalog.HealthStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PetsController.GetTrainingLevels) => (Envelope(
                    PetCatalog.TrainingLevelOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PetsController.GetVaccinations) => (Envelope(
                    PetCatalog.VaccinationOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(PetsController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(FishController))
        {
            (payload, statusCode) = method switch
            {
                nameof(FishController.Create) => (Envelope(
                    AnimalModuleExamples.FishDetails(), UserMessages.Listings.Created), "201"),
                nameof(FishController.GetById) => (Envelope(
                    AnimalModuleExamples.FishDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(FishController.Update) => (Envelope(
                    AnimalModuleExamples.FishDetails(), UserMessages.Listings.Updated), "200"),
                nameof(FishController.GetList) => (Envelope(
                    Page(AnimalModuleExamples.FishListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(FishController.GetTypes) => (Envelope(
                    FishCatalog.TypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(FishController.GetPurposes) => (Envelope(
                    FishCatalog.PurposeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(FishController.GetAges) => (Envelope(
                    FishCatalog.AgeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(FishController.GetHealthStatuses) => (Envelope(
                    FishCatalog.HealthStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(FishController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(BeesController))
        {
            (payload, statusCode) = method switch
            {
                nameof(BeesController.Create) => (Envelope(
                    AnimalModuleExamples.BeeDetails(), UserMessages.Listings.Created), "201"),
                nameof(BeesController.GetById) => (Envelope(
                    AnimalModuleExamples.BeeDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(BeesController.Update) => (Envelope(
                    AnimalModuleExamples.BeeDetails(), UserMessages.Listings.Updated), "200"),
                nameof(BeesController.GetList) => (Envelope(
                    Page(AnimalModuleExamples.BeeListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(BeesController.GetTypes) => (Envelope(
                    BeeCatalog.TypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BeesController.GetPurposes) => (Envelope(
                    BeeCatalog.PurposeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BeesController.GetHealthStatuses) => (Envelope(
                    BeeCatalog.HealthStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BeesController.GetProductions) => (Envelope(
                    BeeCatalog.ProductionOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(BeesController.Delete) => (
                    (object)ApiResponse.Ok(UserMessages.Listings.Deleted), "200"),
                _ => (null, "200")
            };
        }
        else if (declaringType == typeof(OtherAnimalsController))
        {
            (payload, statusCode) = method switch
            {
                nameof(OtherAnimalsController.Create) => (Envelope(
                    AnimalModuleExamples.OtherAnimalDetails(), UserMessages.Listings.Created), "201"),
                nameof(OtherAnimalsController.GetById) => (Envelope(
                    AnimalModuleExamples.OtherAnimalDetails(), UserMessages.Listings.DetailsLoaded), "200"),
                nameof(OtherAnimalsController.Update) => (Envelope(
                    AnimalModuleExamples.OtherAnimalDetails(), UserMessages.Listings.Updated), "200"),
                nameof(OtherAnimalsController.GetList) => (Envelope(
                    Page(AnimalModuleExamples.OtherAnimalListItem()), UserMessages.Listings.Loaded), "200"),
                nameof(OtherAnimalsController.GetTypes) => (Envelope(
                    OtherAnimalCatalog.TypeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(OtherAnimalsController.GetPurposes) => (Envelope(
                    OtherAnimalCatalog.PurposeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(OtherAnimalsController.GetAges) => (Envelope(
                    OtherAnimalCatalog.AgeOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(OtherAnimalsController.GetGenders) => (Envelope(
                    OtherAnimalCatalog.GenderOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(OtherAnimalsController.GetHealthStatuses) => (Envelope(
                    OtherAnimalCatalog.HealthStatusOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(OtherAnimalsController.GetVaccinations) => (Envelope(
                    OtherAnimalCatalog.VaccinationOptions, UserMessages.Lookups.Loaded), "200"),
                nameof(OtherAnimalsController.Delete) => (
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
