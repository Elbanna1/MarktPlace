using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.JobRequests;

public class JobRequestListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string ApplicantName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public JobField JobField { get; set; }

    public string JobFieldName { get; set; } = default!;

    public string JobFieldGroup { get; set; } = default!;

    public string JobFieldGroupAr { get; set; } = default!;
    public string? OtherJobField { get; set; }

    public JobExperienceLevel Experience { get; set; }

    public string ExperienceName { get; set; } = default!;

    public EducationLevel Education { get; set; }

    public string EducationName { get; set; } = default!;

    public string Skills { get; set; } = default!;

    public string Governorate { get; set; } = default!;

    public string? Center { get; set; }

    public string? ProfileImageUrl { get; set; }

    public string? CvFileUrl { get; set; }

    public string? IntroVideoUrl { get; set; }

    public bool HasIntroVideo { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.JobRequest;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
