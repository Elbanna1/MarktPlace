using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Companies;

public class CompanyFilterParams : PaginationParams
{
    public string? CompanyName { get; set; }

    public CompanyField? CompanyField { get; set; }

    public string? Search { get; set; }
}
