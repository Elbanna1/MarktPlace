using Domain.Entities;
using Microsoft.Extensions.Options;
using Shared.Constants;
using Shared.DTOs.Lookups;
using Shared.DTOs.Lookups.Forms;
using Shared.Settings;

namespace Services.Lookups;

public sealed class AdFormBreadcrumbBuilder
{
    public const string HomeName = "Home";

    public const string HomeNameAr = "الرئيسية";

    public const string CreateAdName = "Create Advertisement";

    public const string CreateAdNameAr = "إنشاء إعلان";

    public const string ChooseSubCategoryName = "Choose a sub-category";

    public const string ChooseSubCategoryNameAr = "اختر القسم الفرعي";

    private readonly AppSettings _app;

    public AdFormBreadcrumbBuilder(IOptions<AppSettings> app)
    {
        _app = app.Value ?? new AppSettings();
    }

    public IReadOnlyList<AdFormBreadcrumbItemDto> Build(
        Category category, SubCategory? subCategory, bool requiresSubCategory)
    {
        ArgumentNullException.ThrowIfNull(category);

        return Build(
            category.Id, category.Name, category.NameAr, category.Icon,
            subCategory?.Id, subCategory?.Name, subCategory?.NameAr, subCategory?.Icon,
            requiresSubCategory, _app.HomePath, _app.CreateAdPath);
    }

    public static IReadOnlyList<AdFormBreadcrumbItemDto> Build(
        CategoryDto category, SubCategoryDto? subCategory, bool requiresSubCategory)
    {
        ArgumentNullException.ThrowIfNull(category);

        return Build(
            category.Id, category.Name, category.NameAr, category.Icon,
            subCategory?.Id, subCategory?.Name, subCategory?.NameAr, subCategory?.Icon,
            requiresSubCategory, FrontendRoutes.DefaultHomePath, FrontendRoutes.DefaultCreateAdPath);
    }

    private static IReadOnlyList<AdFormBreadcrumbItemDto> Build(
        int categoryId, string categoryName, string categoryNameAr, string? categoryIcon,
        int? subCategoryId, string? subCategoryName, string? subCategoryNameAr, string? subCategoryIcon,
        bool requiresSubCategory, string homePath, string createAdPath)
    {
        var trail = new List<AdFormBreadcrumbItemDto>
        {
            new()
            {
                Level = AdFormBreadcrumbLevels.Home,
                Name = HomeName,
                NameAr = HomeNameAr,
                Path = homePath
            },
            new()
            {
                Level = AdFormBreadcrumbLevels.Category,
                Id = categoryId,
                Name = categoryName,
                NameAr = categoryNameAr,
                Icon = categoryIcon,
                Path = $"{createAdPath}/{categoryId}"
            }
        };

        if (subCategoryId is { } id)
        {
            trail.Add(new AdFormBreadcrumbItemDto
            {
                Level = AdFormBreadcrumbLevels.SubCategory,
                Id = id,
                Name = subCategoryName!,
                NameAr = subCategoryNameAr!,
                Icon = subCategoryIcon,
                Path = $"{createAdPath}/{categoryId}/{id}"
            });
        }

        trail.Add(new AdFormBreadcrumbItemDto
        {
            Level = AdFormBreadcrumbLevels.Form,
            Name = requiresSubCategory ? ChooseSubCategoryName : CreateAdName,
            NameAr = requiresSubCategory ? ChooseSubCategoryNameAr : CreateAdNameAr,
            Path = trail[^1].Path,
            IsCurrent = true
        });

        return trail;
    }
}
