using Shared.DTOs.Animals;
using Shared.Enums;

namespace Shared.Constants;

public static class SheepGoatCatalog
{
    public static readonly IReadOnlyList<AnimalLookupEntry<SheepGoatBreed>> Breeds =
        new List<AnimalLookupEntry<SheepGoatBreed>>
        {
            new(SheepGoatBreed.Barki, "برقي", "Barki"),
            new(SheepGoatBreed.Rahmani, "رحماني", "Rahmani"),
            new(SheepGoatBreed.Ossimi, "أوسيمي", "Ossimi"),
            new(SheepGoatBreed.Awassi, "عواسي", "Awassi"),
            new(SheepGoatBreed.Naimi, "نعيمي", "Naimi"),
            new(SheepGoatBreed.Harri, "حري", "Harri"),
            new(SheepGoatBreed.BaladiGoat, "ماعز بلدي", "Baladi Goat"),
            new(SheepGoatBreed.ZaraibiGoat, "ماعز زرايبي", "Zaraibi Goat"),
            new(SheepGoatBreed.DamascusGoat, "ماعز شامي", "Damascus Goat"),
            new(SheepGoatBreed.BoerGoat, "ماعز بور", "Boer Goat"),
            new(SheepGoatBreed.Crossbreed, "خليط", "Crossbreed"),
            new(SheepGoatBreed.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<SheepGoatPurpose>> Purposes =
        new List<AnimalLookupEntry<SheepGoatPurpose>>
        {
            new(SheepGoatPurpose.Breeding, "تربية", "Breeding"),
            new(SheepGoatPurpose.Fattening, "تسمين", "Fattening"),
            new(SheepGoatPurpose.MilkProduction, "إنتاج ألبان", "Milk production"),
            new(SheepGoatPurpose.MeatProduction, "إنتاج لحوم", "Meat production"),
            new(SheepGoatPurpose.Sacrifice, "أضاحي", "Sacrifice"),
            new(SheepGoatPurpose.Sale, "بيع", "Sale"),
            new(SheepGoatPurpose.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<SheepGoatAge>> Ages =
        new List<AnimalLookupEntry<SheepGoatAge>>
        {
            new(SheepGoatAge.Newborn, "مولود", "Newborn"),
            new(SheepGoatAge.Young, "صغير", "Young"),
            new(SheepGoatAge.Adult, "بالغ", "Adult"),
            new(SheepGoatAge.Old, "كبير السن", "Old")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<SheepGoatGender>> Genders =
        new List<AnimalLookupEntry<SheepGoatGender>>
        {
            new(SheepGoatGender.Male, "ذكر", "Male"),
            new(SheepGoatGender.Female, "أنثى", "Female"),
            new(SheepGoatGender.Mixed, "مختلط", "Mixed")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<SheepGoatHealthStatus>> HealthStatuses =
        new List<AnimalLookupEntry<SheepGoatHealthStatus>>
        {
            new(SheepGoatHealthStatus.Excellent, "ممتازة", "Excellent"),
            new(SheepGoatHealthStatus.Good, "جيدة", "Good"),
            new(SheepGoatHealthStatus.Fair, "مقبولة", "Fair"),
            new(SheepGoatHealthStatus.UnderTreatment, "تحت العلاج", "Under treatment")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<SheepGoatVaccination>> Vaccinations =
        new List<AnimalLookupEntry<SheepGoatVaccination>>
        {
            new(SheepGoatVaccination.FullyVaccinated, "محصن بالكامل", "Fully vaccinated"),
            new(SheepGoatVaccination.PartiallyVaccinated, "محصن جزئيًا", "Partially vaccinated"),
            new(SheepGoatVaccination.NotVaccinated, "غير محصن", "Not vaccinated"),
            new(SheepGoatVaccination.Unknown, "غير معروف", "Unknown")
        };

    public static readonly IReadOnlyList<AnimalLookupItemDto> BreedOptions = AnimalCatalog.ToOptions(Breeds);
    public static readonly IReadOnlyList<AnimalLookupItemDto> PurposeOptions = AnimalCatalog.ToOptions(Purposes);
    public static readonly IReadOnlyList<AnimalLookupItemDto> AgeOptions = AnimalCatalog.ToOptions(Ages);
    public static readonly IReadOnlyList<AnimalLookupItemDto> GenderOptions = AnimalCatalog.ToOptions(Genders);
    public static readonly IReadOnlyList<AnimalLookupItemDto> HealthStatusOptions = AnimalCatalog.ToOptions(HealthStatuses);
    public static readonly IReadOnlyList<AnimalLookupItemDto> VaccinationOptions = AnimalCatalog.ToOptions(Vaccinations);

    public static string GetBreedName(SheepGoatBreed value) => AnimalCatalog.GetName(Breeds, value);
    public static string GetPurposeName(SheepGoatPurpose value) => AnimalCatalog.GetName(Purposes, value);
    public static string GetAgeName(SheepGoatAge value) => AnimalCatalog.GetName(Ages, value);
    public static string GetGenderName(SheepGoatGender value) => AnimalCatalog.GetName(Genders, value);
    public static string GetHealthStatusName(SheepGoatHealthStatus value) => AnimalCatalog.GetName(HealthStatuses, value);
    public static string GetVaccinationName(SheepGoatVaccination? value) => AnimalCatalog.GetName(Vaccinations, value);
}
