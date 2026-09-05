using Shared.DTOs.Animals;
using Shared.Enums;

namespace Shared.Constants;

public static class LivestockCatalog
{
    public static readonly IReadOnlyList<AnimalLookupEntry<LivestockBreed>> Breeds =
        new List<AnimalLookupEntry<LivestockBreed>>
        {
            new(LivestockBreed.BaladiCow, "بقر بلدي", "Baladi Cow"),
            new(LivestockBreed.FriesianCow, "بقر فريزيان", "Friesian Cow"),
            new(LivestockBreed.HolsteinCow, "بقر هولشتاين", "Holstein Cow"),
            new(LivestockBreed.SimmentalCow, "بقر سيمنتال", "Simmental Cow"),
            new(LivestockBreed.BrownSwissCow, "بقر براون سويس", "Brown Swiss Cow"),
            new(LivestockBreed.BaladiBuffalo, "جاموس بلدي", "Baladi Buffalo"),
            new(LivestockBreed.ItalianBuffalo, "جاموس إيطالي", "Italian Buffalo"),
            new(LivestockBreed.FatteningCalves, "عجول تسمين", "Fattening Calves"),
            new(LivestockBreed.Crossbreed, "خليط", "Crossbreed"),
            new(LivestockBreed.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<LivestockPurpose>> Purposes =
        new List<AnimalLookupEntry<LivestockPurpose>>
        {
            new(LivestockPurpose.Breeding, "تربية", "Breeding"),
            new(LivestockPurpose.Fattening, "تسمين", "Fattening"),
            new(LivestockPurpose.MilkProduction, "إنتاج ألبان", "Milk production"),
            new(LivestockPurpose.MeatProduction, "إنتاج لحوم", "Meat production"),
            new(LivestockPurpose.Work, "عمل", "Work"),
            new(LivestockPurpose.Sale, "بيع", "Sale"),
            new(LivestockPurpose.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<LivestockAge>> Ages =
        new List<AnimalLookupEntry<LivestockAge>>
        {
            new(LivestockAge.Newborn, "مولود", "Newborn"),
            new(LivestockAge.Young, "صغير", "Young"),
            new(LivestockAge.Adult, "بالغ", "Adult"),
            new(LivestockAge.Old, "كبير السن", "Old")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<LivestockGender>> Genders =
        new List<AnimalLookupEntry<LivestockGender>>
        {
            new(LivestockGender.Male, "ذكر", "Male"),
            new(LivestockGender.Female, "أنثى", "Female"),
            new(LivestockGender.Mixed, "مختلط", "Mixed")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<LivestockHealthStatus>> HealthStatuses =
        new List<AnimalLookupEntry<LivestockHealthStatus>>
        {
            new(LivestockHealthStatus.Excellent, "ممتازة", "Excellent"),
            new(LivestockHealthStatus.Good, "جيدة", "Good"),
            new(LivestockHealthStatus.Fair, "مقبولة", "Fair"),
            new(LivestockHealthStatus.UnderTreatment, "تحت العلاج", "Under treatment")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<LivestockVaccination>> Vaccinations =
        new List<AnimalLookupEntry<LivestockVaccination>>
        {
            new(LivestockVaccination.FullyVaccinated, "محصن بالكامل", "Fully vaccinated"),
            new(LivestockVaccination.PartiallyVaccinated, "محصن جزئيًا", "Partially vaccinated"),
            new(LivestockVaccination.NotVaccinated, "غير محصن", "Not vaccinated"),
            new(LivestockVaccination.Unknown, "غير معروف", "Unknown")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<LivestockProduction>> Productions =
        new List<AnimalLookupEntry<LivestockProduction>>
        {
            new(LivestockProduction.Milk, "ألبان", "Milk"),
            new(LivestockProduction.Meat, "لحوم", "Meat"),
            new(LivestockProduction.MilkAndMeat, "ألبان ولحوم", "Milk & meat"),
            new(LivestockProduction.Leather, "جلود", "Leather"),
            new(LivestockProduction.None, "لا يوجد", "None")
        };

    public static readonly IReadOnlyList<AnimalLookupItemDto> BreedOptions = AnimalCatalog.ToOptions(Breeds);
    public static readonly IReadOnlyList<AnimalLookupItemDto> PurposeOptions = AnimalCatalog.ToOptions(Purposes);
    public static readonly IReadOnlyList<AnimalLookupItemDto> AgeOptions = AnimalCatalog.ToOptions(Ages);
    public static readonly IReadOnlyList<AnimalLookupItemDto> GenderOptions = AnimalCatalog.ToOptions(Genders);
    public static readonly IReadOnlyList<AnimalLookupItemDto> HealthStatusOptions = AnimalCatalog.ToOptions(HealthStatuses);
    public static readonly IReadOnlyList<AnimalLookupItemDto> VaccinationOptions = AnimalCatalog.ToOptions(Vaccinations);
    public static readonly IReadOnlyList<AnimalLookupItemDto> ProductionOptions = AnimalCatalog.ToOptions(Productions);

    public static string GetBreedName(LivestockBreed value) => AnimalCatalog.GetName(Breeds, value);
    public static string GetPurposeName(LivestockPurpose value) => AnimalCatalog.GetName(Purposes, value);
    public static string GetAgeName(LivestockAge value) => AnimalCatalog.GetName(Ages, value);
    public static string GetGenderName(LivestockGender value) => AnimalCatalog.GetName(Genders, value);
    public static string GetHealthStatusName(LivestockHealthStatus value) => AnimalCatalog.GetName(HealthStatuses, value);
    public static string GetVaccinationName(LivestockVaccination? value) => AnimalCatalog.GetName(Vaccinations, value);
    public static string GetProductionName(LivestockProduction? value) => AnimalCatalog.GetName(Productions, value);
}
