using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class OtherAnimalFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public OtherAnimalType? AnimalType { get; set; }

    public OtherAnimalPurpose? Purpose { get; set; }

    public OtherAnimalAge? Age { get; set; }

    public OtherAnimalGender? Gender { get; set; }

    public OtherAnimalHealthStatus? HealthStatus { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
