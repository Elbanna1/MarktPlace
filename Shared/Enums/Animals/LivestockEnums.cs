namespace Shared.Enums;

public enum LivestockBreed
{
    BaladiCow = 1,

    FriesianCow = 2,

    HolsteinCow = 3,

    SimmentalCow = 4,

    BrownSwissCow = 5,

    BaladiBuffalo = 6,

    ItalianBuffalo = 7,

    FatteningCalves = 8,

    Crossbreed = 9,

    Other = 10
}

public enum LivestockPurpose
{
    Breeding = 1,

    Fattening = 2,

    MilkProduction = 3,

    MeatProduction = 4,

    Work = 5,

    Sale = 6,

    Other = 7
}

public enum LivestockAge
{
    Newborn = 1,

    Young = 2,

    Adult = 3,

    Old = 4
}

public enum LivestockGender
{
    Male = 1,

    Female = 2,

    Mixed = 3
}

public enum LivestockHealthStatus
{
    Excellent = 1,

    Good = 2,

    Fair = 3,

    UnderTreatment = 4
}

public enum LivestockVaccination
{
    FullyVaccinated = 1,

    PartiallyVaccinated = 2,

    NotVaccinated = 3,

    Unknown = 4
}

public enum LivestockProduction
{
    Milk = 1,

    Meat = 2,

    MilkAndMeat = 3,

    Leather = 4,

    None = 5
}
