using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class BirdFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public BirdType? AnimalType { get; set; }

    public BirdPurpose? Purpose { get; set; }

    public BirdAge? Age { get; set; }

    public BirdGender? Gender { get; set; }

    public BirdHealthStatus? HealthStatus { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
