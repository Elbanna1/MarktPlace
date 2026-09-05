using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class LivestockFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public LivestockBreed? Breed { get; set; }

    public LivestockPurpose? Purpose { get; set; }

    public LivestockAge? Age { get; set; }

    public LivestockGender? Gender { get; set; }

    public LivestockHealthStatus? HealthStatus { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
