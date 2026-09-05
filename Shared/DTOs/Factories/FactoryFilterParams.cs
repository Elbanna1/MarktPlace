using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Factories;

public class FactoryFilterParams : PaginationParams
{
    public string? FactoryName { get; set; }

    public ProductionSpecialty? ProductionSpecialty { get; set; }

    public string? Search { get; set; }
}
