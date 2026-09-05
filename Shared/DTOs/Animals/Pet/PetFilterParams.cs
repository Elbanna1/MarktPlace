using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class PetFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public PetBreed? Breed { get; set; }

    public PetPurpose? Purpose { get; set; }

    public PetAge? Age { get; set; }

    public PetGender? Gender { get; set; }

    public PetHealthStatus? HealthStatus { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
