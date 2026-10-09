using Shared.Enums;

namespace Shared.Constants;

public static class ListingModuleCatalog
{
    private static readonly IReadOnlyDictionary<ListingModuleType, SubCategoryType> SubCategories =
        new Dictionary<ListingModuleType, SubCategoryType>
        {
            [ListingModuleType.Craftsman] = SubCategoryType.Craftsmen,
            [ListingModuleType.Workshop] = SubCategoryType.Workshops,

            [ListingModuleType.Factory] = SubCategoryType.Factories,
            [ListingModuleType.Farm] = SubCategoryType.Farms,
            [ListingModuleType.Company] = SubCategoryType.Companies,
            [ListingModuleType.Supplier] = SubCategoryType.Suppliers,
            [ListingModuleType.WholesaleTrader] = SubCategoryType.WholesaleTraders,
            [ListingModuleType.FruitVegetableMerchant] = SubCategoryType.FruitAndVegetableTraders,

            [ListingModuleType.JobRequest] = SubCategoryType.JobRequests,
            [ListingModuleType.JobOpportunity] = SubCategoryType.JobOpportunities,

            [ListingModuleType.Livestock] = SubCategoryType.Livestock,
            [ListingModuleType.SheepGoat] = SubCategoryType.SheepAndGoats,
            [ListingModuleType.Horse] = SubCategoryType.Horses,
            [ListingModuleType.Camel] = SubCategoryType.Camels,
            [ListingModuleType.Bird] = SubCategoryType.Birds,
            [ListingModuleType.Pet] = SubCategoryType.Pets,
            [ListingModuleType.Fish] = SubCategoryType.Fish,
            [ListingModuleType.Bee] = SubCategoryType.Bees,
            [ListingModuleType.OtherAnimal] = SubCategoryType.OtherAnimals,

            [ListingModuleType.DecorAntique] = SubCategoryType.DecorAntiques,
            [ListingModuleType.Antique] = SubCategoryType.Antiques,
            [ListingModuleType.Painting] = SubCategoryType.Paintings,
            [ListingModuleType.Handmade] = SubCategoryType.Handmade,
            [ListingModuleType.CoinStamp] = SubCategoryType.CoinsAndStamps,

            [ListingModuleType.MenClothing] = SubCategoryType.MenClothing,
            [ListingModuleType.WomenClothing] = SubCategoryType.WomenClothing,
            [ListingModuleType.KidsClothing] = SubCategoryType.KidsClothing,

            [ListingModuleType.Accessory] = SubCategoryType.Accessories,
            [ListingModuleType.Cosmetic] = SubCategoryType.Cosmetics,
            [ListingModuleType.HomeKitchen] = SubCategoryType.HomeAndKitchen,
            [ListingModuleType.ShoppingElectronic] = SubCategoryType.ShoppingElectronics,
            [ListingModuleType.GiftToy] = SubCategoryType.GiftsAndToys,
            [ListingModuleType.HomemadeFood] = SubCategoryType.HomemadeFood,

            [ListingModuleType.Furniture] = SubCategoryType.Furniture,
            [ListingModuleType.FurnishingCurtain] = SubCategoryType.FurnishingsAndCurtains,
            [ListingModuleType.LightingDecor] = SubCategoryType.LightingAndDecor,
            [ListingModuleType.KitchenTool] = SubCategoryType.KitchenTools,
            [ListingModuleType.HomeAppliance] = SubCategoryType.HomeAppliances,
            [ListingModuleType.BathroomSupply] = SubCategoryType.BathroomSupplies,
            [ListingModuleType.PlantOrnament] = SubCategoryType.PlantsAndOrnaments,

            [ListingModuleType.Land] = SubCategoryType.Lands,
            [ListingModuleType.Apartment] = SubCategoryType.Apartments,
            [ListingModuleType.Shop] = SubCategoryType.Shops,

            [ListingModuleType.Rescue] = SubCategoryType.Rescues,
            [ListingModuleType.BloodRequest] = SubCategoryType.BloodRequests,
            [ListingModuleType.AskConsult] = SubCategoryType.AskConsults,

            [ListingModuleType.Doctor] = SubCategoryType.Doctors,
            [ListingModuleType.MedicalLaboratory] = SubCategoryType.MedicalLaboratories,
            [ListingModuleType.DentalLaboratory] = SubCategoryType.DentalLaboratories,
            [ListingModuleType.RadiologyLaboratory] = SubCategoryType.RadiologyLaboratories,
            [ListingModuleType.MedicalCenter] = SubCategoryType.MedicalCenters,
            [ListingModuleType.Hospital] = SubCategoryType.Hospitals,

            [ListingModuleType.LawyerOffice] = SubCategoryType.LawyerOffices,
            [ListingModuleType.EngineeringConsultancyOffice] = SubCategoryType.EngineeringConsultancyOffices,
            [ListingModuleType.Contractor] = SubCategoryType.Contractors,
            [ListingModuleType.IndustrialConsultancyOffice] = SubCategoryType.IndustrialConsultancyOffices,
            [ListingModuleType.AccountantOffice] = SubCategoryType.AccountantOffices,

            [ListingModuleType.LostItem] = SubCategoryType.LostItems,
            [ListingModuleType.FoundItem] = SubCategoryType.FoundItems
        };

    private static readonly IReadOnlySet<ListingModuleType> IsolatedModules =
        new HashSet<ListingModuleType>
        {
            ListingModuleType.LostItem,
            ListingModuleType.FoundItem
        };

    public static bool IsIsolated(ListingModuleType type) => IsolatedModules.Contains(type);

    private static readonly IReadOnlyDictionary<SubCategoryType, CategoryType> Categories =
        CategoryCatalog.SubCategories.ToDictionary(entry => entry.Id, entry => entry.Category);

    private static readonly IReadOnlyDictionary<SubCategoryType, ListingModuleType> Modules =
        BuildModules();

    private static IReadOnlyDictionary<SubCategoryType, ListingModuleType> BuildModules()
    {
        var modules = SubCategories.ToDictionary(entry => entry.Value, entry => entry.Key);

        modules[SubCategoryType.Private] = ListingModuleType.Advertisement;
        modules[SubCategoryType.Taxi] = ListingModuleType.Advertisement;
        modules[SubCategoryType.Motorcycles] = ListingModuleType.Advertisement;
        modules[SubCategoryType.HeavyEquipment] = ListingModuleType.Advertisement;

        return modules;
    }

    public static SubCategoryType? SubCategoryOf(ListingModuleType type) =>
        SubCategories.TryGetValue(type, out var subCategory) ? subCategory : null;

    public static ListingModuleType? ModuleOf(SubCategoryType subCategory) =>
        Modules.TryGetValue(subCategory, out var module) ? module : null;

    public static ListingModuleType? ModuleOf(int subCategoryId) =>
        ModuleOf((SubCategoryType)subCategoryId);

    public static ListingModuleType ModuleOf(PostType postType) =>
        postType == PostType.Lost ? ListingModuleType.LostItem : ListingModuleType.FoundItem;

    public static CategoryType? CategoryOf(SubCategoryType subCategory) =>
        Categories.TryGetValue(subCategory, out var category) ? category : null;

    public static CategoryType? CategoryOf(ListingModuleType type) =>
        SubCategoryOf(type) is { } subCategory ? CategoryOf(subCategory) : null;

    public static string RouteOf(ListingModuleType type) =>
        NotificationCatalog.AllListingSubjects.TryGetValue(type, out var subject)
            ? subject.Route?.TrimStart('/') ?? string.Empty
            : string.Empty;

    public static string NameOf(ListingModuleType type) =>
        NotificationCatalog.AllListingSubjects.TryGetValue(type, out var subject)
            ? subject.EntityName
            : type.ToString();

    public static IReadOnlyList<ListingModuleType> All { get; } =
        Enum.GetValues<ListingModuleType>();

    private static readonly IReadOnlyDictionary<string, ListingModuleType> ModulesByRoute =
        NotificationCatalog.AllListingSubjects
            .Where(entry => !string.IsNullOrEmpty(entry.Value.Route))
            .ToDictionary(
                entry => entry.Value.Route!.TrimStart('/'),
                entry => entry.Key,
                StringComparer.OrdinalIgnoreCase);

    public static ListingModuleType? ModuleOfRoute(string? routeSegment) =>
        !string.IsNullOrWhiteSpace(routeSegment) &&
        ModulesByRoute.TryGetValue(routeSegment.Trim('/'), out var module)
            ? module
            : null;
}
