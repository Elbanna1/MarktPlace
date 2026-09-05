using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Advertisements;

public class AdvertisementFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public AdvertisementSortBy SortBy { get; set; } = AdvertisementSortBy.Newest;

    public int? CategoryId { get; set; }
    public int? SubCategoryId { get; set; }
    public ListingType? ListingType { get; set; }

    public string? Brand { get; set; }
    public string? Model { get; set; }

    public int? Year { get; set; }

    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }

    public string? Color { get; set; }

    public VehicleCondition? Condition { get; set; }
    public TechnicalCondition? TechnicalCondition { get; set; }
    public FuelType? FuelType { get; set; }
    public TransmissionType? Transmission { get; set; }
    public VehicleOriginCountry? OriginCountry { get; set; }
    public LicenseStatus? LicenseStatus { get; set; }

    public int? KilometersTo { get; set; }

    public BodyType? BodyType { get; set; }

    public MotorcycleType? MotorcycleType { get; set; }

    public EquipmentMachineType? MachineType { get; set; }

    public TaxiVehicleType? VehicleType { get; set; }

    public List<int>? FeatureIds { get; set; }

    public string? Center { get; set; }

    public bool? Negotiable { get; set; }

    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
}
