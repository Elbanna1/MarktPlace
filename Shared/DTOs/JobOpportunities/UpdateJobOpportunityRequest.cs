using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.JobOpportunities;

public class UpdateJobOpportunityRequest
{
    public string EmployerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string JobTitle { get; set; } = default!;

    public JobField JobField { get; set; }

    public string? OtherJobField { get; set; }

    public JobExperienceLevel RequiredExperience { get; set; }

    public WorkType WorkType { get; set; }

    public SalaryType SalaryType { get; set; }

    public decimal? Salary { get; set; }

    public string? Center { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public IFormFile? Logo { get; set; }

    public bool RemoveLogo { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public List<Guid> RemoveImageIds { get; set; } = new();

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}
