using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Suppliers;

public class SupplierDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string SupplierName { get; set; } = default!;
    public SupplierSpecialization SupplierType { get; set; }

    public string SupplierTypeName { get; set; } = default!;

    public string SupplierTypeGroup { get; set; } = default!;
    public string? OtherSupplierType { get; set; }

    public string SuppliedProduct { get; set; } = default!;
    public string SupplyDetails { get; set; } = default!;

    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;
    public string? Email { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<SupplierImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Supplier;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
