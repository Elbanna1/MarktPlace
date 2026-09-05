using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Workshops;

public class WorkshopDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string Name { get; set; } = default!;
    public WorkshopType WorkshopType { get; set; }

    public string WorkshopTypeName { get; set; } = default!;
    public string? OtherWorkshopType { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? GoogleMapsUrl { get; set; }

    public string PhoneNumber { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;
    public string? Email { get; set; }

    public string AdTitle { get; set; } = default!;
    public string AdDescription { get; set; } = default!;
    public List<WorkshopImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Workshop;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
