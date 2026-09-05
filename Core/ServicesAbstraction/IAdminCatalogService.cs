using Domain.Entities;
using Shared.DTOs.Admin;

namespace ServicesAbstraction;

public interface IAdminCatalogService
{
    Task<IReadOnlyList<AdminCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<AdminCategoryDto> GetCategoryAsync(int id, CancellationToken cancellationToken = default);

    Task<AdminCategoryDto> CreateCategoryAsync(
        CreateCategoryRequest request, CancellationToken cancellationToken = default);

    Task<AdminCategoryDto> UpdateCategoryAsync(
        int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default);

    Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);

    Task<AdminCategoryDto> SetCategoryStatusAsync(
        int id, bool isActive, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminSubCategoryDto>> GetSubCategoriesAsync(
        int categoryId, CancellationToken cancellationToken = default);

    Task<AdminSubCategoryDto> GetSubCategoryAsync(int id, CancellationToken cancellationToken = default);

    Task<AdminSubCategoryDto> CreateSubCategoryAsync(
        int categoryId, CreateSubCategoryRequest request, CancellationToken cancellationToken = default);

    Task<AdminSubCategoryDto> UpdateSubCategoryAsync(
        int id, UpdateSubCategoryRequest request, CancellationToken cancellationToken = default);

    Task DeleteSubCategoryAsync(int id, CancellationToken cancellationToken = default);

    Task<AdminSubCategoryDto> SetSubCategoryStatusAsync(
        int id, bool isActive, CancellationToken cancellationToken = default);
}

public interface IAdminCatalogRepository
{
    Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<Category?> FindCategoryAsync(int id, CancellationToken cancellationToken = default);

    Task<SubCategory?> FindSubCategoryAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SubCategory>> GetSubCategoriesAsync(
        int categoryId, CancellationToken cancellationToken = default);

    Task<bool> CategoryNameExistsAsync(
        string nameAr, string nameEn, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> SubCategoryNameExistsAsync(
        int categoryId, string nameAr, string nameEn, int? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<int> NextCategoryIdAsync(CancellationToken cancellationToken = default);

    Task<int> NextSubCategoryIdAsync(CancellationToken cancellationToken = default);

    Task<bool> IsUsedByHomeSectionAsync(int categoryId, CancellationToken cancellationToken = default);

    void AddCategory(Category category);

    void RemoveCategory(Category category);

    void AddSubCategory(SubCategory subCategory);

    void RemoveSubCategory(SubCategory subCategory);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
