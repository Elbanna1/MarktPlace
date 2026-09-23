using Shared.Enums;

namespace Services.Lookups;

public static class AdvertiserLookupCatalog
{
    private static readonly IReadOnlyDictionary<SubCategoryType, string> Modules =
        new Dictionary<SubCategoryType, string>
        {
            [SubCategoryType.Livestock] = "livestock",
            [SubCategoryType.SheepAndGoats] = "sheepGoats",
            [SubCategoryType.Horses] = "horses",
            [SubCategoryType.Camels] = "camels",
            [SubCategoryType.Birds] = "birds",
            [SubCategoryType.Pets] = "pets",
            [SubCategoryType.Fish] = "fish",
            [SubCategoryType.Bees] = "bees",
            [SubCategoryType.OtherAnimals] = "otherAnimals",

            [SubCategoryType.DecorAntiques] = "decorAntiques",
            [SubCategoryType.Antiques] = "antiques",
            [SubCategoryType.Paintings] = "paintings",
            [SubCategoryType.Handmade] = "handmade",
            [SubCategoryType.CoinsAndStamps] = "coinsStamps"
        };

    public const string FilterParameter = "sellerName";

    public static IReadOnlyCollection<SubCategoryType> SupportedSubCategories => (IReadOnlyCollection<SubCategoryType>)Modules.Keys;

    public static bool Supports(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        Modules.ContainsKey((SubCategoryType)subCategoryId);

    public static bool Supports(SubCategoryType subCategory) => Modules.ContainsKey(subCategory);

    public static string? Module(SubCategoryType subCategory) =>
        Modules.TryGetValue(subCategory, out var module) ? module : null;
}
