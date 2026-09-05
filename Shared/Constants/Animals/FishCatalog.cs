using Shared.DTOs.Animals;
using Shared.Enums;

namespace Shared.Constants;

public static class FishCatalog
{
    public static readonly IReadOnlyList<AnimalLookupEntry<FishType>> Types =
        new List<AnimalLookupEntry<FishType>>
        {
            new(FishType.Tilapia, "بلطي", "Tilapia"),
            new(FishType.Catfish, "قرموط", "Catfish"),
            new(FishType.Mullet, "بوري", "Mullet"),
            new(FishType.Carp, "مبروك", "Carp"),
            new(FishType.SeaBass, "قاروص", "Sea Bass"),
            new(FishType.SeaBream, "دنيس", "Sea Bream"),
            new(FishType.Shrimp, "جمبري", "Shrimp"),
            new(FishType.OrnamentalFish, "أسماك زينة", "Ornamental Fish"),
            new(FishType.Goldfish, "سمك ذهبي", "Goldfish"),
            new(FishType.Koi, "كوي", "Koi"),
            new(FishType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<FishPurpose>> Purposes =
        new List<AnimalLookupEntry<FishPurpose>>
        {
            new(FishPurpose.Ornamental, "زينة", "Ornamental"),
            new(FishPurpose.Consumption, "استهلاك", "Consumption"),
            new(FishPurpose.FishFarming, "استزراع سمكي", "Fish farming"),
            new(FishPurpose.Breeding, "تربية وإنتاج", "Breeding"),
            new(FishPurpose.Sale, "بيع", "Sale"),
            new(FishPurpose.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<FishAge>> Ages =
        new List<AnimalLookupEntry<FishAge>>
        {
            new(FishAge.Fry, "زريعة", "Fry"),
            new(FishAge.Juvenile, "إصبعيات", "Juvenile"),
            new(FishAge.Adult, "بالغ", "Adult"),
            new(FishAge.Breeder, "أمهات", "Breeder")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<FishHealthStatus>> HealthStatuses =
        new List<AnimalLookupEntry<FishHealthStatus>>
        {
            new(FishHealthStatus.Excellent, "ممتازة", "Excellent"),
            new(FishHealthStatus.Good, "جيدة", "Good"),
            new(FishHealthStatus.Fair, "مقبولة", "Fair"),
            new(FishHealthStatus.UnderTreatment, "تحت العلاج", "Under treatment")
        };

    public static readonly IReadOnlyList<AnimalLookupItemDto> TypeOptions = AnimalCatalog.ToOptions(Types);
    public static readonly IReadOnlyList<AnimalLookupItemDto> PurposeOptions = AnimalCatalog.ToOptions(Purposes);
    public static readonly IReadOnlyList<AnimalLookupItemDto> AgeOptions = AnimalCatalog.ToOptions(Ages);
    public static readonly IReadOnlyList<AnimalLookupItemDto> HealthStatusOptions = AnimalCatalog.ToOptions(HealthStatuses);

    public static string GetTypeName(FishType value) => AnimalCatalog.GetName(Types, value);
    public static string GetPurposeName(FishPurpose value) => AnimalCatalog.GetName(Purposes, value);
    public static string GetAgeName(FishAge value) => AnimalCatalog.GetName(Ages, value);
    public static string GetHealthStatusName(FishHealthStatus value) => AnimalCatalog.GetName(HealthStatuses, value);
}
