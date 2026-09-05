using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.JobRequests;

public class JobRequestFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public JobField? JobField { get; set; }

    public JobExperienceLevel? Experience { get; set; }

    public EducationLevel? Education { get; set; }

    public string? Center { get; set; }
}
