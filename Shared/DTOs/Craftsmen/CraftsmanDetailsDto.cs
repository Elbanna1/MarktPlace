using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Craftsmen;

public class CraftsmanDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string Name { get; set; } = default!;
    public CraftsmanSpecialization Specialization { get; set; }

    public string SpecializationName { get; set; } = default!;
    public string? OtherSpecialization { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }

    public string ExperienceLevelName { get; set; } = default!;

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? GoogleMapsUrl { get; set; }

    public string PhoneNumber { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;
    public string? Email { get; set; }

    public string AdTitle { get; set; } = default!;
    public string AdDescription { get; set; } = default!;
    public List<CraftsmanImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Craftsman;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
