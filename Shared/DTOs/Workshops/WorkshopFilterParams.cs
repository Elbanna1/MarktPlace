using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Workshops;

public class WorkshopFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public WorkshopType? WorkshopType { get; set; }

    public string? City { get; set; }
}
