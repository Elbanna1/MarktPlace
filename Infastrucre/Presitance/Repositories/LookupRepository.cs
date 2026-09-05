using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;

namespace Persistence.Repositories;

public class LookupRepository : ILookupRepository
{
    private readonly AppDbContext _context;

    public LookupRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default) =>
        _context.Categories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Id)
            .ToListAsync(cancellationToken);

    public Task<List<Category>> GetCategoriesWithSubCategoriesAsync(CancellationToken cancellationToken = default) =>
        _context.Categories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .Include(c => c.SubCategories
                .Where(s => s.IsActive)
                .OrderBy(s => s.SortOrder)
                .ThenBy(s => s.Id))
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Id)
            .ToListAsync(cancellationToken);

    public Task<List<SubCategory>> GetSubCategoriesAsync(
        int? categoryId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.SubCategories
            .AsNoTracking()
            .Include(s => s.Category)
            .Where(s => s.IsActive && s.Category.IsActive)
            .AsQueryable();

        if (categoryId is { } cid)
            query = query.Where(s => s.CategoryId == cid);

        return query.OrderBy(s => s.SortOrder).ThenBy(s => s.Id).ToListAsync(cancellationToken);
    }

    public Task<List<Feature>> GetFeaturesAsync(CancellationToken cancellationToken = default) =>
        _context.Features.AsNoTracking().OrderBy(f => f.Id).ToListAsync(cancellationToken);

    public Task<List<ListingTypeLookup>> GetListingTypesAsync(CancellationToken cancellationToken = default) =>
        _context.ListingTypes.AsNoTracking().OrderBy(l => l.Id).ToListAsync(cancellationToken);

    public Task<List<Governorate>> GetGovernoratesAsync(CancellationToken cancellationToken = default) =>
        _context.Governorates
            .AsNoTracking()
            .Where(g => g.IsActive)
            .OrderBy(g => g.SortOrder)
            .ThenBy(g => g.Id)
            .ToListAsync(cancellationToken);

    public Task<List<Center>> GetCentersAsync(
        int? governorateId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Centers
            .AsNoTracking()
            .Include(c => c.Governorate)
            .Include(c => c.Projects.Where(p => p.IsActive))
            .Where(c => c.IsActive && c.Governorate.IsActive)
            .AsQueryable();

        if (governorateId is { } gid)
            query = query.Where(c => c.GovernorateId == gid);

        return query.OrderBy(c => c.SortOrder).ThenBy(c => c.Id).ToListAsync(cancellationToken);
    }

    public Task<List<Project>> GetProjectsAsync(
        int? centerId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Projects
            .AsNoTracking()
            .Include(p => p.Center)
                .ThenInclude(c => c.Governorate)
            .Where(p => p.IsActive && p.Center.IsActive)
            .AsQueryable();

        if (centerId is { } cid)
            query = query.Where(p => p.CenterId == cid);

        return query.OrderBy(p => p.SortOrder).ThenBy(p => p.Name).ToListAsync(cancellationToken);
    }

    public Task<List<WorkshopTypeLookup>> GetWorkshopTypesAsync(CancellationToken cancellationToken = default) =>
        _context.WorkshopTypes.AsNoTracking().OrderBy(w => w.Id).ToListAsync(cancellationToken);

    public Task<List<CraftsmanSpecializationLookup>> GetSpecializationsAsync(CancellationToken cancellationToken = default) =>
        _context.CraftsmanSpecializations.AsNoTracking().OrderBy(s => s.Id).ToListAsync(cancellationToken);

    public Task<List<ExperienceLevelLookup>> GetExperienceLevelsAsync(CancellationToken cancellationToken = default) =>
        _context.ExperienceLevels.AsNoTracking().OrderBy(e => e.Id).ToListAsync(cancellationToken);

    public Task<List<ProductionSpecialtyLookup>> GetProductionSpecialtiesAsync(CancellationToken cancellationToken = default) =>
        _context.ProductionSpecialties.AsNoTracking()
            .OrderBy(p => p.Id == BusinessCatalog.ProductionSpecialtyLastId ? 1 : 0)
            .ThenBy(p => p.Id)
            .ToListAsync(cancellationToken);

    public Task<List<FarmTypeLookup>> GetFarmTypesAsync(CancellationToken cancellationToken = default) =>
        _context.FarmTypes.AsNoTracking().OrderBy(f => f.Id).ToListAsync(cancellationToken);

    public Task<List<AvailabilitySeasonLookup>> GetAvailabilitySeasonsAsync(CancellationToken cancellationToken = default) =>
        _context.AvailabilitySeasons.AsNoTracking().OrderBy(a => a.Id).ToListAsync(cancellationToken);

    public Task<List<FarmingMethodLookup>> GetFarmingMethodsAsync(CancellationToken cancellationToken = default) =>
        _context.FarmingMethods.AsNoTracking().OrderBy(f => f.Id).ToListAsync(cancellationToken);

    public Task<List<CompanyFieldLookup>> GetCompanyFieldsAsync(CancellationToken cancellationToken = default) =>
        _context.CompanyFields.AsNoTracking().OrderBy(c => c.Id).ToListAsync(cancellationToken);

    public Task<List<SupplierTypeLookup>> GetSupplierTypesAsync(CancellationToken cancellationToken = default) =>
        _context.SupplierTypes.AsNoTracking().OrderBy(s => s.Id).ToListAsync(cancellationToken);

    public Task<List<TradeTypeLookup>> GetTradeTypesAsync(CancellationToken cancellationToken = default) =>
        _context.TradeTypes.AsNoTracking().OrderBy(t => t.Id).ToListAsync(cancellationToken);

    public Task<List<SaleTypeLookup>> GetSaleTypesAsync(CancellationToken cancellationToken = default) =>
        _context.SaleTypes.AsNoTracking().OrderBy(s => s.Id).ToListAsync(cancellationToken);
}
