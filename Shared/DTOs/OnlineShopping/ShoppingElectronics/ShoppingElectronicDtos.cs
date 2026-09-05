using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.OnlineShopping;

public class ShoppingElectronicImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class CreateShoppingElectronicRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public ShoppingElectronicSection Section { get; set; }

    public string? OtherSection { get; set; }

    public string Brand { get; set; } = default!;

    public ShoppingElectronicCompatibility CompatibleWith { get; set; }

    public ShoppingElectronicCondition ProductCondition { get; set; }

    public ShoppingElectronicWarranty Warranty { get; set; }

    public decimal Price { get; set; }

    public bool ShippingAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class UpdateShoppingElectronicRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public ShoppingElectronicSection Section { get; set; }

    public string? OtherSection { get; set; }

    public string Brand { get; set; } = default!;

    public ShoppingElectronicCompatibility CompatibleWith { get; set; }

    public ShoppingElectronicCondition ProductCondition { get; set; }

    public ShoppingElectronicWarranty Warranty { get; set; }

    public decimal Price { get; set; }

    public bool ShippingAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public List<Guid> RemoveImageIds { get; set; } = new();

    public IFormFile? Video { get; set; }

    public bool RemoveVideo { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class ShoppingElectronicDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public ShoppingElectronicSection Section { get; set; }

    public string SectionName { get; set; } = default!;
    public string? OtherSection { get; set; }

    public string Brand { get; set; } = default!;

    public ShoppingElectronicCompatibility CompatibleWith { get; set; }

    public string CompatibleWithName { get; set; } = default!;

    public ShoppingElectronicCondition ProductCondition { get; set; }

    public string ProductConditionName { get; set; } = default!;

    public ShoppingElectronicWarranty Warranty { get; set; }

    public string WarrantyName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool ShippingAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<ShoppingElectronicImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.ShoppingElectronic;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class ShoppingElectronicListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public ShoppingElectronicSection Section { get; set; }
    public string SectionName { get; set; } = default!;
    public string? OtherSection { get; set; }

    public string Brand { get; set; } = default!;

    public ShoppingElectronicCompatibility CompatibleWith { get; set; }
    public string CompatibleWithName { get; set; } = default!;

    public ShoppingElectronicCondition ProductCondition { get; set; }
    public string ProductConditionName { get; set; } = default!;

    public ShoppingElectronicWarranty Warranty { get; set; }
    public string WarrantyName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool ShippingAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<ShoppingElectronicImageDto> Images { get; set; } = new();
    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.ShoppingElectronic;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class ShoppingElectronicFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? StoreName { get; set; }

    public ShoppingElectronicSection? Section { get; set; }

    public string? Brand { get; set; }

    public ShoppingElectronicCompatibility? CompatibleWith { get; set; }

    public ShoppingElectronicCondition? ProductCondition { get; set; }

    public ShoppingElectronicWarranty? Warranty { get; set; }

    public bool? ShippingAvailable { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public OnlineShoppingSortBy SortBy { get; set; } = OnlineShoppingSortBy.Newest;
}
