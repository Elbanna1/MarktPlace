using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.OnlineShopping;

public class HomemadeFoodImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class CreateHomemadeFoodRequest
{
    public string ProjectName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public HomemadeFoodSection Section { get; set; }

    public string? OtherSection { get; set; }

    public bool PreparedOnDemand { get; set; }

    public int MinimumOrderQuantity { get; set; }

    public string PreparationTime { get; set; } = default!;

    public bool DeliveryAvailable { get; set; }

    public List<HomemadeFoodDeliveryArea> DeliveryAreas { get; set; } = new();

    public decimal Price { get; set; }

    public string? Ingredients { get; set; }

    public string? WeightOrSize { get; set; }

    public string? StorageMethod { get; set; }

    public string? AvailableOrderingHours { get; set; }

    public string? AdditionalNotes { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class UpdateHomemadeFoodRequest
{
    public string ProjectName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public HomemadeFoodSection Section { get; set; }

    public string? OtherSection { get; set; }

    public bool PreparedOnDemand { get; set; }

    public int MinimumOrderQuantity { get; set; }

    public string PreparationTime { get; set; } = default!;

    public bool DeliveryAvailable { get; set; }

    public List<HomemadeFoodDeliveryArea> DeliveryAreas { get; set; } = new();

    public decimal Price { get; set; }

    public string? Ingredients { get; set; }

    public string? WeightOrSize { get; set; }

    public string? StorageMethod { get; set; }

    public string? AvailableOrderingHours { get; set; }

    public string? AdditionalNotes { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public List<Guid> RemoveImageIds { get; set; } = new();

    public IFormFile? Video { get; set; }

    public bool RemoveVideo { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class HomemadeFoodDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string ProjectName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public HomemadeFoodSection Section { get; set; }

    public string SectionName { get; set; } = default!;
    public string? OtherSection { get; set; }

    public bool PreparedOnDemand { get; set; }
    public int MinimumOrderQuantity { get; set; }
    public string PreparationTime { get; set; } = default!;

    public bool DeliveryAvailable { get; set; }

    public List<OnlineShoppingLookupItemDto> DeliveryAreas { get; set; } = new();

    public decimal Price { get; set; }

    public string? Ingredients { get; set; }
    public string? WeightOrSize { get; set; }
    public string? StorageMethod { get; set; }
    public string? AvailableOrderingHours { get; set; }
    public string? AdditionalNotes { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<HomemadeFoodImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.HomemadeFood;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class HomemadeFoodListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string ProjectName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public HomemadeFoodSection Section { get; set; }
    public string SectionName { get; set; } = default!;
    public string? OtherSection { get; set; }

    public bool PreparedOnDemand { get; set; }
    public int MinimumOrderQuantity { get; set; }
    public string PreparationTime { get; set; } = default!;

    public bool DeliveryAvailable { get; set; }
    public List<OnlineShoppingLookupItemDto> DeliveryAreas { get; set; } = new();

    public decimal Price { get; set; }

    public string? Ingredients { get; set; }
    public string? WeightOrSize { get; set; }
    public string? StorageMethod { get; set; }
    public string? AvailableOrderingHours { get; set; }
    public string? AdditionalNotes { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<HomemadeFoodImageDto> Images { get; set; } = new();
    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.HomemadeFood;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class HomemadeFoodFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? ProjectName { get; set; }

    public HomemadeFoodSection? Section { get; set; }

    public bool? DeliveryAvailable { get; set; }

    public HomemadeFoodDeliveryArea? DeliveryArea { get; set; }

    public bool? PreparedOnDemand { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public OnlineShoppingSortBy SortBy { get; set; } = OnlineShoppingSortBy.Newest;
}
