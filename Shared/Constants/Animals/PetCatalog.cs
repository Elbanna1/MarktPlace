using Shared.DTOs.Animals;
using Shared.Enums;

namespace Shared.Constants;

public static class PetCatalog
{
    public static readonly IReadOnlyList<AnimalLookupEntry<PetBreed>> Breeds =
        new List<AnimalLookupEntry<PetBreed>>
        {
            new(PetBreed.Dogs, "كلاب", "Dogs"),
            new(PetBreed.GermanShepherd, "جيرمن شيبرد", "German Shepherd"),
            new(PetBreed.Husky, "هاسكي", "Husky"),
            new(PetBreed.GoldenRetriever, "جولدن ريتريفر", "Golden Retriever"),
            new(PetBreed.Griffon, "جريفون", "Griffon"),
            new(PetBreed.Cats, "قطط", "Cats"),
            new(PetBreed.PersianCat, "قط شيرازي", "Persian Cat"),
            new(PetBreed.SiameseCat, "قط سيامي", "Siamese Cat"),
            new(PetBreed.ShiraziCat, "قط هيمالايا", "Himalayan Cat"),
            new(PetBreed.Rabbits, "أرانب", "Rabbits"),
            new(PetBreed.Hamster, "هامستر", "Hamster"),
            new(PetBreed.Turtles, "سلاحف", "Turtles"),
            new(PetBreed.Squirrel, "سنجاب", "Squirrel"),
            new(PetBreed.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<PetPurpose>> Purposes =
        new List<AnimalLookupEntry<PetPurpose>>
        {
            new(PetPurpose.Companionship, "تربية منزلية", "Companionship"),
            new(PetPurpose.Breeding, "تربية وإنتاج", "Breeding"),
            new(PetPurpose.Guarding, "حراسة", "Guarding"),
            new(PetPurpose.Shows, "عروض", "Shows"),
            new(PetPurpose.Sale, "بيع", "Sale"),
            new(PetPurpose.Adoption, "تبني", "Adoption"),
            new(PetPurpose.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<PetAge>> Ages =
        new List<AnimalLookupEntry<PetAge>>
        {
            new(PetAge.Newborn, "مولود", "Newborn"),
            new(PetAge.Young, "صغير", "Young"),
            new(PetAge.Adult, "بالغ", "Adult"),
            new(PetAge.Old, "كبير السن", "Old")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<PetGender>> Genders =
        new List<AnimalLookupEntry<PetGender>>
        {
            new(PetGender.Male, "ذكر", "Male"),
            new(PetGender.Female, "أنثى", "Female"),
            new(PetGender.Mixed, "مختلط", "Mixed")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<PetHealthStatus>> HealthStatuses =
        new List<AnimalLookupEntry<PetHealthStatus>>
        {
            new(PetHealthStatus.Excellent, "ممتازة", "Excellent"),
            new(PetHealthStatus.Good, "جيدة", "Good"),
            new(PetHealthStatus.Fair, "مقبولة", "Fair"),
            new(PetHealthStatus.UnderTreatment, "تحت العلاج", "Under treatment")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<PetTrainingLevel>> TrainingLevels =
        new List<AnimalLookupEntry<PetTrainingLevel>>
        {
            new(PetTrainingLevel.Untrained, "غير مدرب", "Untrained"),
            new(PetTrainingLevel.Basic, "تدريب أساسي", "Basic training"),
            new(PetTrainingLevel.Advanced, "تدريب متقدم", "Advanced training"),
            new(PetTrainingLevel.Professional, "تدريب احترافي", "Professional training")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<PetVaccination>> Vaccinations =
        new List<AnimalLookupEntry<PetVaccination>>
        {
            new(PetVaccination.FullyVaccinated, "محصن بالكامل", "Fully vaccinated"),
            new(PetVaccination.PartiallyVaccinated, "محصن جزئيًا", "Partially vaccinated"),
            new(PetVaccination.NotVaccinated, "غير محصن", "Not vaccinated"),
            new(PetVaccination.Unknown, "غير معروف", "Unknown")
        };

    public static readonly IReadOnlyList<AnimalLookupItemDto> BreedOptions = AnimalCatalog.ToOptions(Breeds);
    public static readonly IReadOnlyList<AnimalLookupItemDto> PurposeOptions = AnimalCatalog.ToOptions(Purposes);
    public static readonly IReadOnlyList<AnimalLookupItemDto> AgeOptions = AnimalCatalog.ToOptions(Ages);
    public static readonly IReadOnlyList<AnimalLookupItemDto> GenderOptions = AnimalCatalog.ToOptions(Genders);
    public static readonly IReadOnlyList<AnimalLookupItemDto> HealthStatusOptions = AnimalCatalog.ToOptions(HealthStatuses);
    public static readonly IReadOnlyList<AnimalLookupItemDto> TrainingLevelOptions = AnimalCatalog.ToOptions(TrainingLevels);
    public static readonly IReadOnlyList<AnimalLookupItemDto> VaccinationOptions = AnimalCatalog.ToOptions(Vaccinations);

    public static string GetBreedName(PetBreed value) => AnimalCatalog.GetName(Breeds, value);
    public static string GetPurposeName(PetPurpose value) => AnimalCatalog.GetName(Purposes, value);
    public static string GetAgeName(PetAge value) => AnimalCatalog.GetName(Ages, value);
    public static string GetGenderName(PetGender value) => AnimalCatalog.GetName(Genders, value);
    public static string GetHealthStatusName(PetHealthStatus value) => AnimalCatalog.GetName(HealthStatuses, value);
    public static string GetTrainingLevelName(PetTrainingLevel? value) => AnimalCatalog.GetName(TrainingLevels, value);
    public static string GetVaccinationName(PetVaccination? value) => AnimalCatalog.GetName(Vaccinations, value);
}
