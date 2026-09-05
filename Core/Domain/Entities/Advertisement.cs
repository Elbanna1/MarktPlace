using System.ComponentModel.DataAnnotations.Schema;
using Shared.Constants;
using Shared.Enums;

namespace Domain.Entities;

public class Advertisement : IModeratedListing
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public decimal? Price { get; set; }
    public bool Negotiable { get; set; }

    public ListingType ListingType { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;

    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; } = default!;

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public string OwnerId { get; set; } = default!;
    public ApplicationUser Owner { get; set; } = default!;

    public AdvertisementStatus Status { get; set; } = AdvertisementStatus.Active;
    public int Views { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? FirstPublishedAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public DateTime? ExpiredAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public int RepublishCount { get; set; }

    public bool IsExpiredAt(DateTime utcNow) =>
        Status == AdvertisementStatus.Expired ||
        (Status == AdvertisementStatus.Active && ExpireAt is { } end && end <= utcNow);

    public AdvertisementStatus EffectiveStatusAt(DateTime utcNow) =>
        Status == AdvertisementStatus.Active && ExpireAt is { } end && end <= utcNow
            ? AdvertisementStatus.Expired
            : Status;

    public int? RemainingDaysAt(DateTime utcNow)
    {
        if (ExpireAt is null)
            return null;

        if (IsExpiredAt(utcNow) || Status is AdvertisementStatus.Deleted)
            return 0;

        return ListingLifecycle.RemainingDays(ExpireAt, utcNow);
    }

    public string? DetailedAddress { get; set; }

    public string? VideoPath { get; set; }

    public string? VideoUrl { get; set; }

    public string Brand { get; set; } = default!;

    public string? OtherBrand { get; set; }

    public string Model { get; set; } = default!;
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

    public int? ChangedParts { get; set; }

    public AccidentsCount? AccidentsCount { get; set; }

    public bool? HasMaintenanceBook { get; set; }

    public bool? AllMaintenanceAtDealer { get; set; }

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

    public int? UsageFields { get; set; }

    public bool? CurrentlyWorking { get; set; }

    public bool? ReadyToWork { get; set; }

    public TaxiVehicleType? VehicleType { get; set; }

    public string? OtherVehicleType { get; set; }

    public int? PassengersCount { get; set; }

    public TaxiActivityType? ActivityType { get; set; }

    public string? Route { get; set; }

    public OperatingLicenseStatus? OperatingLicenseStatus { get; set; }

    public DateTime? OperatingLicenseExpiryDate { get; set; }

    public int? RentSystems { get; set; }

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

    public string? BusinessName { get; set; }

    public string? Address { get; set; }

    public string? GoogleMapsUrl { get; set; }

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public ProductionSpecialty? ProductionSpecialty { get; set; }
    public string? OtherProductionSpecialty { get; set; }

    public FarmType? FarmType { get; set; }
    public string? OtherFarmType { get; set; }
    public FarmingMethod? FarmingMethod { get; set; }
    public AvailabilitySeason? AvailabilitySeason { get; set; }

    public CompanyField? CompanyField { get; set; }
    public string? OtherCompanyField { get; set; }

    public SupplierType? SupplierType { get; set; }
    public string? OtherSupplierType { get; set; }

    public TradeType? TradeType { get; set; }
    public string? OtherTradeType { get; set; }

    public SaleType? SaleType { get; set; }

    public ICollection<AdvertisementImage> Images { get; set; } = new List<AdvertisementImage>();
    public ICollection<AdvertisementFeature> AdvertisementFeatures { get; set; } = new List<AdvertisementFeature>();
    public ICollection<AdvertisementView> AdvertisementViews { get; set; } = new List<AdvertisementView>();

    public ModerationStatus ModerationStatus { get; set; } = ModerationStatus.Pending;

    public DateTime? ModeratedAt { get; set; }

    public string? ModeratedBy { get; set; }

    public ListingRejectionReason? RejectionReason { get; set; }

    public string? ModerationNotes { get; set; }

    [NotMapped]
    public string OwnerUserId => OwnerId;

    [NotMapped]
    public string ListingTitle => Title;
}
