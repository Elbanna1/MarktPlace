using Shared.DTOs.Animals;
using Shared.Enums;

namespace Shared.Constants;

public static class HorseCatalog
{
    public static readonly IReadOnlyList<AnimalLookupEntry<HorseBreed>> Breeds =
        new List<AnimalLookupEntry<HorseBreed>>
        {
            new(HorseBreed.ArabianHorse, "عربي أصيل", "Arabian Horse"),
            new(HorseBreed.EgyptianArabian, "عربي مصري", "Egyptian Arabian"),
            new(HorseBreed.Thoroughbred, "إنجليزي أصيل", "Thoroughbred"),
            new(HorseBreed.Barb, "بربري", "Barb"),
            new(HorseBreed.Andalusian, "أندلسي", "Andalusian"),
            new(HorseBreed.Friesian, "فريزيان", "Friesian"),
            new(HorseBreed.QuarterHorse, "كوارتر", "Quarter Horse"),
            new(HorseBreed.Pony, "بوني", "Pony"),
            new(HorseBreed.Crossbreed, "خليط", "Crossbreed"),
            new(HorseBreed.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<HorsePurpose>> Purposes =
        new List<AnimalLookupEntry<HorsePurpose>>
        {
            new(HorsePurpose.Riding, "ركوب", "Riding"),
            new(HorsePurpose.Racing, "سباق", "Racing"),
            new(HorsePurpose.Breeding, "تربية وإنتاج", "Breeding"),
            new(HorsePurpose.Shows, "عروض", "Shows"),
            new(HorsePurpose.Work, "عمل", "Work"),
            new(HorsePurpose.Sale, "بيع", "Sale"),
            new(HorsePurpose.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<HorseAge>> Ages =
        new List<AnimalLookupEntry<HorseAge>>
        {
            new(HorseAge.Newborn, "مولود", "Newborn"),
            new(HorseAge.Young, "صغير", "Young"),
            new(HorseAge.Adult, "بالغ", "Adult"),
            new(HorseAge.Old, "كبير السن", "Old")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<HorseGender>> Genders =
        new List<AnimalLookupEntry<HorseGender>>
        {
            new(HorseGender.Male, "ذكر", "Male"),
            new(HorseGender.Female, "أنثى", "Female"),
            new(HorseGender.Mixed, "مختلط", "Mixed")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<HorseHealthStatus>> HealthStatuses =
        new List<AnimalLookupEntry<HorseHealthStatus>>
        {
            new(HorseHealthStatus.Excellent, "ممتازة", "Excellent"),
            new(HorseHealthStatus.Good, "جيدة", "Good"),
            new(HorseHealthStatus.Fair, "مقبولة", "Fair"),
            new(HorseHealthStatus.UnderTreatment, "تحت العلاج", "Under treatment")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<HorseTrainingLevel>> TrainingLevels =
        new List<AnimalLookupEntry<HorseTrainingLevel>>
        {
            new(HorseTrainingLevel.Untrained, "غير مدرب", "Untrained"),
            new(HorseTrainingLevel.Basic, "تدريب أساسي", "Basic training"),
            new(HorseTrainingLevel.Advanced, "تدريب متقدم", "Advanced training"),
            new(HorseTrainingLevel.Professional, "تدريب احترافي", "Professional training")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<HorseVaccination>> Vaccinations =
        new List<AnimalLookupEntry<HorseVaccination>>
        {
            new(HorseVaccination.FullyVaccinated, "محصن بالكامل", "Fully vaccinated"),
            new(HorseVaccination.PartiallyVaccinated, "محصن جزئيًا", "Partially vaccinated"),
            new(HorseVaccination.NotVaccinated, "غير محصن", "Not vaccinated"),
            new(HorseVaccination.Unknown, "غير معروف", "Unknown")
        };

    public static readonly IReadOnlyList<AnimalLookupItemDto> BreedOptions = AnimalCatalog.ToOptions(Breeds);
    public static readonly IReadOnlyList<AnimalLookupItemDto> PurposeOptions = AnimalCatalog.ToOptions(Purposes);
    public static readonly IReadOnlyList<AnimalLookupItemDto> AgeOptions = AnimalCatalog.ToOptions(Ages);
    public static readonly IReadOnlyList<AnimalLookupItemDto> GenderOptions = AnimalCatalog.ToOptions(Genders);
    public static readonly IReadOnlyList<AnimalLookupItemDto> HealthStatusOptions = AnimalCatalog.ToOptions(HealthStatuses);
    public static readonly IReadOnlyList<AnimalLookupItemDto> TrainingLevelOptions = AnimalCatalog.ToOptions(TrainingLevels);
    public static readonly IReadOnlyList<AnimalLookupItemDto> VaccinationOptions = AnimalCatalog.ToOptions(Vaccinations);

    public static string GetBreedName(HorseBreed value) => AnimalCatalog.GetName(Breeds, value);
    public static string GetPurposeName(HorsePurpose value) => AnimalCatalog.GetName(Purposes, value);
    public static string GetAgeName(HorseAge value) => AnimalCatalog.GetName(Ages, value);
    public static string GetGenderName(HorseGender value) => AnimalCatalog.GetName(Genders, value);
    public static string GetHealthStatusName(HorseHealthStatus value) => AnimalCatalog.GetName(HealthStatuses, value);
    public static string GetTrainingLevelName(HorseTrainingLevel? value) => AnimalCatalog.GetName(TrainingLevels, value);
    public static string GetVaccinationName(HorseVaccination? value) => AnimalCatalog.GetName(Vaccinations, value);
}
