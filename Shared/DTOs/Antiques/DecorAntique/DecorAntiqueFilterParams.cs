using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Antiques;

public class DecorAntiqueFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public DecorAntiqueItemType? ItemType { get; set; }

    public DecorAntiqueMaterial? Material { get; set; }

    public DecorAntiqueCondition? Condition { get; set; }

    public DecorAntiqueOriginality? Originality { get; set; }

    public bool? Negotiable { get; set; }

    public string? Center { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public AntiqueSortBy SortBy { get; set; } = AntiqueSortBy.Newest;
}
