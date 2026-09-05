using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Antiques;

public class HandmadeFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public HandmadeType? HandmadeType { get; set; }

    public bool? IsFullyHandmade { get; set; }

    public bool? CustomOrder { get; set; }

    public HandmadeColor? Color { get; set; }

    public bool? Negotiable { get; set; }

    public string? Center { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public AntiqueSortBy SortBy { get; set; } = AntiqueSortBy.Newest;
}
