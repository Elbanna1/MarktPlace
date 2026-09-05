using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.JobRequests;

public class UpdateJobRequestRequest
{
    public string ApplicantName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public JobField JobField { get; set; }

    public string? OtherJobField { get; set; }

    public JobExperienceLevel Experience { get; set; }

    public EducationLevel Education { get; set; }

    public string Skills { get; set; } = default!;

    public string? Center { get; set; }

    public string Address { get; set; } = default!;

    public IFormFile? ProfileImage { get; set; }

    public bool RemoveProfileImage { get; set; }

    public IFormFile? CvFile { get; set; }

    public IFormFile? IntroVideo { get; set; }

    public bool RemoveIntroVideo { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}
