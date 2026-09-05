using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Farms;

public class FarmFilterParams : PaginationParams
{
    public string? FarmName { get; set; }

    public FarmType? FarmType { get; set; }

    public string? Search { get; set; }
}
