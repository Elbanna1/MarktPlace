using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;

namespace Persistence.Repositories;

public class AdminHomeRepository : IAdminHomeRepository
{
    private readonly AppDbContext _context;

    public AdminHomeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<HomeSection>> GetSectionsAsync(
        CancellationToken cancellationToken = default) =>
        await _context.HomeSections
            .AsNoTracking()
            .Include(section => section.Category)
            .OrderBy(section => section.SortOrder)
            .ThenBy(section => section.Id)
            .ToListAsync(cancellationToken);

    public Task<HomeSection?> FindSectionAsync(int id, CancellationToken cancellationToken = default) =>
        _context.HomeSections
            .Include(section => section.Category)
            .FirstOrDefaultAsync(section => section.Id == id, cancellationToken);

    public async Task<IReadOnlyList<HomeSection>> GetSectionsForUpdateAsync(
        CancellationToken cancellationToken = default) =>
        await _context.HomeSections
            .OrderBy(section => section.SortOrder)
            .ThenBy(section => section.Id)
            .ToListAsync(cancellationToken);

    public Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellationToken = default) =>
        _context.Categories.AsNoTracking().AnyAsync(category => category.Id == categoryId, cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}

public class AdminSettingsRepository : IAdminSettingsRepository
{
    private readonly AppDbContext _context;

    public AdminSettingsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PlatformSetting> GetAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _context.PlatformSettings
            .FirstOrDefaultAsync(
                row => row.Id == Configurations.PlatformSettingConfiguration.SingletonId,
                cancellationToken);

        if (settings is not null)
            return settings;

        settings = new PlatformSetting
        {
            Id = Configurations.PlatformSettingConfiguration.SingletonId,
            SiteName = "شبيك لبيك",
            SiteNameEn = "Shobik Lobik"
        };

        _context.PlatformSettings.Add(settings);
        await _context.SaveChangesAsync(cancellationToken);

        return settings;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}

public class AdminFormRepository : IAdminFormRepository
{
    private readonly AppDbContext _context;

    public AdminFormRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AdFormFieldOverride>> GetOverridesAsync(
        int categoryId, int? subCategoryId, CancellationToken cancellationToken = default) =>
        await _context.AdFormFieldOverrides
            .AsNoTracking()
            .Include(field => field.Options)
            .Where(field => field.CategoryId == categoryId && field.SubCategoryId == subCategoryId)
            .ToListAsync(cancellationToken);

    public Task<AdFormFieldOverride?> FindOverrideAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        _context.AdFormFieldOverrides
            .Include(field => field.Options)
            .FirstOrDefaultAsync(field => field.Id == id, cancellationToken);

    public Task<AdFormFieldOverride?> FindOverrideAsync(
        int categoryId, int? subCategoryId, string fieldName,
        CancellationToken cancellationToken = default) =>
        _context.AdFormFieldOverrides
            .Include(field => field.Options)
            .FirstOrDefaultAsync(
                field => field.CategoryId == categoryId &&
                         field.SubCategoryId == subCategoryId &&
                         field.FieldName == fieldName,
                cancellationToken);

    public Task<AdFormFieldOption?> FindOptionAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        _context.AdFormFieldOptions
            .Include(option => option.FieldOverride)
            .FirstOrDefaultAsync(option => option.Id == id, cancellationToken);

    public void AddOverride(AdFormFieldOverride fieldOverride) =>
        _context.AdFormFieldOverrides.Add(fieldOverride);

    public void RemoveOverride(AdFormFieldOverride fieldOverride) =>
        _context.AdFormFieldOverrides.Remove(fieldOverride);

    public void AddOption(AdFormFieldOption option) => _context.AdFormFieldOptions.Add(option);

    public void RemoveOption(AdFormFieldOption option) => _context.AdFormFieldOptions.Remove(option);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
