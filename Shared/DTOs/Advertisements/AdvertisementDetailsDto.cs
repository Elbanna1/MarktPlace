using Shared.DTOs.Listings;
using System.Text.Json.Serialization;
using Shared.Enums;

namespace Shared.DTOs.Advertisements;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "adType")]
[JsonDerivedType(typeof(CarDetailsDto), "car")]
public class AdvertisementDetailsDto : IListingViews
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal? Price { get; set; }
    public bool Negotiable { get; set; }
    public ListingType ListingType { get; set; }
    public AdvertisementStatus Status { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public int SubCategoryId { get; set; }
    public string SubCategoryName { get; set; } = default!;

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public string? DetailedAddress { get; set; }

    public string PhoneNumber { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public int FavoriteCount { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }

    public bool CanReport { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsExpired { get; set; }

    public List<AdvertisementImageDto> Images { get; set; } = new();
    public List<FeatureDto> Features { get; set; } = new();
    public OwnerDto Owner { get; set; } = default!;

    ListingModuleType IListingViews.ViewsListingType => ListingModuleType.Advertisement;

    Guid IListingViews.ViewsListingId => Id;
}

public class CarDetailsDto : AdvertisementDetailsDto
{
    public string? VideoUrl { get; set; }

    public string? Brand { get; set; }

    public string? OtherBrand { get; set; }

    public string? Model { get; set; }
    public int ManufacturingYear { get; set; }
    public string? Color { get; set; }

    public string? OtherColor { get; set; }

    public int? Kilometers { get; set; }
    public TransmissionType? Transmission { get; set; }
    public FuelType? FuelType { get; set; }
    public VehicleCondition? Condition { get; set; }
    public TechnicalCondition? TechnicalCondition { get; set; }
    public int? EngineCC { get; set; }
    public VehicleOriginCountry? OriginCountry { get; set; }
    public VehicleAssemblyCountry? AssemblyCountry { get; set; }
    public bool? FirstOwner { get; set; }
    public PreviousOwnersCount? PreviousOwners { get; set; }
    public VehicleUsageType? UsageType { get; set; }
    public bool? PartsChanged { get; set; }
    public AccidentsCount? AccidentsCount { get; set; }
    public bool? HasMaintenanceBook { get; set; }
    public bool? AllMaintenanceAtDealer { get; set; }

    public List<ChangedVehiclePart> ChangedParts { get; set; } = new();

    public List<string> ChangedPartNames { get; set; } = new();

    public BodyType? BodyType { get; set; }
    public int? DoorsCount { get; set; }
    public int? SeatsCount { get; set; }

    public LicenseStatus? LicenseStatus { get; set; }
    public DateTime? LicenseIssueDate { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public bool? LicenseInOwnerName { get; set; }
    public bool? LicenseTransferable { get; set; }

    public bool? IsInsured { get; set; }
    public InsuranceType? InsuranceType { get; set; }
    public DateTime? InsuranceExpiryDate { get; set; }

    public bool? InspectionAllowed { get; set; }
    public string? InspectionLocation { get; set; }
    public bool? ServiceCenterInspection { get; set; }

    public bool? InstallmentsAccepted { get; set; }
    public decimal? DownPayment { get; set; }
    public int? InstallmentMonths { get; set; }
    public decimal? MonthlyInstallment { get; set; }

    public MotorcycleType? MotorcycleType { get; set; }
    public CoolingType? CoolingType { get; set; }
    public MotorcycleStartType? StartType { get; set; }
    public bool? EngineChanged { get; set; }
    public bool? ChassisChanged { get; set; }

    public EquipmentMachineType? MachineType { get; set; }

    public string? OtherMachineType { get; set; }

    public int? WorkingHours { get; set; }
    public decimal? PowerValue { get; set; }
    public PowerUnit? PowerUnit { get; set; }
    public decimal? OperatingWeightTons { get; set; }
    public string? BucketCapacity { get; set; }
    public EquipmentDriveSystem? DriveSystem { get; set; }
    public bool? EngineOverhauled { get; set; }
    public bool? CurrentlyWorking { get; set; }
    public bool? ReadyToWork { get; set; }

    public List<EquipmentUsageField> UsageFields { get; set; } = new();

    public List<string> UsageFieldNames { get; set; } = new();

    public TaxiVehicleType? VehicleType { get; set; }

    public string? OtherVehicleType { get; set; }

    public int? PassengersCount { get; set; }
    public TaxiActivityType? ActivityType { get; set; }
    public string? Route { get; set; }
    public OperatingLicenseStatus? OperatingLicenseStatus { get; set; }
    public DateTime? OperatingLicenseExpiryDate { get; set; }

    public List<RentSystem> RentSystems { get; set; } = new();

    public List<string> RentSystemNames { get; set; } = new();

    public decimal? HourlyPrice { get; set; }
    public decimal? DailyPrice { get; set; }
    public decimal? WeeklyPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public int? MinimumRentPeriod { get; set; }
    public int? MaximumRentPeriod { get; set; }
    public bool? DriverIncluded { get; set; }
    public bool? FuelIncluded { get; set; }
    public bool? HasRefundableDeposit { get; set; }
    public decimal? DepositAmount { get; set; }
    public int? MaximumDistanceKm { get; set; }
    public bool? HasHelmet { get; set; }
    public bool? HasInsurance { get; set; }

    public DamageLevel? DamageLevel { get; set; }
    public bool? IsMoving { get; set; }
    public bool? EngineWorks { get; set; }
    public bool? GearboxWorks { get; set; }
    public bool? ChassisIntact { get; set; }
    public bool? AirbagsDeployed { get; set; }
    public bool? HydraulicSystemWorks { get; set; }
    public bool? SellAsParts { get; set; }
    public string? ConditionReport { get; set; }

    public InterestedIn? InterestedIn { get; set; }
    public bool? DifferencePayment { get; set; }
    public decimal? DifferenceAmount { get; set; }
    public bool? AcceptsHigherPriced { get; set; }
    public bool? AcceptsLowerPriced { get; set; }
    public string? ExchangeDetails { get; set; }
}
