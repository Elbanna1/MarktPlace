using Shared.DTOs.Animals;
using Shared.Enums;

namespace Shared.Constants;

public static class CamelCatalog
{
    public static readonly IReadOnlyList<AnimalLookupEntry<CamelBreed>> Breeds =
        new List<AnimalLookupEntry<CamelBreed>>
        {
            new(CamelBreed.Maghrabi, "مغربي", "Maghrabi"),
            new(CamelBreed.Sudani, "سوداني", "Sudani"),
            new(CamelBreed.Somali, "صومالي", "Somali"),
            new(CamelBreed.Bishari, "بشاري", "Bishari"),
            new(CamelBreed.Falahi, "فلاحي", "Falahi"),
            new(CamelBreed.RacingHijin, "هجن سباق", "Racing (Hijin)"),
            new(CamelBreed.Crossbreed, "خليط", "Crossbreed"),
            new(CamelBreed.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<CamelPurpose>> Purposes =
        new List<AnimalLookupEntry<CamelPurpose>>
        {
            new(CamelPurpose.Breeding, "تربية", "Breeding"),
            new(CamelPurpose.Racing, "سباق الهجن", "Camel racing"),
            new(CamelPurpose.MilkProduction, "إنتاج ألبان", "Milk production"),
            new(CamelPurpose.MeatProduction, "إنتاج لحوم", "Meat production"),
            new(CamelPurpose.Transport, "نقل", "Transport"),
            new(CamelPurpose.Sale, "بيع", "Sale"),
            new(CamelPurpose.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<CamelAge>> Ages =
        new List<AnimalLookupEntry<CamelAge>>
        {
            new(CamelAge.Newborn, "مولود", "Newborn"),
            new(CamelAge.Young, "صغير", "Young"),
            new(CamelAge.Adult, "بالغ", "Adult"),
            new(CamelAge.Old, "كبير السن", "Old")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<CamelGender>> Genders =
        new List<AnimalLookupEntry<CamelGender>>
        {
            new(CamelGender.Male, "ذكر", "Male"),
            new(CamelGender.Female, "أنثى", "Female"),
            new(CamelGender.Mixed, "مختلط", "Mixed")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<CamelHealthStatus>> HealthStatuses =
        new List<AnimalLookupEntry<CamelHealthStatus>>
        {
            new(CamelHealthStatus.Excellent, "ممتازة", "Excellent"),
            new(CamelHealthStatus.Good, "جيدة", "Good"),
            new(CamelHealthStatus.Fair, "مقبولة", "Fair"),
            new(CamelHealthStatus.UnderTreatment, "تحت العلاج", "Under treatment")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<CamelVaccination>> Vaccinations =
        new List<AnimalLookupEntry<CamelVaccination>>
        {
            new(CamelVaccination.FullyVaccinated, "محصن بالكامل", "Fully vaccinated"),
            new(CamelVaccination.PartiallyVaccinated, "محصن جزئيًا", "Partially vaccinated"),
            new(CamelVaccination.NotVaccinated, "غير محصن", "Not vaccinated"),
            new(CamelVaccination.Unknown, "غير معروف", "Unknown")
        };

    public static readonly IReadOnlyList<AnimalLookupItemDto> BreedOptions = AnimalCatalog.ToOptions(Breeds);
    public static readonly IReadOnlyList<AnimalLookupItemDto> PurposeOptions = AnimalCatalog.ToOptions(Purposes);
    public static readonly IReadOnlyList<AnimalLookupItemDto> AgeOptions = AnimalCatalog.ToOptions(Ages);
    public static readonly IReadOnlyList<AnimalLookupItemDto> GenderOptions = AnimalCatalog.ToOptions(Genders);
    public static readonly IReadOnlyList<AnimalLookupItemDto> HealthStatusOptions = AnimalCatalog.ToOptions(HealthStatuses);
    public static readonly IReadOnlyList<AnimalLookupItemDto> VaccinationOptions = AnimalCatalog.ToOptions(Vaccinations);

    public static string GetBreedName(CamelBreed value) => AnimalCatalog.GetName(Breeds, value);
    public static string GetPurposeName(CamelPurpose value) => AnimalCatalog.GetName(Purposes, value);
    public static string GetAgeName(CamelAge value) => AnimalCatalog.GetName(Ages, value);
    public static string GetGenderName(CamelGender value) => AnimalCatalog.GetName(Genders, value);
    public static string GetHealthStatusName(CamelHealthStatus value) => AnimalCatalog.GetName(HealthStatuses, value);
    public static string GetVaccinationName(CamelVaccination? value) => AnimalCatalog.GetName(Vaccinations, value);
}
