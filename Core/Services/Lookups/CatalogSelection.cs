using Shared.Constants;
using Shared.Enums;

namespace Services.Lookups;

public static class CatalogSelection
{
    public static bool TryResolve(int categoryId, int? subCategoryId, out SubCategoryType subCategory)
    {
        subCategory = default;

        if (subCategoryId is not { } id || !Enum.IsDefined(typeof(SubCategoryType), id))
            return false;

        subCategory = (SubCategoryType)id;

        return ListingModuleCatalog.CategoryOf(subCategory) is { } parent && (int)parent == categoryId;
    }
}
