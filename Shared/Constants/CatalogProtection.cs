using Shared.Enums;

namespace Shared.Constants;

public static class CatalogProtection
{
    public static readonly IReadOnlySet<int> FreeCategoryIds = new HashSet<int> { (int)CategoryType.Charity };

    public const string FreeCategoryMessage =
        "بوابة الخيرات وكل أقسامها مجانية دايماً، ومينفعش تفعيل رسوم النشر عليها.";

    public static bool IsFreeCategory(int? categoryId) =>
        categoryId is { } id && FreeCategoryIds.Contains(id);

    public static bool IsFreeModule(ListingModuleType module) =>
        ListingModuleCatalog.CategoryOf(module) is { } category && FreeCategoryIds.Contains((int)category);
}
