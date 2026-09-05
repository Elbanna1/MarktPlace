using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.WholesaleTraders;

public class WholesaleTraderFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? TraderName { get; set; }

    public WholesaleTradeType? TradeType { get; set; }

    public string? ProductsName { get; set; }

    public WholesaleSaleType? SaleType { get; set; }
}
