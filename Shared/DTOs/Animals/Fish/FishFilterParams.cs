using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class FishFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public FishType? AnimalType { get; set; }

    public FishPurpose? Purpose { get; set; }

    public FishAge? Age { get; set; }

    public FishHealthStatus? HealthStatus { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
