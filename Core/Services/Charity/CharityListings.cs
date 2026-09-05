using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Charity;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.Charity;

internal static class CharityListings
{
    public static string ShareUrl(IFileService fileService, ListingModuleType module, Guid id) =>
        fileService.BuildPublicUrl($"/{ListingModuleCatalog.RouteOf(module)}/{id}");

    public static string? Trimmed(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public static void EnsureResponsibilityAccepted(bool accepted)
    {
        if (!accepted)
            throw new BadRequestException(CharityCatalog.ResponsibilityRequiredMessage);
    }

    public static void EnsureLocation(double? latitude, double? longitude)
    {
        if (!CharityCatalog.IsRealLocation(latitude, longitude))
            throw new BadRequestException("من فضلك حدد الموقع على الخريطة.");
    }

    public static void ApplyClassification(
        CharityDetailsDtoBase dto, ListingModuleType module, IFileService fileService, string ownerName)
    {
        var subCategory = ListingModuleCatalog.SubCategoryOf(module);
        var category = ListingModuleCatalog.CategoryOf(module);

        dto.ListingType = module;
        dto.ListingTypeName = ListingModuleCatalog.NameOf(module);
        dto.CategoryId = (int?)category ?? 0;
        dto.CategoryName = CharityCategoryName;
        dto.SubCategoryId = (int?)subCategory ?? 0;
        dto.SubCategoryName = ListingModuleCatalog.NameOf(module);
        dto.OwnerName = ownerName;

        dto.IsUrgent = module == ListingModuleType.BloodRequest && CharityCatalog.BloodRequestsAreUrgent;

        dto.HasLocation = CharityCatalog.IsRealLocation(dto.Latitude, dto.Longitude);
        dto.ShareUrl = ShareUrl(fileService, module, dto.Id);
        dto.PrimaryImageUrl = dto.Images
            .OrderByDescending(image => image.IsPrimary)
            .ThenBy(image => image.SortOrder)
            .Select(image => image.Url)
            .FirstOrDefault();
    }

    public const string CharityCategoryName = "بوابة الخيرات";
}
