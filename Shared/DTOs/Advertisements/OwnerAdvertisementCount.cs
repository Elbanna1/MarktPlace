using Shared.Enums;

namespace Shared.DTOs.Advertisements;

public readonly record struct OwnerAdvertisementCount(
    int CategoryId,
    string CategoryName,
    string CategoryNameAr,
    int SubCategoryId,
    string SubCategoryName,
    string SubCategoryNameAr,
    AdvertisementStatus Status,
    int Count,
    int Views);
