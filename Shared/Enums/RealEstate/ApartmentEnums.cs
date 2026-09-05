namespace Shared.Enums;

public enum ApartmentType
{
    Residential = 1,

    Administrative = 2,

    Studio = 3,

    Duplex = 4,

    Penthouse = 5,

    Roof = 6,

    Other = 7
}

public enum ApartmentOwnershipType
{
    Freehold = 1,

    Government = 2,

    Private = 3,

    OwnersAssociation = 4,

    SocialHousing = 5,

    DistinguishedHousing = 6,

    Compound = 7,

    Other = 8
}

public enum ApartmentReceptionPieces
{
    One = 1,

    Two = 2,

    Three = 3,

    Four = 4,

    Open = 5
}

public enum ApartmentFloorType
{
    Ground = 1,

    Repeated = 2,

    Last = 3,

    Roof = 4,

    Basement = 5,

    Mezzanine = 6
}

public enum ApartmentFurnishedStatus
{
    Yes = 1,

    No = 2,

    SemiFurnished = 3
}

public enum ApartmentFinishingType
{
    SuperLux = 1,

    Lux = 2,

    Luxury = 3,

    SemiFinished = 4,

    RedBrick = 5,

    Unfinished = 6
}

public enum ApartmentPropertyAge
{
    New = 1,

    UnderFiveYears = 2,

    FiveToTenYears = 3,

    OverTenYears = 4
}

public enum ApartmentDirection
{
    North = 1,

    South = 2,

    East = 3,

    West = 4,

    Corner = 5
}

public enum ApartmentViewType
{
    MainStreet = 1,

    SideStreet = 2,

    Garden = 3,

    Nile = 4,

    Sea = 5,

    Other = 6
}

public enum ApartmentLegalStatus
{
    Licensed = 1,

    Reconciliation = 2,

    Unlicensed = 3
}

public enum ApartmentReconciliationForm
{
    Form1 = 1,

    Form3 = 2,

    Form8 = 3,

    Form10 = 4,

    FinalForm = 5,

    Other = 6
}

public enum ApartmentOwnershipDocument
{
    FinalContract = 1,

    GreenContract = 2,

    PreliminaryContract = 3,

    PowerOfAttorney = 4,

    Other = 5
}

public enum ApartmentFeature
{
    Elevator = 1,

    Garage = 2,

    Security = 3,

    SurveillanceCameras = 4,

    NaturalGas = 5,

    ElectricityMeter = 6,

    WaterMeter = 7,

    Internet = 8,

    AirConditioning = 9,

    Kitchen = 10,

    Balcony = 11,

    DressingRoom = 12,

    LaundryRoom = 13,

    Storage = 14,

    Generator = 15,

    Garden = 16,

    SwimmingPool = 17,

    Club = 18,

    Gym = 19,

    PrivateEntrance = 20,

    ArmoredDoor = 21,

    Intercom = 22,

    CentralSatellite = 23,

    WaterTanks = 24,

    SolarPanels = 25
}

public enum ApartmentPaymentMethod
{
    Cash = 1,

    Installments = 2,

    CashOrInstallments = 3
}

public enum ApartmentInstallmentProvider
{
    Owner = 1,

    Company = 2,

    Bank = 3
}

public enum ApartmentRentType
{
    Daily = 1,

    Weekly = 2,

    Monthly = 3,

    Yearly = 4
}

public enum ApartmentRentInclusion
{
    Electricity = 1,

    Water = 2,

    Gas = 3,

    Internet = 4,

    Maintenance = 5
}

public enum ApartmentSuitableFor
{
    Individuals = 1,

    Families = 2,

    Students = 3,

    Companies = 4
}

public enum ApartmentExchangeWith
{
    Apartment = 1,

    Villa = 2,

    Land = 3,

    Shop = 4,

    Car = 5,

    Other = 6
}
