using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class HorseFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public HorseBreed? Breed { get; set; }

    public HorsePurpose? Purpose { get; set; }

    public HorseAge? Age { get; set; }

    public HorseGender? Gender { get; set; }

    public HorseHealthStatus? HealthStatus { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
