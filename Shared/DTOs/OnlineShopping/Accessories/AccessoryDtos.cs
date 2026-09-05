using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.OnlineShopping;

public class AccessoryImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class CreateAccessoryRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public IFormFile? Logo { get; set; }

    public AccessoryType AccessoryType { get; set; }

    public string? OtherAccessoryType { get; set; }

    public AccessoryCategory Category { get; set; }

    public AccessoryMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public List<AccessoryColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public decimal Price { get; set; }

    public bool ShippingAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class UpdateAccessoryRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public IFormFile? Logo { get; set; }

    public bool RemoveLogo { get; set; }

    public AccessoryType AccessoryType { get; set; }

    public string? OtherAccessoryType { get; set; }

    public AccessoryCategory Category { get; set; }

    public AccessoryMaterial Material { get; set; }

    public string? OtherMaterial { get; set; }

    public List<AccessoryColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public decimal Price { get; set; }

    public bool ShippingAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public List<Guid> RemoveImageIds { get; set; } = new();

    public IFormFile? Video { get; set; }

    public bool RemoveVideo { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class AccessoryDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public string? LogoUrl { get; set; }

    public AccessoryType AccessoryType { get; set; }

    public string AccessoryTypeName { get; set; } = default!;
    public string? OtherAccessoryType { get; set; }

    public AccessoryCategory Category { get; set; }

    public string CategoryName { get; set; } = default!;

    public AccessoryMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<OnlineShoppingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public decimal Price { get; set; }
    public bool ShippingAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<AccessoryImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Accessory;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class AccessoryListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;
    public string? LogoUrl { get; set; }

    public AccessoryType AccessoryType { get; set; }
    public string AccessoryTypeName { get; set; } = default!;
    public string? OtherAccessoryType { get; set; }

    public AccessoryCategory Category { get; set; }
    public string CategoryName { get; set; } = default!;

    public AccessoryMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public List<OnlineShoppingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public decimal Price { get; set; }
    public bool ShippingAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<AccessoryImageDto> Images { get; set; } = new();
    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Accessory;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class AccessoryFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? StoreName { get; set; }

    public AccessoryType? AccessoryType { get; set; }

    public AccessoryCategory? Category { get; set; }

    public AccessoryMaterial? Material { get; set; }

    public AccessoryColor? Color { get; set; }

    public bool? ShippingAvailable { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public OnlineShoppingSortBy SortBy { get; set; } = OnlineShoppingSortBy.Newest;
}
