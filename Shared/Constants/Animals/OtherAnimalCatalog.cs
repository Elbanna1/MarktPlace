using Shared.DTOs.Animals;
using Shared.Enums;

namespace Shared.Constants;

public static class OtherAnimalCatalog
{
    public static readonly IReadOnlyList<AnimalLookupEntry<OtherAnimalType>> Types =
        new List<AnimalLookupEntry<OtherAnimalType>>
        {
            new(OtherAnimalType.Donkeys, "حمير", "Donkeys"),
            new(OtherAnimalType.Mules, "بغال", "Mules"),
            new(OtherAnimalType.Rabbits, "أرانب", "Rabbits"),
            new(OtherAnimalType.GuineaPig, "خنزير غينيا", "Guinea Pig"),
            new(OtherAnimalType.Deer, "غزلان", "Deer"),
            new(OtherAnimalType.Reptiles, "زواحف", "Reptiles"),
            new(OtherAnimalType.Monkeys, "قرود", "Monkeys"),
            new(OtherAnimalType.FarmInsects, "حشرات مزرعية", "Farm Insects"),
            new(OtherAnimalType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<OtherAnimalPurpose>> Purposes =
        new List<AnimalLookupEntry<OtherAnimalPurpose>>
        {
            new(OtherAnimalPurpose.Breeding, "تربية", "Breeding"),
            new(OtherAnimalPurpose.Work, "عمل", "Work"),
            new(OtherAnimalPurpose.Ornamental, "زينة", "Ornamental"),
            new(OtherAnimalPurpose.Sale, "بيع", "Sale"),
            new(OtherAnimalPurpose.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<OtherAnimalAge>> Ages =
        new List<AnimalLookupEntry<OtherAnimalAge>>
        {
            new(OtherAnimalAge.Newborn, "مولود", "Newborn"),
            new(OtherAnimalAge.Young, "صغير", "Young"),
            new(OtherAnimalAge.Adult, "بالغ", "Adult"),
            new(OtherAnimalAge.Old, "كبير السن", "Old")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<OtherAnimalGender>> Genders =
        new List<AnimalLookupEntry<OtherAnimalGender>>
        {
            new(OtherAnimalGender.Male, "ذكر", "Male"),
            new(OtherAnimalGender.Female, "أنثى", "Female"),
            new(OtherAnimalGender.Mixed, "مختلط", "Mixed")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<OtherAnimalHealthStatus>> HealthStatuses =
        new List<AnimalLookupEntry<OtherAnimalHealthStatus>>
        {
            new(OtherAnimalHealthStatus.Excellent, "ممتازة", "Excellent"),
            new(OtherAnimalHealthStatus.Good, "جيدة", "Good"),
            new(OtherAnimalHealthStatus.Fair, "مقبولة", "Fair"),
            new(OtherAnimalHealthStatus.UnderTreatment, "تحت العلاج", "Under treatment")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<OtherAnimalVaccination>> Vaccinations =
        new List<AnimalLookupEntry<OtherAnimalVaccination>>
        {
            new(OtherAnimalVaccination.FullyVaccinated, "محصن بالكامل", "Fully vaccinated"),
            new(OtherAnimalVaccination.PartiallyVaccinated, "محصن جزئيًا", "Partially vaccinated"),
            new(OtherAnimalVaccination.NotVaccinated, "غير محصن", "Not vaccinated"),
            new(OtherAnimalVaccination.Unknown, "غير معروف", "Unknown")
        };

    public static readonly IReadOnlyList<AnimalLookupItemDto> TypeOptions = AnimalCatalog.ToOptions(Types);
    public static readonly IReadOnlyList<AnimalLookupItemDto> PurposeOptions = AnimalCatalog.ToOptions(Purposes);
    public static readonly IReadOnlyList<AnimalLookupItemDto> AgeOptions = AnimalCatalog.ToOptions(Ages);
    public static readonly IReadOnlyList<AnimalLookupItemDto> GenderOptions = AnimalCatalog.ToOptions(Genders);
    public static readonly IReadOnlyList<AnimalLookupItemDto> HealthStatusOptions = AnimalCatalog.ToOptions(HealthStatuses);
    public static readonly IReadOnlyList<AnimalLookupItemDto> VaccinationOptions = AnimalCatalog.ToOptions(Vaccinations);

    public static string GetTypeName(OtherAnimalType value) => AnimalCatalog.GetName(Types, value);
    public static string GetPurposeName(OtherAnimalPurpose value) => AnimalCatalog.GetName(Purposes, value);
    public static string GetAgeName(OtherAnimalAge value) => AnimalCatalog.GetName(Ages, value);
    public static string GetGenderName(OtherAnimalGender value) => AnimalCatalog.GetName(Genders, value);
    public static string GetHealthStatusName(OtherAnimalHealthStatus value) => AnimalCatalog.GetName(HealthStatuses, value);
    public static string GetVaccinationName(OtherAnimalVaccination? value) => AnimalCatalog.GetName(Vaccinations, value);
}
