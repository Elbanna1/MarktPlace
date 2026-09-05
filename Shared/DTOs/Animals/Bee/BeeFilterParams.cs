using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class BeeFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public BeeType? AnimalType { get; set; }

    public BeePurpose? Purpose { get; set; }

    public BeeHealthStatus? HealthStatus { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
