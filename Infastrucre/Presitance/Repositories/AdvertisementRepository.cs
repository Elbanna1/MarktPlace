using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.Enums;

namespace Persistence.Repositories;

public class AdvertisementRepository : IAdvertisementRepository
{
    private readonly AppDbContext _context;

    public AdvertisementRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<Advertisement> WithDetails(IQueryable<Advertisement> query) =>
        query
            .Include(a => a.Owner)
            .Include(a => a.Category)
            .Include(a => a.SubCategory)
            .Include(a => a.Images)
            .Include(a => a.AdvertisementFeatures)
                .ThenInclude(af => af.Feature);

    public Task<Advertisement?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Advertisement> query = _context.Advertisements
            .Include(a => a.Images)
            .Include(a => a.AdvertisementFeatures);

            if (includeUnmoderated)
                query = query.IncludingUnmoderated();

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Advertisement?> GetDetailsAsync(
        Guid id, bool includeUnmoderated = false, CancellationToken cancellationToken = default)
    {
        var advertisement = await _context.Advertisements
            .AsNoTracking()
            .Include(a => a.Owner)
            .Include(a => a.Category)
            .Include(a => a.SubCategory)
            .Include(a => a.Images)
            .IncludingUnmoderatedIf(includeUnmoderated)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (advertisement is null)
            return null;

        advertisement.AdvertisementFeatures = await _context.AdvertisementFeatures
            .AsNoTracking()
            .Include(af => af.Feature)
            .Where(af => af.AdvertisementId == id)
            .ToListAsync(cancellationToken);

        return advertisement;
    }

    public Task<Advertisement?> GetOwnedAsync(
        Guid id, string ownerId, CancellationToken cancellationToken = default) =>
        _context.Advertisements
            .Include(a => a.Images)
            .Include(a => a.AdvertisementFeatures)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(a => a.Id == id && a.OwnerId == ownerId, cancellationToken);

    public Task<AdvertisementOwnership?> GetOwnershipAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        _context.Advertisements
            .AsNoTracking()
            .IncludingUnmoderated()
            .Where(a => a.Id == id)
            .Select(a => new AdvertisementOwnership(
                a.OwnerId, a.Status, a.ExpireAt, a.ModerationStatus, a.DeletedAt))
            .FirstOrDefaultAsync(cancellationToken);

    public Task IncrementViewsAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Advertisements
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(a => a.Views, a => a.Views + 1),
                cancellationToken);

    public async Task<(IReadOnlyList<AdvertisementCardRow> Items, int TotalCount)> GetPagedAsync(
        AdvertisementFilterParams filter,
        string? ownerId = null,
        bool includeNonActive = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Advertisements.AsNoTracking().AsQueryable();

        query = query.IncludingUnmoderatedIf(ownerId is not null);

        if (ownerId is not null)
            query = query.Where(a => a.OwnerId == ownerId);

        if (!includeNonActive)
        {
            var now = DateTime.UtcNow;
            query = query.Where(a => a.Status == AdvertisementStatus.Active && a.ExpireAt > now);
        }
        else

            query = query.Where(a => a.Status != AdvertisementStatus.Deleted && a.DeletedAt == null);

        query = ApplyFilters(query, filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<AdvertisementCardRow>(), 0);

        query = ApplySorting(query, filter.SortBy);

        var items = await query
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(a => new AdvertisementCardRow
            {
                Id = a.Id,
                Title = a.Title,
                Price = a.Price,
                Negotiable = a.Negotiable,
                ListingType = a.ListingType,
                Status = a.Status,
                ExpireAt = a.ExpireAt,
                CategoryId = a.CategoryId,
                CategoryName = a.Category.Name,
                SubCategoryId = a.SubCategoryId,
                SubCategoryName = a.SubCategory.Name,
                Brand = a.Brand,
                Model = a.Model,
                ManufacturingYear = a.ManufacturingYear,
                BusinessName = a.BusinessName,
                Governorate = a.Governorate,
                Center = a.Center,
                Views = a.Views,
                CreatedAt = a.CreatedAt,

                OwnerId = a.OwnerId,
                OwnerFirstName = a.Owner.FirstName,
                OwnerSecondName = a.Owner.SecondName,
                OwnerPhoneNumber = a.Owner.PhoneNumber,
                OwnerGovernorate = a.Owner.Governorate,
                OwnerCenter = a.Owner.Center,
                OwnerProfileImageUrl = a.Owner.ProfileImageUrl,
                OwnerCreatedAt = a.Owner.CreatedAt,

                Images = a.Images
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenBy(i => i.CreatedAt)
                    .Select(i => new AdvertisementImageDto
                    {
                        Id = i.Id,
                        Url = i.ImageUrl,
                        IsPrimary = i.IsPrimary
                    })
                    .ToList(),

                Features = a.AdvertisementFeatures
                    .Select(af => new FeatureDto
                    {
                        Id = af.Feature.Id,
                        Name = af.Feature.Name,
                        Group = af.Feature.Group
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    private static IQueryable<Advertisement> ApplyFilters(IQueryable<Advertisement> query, AdvertisementFilterParams f)
    {
        if (!string.IsNullOrWhiteSpace(f.Search))
        {
            var term = f.Search.Trim();

            query = query.Where(a =>
                EF.Functions.Like(a.Title, $"%{term}%") ||
                EF.Functions.Like(a.Description, $"%{term}%") ||
                (a.Brand != null && EF.Functions.Like(a.Brand, $"%{term}%")) ||
                (a.Model != null && EF.Functions.Like(a.Model, $"%{term}%")) ||
                (a.BusinessName != null && EF.Functions.Like(a.BusinessName, $"%{term}%")));
        }

        if (f.CategoryId is { } categoryId)
            query = query.Where(a => a.CategoryId == categoryId);

        if (f.SubCategoryId is { } subCategoryId)
            query = query.Where(a => a.SubCategoryId == subCategoryId);

        if (f.ListingType is { } listingType)
            query = query.Where(a => a.ListingType == listingType);

        if (!string.IsNullOrWhiteSpace(f.Brand))
            query = query.Where(a => a.Brand == f.Brand);

        if (!string.IsNullOrWhiteSpace(f.Model))
            query = query.Where(a => a.Model == f.Model);

        if (!string.IsNullOrWhiteSpace(f.Color))
            query = query.Where(a => a.Color == f.Color);

        if (f.Year is { } year)
            query = query.Where(a => a.ManufacturingYear == year);

        if (f.YearFrom is { } yearFrom)
            query = query.Where(a => a.ManufacturingYear >= yearFrom);

        if (f.YearTo is { } yearTo)
            query = query.Where(a => a.ManufacturingYear <= yearTo);

        if (f.KilometersTo is { } kilometersTo)
            query = query.Where(a => a.Kilometers != null && a.Kilometers <= kilometersTo);

        if (f.Condition is { } condition)
            query = query.Where(a => a.Condition == condition);

        if (f.TechnicalCondition is { } technicalCondition)
            query = query.Where(a => a.TechnicalCondition == technicalCondition);

        if (f.FuelType is { } fuelType)
            query = query.Where(a => a.FuelType == fuelType);

        if (f.Transmission is { } transmission)
            query = query.Where(a => a.Transmission == transmission);

        if (f.OriginCountry is { } originCountry)
            query = query.Where(a => a.OriginCountry == originCountry);

        if (f.LicenseStatus is { } licenseStatus)
            query = query.Where(a => a.LicenseStatus == licenseStatus);

        if (f.BodyType is { } bodyType)
            query = query.Where(a => a.BodyType == bodyType);

        if (f.MotorcycleType is { } motorcycleType)
            query = query.Where(a => a.MotorcycleType == motorcycleType);

        if (f.MachineType is { } machineType)
            query = query.Where(a => a.MachineType == machineType);

        if (f.VehicleType is { } vehicleType)
            query = query.Where(a => a.VehicleType == vehicleType);

        if (f.FeatureIds is { Count: > 0 })
        {
            foreach (var featureId in f.FeatureIds.Distinct())
            {
                var id = featureId;
                query = query.Where(a => a.AdvertisementFeatures.Any(af => af.FeatureId == id));
            }
        }

        if (!string.IsNullOrWhiteSpace(f.Center))
            query = query.Where(a => a.Center == f.Center);

        if (f.Negotiable is { } negotiable)
            query = query.Where(a => a.Negotiable == negotiable);

        if (f.PriceFrom is { } priceFrom)
            query = query.Where(a => a.Price >= priceFrom);

        if (f.PriceTo is { } priceTo)
            query = query.Where(a => a.Price <= priceTo);

        return query;
    }

    private static IQueryable<Advertisement> ApplySorting(IQueryable<Advertisement> query, AdvertisementSortBy sortBy) =>
        sortBy switch
        {
            AdvertisementSortBy.Oldest => query.OrderBy(a => a.CreatedAt),
            AdvertisementSortBy.PriceAsc => query.OrderBy(a => a.Price).ThenByDescending(a => a.CreatedAt),
            AdvertisementSortBy.PriceDesc => query.OrderByDescending(a => a.Price).ThenByDescending(a => a.CreatedAt),
            AdvertisementSortBy.MostViewed => query.OrderByDescending(a => a.Views).ThenByDescending(a => a.CreatedAt),
            _ => query.OrderByDescending(a => a.CreatedAt)
        };

    public async Task<IReadOnlyList<Advertisement>> GetAdvertisementsExpiringBetweenAsync(
        DateTime fromUtc, DateTime toUtc) =>
        await _context.Advertisements
            .Where(a => a.Status == AdvertisementStatus.Active
                        && a.ExpireAt > fromUtc
                        && a.ExpireAt <= toUtc)
            .ToListAsync();

    public async Task<IReadOnlyList<Advertisement>> GetAdvertisementsToExpireAsync(DateTime utcNow) =>
        await _context.Advertisements
            .Where(a => a.Status == AdvertisementStatus.Active && a.ExpireAt <= utcNow)
            .ToListAsync();

    public async Task<IReadOnlyList<Advertisement>> GetAdvertisementsToDeleteAsync(DateTime utcNow)
    {
        var cutoff = utcNow.AddDays(-AdvertisementConstants.ExpiredGracePeriodDays);

        return await _context.Advertisements
            .Include(a => a.Images)
            .Where(a => a.Status == AdvertisementStatus.Expired
                        && a.ExpiredAt != null
                        && a.ExpiredAt <= cutoff)
            .ToListAsync();
    }

    public Task<AdvertisementImage?> GetImageAsync(Guid imageId) =>
        _context.AdvertisementImages
            .IncludingUnmoderatedParent()
            .Include(i => i.Advertisement)
            .FirstOrDefaultAsync(i => i.Id == imageId);

    public async Task<List<Feature>> GetFeaturesByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.Distinct().ToList();
        return await _context.Features.Where(f => idList.Contains(f.Id)).ToListAsync();
    }

    public async Task<IReadOnlyList<OwnerAdvertisementCount>> GetOwnerStatusCountsAsync(
        string ownerId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var rows = await _context.Advertisements
            .AsNoTracking()
            .IncludingUnmoderated()
            .Where(a => a.OwnerId == ownerId)
            .GroupBy(a => new
            {
                a.CategoryId,
                CategoryName = a.Category.Name,
                CategoryNameAr = a.Category.NameAr,
                a.SubCategoryId,
                SubCategoryName = a.SubCategory.Name,
                SubCategoryNameAr = a.SubCategory.NameAr,

                EffectiveStatus = a.ModerationStatus != ModerationStatus.Approved
                    ? AdvertisementStatus.Pending
                    : a.Status == AdvertisementStatus.Active && a.ExpireAt <= utcNow
                        ? AdvertisementStatus.Expired
                        : a.Status
            })
            .Select(g => new
            {
                g.Key.CategoryId,
                g.Key.CategoryName,
                g.Key.CategoryNameAr,
                g.Key.SubCategoryId,
                g.Key.SubCategoryName,
                g.Key.SubCategoryNameAr,
                g.Key.EffectiveStatus,
                Count = g.Count(),
                Views = g.Sum(a => a.Views)
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(r => new OwnerAdvertisementCount(
                r.CategoryId, r.CategoryName, r.CategoryNameAr,
                r.SubCategoryId, r.SubCategoryName, r.SubCategoryNameAr,
                r.EffectiveStatus, r.Count, r.Views))
            .ToList();
    }

    public Task<bool> SubCategoryExistsAsync(int subCategoryId) =>
        _context.SubCategories.AnyAsync(s => s.Id == subCategoryId);

    public async Task<int?> GetSubCategoryCategoryIdAsync(int subCategoryId) =>
        await _context.SubCategories
            .AsNoTracking()
            .Where(s => s.Id == subCategoryId)
            .Select(s => (int?)s.CategoryId)
            .FirstOrDefaultAsync();

    public async Task AddAsync(Advertisement advertisement) =>
        await _context.Advertisements.AddAsync(advertisement);

    public void Update(Advertisement advertisement) =>
        _context.Advertisements.Update(advertisement);

    public void Remove(Advertisement advertisement) =>
        _context.Advertisements.Remove(advertisement);

    public void RemoveImage(AdvertisementImage image) =>
        _context.AdvertisementImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<AdvertisementImage> images) =>
        await _context.AdvertisementImages.AddRangeAsync(images);

    public async Task AddViewAsync(AdvertisementView view) =>
        await _context.AdvertisementViews.AddAsync(view);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
