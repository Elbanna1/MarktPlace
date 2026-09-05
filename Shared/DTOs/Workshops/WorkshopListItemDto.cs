using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Workshops;

public class WorkshopListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public WorkshopType WorkshopType { get; set; }

    public string WorkshopTypeName { get; set; } = default!;
    public string? OtherWorkshopType { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public string AdTitle { get; set; } = default!;
    public string AdDescription { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<WorkshopImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Workshop;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
