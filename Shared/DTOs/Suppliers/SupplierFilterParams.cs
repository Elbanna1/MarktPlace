using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Suppliers;

public class SupplierFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SupplierName { get; set; }

    public SupplierSpecialization? SupplierType { get; set; }

    public string? SuppliedProduct { get; set; }
}
