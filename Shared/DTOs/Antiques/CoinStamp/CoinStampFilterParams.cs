using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Antiques;

public class CoinStampFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public CoinStampItemType? ItemType { get; set; }

    public string? Country { get; set; }

    public int? IssueYear { get; set; }

    public bool? IsOriginal { get; set; }

    public bool? IsRare { get; set; }

    public CoinStampCondition? Condition { get; set; }

    public bool? Negotiable { get; set; }

    public string? Center { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public AntiqueSortBy SortBy { get; set; } = AntiqueSortBy.Newest;
}
