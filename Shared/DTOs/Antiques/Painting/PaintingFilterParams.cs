using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Antiques;

public class PaintingFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? SellerName { get; set; }

    public PaintingType? PaintingType { get; set; }

    public string? ArtistName { get; set; }

    public PaintingOriginality? Originality { get; set; }

    public bool? Framed { get; set; }

    public bool? Negotiable { get; set; }

    public string? Center { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public AntiqueSortBy SortBy { get; set; } = AntiqueSortBy.Newest;
}
