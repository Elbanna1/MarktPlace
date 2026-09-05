using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class SheepGoatFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public SheepGoatBreed? Breed { get; set; }

    public SheepGoatPurpose? Purpose { get; set; }

    public SheepGoatAge? Age { get; set; }

    public SheepGoatGender? Gender { get; set; }

    public SheepGoatHealthStatus? HealthStatus { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
