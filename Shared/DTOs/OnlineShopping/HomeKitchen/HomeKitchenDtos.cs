using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.OnlineShopping;

public class HomeKitchenImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class CreateHomeKitchenRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public HomeKitchenSection Section { get; set; }

    public string? OtherSection { get; set; }

    public HomeKitchenMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public List<HomeKitchenColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public decimal Price { get; set; }

    public bool DeliveryAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class UpdateHomeKitchenRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public HomeKitchenSection Section { get; set; }

    public string? OtherSection { get; set; }

    public HomeKitchenMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public List<HomeKitchenColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public decimal Price { get; set; }

    public bool DeliveryAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public List<Guid> RemoveImageIds { get; set; } = new();

    public IFormFile? Video { get; set; }

    public bool RemoveVideo { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class HomeKitchenDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public HomeKitchenSection Section { get; set; }

    public string SectionName { get; set; } = default!;
    public string? OtherSection { get; set; }

    public HomeKitchenMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<OnlineShoppingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public decimal Price { get; set; }
    public bool DeliveryAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<HomeKitchenImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.HomeKitchen;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class HomeKitchenListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public HomeKitchenSection Section { get; set; }
    public string SectionName { get; set; } = default!;
    public string? OtherSection { get; set; }

    public HomeKitchenMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<OnlineShoppingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public decimal Price { get; set; }
    public bool DeliveryAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<HomeKitchenImageDto> Images { get; set; } = new();
    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.HomeKitchen;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class HomeKitchenFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? StoreName { get; set; }

    public HomeKitchenSection? Section { get; set; }

    public HomeKitchenMaterial? Material { get; set; }

    public HomeKitchenColor? Color { get; set; }

    public bool? DeliveryAvailable { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public OnlineShoppingSortBy SortBy { get; set; } = OnlineShoppingSortBy.Newest;
}
