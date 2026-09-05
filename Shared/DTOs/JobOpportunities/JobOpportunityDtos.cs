using System.Text.Json.Serialization;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.JobOpportunities;

public class JobOpportunityImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class JobOpportunityDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string EmployerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public string JobTitle { get; set; } = default!;

    public JobField JobField { get; set; }

    public string JobFieldName { get; set; } = default!;

    public string JobFieldGroup { get; set; } = default!;

    public string JobFieldGroupAr { get; set; } = default!;
    public string? OtherJobField { get; set; }

    public JobExperienceLevel RequiredExperience { get; set; }

    public string RequiredExperienceName { get; set; } = default!;

    public WorkType WorkType { get; set; }

    public string WorkTypeName { get; set; } = default!;

    public SalaryType SalaryType { get; set; }

    public string SalaryTypeName { get; set; } = default!;

    public decimal? Salary { get; set; }

    public string Governorate { get; set; } = default!;

    public string? Center { get; set; }
    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string? LogoUrl { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<JobOpportunityImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.JobOpportunity;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class JobOpportunityListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string EmployerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public string JobTitle { get; set; } = default!;

    public JobField JobField { get; set; }
    public string JobFieldName { get; set; } = default!;
    public string JobFieldGroup { get; set; } = default!;
    public string JobFieldGroupAr { get; set; } = default!;
    public string? OtherJobField { get; set; }

    public JobExperienceLevel RequiredExperience { get; set; }
    public string RequiredExperienceName { get; set; } = default!;

    public WorkType WorkType { get; set; }
    public string WorkTypeName { get; set; } = default!;

    public SalaryType SalaryType { get; set; }
    public string SalaryTypeName { get; set; } = default!;
    public decimal? Salary { get; set; }

    public string Governorate { get; set; } = default!;

    public string? Center { get; set; }

    public string? LogoUrl { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<JobOpportunityImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.JobOpportunity;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class JobOpportunityFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? JobTitle { get; set; }

    public JobField? JobField { get; set; }

    public WorkType? WorkType { get; set; }

    public JobExperienceLevel? RequiredExperience { get; set; }

    public string? Center { get; set; }
}

public class WorkTypeDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}

public class SalaryTypeDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameEn { get; set; } = default!;
}
