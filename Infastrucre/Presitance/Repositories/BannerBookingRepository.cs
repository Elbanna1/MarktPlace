using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.BannerBookings;
using Shared.Enums;

namespace Persistence.Repositories;

public class BannerBookingRepository : IBannerBookingRepository
{
    private static readonly BannerBookingStatus[] OccupyingStatuses =
    [
        BannerBookingStatus.PendingReview,
        BannerBookingStatus.PaymentApproved,
        BannerBookingStatus.Approved,
        BannerBookingStatus.Published
    ];

    private readonly AppDbContext _context;

    public BannerBookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<BannerPlacementSetting>> GetPlacementsAsync(
        bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _context.BannerPlacementSettings.AsNoTracking();

        if (activeOnly)
            query = query.Where(p => p.IsActive);

        return await query
            .OrderBy(p => p.DisplayOrder)
            .ThenBy(p => p.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<BannerPlacementSetting?> GetPlacementAsync(
        BannerLocation location, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<BannerPlacementSetting> query = _context.BannerPlacementSettings;

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(p => p.Location == location, cancellationToken);
    }

    public void UpdatePlacement(BannerPlacementSetting placement) =>
        _context.BannerPlacementSettings.Update(placement);

    public Task<BannerBooking?> GetByIdAsync(
        Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = WithRelations(_context.BannerBookings);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<BannerBooking> Items, int TotalCount)> GetPagedAsync(
        BannerBookingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.BannerBookings.AsNoTracking().AsQueryable();

        if (filter.Status is { } status)
            query = query.Where(b => b.Status == status);

        if (filter.PaymentStatus is { } paymentStatus)
            query = query.Where(b => b.PaymentStatus == paymentStatus);

        if (filter.Location is { } location)
            query = query.Where(b => b.Location == location);

        if (filter.CategoryId is { } categoryId)
            query = query.Where(b => b.CategoryId == categoryId);

        if (filter.SubCategoryId is { } subCategoryId)
            query = query.Where(b => b.SubCategoryId == subCategoryId);

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            var userId = filter.UserId.Trim();
            query = query.Where(b => b.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(b =>
                EF.Functions.Like(b.Title, $"%{term}%") ||
                EF.Functions.Like(b.AdvertiserName, $"%{term}%"));
        }

        if (filter.FromDate is { } from)
            query = query.Where(b => b.SubmittedAt >= from);

        if (filter.ToDate is { } to)
            query = query.Where(b => b.SubmittedAt <= to);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await WithRelations(query)
            .OrderByDescending(b => b.SubmittedAt)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<BannerBooking>> GetOverlappingAsync(
        BannerLocation location,
        int? slotNumber,
        int? categoryId,
        int? subCategoryId,
        DateTime startDate,
        DateTime endDate,
        Guid? excludeBookingId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.BannerBookings
            .AsNoTracking()
            .Where(b => b.Location == location)
            .Where(b => OccupyingStatuses.Contains(b.Status))
            .Where(b => b.StartDate < endDate && b.EndDate > startDate);

        if (excludeBookingId is { } excludeId)
            query = query.Where(b => b.Id != excludeId);

        if (location == BannerLocation.SubCategoryBanner)
        {
            query = query.Where(b => b.CategoryId == categoryId && b.SubCategoryId == subCategoryId);
        }
        else if (slotNumber is { } slot)
        {
            query = query.Where(b => b.SlotNumber == slot);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BannerBooking>> GetByUserAsync(
        string userId, BannerBookingStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = _context.BannerBookings
            .AsNoTracking()
            .Where(b => b.UserId == userId);

        if (status is { } value)
            query = query.Where(b => b.Status == value);

        return await WithRelations(query)
            .OrderByDescending(b => b.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BannerBooking>> GetPublishedAsync(
        DateTime utcNow,
        BannerLocation? location = null,
        int? categoryId = null,
        int? subCategoryId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.BannerBookings
            .AsNoTracking()
            .Where(b => b.Status == BannerBookingStatus.Published)
            .Where(b => b.StartDate <= utcNow && b.EndDate > utcNow);

        if (location is { } value)
            query = query.Where(b => b.Location == value);

        if (categoryId is { } category)
            query = query.Where(b => b.CategoryId == category);

        if (subCategoryId is { } subCategory)
            query = query.Where(b => b.SubCategoryId == subCategory);

        return await query
            .OrderBy(b => b.Location)
            .ThenBy(b => b.SlotNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BannerBooking>> GetBookingsToPublishAsync(
        DateTime utcNow, CancellationToken cancellationToken = default) =>
        await _context.BannerBookings
            .Where(b => b.Status == BannerBookingStatus.Approved)
            .Where(b => b.StartDate <= utcNow && b.EndDate > utcNow)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<BannerBooking>> GetBookingsToExpireAsync(
        DateTime utcNow, CancellationToken cancellationToken = default) =>
        await _context.BannerBookings
            .Where(b => b.Status == BannerBookingStatus.Published || b.Status == BannerBookingStatus.Approved)
            .Where(b => b.EndDate <= utcNow)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(BannerBooking booking, CancellationToken cancellationToken = default) =>
        await _context.BannerBookings.AddAsync(booking, cancellationToken);

    public void Update(BannerBooking booking) => _context.BannerBookings.Update(booking);

    public void Remove(BannerBooking booking) => _context.BannerBookings.Remove(booking);

    public Task<SubCategory?> GetSubCategoryAsync(int subCategoryId, CancellationToken cancellationToken = default) =>
        _context.SubCategories
            .AsNoTracking()
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == subCategoryId, cancellationToken);

    public Task<PaymentMethod?> GetPaymentMethodAsync(int id, CancellationToken cancellationToken = default) =>
        _context.PaymentMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    private static IQueryable<BannerBooking> WithRelations(IQueryable<BannerBooking> query) =>
        query
            .Include(b => b.PlacementSetting)
            .Include(b => b.PaymentMethod)
            .Include(b => b.Category)
            .Include(b => b.SubCategory);
}
