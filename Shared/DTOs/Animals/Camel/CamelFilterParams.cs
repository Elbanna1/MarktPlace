using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class CamelFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public CamelBreed? Breed { get; set; }

    public CamelPurpose? Purpose { get; set; }

    public CamelAge? Age { get; set; }

    public CamelGender? Gender { get; set; }

    public CamelHealthStatus? HealthStatus { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
