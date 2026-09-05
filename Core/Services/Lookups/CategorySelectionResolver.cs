using Domain.Entities;
using ServicesAbstraction;
using Shared.Exceptions;

namespace Services.Lookups;

public sealed class CategorySelectionResolver
{
    private readonly ILookupRepository _repository;

    public CategorySelectionResolver(ILookupRepository repository)
    {
        _repository = repository;
    }

    public sealed record CategorySelection(
        Category Category,
        IReadOnlyList<SubCategory> SubCategories,
        SubCategory? SubCategory,
        bool RequiresSubCategory);

    public async Task<CategorySelection> ResolveAsync(
        int categoryId, int? subCategoryId, CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetCategoriesWithSubCategoriesAsync(cancellationToken);

        var category = categories.FirstOrDefault(c => c.Id == categoryId)
            ?? throw new NotFoundException($"القسم {categoryId} مش موجود.");

        var subCategories = category.SubCategories.OrderBy(s => s.Id).ToList();

        if (subCategoryId is null && subCategories.Count > 0)
            return new CategorySelection(category, subCategories, null, RequiresSubCategory: true);

        SubCategory? subCategory = null;

        if (subCategoryId is { } id)
        {
            subCategory = subCategories.FirstOrDefault(s => s.Id == id);

            if (subCategory is null)
            {
                if (categories.SelectMany(c => c.SubCategories).Any(s => s.Id == id))
                    throw new BadRequestException(
                        $"القسم الفرعي {id} مش تابع للقسم {categoryId}.");

                throw new NotFoundException($"القسم الفرعي {id} مش موجود.");
            }
        }

        return new CategorySelection(category, subCategories, subCategory, RequiresSubCategory: false);
    }
}
