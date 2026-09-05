namespace Shared.Enums;

public enum LandType
{
    Residential = 1,

    Building = 2,

    Commercial = 3,

    Administrative = 4,

    Industrial = 5,

    Agricultural = 6,

    Reclamation = 7,

    Cemeteries = 8,

    Other = 9
}

public static class LandTypeGroups
{
    public static readonly IReadOnlySet<LandType> Agricultural = new HashSet<LandType>
    {
        LandType.Agricultural,
        LandType.Reclamation
    };

    public static readonly IReadOnlySet<LandType> Building = new HashSet<LandType>
    {
        LandType.Building,
        LandType.Residential,
        LandType.Commercial,
        LandType.Industrial
    };

    public static bool IsAgricultural(LandType type) => Agricultural.Contains(type);

    public static bool IsBuilding(LandType type) => Building.Contains(type);
}

public enum LandAreaUnit
{
    SquareMeter = 1,

    Feddan = 2
}

public enum LandFacadesCount
{
    One = 1,

    Two = 2,

    Three = 3,

    Four = 4
}

public enum LandDirection
{
    North = 1,

    South = 2,

    East = 3,

    West = 4,

    Corner = 5
}

public enum LandRoadType
{
    Asphalt = 1,

    Interlock = 2,

    Dirt = 3,

    Graded = 4
}

public enum LandLegalStatus
{
    Licensed = 1,

    Reconciliation = 2,

    Unlicensed = 3
}

public enum LandReconciliationForm
{
    Form1 = 1,

    Form3 = 2,

    Form8 = 3,

    Form10 = 4,

    FinalForm = 5,

    Other = 6
}

public enum LandOwnershipDocument
{
    FinalContract = 1,

    GreenContract = 2,

    PreliminaryContract = 3,

    PowerOfAttorney = 4,

    Other = 5
}

public enum LandUtility
{
    ElectricityMeter = 1,

    WaterMeter = 2,

    NaturalGas = 3,

    Sewage = 4,

    Internet = 5,

    WaterWell = 6,

    IrrigationNetwork = 7,

    Walled = 8,

    Gate = 9,

    StreetLighting = 10,

    AgriculturalDrainage = 11
}

public enum LandRentType
{
    Daily = 1,

    Weekly = 2,

    Monthly = 3,

    Yearly = 4
}

public enum LandMinimumRentPeriod
{
    Day = 1,

    Week = 2,

    Month = 3,

    Year = 4
}

public enum LandRentInclusion
{
    Electricity = 1,

    Water = 2,

    Gas = 3,

    Maintenance = 4,

    Security = 5
}

public enum LandContractDuration
{
    OneYear = 1,

    TwoYears = 2,

    ThreeYears = 3,

    AsAgreed = 4
}

public enum LandExchangeWith
{
    Land = 1,

    Apartment = 2,

    Shop = 3,

    Villa = 4,

    Farm = 5,

    Car = 6,

    Factory = 7,

    Other = 8
}

public enum LandHarvestSeason
{
    Summer = 1,

    Winter = 2,

    AllYear = 3
}

public enum LandSoilType
{
    Clay = 1,

    Sandy = 2,

    Yellow = 3,

    Limestone = 4,

    Mixed = 5
}

public enum LandIrrigationSource
{
    Canal = 1,

    Well = 2,

    Drip = 3,

    Sprinkler = 4,

    Other = 5
}

public enum LandQualityCertificate
{
    GlobalGap = 1,

    Organic = 2,

    Other = 3
}

public enum LandExistingBuildingType
{
    House = 1,

    Storage = 2,

    Shop = 3,

    Factory = 4,

    Other = 5
}

public enum LandBuildingCompletionRatio
{
    TwentyFivePercent = 1,

    FiftyPercent = 2,

    SeventyFivePercent = 3,

    Completed = 4
}
