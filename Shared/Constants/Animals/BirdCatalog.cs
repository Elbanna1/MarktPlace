using Shared.DTOs.Animals;
using Shared.Enums;

namespace Shared.Constants;

public static class BirdCatalog
{
    public static readonly IReadOnlyList<AnimalLookupEntry<BirdType>> Types =
        new List<AnimalLookupEntry<BirdType>>
        {
            new(BirdType.Chickens, "دجاج", "Chickens"),
            new(BirdType.BaladiChickens, "دجاج بلدي", "Baladi Chickens"),
            new(BirdType.Ducks, "بط", "Ducks"),
            new(BirdType.Geese, "إوز", "Geese"),
            new(BirdType.Turkey, "رومي", "Turkey"),
            new(BirdType.Pigeons, "حمام", "Pigeons"),
            new(BirdType.Quail, "سمان", "Quail"),
            new(BirdType.Ostrich, "نعام", "Ostrich"),
            new(BirdType.Canary, "كناري", "Canary"),
            new(BirdType.Parrots, "ببغاء", "Parrots"),
            new(BirdType.LoveBirds, "طيور الحب", "Love Birds"),
            new(BirdType.Falcons, "صقور", "Falcons"),
            new(BirdType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<BirdPurpose>> Purposes =
        new List<AnimalLookupEntry<BirdPurpose>>
        {
            new(BirdPurpose.Breeding, "تربية", "Breeding"),
            new(BirdPurpose.EggProduction, "إنتاج بيض", "Egg production"),
            new(BirdPurpose.MeatProduction, "إنتاج لحوم", "Meat production"),
            new(BirdPurpose.Ornamental, "زينة", "Ornamental"),
            new(BirdPurpose.Hunting, "صيد", "Hunting"),
            new(BirdPurpose.Sale, "بيع", "Sale"),
            new(BirdPurpose.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<BirdAge>> Ages =
        new List<AnimalLookupEntry<BirdAge>>
        {
            new(BirdAge.Newborn, "مولود", "Newborn"),
            new(BirdAge.Young, "صغير", "Young"),
            new(BirdAge.Adult, "بالغ", "Adult"),
            new(BirdAge.Old, "كبير السن", "Old")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<BirdGender>> Genders =
        new List<AnimalLookupEntry<BirdGender>>
        {
            new(BirdGender.Male, "ذكر", "Male"),
            new(BirdGender.Female, "أنثى", "Female"),
            new(BirdGender.Mixed, "مختلط", "Mixed")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<BirdHealthStatus>> HealthStatuses =
        new List<AnimalLookupEntry<BirdHealthStatus>>
        {
            new(BirdHealthStatus.Excellent, "ممتازة", "Excellent"),
            new(BirdHealthStatus.Good, "جيدة", "Good"),
            new(BirdHealthStatus.Fair, "مقبولة", "Fair"),
            new(BirdHealthStatus.UnderTreatment, "تحت العلاج", "Under treatment")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<BirdVaccination>> Vaccinations =
        new List<AnimalLookupEntry<BirdVaccination>>
        {
            new(BirdVaccination.FullyVaccinated, "محصن بالكامل", "Fully vaccinated"),
            new(BirdVaccination.PartiallyVaccinated, "محصن جزئيًا", "Partially vaccinated"),
            new(BirdVaccination.NotVaccinated, "غير محصن", "Not vaccinated"),
            new(BirdVaccination.Unknown, "غير معروف", "Unknown")
        };

    public static readonly IReadOnlyList<AnimalLookupItemDto> TypeOptions = AnimalCatalog.ToOptions(Types);
    public static readonly IReadOnlyList<AnimalLookupItemDto> PurposeOptions = AnimalCatalog.ToOptions(Purposes);
    public static readonly IReadOnlyList<AnimalLookupItemDto> AgeOptions = AnimalCatalog.ToOptions(Ages);
    public static readonly IReadOnlyList<AnimalLookupItemDto> GenderOptions = AnimalCatalog.ToOptions(Genders);
    public static readonly IReadOnlyList<AnimalLookupItemDto> HealthStatusOptions = AnimalCatalog.ToOptions(HealthStatuses);
    public static readonly IReadOnlyList<AnimalLookupItemDto> VaccinationOptions = AnimalCatalog.ToOptions(Vaccinations);

    public static string GetTypeName(BirdType value) => AnimalCatalog.GetName(Types, value);
    public static string GetPurposeName(BirdPurpose value) => AnimalCatalog.GetName(Purposes, value);
    public static string GetAgeName(BirdAge value) => AnimalCatalog.GetName(Ages, value);
    public static string GetGenderName(BirdGender value) => AnimalCatalog.GetName(Genders, value);
    public static string GetHealthStatusName(BirdHealthStatus value) => AnimalCatalog.GetName(HealthStatuses, value);
    public static string GetVaccinationName(BirdVaccination? value) => AnimalCatalog.GetName(Vaccinations, value);
}
