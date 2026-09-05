using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.FruitVegetableMerchants;

public class FruitVegetableMerchantFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? MerchantName { get; set; }

    public string? StallName { get; set; }

    public string? ProductName { get; set; }

    public MerchantSaleType? SaleType { get; set; }
}
