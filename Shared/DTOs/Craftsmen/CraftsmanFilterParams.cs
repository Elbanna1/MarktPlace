using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Craftsmen;

public class CraftsmanFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public CraftsmanSpecialization? Specialization { get; set; }

    public ExperienceLevel? ExperienceLevel { get; set; }

    public string? City { get; set; }
}
