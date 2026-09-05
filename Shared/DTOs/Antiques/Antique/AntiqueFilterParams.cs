using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Antiques;

public class AntiqueFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public AntiqueType? AntiqueType { get; set; }

    public int? ManufactureYear { get; set; }

    public string? CountryOfOrigin { get; set; }

    public AntiqueOriginality? Originality { get; set; }

    public AntiqueCondition? Condition { get; set; }

    public bool? Negotiable { get; set; }

    public string? Center { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public AntiqueSortBy SortBy { get; set; } = AntiqueSortBy.Newest;
}
