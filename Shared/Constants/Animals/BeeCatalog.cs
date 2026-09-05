using Shared.DTOs.Animals;
using Shared.Enums;

namespace Shared.Constants;

public static class BeeCatalog
{
    public static readonly IReadOnlyList<AnimalLookupEntry<BeeType>> Types =
        new List<AnimalLookupEntry<BeeType>>
        {
            new(BeeType.Carniolan, "كرنيولي", "Carniolan"),
            new(BeeType.Italian, "إيطالي", "Italian"),
            new(BeeType.EgyptianBaladi, "بلدي مصري", "Egyptian Baladi"),
            new(BeeType.Buckfast, "بكفاست", "Buckfast"),
            new(BeeType.Caucasian, "قوقازي", "Caucasian"),
            new(BeeType.Hybrid, "هجين", "Hybrid"),
            new(BeeType.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<BeePurpose>> Purposes =
        new List<AnimalLookupEntry<BeePurpose>>
        {
            new(BeePurpose.HoneyProduction, "إنتاج عسل", "Honey production"),
            new(BeePurpose.Pollination, "تلقيح المحاصيل", "Pollination"),
            new(BeePurpose.Breeding, "تربية وإنتاج", "Breeding"),
            new(BeePurpose.QueenProduction, "إنتاج ملكات", "Queen production"),
            new(BeePurpose.Sale, "بيع", "Sale"),
            new(BeePurpose.Other, "أخرى", "Other")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<BeeHealthStatus>> HealthStatuses =
        new List<AnimalLookupEntry<BeeHealthStatus>>
        {
            new(BeeHealthStatus.Excellent, "ممتازة", "Excellent"),
            new(BeeHealthStatus.Good, "جيدة", "Good"),
            new(BeeHealthStatus.Fair, "مقبولة", "Fair"),
            new(BeeHealthStatus.UnderTreatment, "تحت العلاج", "Under treatment")
        };

    public static readonly IReadOnlyList<AnimalLookupEntry<BeeProduction>> Productions =
        new List<AnimalLookupEntry<BeeProduction>>
        {
            new(BeeProduction.Honey, "عسل", "Honey"),
            new(BeeProduction.Wax, "شمع", "Wax"),
            new(BeeProduction.RoyalJelly, "غذاء ملكات", "Royal jelly"),
            new(BeeProduction.Propolis, "عكبر", "Propolis"),
            new(BeeProduction.Pollen, "حبوب لقاح", "Pollen")
        };

    public static readonly IReadOnlyList<AnimalLookupItemDto> TypeOptions = AnimalCatalog.ToOptions(Types);
    public static readonly IReadOnlyList<AnimalLookupItemDto> PurposeOptions = AnimalCatalog.ToOptions(Purposes);
    public static readonly IReadOnlyList<AnimalLookupItemDto> HealthStatusOptions = AnimalCatalog.ToOptions(HealthStatuses);
    public static readonly IReadOnlyList<AnimalLookupItemDto> ProductionOptions = AnimalCatalog.ToOptions(Productions);

    public static string GetTypeName(BeeType value) => AnimalCatalog.GetName(Types, value);
    public static string GetPurposeName(BeePurpose value) => AnimalCatalog.GetName(Purposes, value);
    public static string GetHealthStatusName(BeeHealthStatus value) => AnimalCatalog.GetName(HealthStatuses, value);
    public static string GetProductionName(BeeProduction? value) => AnimalCatalog.GetName(Productions, value);
}
