using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.Admin;

public class AdminCatalogService : IAdminCatalogService
{
    private readonly IAdminCatalogRepository _repository;
    private readonly IAdminAdRepository _ads;
    private readonly ILookupCache _lookupCache;
    private readonly IAdminAuditService _audit;

    public AdminCatalogService(
        IAdminCatalogRepository repository, IAdminAdRepository ads, ILookupCache lookupCache,
        IAdminAuditService audit)
    {
        _repository = repository;
        _ads = ads;
        _lookupCache = lookupCache;
        _audit = audit;
    }

    private static string Describe(string nameAr, string nameEn, bool isActive, int sortOrder) =>
        $"{nameAr} / {nameEn} — {(isActive ? "مفعل" : "معطل")} — الترتيب: {sortOrder}";

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _repository.SaveChangesAsync(cancellationToken);
        _lookupCache.Invalidate();
    }

    public async Task<IReadOnlyList<AdminCategoryDto>> GetCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetCategoriesAsync(cancellationToken);
        var counts = await GetAdCountsAsync(cancellationToken);

        return categories.Select(category => Map(category, counts)).ToList();
    }

    public async Task<AdminCategoryDto> GetCategoryAsync(
        int id, CancellationToken cancellationToken = default)
    {
        var category = await _repository.FindCategoryAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم غير موجود.");

        var counts = await GetAdCountsAsync(cancellationToken);

        return Map(category, counts);
    }

    public async Task<AdminCategoryDto> CreateCategoryAsync(
        CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var nameAr = request.NameAr.Trim();
        var nameEn = request.NameEn.Trim();

        if (await _repository.CategoryNameExistsAsync(nameAr, nameEn, null, cancellationToken))
            throw new ConflictException("يوجد قسم بنفس الاسم بالفعل.");

        var category = new Category
        {
            Id = await _repository.NextCategoryIdAsync(cancellationToken),
            NameAr = nameAr,
            Name = nameEn,
            Icon = Normalize(request.Icon),
            IsActive = request.IsActive,
            SortOrder = request.SortOrder
        };

        _repository.AddCategory(category);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.CreateCategory, AdminAuditCatalog.Targets.Category,
            category.Id.ToString(), $"إضافة قسم: {category.NameAr}",
            newValue: Describe(category.NameAr, category.Name, category.IsActive, category.SortOrder),
            cancellationToken: cancellationToken);

        return Map(category, new Dictionary<int, int>());
    }

    public async Task<AdminCategoryDto> UpdateCategoryAsync(
        int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _repository.FindCategoryAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم غير موجود.");

        var nameAr = request.NameAr.Trim();
        var nameEn = request.NameEn.Trim();

        if (await _repository.CategoryNameExistsAsync(nameAr, nameEn, id, cancellationToken))
            throw new ConflictException("يوجد قسم بنفس الاسم بالفعل.");

        var before = Describe(category.NameAr, category.Name, category.IsActive, category.SortOrder);

        category.NameAr = nameAr;
        category.Name = nameEn;
        category.Icon = Normalize(request.Icon);
        category.IsActive = request.IsActive;
        category.SortOrder = request.SortOrder;

        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdateCategory, AdminAuditCatalog.Targets.Category,
            category.Id.ToString(), $"تعديل قسم: {category.NameAr}",
            oldValue: before,
            newValue: Describe(category.NameAr, category.Name, category.IsActive, category.SortOrder),
            cancellationToken: cancellationToken);

        var counts = await GetAdCountsAsync(cancellationToken);

        return Map(category, counts);
    }

    public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _repository.FindCategoryAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم غير موجود.");

        if (IsSystemCategory(id))
            throw new ConflictException(
                "لا يمكن حذف قسم أساسي في المنصة. يمكنك تعطيله بدلًا من ذلك.");

        if (category.SubCategories.Count > 0)
            throw new ConflictException("لا يمكن حذف قسم يحتوي على أقسام فرعية.");

        var counts = await GetAdCountsAsync(cancellationToken);

        if (counts.TryGetValue(id, out var adsCount) && adsCount > 0)
            throw new ConflictException($"لا يمكن حذف قسم يحتوي على {adsCount} إعلانًا.");

        if (await _repository.IsUsedByHomeSectionAsync(id, cancellationToken))
            throw new ConflictException("لا يمكن حذف قسم مستخدم في الصفحة الرئيسية.");

        _repository.RemoveCategory(category);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.DeleteCategory, AdminAuditCatalog.Targets.Category,
            id.ToString(), $"حذف قسم: {category.NameAr}",
            oldValue: Describe(category.NameAr, category.Name, category.IsActive, category.SortOrder),
            cancellationToken: cancellationToken);
    }

    public async Task<AdminCategoryDto> SetCategoryStatusAsync(
        int id, bool isActive, CancellationToken cancellationToken = default)
    {
        var category = await _repository.FindCategoryAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم غير موجود.");

        var wasActive = category.IsActive;

        category.IsActive = isActive;

        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.SetCategoryStatus, AdminAuditCatalog.Targets.Category,
            id.ToString(), $"{(isActive ? "تفعيل" : "تعطيل")} قسم: {category.NameAr}",
            oldValue: wasActive ? "مفعل" : "معطل", newValue: isActive ? "مفعل" : "معطل",
            cancellationToken: cancellationToken);

        var counts = await GetAdCountsAsync(cancellationToken);

        return Map(category, counts);
    }

    public async Task<IReadOnlyList<AdminSubCategoryDto>> GetSubCategoriesAsync(
        int categoryId, CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindCategoryAsync(categoryId, cancellationToken)
            ?? throw new NotFoundException("القسم غير موجود.");

        var subCategories = await _repository.GetSubCategoriesAsync(categoryId, cancellationToken);
        var counts = await GetSubCategoryAdCountsAsync(cancellationToken);

        return subCategories.Select(subCategory => Map(subCategory, counts)).ToList();
    }

    public async Task<AdminSubCategoryDto> GetSubCategoryAsync(
        int id, CancellationToken cancellationToken = default)
    {
        var subCategory = await _repository.FindSubCategoryAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم الفرعي غير موجود.");

        var counts = await GetSubCategoryAdCountsAsync(cancellationToken);

        return Map(subCategory, counts);
    }

    public async Task<AdminSubCategoryDto> CreateSubCategoryAsync(
        int categoryId, CreateSubCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _repository.FindCategoryAsync(categoryId, cancellationToken)
            ?? throw new NotFoundException("القسم غير موجود.");

        var nameAr = request.NameAr.Trim();
        var nameEn = request.NameEn.Trim();

        if (await _repository.SubCategoryNameExistsAsync(categoryId, nameAr, nameEn, null, cancellationToken))
            throw new ConflictException("يوجد قسم فرعي بنفس الاسم داخل هذا القسم.");

        var subCategory = new SubCategory
        {
            Id = await _repository.NextSubCategoryIdAsync(cancellationToken),
            CategoryId = categoryId,
            Category = category,
            NameAr = nameAr,
            Name = nameEn,
            Icon = Normalize(request.Icon),
            IsActive = request.IsActive,
            SortOrder = request.SortOrder
        };

        _repository.AddSubCategory(subCategory);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.CreateSubCategory, AdminAuditCatalog.Targets.SubCategory,
            subCategory.Id.ToString(), $"إضافة قسم فرعي: {subCategory.NameAr}",
            newValue: Describe(subCategory.NameAr, subCategory.Name, subCategory.IsActive, subCategory.SortOrder),
            cancellationToken: cancellationToken);

        return Map(subCategory, new Dictionary<int, int>());
    }

    public async Task<AdminSubCategoryDto> UpdateSubCategoryAsync(
        int id, UpdateSubCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var subCategory = await _repository.FindSubCategoryAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم الفرعي غير موجود.");

        var targetCategoryId = request.CategoryId ?? subCategory.CategoryId;

        if (targetCategoryId != subCategory.CategoryId)
        {
            if (IsSystemSubCategory(id))
                throw new ConflictException(
                    "لا يمكن نقل قسم فرعي أساسي إلى قسم آخر.");

            _ = await _repository.FindCategoryAsync(targetCategoryId, cancellationToken)
                ?? throw new NotFoundException("القسم المستهدف غير موجود.");
        }

        var nameAr = request.NameAr.Trim();
        var nameEn = request.NameEn.Trim();

        if (await _repository.SubCategoryNameExistsAsync(
                targetCategoryId, nameAr, nameEn, id, cancellationToken))
        {
            throw new ConflictException("يوجد قسم فرعي بنفس الاسم داخل هذا القسم.");
        }

        var before = Describe(subCategory.NameAr, subCategory.Name, subCategory.IsActive, subCategory.SortOrder);

        subCategory.CategoryId = targetCategoryId;
        subCategory.NameAr = nameAr;
        subCategory.Name = nameEn;
        subCategory.Icon = Normalize(request.Icon);
        subCategory.IsActive = request.IsActive;
        subCategory.SortOrder = request.SortOrder;

        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdateSubCategory, AdminAuditCatalog.Targets.SubCategory,
            id.ToString(), $"تعديل قسم فرعي: {subCategory.NameAr}",
            oldValue: before,
            newValue: Describe(subCategory.NameAr, subCategory.Name, subCategory.IsActive, subCategory.SortOrder),
            cancellationToken: cancellationToken);

        var refreshed = await _repository.FindSubCategoryAsync(id, cancellationToken)!;
        var counts = await GetSubCategoryAdCountsAsync(cancellationToken);

        return Map(refreshed!, counts);
    }

    public async Task DeleteSubCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var subCategory = await _repository.FindSubCategoryAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم الفرعي غير موجود.");

        if (IsSystemSubCategory(id))
            throw new ConflictException(
                "لا يمكن حذف قسم فرعي أساسي في المنصة. يمكنك تعطيله بدلًا من ذلك.");

        var counts = await GetSubCategoryAdCountsAsync(cancellationToken);

        if (counts.TryGetValue(id, out var adsCount) && adsCount > 0)
            throw new ConflictException($"لا يمكن حذف قسم فرعي يحتوي على {adsCount} إعلانًا.");

        _repository.RemoveSubCategory(subCategory);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.DeleteSubCategory, AdminAuditCatalog.Targets.SubCategory,
            id.ToString(), $"حذف قسم فرعي: {subCategory.NameAr}",
            oldValue: Describe(subCategory.NameAr, subCategory.Name, subCategory.IsActive, subCategory.SortOrder),
            cancellationToken: cancellationToken);
    }

    public async Task<AdminSubCategoryDto> SetSubCategoryStatusAsync(
        int id, bool isActive, CancellationToken cancellationToken = default)
    {
        var subCategory = await _repository.FindSubCategoryAsync(id, cancellationToken)
            ?? throw new NotFoundException("القسم الفرعي غير موجود.");

        var wasActive = subCategory.IsActive;
        subCategory.IsActive = isActive;

        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.SetSubCategoryStatus, AdminAuditCatalog.Targets.SubCategory,
            id.ToString(), $"{(isActive ? "تفعيل" : "تعطيل")} قسم فرعي: {subCategory.NameAr}",
            oldValue: wasActive ? "مفعل" : "معطل", newValue: isActive ? "مفعل" : "معطل",
            cancellationToken: cancellationToken);

        var counts = await GetSubCategoryAdCountsAsync(cancellationToken);

        return Map(subCategory, counts);
    }

    private async Task<IReadOnlyDictionary<int, int>> GetAdCountsAsync(CancellationToken cancellationToken)
    {
        var counts = await _ads.CountByModuleAsync(DateTime.UtcNow, cancellationToken);
        var byCategory = new Dictionary<int, int>();

        foreach (var entry in counts)
        {
            var categoryId = entry.SubCategoryId is { } subCategoryId
                ? (int?)ListingModuleCatalog.CategoryOf((SubCategoryType)subCategoryId)
                : (int?)ListingModuleCatalog.CategoryOf(entry.Type);

            if (categoryId is not { } key)
                continue;

            byCategory[key] = byCategory.GetValueOrDefault(key) + entry.Count;
        }

        return byCategory;
    }

    private async Task<IReadOnlyDictionary<int, int>> GetSubCategoryAdCountsAsync(
        CancellationToken cancellationToken)
    {
        var counts = await _ads.CountByModuleAsync(DateTime.UtcNow, cancellationToken);
        var bySubCategory = new Dictionary<int, int>();

        foreach (var entry in counts)
        {
            var subCategoryId = entry.SubCategoryId
                ?? (int?)ListingModuleCatalog.SubCategoryOf(entry.Type);

            if (subCategoryId is not { } key)
                continue;

            bySubCategory[key] = bySubCategory.GetValueOrDefault(key) + entry.Count;
        }

        return bySubCategory;
    }

    private static bool IsSystemCategory(int id) => Enum.IsDefined(typeof(CategoryType), id);

    private static bool IsSystemSubCategory(int id) => Enum.IsDefined(typeof(SubCategoryType), id);

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static AdminCategoryDto Map(Category category, IReadOnlyDictionary<int, int> adCounts) =>
        new()
        {
            Id = category.Id,
            NameAr = category.NameAr,
            NameEn = category.Name,
            Icon = category.Icon,
            IsActive = category.IsActive,
            SortOrder = category.SortOrder,
            SubCategoriesCount = category.SubCategories.Count,
            AdsCount = adCounts.GetValueOrDefault(category.Id),
            IsSystem = IsSystemCategory(category.Id),
            SubCategories = category.SubCategories
                .OrderBy(subCategory => subCategory.SortOrder)
                .ThenBy(subCategory => subCategory.Id)
                .Select(subCategory => new AdminSubCategoryDto
                {
                    Id = subCategory.Id,
                    CategoryId = category.Id,
                    CategoryNameAr = category.NameAr,
                    NameAr = subCategory.NameAr,
                    NameEn = subCategory.Name,
                    Icon = subCategory.Icon,
                    IsActive = subCategory.IsActive,
                    SortOrder = subCategory.SortOrder,
                    IsSystem = IsSystemSubCategory(subCategory.Id)
                })
                .ToList()
        };

    private static AdminSubCategoryDto Map(
        SubCategory subCategory, IReadOnlyDictionary<int, int> adCounts) =>
        new()
        {
            Id = subCategory.Id,
            CategoryId = subCategory.CategoryId,
            CategoryNameAr = subCategory.Category?.NameAr ?? string.Empty,
            NameAr = subCategory.NameAr,
            NameEn = subCategory.Name,
            Icon = subCategory.Icon,
            IsActive = subCategory.IsActive,
            SortOrder = subCategory.SortOrder,
            AdsCount = adCounts.GetValueOrDefault(subCategory.Id),
            IsSystem = IsSystemSubCategory(subCategory.Id)
        };
}
