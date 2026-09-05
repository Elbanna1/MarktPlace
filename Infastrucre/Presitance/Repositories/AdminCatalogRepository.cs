using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;

namespace Persistence.Repositories;

public class AdminCatalogRepository : IAdminCatalogRepository
{
    private readonly AppDbContext _context;

    public AdminCatalogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Category>> GetCategoriesAsync(
        CancellationToken cancellationToken = default) =>
        await _context.Categories
            .AsNoTracking()
            .Include(category => category.SubCategories)
            .OrderBy(category => category.SortOrder)
            .ThenBy(category => category.Id)
            .ToListAsync(cancellationToken);

    public Task<Category?> FindCategoryAsync(int id, CancellationToken cancellationToken = default) =>
        _context.Categories
            .Include(category => category.SubCategories)
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

    public Task<SubCategory?> FindSubCategoryAsync(int id, CancellationToken cancellationToken = default) =>
        _context.SubCategories
            .Include(subCategory => subCategory.Category)
            .FirstOrDefaultAsync(subCategory => subCategory.Id == id, cancellationToken);

    public async Task<IReadOnlyList<SubCategory>> GetSubCategoriesAsync(
        int categoryId, CancellationToken cancellationToken = default) =>
        await _context.SubCategories
            .AsNoTracking()
            .Include(subCategory => subCategory.Category)
            .Where(subCategory => subCategory.CategoryId == categoryId)
            .OrderBy(subCategory => subCategory.SortOrder)
            .ThenBy(subCategory => subCategory.Id)
            .ToListAsync(cancellationToken);

    public Task<bool> CategoryNameExistsAsync(
        string nameAr, string nameEn, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Categories
            .AsNoTracking()
            .Where(category => category.NameAr == nameAr || category.Name == nameEn);

        if (excludeId is { } id)
            query = query.Where(category => category.Id != id);

        return query.AnyAsync(cancellationToken);
    }

    public Task<bool> SubCategoryNameExistsAsync(
        int categoryId, string nameAr, string nameEn, int? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.SubCategories
            .AsNoTracking()
            .Where(subCategory =>
                subCategory.CategoryId == categoryId &&
                (subCategory.NameAr == nameAr || subCategory.Name == nameEn));

        if (excludeId is { } id)
            query = query.Where(subCategory => subCategory.Id != id);

        return query.AnyAsync(cancellationToken);
    }

    public async Task<int> NextCategoryIdAsync(CancellationToken cancellationToken = default) =>
        await _context.Categories
            .MaxAsync(category => (int?)category.Id, cancellationToken) is { } max
            ? max + 1
            : 1;

    public async Task<int> NextSubCategoryIdAsync(CancellationToken cancellationToken = default) =>
        await _context.SubCategories
            .MaxAsync(subCategory => (int?)subCategory.Id, cancellationToken) is { } max
            ? max + 1
            : 1;

    public Task<bool> IsUsedByHomeSectionAsync(
        int categoryId, CancellationToken cancellationToken = default) =>
        _context.HomeSections
            .AsNoTracking()
            .AnyAsync(section => section.CategoryId == categoryId, cancellationToken);

    public void AddCategory(Category category) => _context.Categories.Add(category);

    public void RemoveCategory(Category category) => _context.Categories.Remove(category);

    public void AddSubCategory(SubCategory subCategory) => _context.SubCategories.Add(subCategory);

    public void RemoveSubCategory(SubCategory subCategory) => _context.SubCategories.Remove(subCategory);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
