using Shared.Enums;

namespace Services.Lookups;

public static class AdvertiserLookupCatalog
{
    public const string SellerNameParameter = "sellerName";

    public const string NameParameter = "name";

    private static readonly IReadOnlyDictionary<SubCategoryType, string> FilterParameters =
        new Dictionary<SubCategoryType, string>
        {
            [SubCategoryType.Livestock] = SellerNameParameter,
            [SubCategoryType.SheepAndGoats] = SellerNameParameter,
            [SubCategoryType.Horses] = SellerNameParameter,
            [SubCategoryType.Camels] = SellerNameParameter,
            [SubCategoryType.Birds] = SellerNameParameter,
            [SubCategoryType.Pets] = SellerNameParameter,
            [SubCategoryType.Fish] = SellerNameParameter,
            [SubCategoryType.Bees] = SellerNameParameter,
            [SubCategoryType.OtherAnimals] = SellerNameParameter,

            [SubCategoryType.DecorAntiques] = SellerNameParameter,
            [SubCategoryType.Antiques] = SellerNameParameter,
            [SubCategoryType.Paintings] = SellerNameParameter,
            [SubCategoryType.Handmade] = SellerNameParameter,
            [SubCategoryType.CoinsAndStamps] = SellerNameParameter,

            [SubCategoryType.Doctors] = NameParameter,
            [SubCategoryType.MedicalLaboratories] = NameParameter,
            [SubCategoryType.DentalLaboratories] = NameParameter,
            [SubCategoryType.RadiologyLaboratories] = NameParameter,
            [SubCategoryType.MedicalCenters] = NameParameter,
            [SubCategoryType.Hospitals] = NameParameter,

            [SubCategoryType.LawyerOffices] = NameParameter,
            [SubCategoryType.EngineeringConsultancyOffices] = NameParameter,
            [SubCategoryType.Contractors] = NameParameter,
            [SubCategoryType.IndustrialConsultancyOffices] = NameParameter,
            [SubCategoryType.AccountantOffices] = NameParameter
        };

    public static IReadOnlyCollection<SubCategoryType> SupportedSubCategories =>
        (IReadOnlyCollection<SubCategoryType>)FilterParameters.Keys;

    public static bool Supports(int subCategoryId) =>
        Enum.IsDefined(typeof(SubCategoryType), subCategoryId) &&
        FilterParameters.ContainsKey((SubCategoryType)subCategoryId);

    public static bool Supports(SubCategoryType subCategory) => FilterParameters.ContainsKey(subCategory);

    public static string? FilterParameterOf(SubCategoryType subCategory) =>
        FilterParameters.TryGetValue(subCategory, out var parameter) ? parameter : null;
}
