using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.OnlineShopping;

public class GiftToyImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class CreateGiftToyRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public GiftToyType GiftType { get; set; }

    public string? OtherGiftType { get; set; }

    public GiftToySuitableFor SuitableFor { get; set; }

    public bool GiftWrapping { get; set; }

    public decimal Price { get; set; }

    public bool DeliveryAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class UpdateGiftToyRequest
{
    public string StoreName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public GiftToyType GiftType { get; set; }

    public string? OtherGiftType { get; set; }

    public GiftToySuitableFor SuitableFor { get; set; }

    public bool GiftWrapping { get; set; }

    public decimal Price { get; set; }

    public bool DeliveryAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public List<Guid> RemoveImageIds { get; set; } = new();

    public IFormFile? Video { get; set; }

    public bool RemoveVideo { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class GiftToyDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public GiftToyType GiftType { get; set; }

    public string GiftTypeName { get; set; } = default!;
    public string? OtherGiftType { get; set; }

    public GiftToySuitableFor SuitableFor { get; set; }

    public string SuitableForName { get; set; } = default!;

    public bool GiftWrapping { get; set; }

    public decimal Price { get; set; }
    public bool DeliveryAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<GiftToyImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.GiftToy;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class GiftToyListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string StoreName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public GiftToyType GiftType { get; set; }
    public string GiftTypeName { get; set; } = default!;
    public string? OtherGiftType { get; set; }

    public GiftToySuitableFor SuitableFor { get; set; }
    public string SuitableForName { get; set; } = default!;

    public bool GiftWrapping { get; set; }

    public decimal Price { get; set; }
    public bool DeliveryAvailable { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<GiftToyImageDto> Images { get; set; } = new();
    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.GiftToy;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class GiftToyFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? StoreName { get; set; }

    public GiftToyType? GiftType { get; set; }

    public GiftToySuitableFor? SuitableFor { get; set; }

    public bool? GiftWrapping { get; set; }

    public bool? DeliveryAvailable { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public OnlineShoppingSortBy SortBy { get; set; } = OnlineShoppingSortBy.Newest;
}
