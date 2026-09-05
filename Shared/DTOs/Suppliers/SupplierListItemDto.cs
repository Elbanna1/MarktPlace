using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Suppliers;

public class SupplierListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string SupplierName { get; set; } = default!;
    public SupplierSpecialization SupplierType { get; set; }

    public string SupplierTypeName { get; set; } = default!;

    public string SupplierTypeGroup { get; set; } = default!;
    public string? OtherSupplierType { get; set; }

    public string SuppliedProduct { get; set; } = default!;
    public string SupplyDetails { get; set; } = default!;

    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<SupplierImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Supplier;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
