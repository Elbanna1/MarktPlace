using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Payments;
using Shared.Enums;

namespace Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PaymentMethod>> GetMethodsAsync(
        bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _context.PaymentMethods.AsNoTracking();

        if (activeOnly)
            query = query.Where(m => m.IsActive);

        return await query
            .OrderBy(m => m.DisplayOrder)
            .ThenBy(m => m.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<PaymentMethod?> GetMethodByIdAsync(
        int id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<PaymentMethod> query = _context.PaymentMethods;

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public Task<bool> MethodNameExistsAsync(
        string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.PaymentMethods.AsNoTracking().Where(m => m.Name == name);

        if (excludeId is { } id)
            query = query.Where(m => m.Id != id);

        return query.AnyAsync(cancellationToken);
    }

    public void AddMethod(PaymentMethod method) => _context.PaymentMethods.Add(method);

    public void UpdateMethod(PaymentMethod method) => _context.PaymentMethods.Update(method);

    public void RemoveMethod(PaymentMethod method) => _context.PaymentMethods.Remove(method);

    public async Task<int> CountMethodUsagesAsync(int methodId, CancellationToken cancellationToken = default)
    {
        var payments = await _context.Payments
            .AsNoTracking()
            .CountAsync(p => p.PaymentMethodId == methodId, cancellationToken);

        var bookings = await _context.BannerBookings
            .AsNoTracking()
            .CountAsync(b => b.PaymentMethodId == methodId, cancellationToken);

        return payments + bookings;
    }

    public async Task<(IReadOnlyList<Payment> Items, int TotalCount)> GetPagedForAdminAsync(
        Shared.DTOs.Admin.AdminPaymentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Payments.AsNoTracking().AsQueryable();

        if (filter.Status is { } status)
            query = query.Where(p => p.Status == status);

        if (filter.PaymentMethodId is { } methodId)
            query = query.Where(p => p.PaymentMethodId == methodId);

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            var userId = filter.UserId.Trim();
            query = query.Where(p => p.UserId == userId);
        }

        if (filter.FromDate is { } from)
            query = query.Where(p => p.SubmittedAt >= from);

        if (filter.ToDate is { } to)
            query = query.Where(p => p.SubmittedAt <= to);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();

            query = query.Where(p =>
                EF.Functions.Like(p.User.FirstName, $"%{term}%") ||
                EF.Functions.Like(p.User.SecondName, $"%{term}%") ||
                (p.User.PhoneNumber != null && EF.Functions.Like(p.User.PhoneNumber, $"%{term}%")) ||
                (p.User.Email != null && EF.Functions.Like(p.User.Email, $"%{term}%")));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Payment>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(p => p.SubmittedAt),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(p => p.PaymentMethod)
                .Include(p => p.User),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<Payment?> GetForAdminAsync(
        Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<Payment> query = _context.Payments
            .Include(p => p.PaymentMethod)
            .Include(p => p.User)
            .Include(p => p.Reviewer);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<BannerBooking?> FindTargetBannerBookingAsync(
        Payment payment, CancellationToken cancellationToken = default)
    {
        if (payment.TargetId is not { } targetId ||
            !string.Equals(payment.TargetType, nameof(BannerBooking), StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return await _context.BannerBookings
            .AsNoTracking()
            .FirstOrDefaultAsync(booking => booking.Id == targetId, cancellationToken);
    }

    public async Task<IReadOnlyDictionary<string, string>> GetUserNamesAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
            return new Dictionary<string, string>();

        var names = await _context.Users
            .AsNoTracking()
            .Where(user => userIds.Contains(user.Id))
            .Select(user => new
            {
                user.Id,
                Name = ((user.FirstName ?? string.Empty) + " " + (user.SecondName ?? string.Empty)).Trim()
            })
            .ToListAsync(cancellationToken);

        return names.ToDictionary(entry => entry.Id, entry => entry.Name);
    }

    public Task<Payment?> GetByIdAsync(
        Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<Payment> query = _context.Payments.Include(p => p.PaymentMethod);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetByUserAsync(
        string userId, PaymentStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Payments
            .AsNoTracking()
            .Include(p => p.PaymentMethod)
            .Where(p => p.UserId == userId);

        if (status is { } value)
            query = query.Where(p => p.Status == value);

        return await query
            .OrderByDescending(p => p.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Payment> Items, int TotalCount)> GetPagedAsync(
        PaymentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Payments.AsNoTracking().AsQueryable();

        if (filter.Status is { } status)
            query = query.Where(p => p.Status == status);

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            var userId = filter.UserId.Trim();
            query = query.Where(p => p.UserId == userId);
        }

        if (filter.PaymentMethodId is { } methodId)
            query = query.Where(p => p.PaymentMethodId == methodId);

        if (filter.FromDate is { } from)
            query = query.Where(p => p.SubmittedAt >= from);

        if (filter.ToDate is { } to)
            query = query.Where(p => p.SubmittedAt <= to);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(p => p.PaymentMethod)
            .OrderByDescending(p => p.SubmittedAt)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default) =>
        await _context.Payments.AddAsync(payment, cancellationToken);

    public void Update(Payment payment) => _context.Payments.Update(payment);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
