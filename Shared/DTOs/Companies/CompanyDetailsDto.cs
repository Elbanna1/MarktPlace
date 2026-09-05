using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Companies;

public class CompanyDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string CompanyName { get; set; } = default!;
    public CompanyField CompanyField { get; set; }

    public string CompanyFieldName { get; set; } = default!;

    public string CompanyFieldGroup { get; set; } = default!;

    public string? OtherCompanyField { get; set; }

    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;
    public string? Email { get; set; }
    public string? Website { get; set; }

    public string? LogoUrl { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<CompanyImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Company;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
