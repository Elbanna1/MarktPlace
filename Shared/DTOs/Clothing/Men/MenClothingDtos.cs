using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Clothing;

public class MenClothingImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class CreateMenClothingRequest
{
    public string StoreName { get; set; } = default!;

    public MenClothingSellingMethod SellingMethod { get; set; }

    public MenClothingType ClothingType { get; set; }

    public string? OtherClothingType { get; set; }

    public MenClothingBrand Brand { get; set; }

    public string? OtherBrand { get; set; }

    public List<MenClothingSize> Sizes { get; set; } = new();

    public List<MenClothingColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public MenClothingCondition Condition { get; set; }

    public decimal Price { get; set; }

    public bool DeliveryAvailable { get; set; }

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class UpdateMenClothingRequest
{
    public string StoreName { get; set; } = default!;

    public MenClothingSellingMethod SellingMethod { get; set; }

    public MenClothingType ClothingType { get; set; }

    public string? OtherClothingType { get; set; }

    public MenClothingBrand Brand { get; set; }

    public string? OtherBrand { get; set; }

    public List<MenClothingSize> Sizes { get; set; } = new();

    public List<MenClothingColor> Colors { get; set; } = new();

    public string? OtherColor { get; set; }

    public MenClothingCondition Condition { get; set; }

    public decimal Price { get; set; }

    public bool DeliveryAvailable { get; set; }

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string? Email { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public List<Guid> RemoveImageIds { get; set; } = new();

    public IFormFile? Video { get; set; }

    public bool RemoveVideo { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class MenClothingDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string StoreName { get; set; } = default!;

    public MenClothingSellingMethod SellingMethod { get; set; }

    public string SellingMethodName { get; set; } = default!;

    public MenClothingType ClothingType { get; set; }

    public string ClothingTypeName { get; set; } = default!;
    public string? OtherClothingType { get; set; }

    public MenClothingBrand Brand { get; set; }

    public string BrandName { get; set; } = default!;
    public string? OtherBrand { get; set; }

    public List<ClothingLookupItemDto> Sizes { get; set; } = new();

    public List<ClothingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public MenClothingCondition Condition { get; set; }

    public string ConditionName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool DeliveryAvailable { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;
    public string? Email { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<MenClothingImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.MenClothing;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class MenClothingListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string StoreName { get; set; } = default!;

    public MenClothingSellingMethod SellingMethod { get; set; }
    public string SellingMethodName { get; set; } = default!;

    public MenClothingType ClothingType { get; set; }
    public string ClothingTypeName { get; set; } = default!;
    public string? OtherClothingType { get; set; }

    public MenClothingBrand Brand { get; set; }
    public string BrandName { get; set; } = default!;
    public string? OtherBrand { get; set; }

    public List<ClothingLookupItemDto> Sizes { get; set; } = new();
    public List<ClothingLookupItemDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public MenClothingCondition Condition { get; set; }
    public string ConditionName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool DeliveryAvailable { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<MenClothingImageDto> Images { get; set; } = new();
    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.MenClothing;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class MenClothingFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? StoreName { get; set; }

    public MenClothingType? ClothingType { get; set; }

    public MenClothingBrand? Brand { get; set; }

    public MenClothingSize? Size { get; set; }

    public MenClothingColor? Color { get; set; }

    public MenClothingSellingMethod? SellingMethod { get; set; }

    public string? Center { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }
}
