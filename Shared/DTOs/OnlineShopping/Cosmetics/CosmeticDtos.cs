using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.OnlineShopping;

public class CosmeticImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class CreateCosmeticRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public CosmeticSection Section { get; set; }

    public string? OtherSection { get; set; }

    public string Brand { get; set; } = default!;

    public CosmeticSuitableFor SuitableFor { get; set; }

    public decimal Price { get; set; }

    public bool DiscountAvailable { get; set; }

    public bool ShippingAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class UpdateCosmeticRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public CosmeticSection Section { get; set; }

    public string? OtherSection { get; set; }

    public string Brand { get; set; } = default!;

    public CosmeticSuitableFor SuitableFor { get; set; }

    public decimal Price { get; set; }

    public bool DiscountAvailable { get; set; }

    public bool ShippingAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public List<Guid> RemoveImageIds { get; set; } = new();

    public IFormFile? Video { get; set; }

    public bool RemoveVideo { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class CosmeticDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public CosmeticSection Section { get; set; }

    public string SectionName { get; set; } = default!;
    public string? OtherSection { get; set; }

    public string Brand { get; set; } = default!;

    public CosmeticSuitableFor SuitableFor { get; set; }

    public string SuitableForName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool DiscountAvailable { get; set; }
    public bool ShippingAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<CosmeticImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Cosmetic;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class CosmeticListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public CosmeticSection Section { get; set; }
    public string SectionName { get; set; } = default!;
    public string? OtherSection { get; set; }

    public string Brand { get; set; } = default!;

    public CosmeticSuitableFor SuitableFor { get; set; }
    public string SuitableForName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool DiscountAvailable { get; set; }
    public bool ShippingAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<CosmeticImageDto> Images { get; set; } = new();
    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Cosmetic;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class CosmeticFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? StoreName { get; set; }

    public CosmeticSection? Section { get; set; }

    public string? Brand { get; set; }

    public CosmeticSuitableFor? SuitableFor { get; set; }

    public bool? DiscountAvailable { get; set; }

    public bool? ShippingAvailable { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public OnlineShoppingSortBy SortBy { get; set; } = OnlineShoppingSortBy.Newest;
}
