namespace Shared.Enums;

public enum TransmissionType
{
    Manual = 1,

    Automatic = 2,

    SemiAutomatic = 3
}

public enum FuelType
{
    Gasoline = 1,

    Diesel = 2,

    Electric = 3,

    Hybrid = 4,

    NaturalGas = 5
}

public enum VehicleCondition
{
    New = 1,

    Used = 2
}

public enum TechnicalCondition
{
    Excellent = 1,

    VeryGood = 2,

    Good = 3,

    NeedsMaintenance = 4
}

public enum BodyType
{
    Sedan = 1,

    SUV = 2,

    Hatchback = 3,

    Coupe = 4,

    Van = 5,

    Crossover = 6,

    Pickup = 7,

    Convertible = 8
}

public enum CoolingType
{
    Air = 1,

    Water = 2,

    Oil = 3
}

public enum VehicleOriginCountry
{
    Japan = 1,

    Korea = 2,

    Germany = 3,

    America = 4,

    France = 5,

    Italy = 6,

    China = 7,

    India = 8,

    Other = 9
}

public enum VehicleAssemblyCountry
{
    Egypt = 1,

    Japan = 2,

    Korea = 3,

    Germany = 4,

    China = 5,

    India = 6,

    America = 7,

    Other = 8
}

public enum PreviousOwnersCount
{
    First = 1,

    Second = 2,

    Third = 3,

    MoreThanThree = 4
}

public enum VehicleUsageType
{
    Personal = 1,

    Uber = 2,

    Careem = 3,

    Company = 4,

    Other = 5
}

public enum AccidentsCount
{
    None = 1,

    One = 2,

    Two = 3,

    More = 4
}

public enum LicenseStatus
{
    Valid = 1,

    Expired = 2,

    Unlicensed = 3,

    NotRequired = 4
}

public enum OperatingLicenseStatus
{
    Valid = 1,

    Expired = 2,

    None = 3
}

public enum InsuranceType
{
    Comprehensive = 1,

    ThirdParty = 2
}

public enum MotorcycleType
{
    Motorcycle = 1,

    Scooter = 2,

    Sport = 3,

    Cruiser = 4,

    OffRoad = 5,

    Trail = 6,

    ATV = 7,

    Other = 8
}

public enum MotorcycleStartType
{
    Electric = 1,

    Kick = 2,

    Both = 3
}

public enum EquipmentMachineType
{
    Loader = 1,

    Excavator = 2,

    Bulldozer = 3,

    Grader = 4,

    Crane = 5,

    Forklift = 6,

    Backhoe = 7,

    ConcreteMixer = 8,

    ConcretePump = 9,

    Dumper = 10,

    Tractor = 11,

    Roller = 12,

    TelescopicHandler = 13,

    Other = 14
}

public enum EquipmentDriveSystem
{
    Wheels = 1,

    Tracks = 2
}

public enum PowerUnit
{
    Horsepower = 1,

    Kilowatt = 2
}

public enum TaxiVehicleType
{
    Taxi = 1,

    Microbus = 2,

    TouristMicrobus = 3,

    Service = 4,

    Limousine = 5,

    PassengerVan = 6,

    MiniBus = 7,

    Other = 8
}

public enum TaxiActivityType
{
    Taxi = 1,

    Service = 2,

    Microbus = 3,

    Limousine = 4,

    EmployeeTransport = 5,

    SchoolTransport = 6,

    Tourism = 7,

    Other = 8
}

[Flags]
public enum ChangedVehiclePart
{
    Engine = 1,

    Gearbox = 2,

    Chassis = 4,

    Hood = 8,

    Trunk = 16,

    Fender = 32,

    Door = 64,

    FrontBumper = 128,

    RearBumper = 256,

    Bumper = 512,

    Other = 1024
}

[Flags]
public enum EquipmentUsageField
{
    Construction = 1,

    RoadsAndBridges = 2,

    Agriculture = 4,

    Quarries = 8,

    Factories = 16,

    Transport = 32,

    Contracting = 64,

    Other = 128
}

[Flags]
public enum RentSystem
{
    Hourly = 1,

    Daily = 2,

    Weekly = 4,

    Monthly = 8
}

[Flags]
public enum VehicleFeatureScope
{
    Private = 1,

    Taxi = 2,

    Motorcycles = 4,

    HeavyEquipment = 8
}
