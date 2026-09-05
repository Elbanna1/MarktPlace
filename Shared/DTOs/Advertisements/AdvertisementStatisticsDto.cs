namespace Shared.DTOs.Advertisements;

public class AdvertisementStatisticsDto
{
    public int Total { get; set; }
    public int Active { get; set; }

    public int Expired { get; set; }

    public int Pending { get; set; }

    public int Deleted { get; set; }

    public int TotalViews { get; set; }

    public int CanRepublish { get; set; }

    public List<AdvertisementCategoryStatisticsDto> ByCategory { get; set; } = new();

    public List<AdvertisementSubCategoryStatisticsDto> BySubCategory { get; set; } = new();
}

public class AdvertisementCategoryStatisticsDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public string CategoryNameAr { get; set; } = default!;
    public int Total { get; set; }
    public int Active { get; set; }
    public int Expired { get; set; }
}

public class AdvertisementSubCategoryStatisticsDto
{
    public int SubCategoryId { get; set; }
    public string SubCategoryName { get; set; } = default!;
    public string SubCategoryNameAr { get; set; } = default!;
    public int CategoryId { get; set; }
    public int Total { get; set; }
    public int Active { get; set; }
    public int Expired { get; set; }
}
