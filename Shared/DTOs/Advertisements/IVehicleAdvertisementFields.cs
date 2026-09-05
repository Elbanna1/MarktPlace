using Shared.Enums;

namespace Shared.DTOs.Advertisements;

public interface IVehicleAdvertisementFields
{
    int SubCategoryId { get; }

    ListingType ListingType { get; }

    string? Brand { get; }
    string? OtherBrand { get; }
    string? Model { get; }
    int ManufacturingYear { get; }
    string? Color { get; }
    string? OtherColor { get; }
    int? Kilometers { get; }
    TransmissionType? Transmission { get; }
    FuelType? FuelType { get; }
    VehicleCondition? Condition { get; }
    TechnicalCondition? TechnicalCondition { get; }
    int? EngineCC { get; }
    VehicleOriginCountry? OriginCountry { get; }
    VehicleAssemblyCountry? AssemblyCountry { get; }
    bool? FirstOwner { get; }
    PreviousOwnersCount? PreviousOwners { get; }
    VehicleUsageType? UsageType { get; }
    bool? PartsChanged { get; }
    IReadOnlyList<ChangedVehiclePart>? ChangedParts { get; }
    AccidentsCount? AccidentsCount { get; }
    bool? HasMaintenanceBook { get; }
    bool? AllMaintenanceAtDealer { get; }

    BodyType? BodyType { get; }
    int? DoorsCount { get; }
    int? SeatsCount { get; }

    LicenseStatus? LicenseStatus { get; }
    DateTime? LicenseIssueDate { get; }
    DateTime? LicenseExpiryDate { get; }
    bool? LicenseInOwnerName { get; }
    bool? LicenseTransferable { get; }

    bool? IsInsured { get; }
    InsuranceType? InsuranceType { get; }
    DateTime? InsuranceExpiryDate { get; }

    bool? InspectionAllowed { get; }
    string? InspectionLocation { get; }
    bool? ServiceCenterInspection { get; }

    bool? InstallmentsAccepted { get; }
    decimal? DownPayment { get; }
    int? InstallmentMonths { get; }
    decimal? MonthlyInstallment { get; }

    MotorcycleType? MotorcycleType { get; }
    CoolingType? CoolingType { get; }
    MotorcycleStartType? StartType { get; }
    bool? EngineChanged { get; }
    bool? ChassisChanged { get; }

    EquipmentMachineType? MachineType { get; }
    string? OtherMachineType { get; }
    int? WorkingHours { get; }
    decimal? PowerValue { get; }
    PowerUnit? PowerUnit { get; }
    decimal? OperatingWeightTons { get; }
    string? BucketCapacity { get; }
    EquipmentDriveSystem? DriveSystem { get; }
    bool? EngineOverhauled { get; }
    IReadOnlyList<EquipmentUsageField>? UsageFields { get; }
    bool? CurrentlyWorking { get; }
    bool? ReadyToWork { get; }

    TaxiVehicleType? VehicleType { get; }
    string? OtherVehicleType { get; }
    int? PassengersCount { get; }
    TaxiActivityType? ActivityType { get; }
    string? Route { get; }
    OperatingLicenseStatus? OperatingLicenseStatus { get; }
    DateTime? OperatingLicenseExpiryDate { get; }

    IReadOnlyList<RentSystem>? RentSystems { get; }
    decimal? HourlyPrice { get; }
    decimal? DailyPrice { get; }
    decimal? WeeklyPrice { get; }
    decimal? MonthlyPrice { get; }
    int? MinimumRentPeriod { get; }
    int? MaximumRentPeriod { get; }
    bool? DriverIncluded { get; }
    bool? FuelIncluded { get; }
    bool? HasRefundableDeposit { get; }
    decimal? DepositAmount { get; }
    int? MaximumDistanceKm { get; }
    bool? HasHelmet { get; }
    bool? HasInsurance { get; }

    DamageLevel? DamageLevel { get; }
    bool? IsMoving { get; }
    bool? EngineWorks { get; }
    bool? GearboxWorks { get; }
    bool? ChassisIntact { get; }
    bool? AirbagsDeployed { get; }
    bool? HydraulicSystemWorks { get; }
    bool? SellAsParts { get; }
    string? ConditionReport { get; }

    InterestedIn? InterestedIn { get; }
    bool? DifferencePayment { get; }
    decimal? DifferenceAmount { get; }
    bool? AcceptsHigherPriced { get; }
    bool? AcceptsLowerPriced { get; }
    string? ExchangeDetails { get; }

    string? DetailedAddress { get; }
}
