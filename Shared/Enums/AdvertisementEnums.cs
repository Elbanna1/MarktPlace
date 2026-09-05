namespace Shared.Enums;

public enum AdvertisementStatus
{
    Pending = 0,

    Active = 1,

    Expired = 2,

    Deleted = 3
}

public enum CategoryType
{
    Cars = 1,

    WorkshopsAndCraftsmen = 2,

    LostAndFound = 3,

    Business = 4,

    Jobs = 5,

    Animals = 6,

    Antiques = 7,

    Clothing = 8,

    OnlineShopping = 9,

    HomeFurnishing = 10,

    RealEstate = 11,

    Charity = 12
}

public enum SubCategoryType
{
    Private = 1,

    Taxi = 2,

    Motorcycles = 3,

    HeavyEquipment = 4,

    Workshops = 5,

    Craftsmen = 6,

    LostItems = 7,

    FoundItems = 8,

    Factories = 9,

    Farms = 10,

    Companies = 11,

    Suppliers = 12,

    WholesaleTraders = 13,

    FruitAndVegetableTraders = 14,

    JobRequests = 15,

    JobOpportunities = 16,

    Livestock = 17,

    SheepAndGoats = 18,

    Horses = 19,

    Camels = 20,

    Birds = 21,

    Pets = 22,

    Fish = 23,

    Bees = 24,

    OtherAnimals = 25,

    DecorAntiques = 26,

    Antiques = 27,

    Paintings = 28,

    Handmade = 29,

    CoinsAndStamps = 30,

    MenClothing = 31,

    WomenClothing = 32,

    KidsClothing = 33,

    Accessories = 34,

    Cosmetics = 35,

    HomeAndKitchen = 36,

    ShoppingElectronics = 37,

    GiftsAndToys = 38,

    HomemadeFood = 39,

    Furniture = 40,

    FurnishingsAndCurtains = 41,

    LightingAndDecor = 42,

    KitchenTools = 43,

    HomeAppliances = 44,

    BathroomSupplies = 45,

    PlantsAndOrnaments = 46,

    Lands = 47,

    Apartments = 48,

    Shops = 49,

    Rescues = 50,

    BloodRequests = 51,

    AskConsults = 52
}

public static class CharitySubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.Rescues,
        SubCategoryType.BloodRequests,
        SubCategoryType.AskConsults
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public static class RealEstateSubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.Lands,
        SubCategoryType.Apartments,
        SubCategoryType.Shops
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public static class HomeFurnishingSubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.Furniture,
        SubCategoryType.FurnishingsAndCurtains,
        SubCategoryType.LightingAndDecor,
        SubCategoryType.KitchenTools,
        SubCategoryType.HomeAppliances,
        SubCategoryType.BathroomSupplies,
        SubCategoryType.PlantsAndOrnaments
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public static class OnlineShoppingSubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.Accessories,
        SubCategoryType.Cosmetics,
        SubCategoryType.HomeAndKitchen,
        SubCategoryType.ShoppingElectronics,
        SubCategoryType.GiftsAndToys,
        SubCategoryType.HomemadeFood
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public static class ClothingSubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.MenClothing,
        SubCategoryType.WomenClothing,
        SubCategoryType.KidsClothing
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public static class AntiqueSubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.DecorAntiques,
        SubCategoryType.Antiques,
        SubCategoryType.Paintings,
        SubCategoryType.Handmade,
        SubCategoryType.CoinsAndStamps
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public static class AnimalSubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.Livestock,
        SubCategoryType.SheepAndGoats,
        SubCategoryType.Horses,
        SubCategoryType.Camels,
        SubCategoryType.Birds,
        SubCategoryType.Pets,
        SubCategoryType.Fish,
        SubCategoryType.Bees,
        SubCategoryType.OtherAnimals
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public static class CarSubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.Private,
        SubCategoryType.Taxi,
        SubCategoryType.Motorcycles,
        SubCategoryType.HeavyEquipment
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public static class BusinessSubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.Factories,
        SubCategoryType.Farms,
        SubCategoryType.Companies,
        SubCategoryType.Suppliers,
        SubCategoryType.WholesaleTraders,
        SubCategoryType.FruitAndVegetableTraders
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public static class JobsSubCategories
{
    public static readonly IReadOnlySet<SubCategoryType> All = new HashSet<SubCategoryType>
    {
        SubCategoryType.JobRequests,
        SubCategoryType.JobOpportunities
    };

    public static bool Contains(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        All.Contains((SubCategoryType)subCategoryId);
}

public enum AdvertisementSortBy
{
    Newest = 1,
    Oldest = 2,
    PriceAsc = 3,
    PriceDesc = 4,
    MostViewed = 5
}
