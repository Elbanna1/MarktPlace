namespace Shared.DTOs.Profile;

public class ProfileStatisticsDto
{
    public int TotalListings { get; set; }

    public int ActiveListings { get; set; }

    public int ExpiredListings { get; set; }

    public int PendingListings { get; set; }

    public int RejectedListings { get; set; }

    public int TotalViews { get; set; }

    public int TotalFavorites { get; set; }

    public IReadOnlyList<ListingTypeStatisticsDto> ByType { get; set; } = Array.Empty<ListingTypeStatisticsDto>();
}

public class ListingTypeStatisticsDto
{
    public string Type { get; set; } = default!;

    public int Total { get; set; }

    public int Active { get; set; }

    public int Expired { get; set; }

    public int Pending { get; set; }

    public int Rejected { get; set; }
}
