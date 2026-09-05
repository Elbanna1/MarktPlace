using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;

namespace Persistence.Repositories;

public class AdminUserRepository : IAdminUserRepository
{
    private readonly AppDbContext _context;

    public AdminUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<ApplicationUser> Items, int TotalCount)> GetPagedAsync(
        AdminUserFilterParams filter, IReadOnlyCollection<string> adminUserIds,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsNoTracking();

        if (filter.Status is { } status)
            query = query.Where(user => user.Status == status);

        if (filter.IsAdmin is { } isAdmin)
        {
            query = isAdmin
                ? query.Where(user => adminUserIds.Contains(user.Id))
                : query.Where(user => !adminUserIds.Contains(user.Id));
        }

        if (filter.FromDate is { } from)
            query = query.Where(user => user.CreatedAt >= from);

        if (filter.ToDate is { } to)
            query = query.Where(user => user.CreatedAt <= to);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();

            query = query.Where(user =>
                EF.Functions.Like(user.FirstName, $"%{term}%") ||
                EF.Functions.Like(user.SecondName, $"%{term}%") ||
                (user.UserName != null && EF.Functions.Like(user.UserName, $"%{term}%")) ||
                (user.PhoneNumber != null && EF.Functions.Like(user.PhoneNumber, $"%{term}%")) ||
                (user.Email != null && EF.Functions.Like(user.Email, $"%{term}%")));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<ApplicationUser>(), 0);

        var items = await query
            .OrderByDescending(user => user.CreatedAt)
            .ThenBy(user => user.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<ApplicationUser?> FindAsync(string id, CancellationToken cancellationToken = default) =>
        _context.Users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<IReadOnlyList<AdminDashboardBannerRequestDto>> GetBannerRequestsAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var rows = await _context.BannerBookings
            .AsNoTracking()
            .Where(booking => booking.UserId == userId)
            .OrderByDescending(booking => booking.SubmittedAt)
            .Select(booking => new AdminDashboardBannerRequestDto
            {
                Id = booking.Id,
                AdvertiserName = booking.AdvertiserName,
                Title = booking.Title,
                Location = booking.Location,
                SlotNumber = booking.SlotNumber,
                Price = booking.Price,
                Currency = booking.Currency,
                PaymentStatus = booking.PaymentStatus,
                Status = booking.Status,
                SubmittedAt = booking.SubmittedAt
            })
            .ToListAsync(cancellationToken);

        return rows.Select(AdminDashboardRepository.NameBannerRequest).ToList();
    }

    public Task<int> CountActiveBannersAsync(
        string userId, DateTime utcNow, CancellationToken cancellationToken = default) =>
        _context.BannerBookings
            .CountAsync(
                booking => booking.UserId == userId &&
                           booking.Status == BannerBookingStatus.Published &&
                           booking.StartDate <= utcNow &&
                           booking.EndDate > utcNow,
                cancellationToken);

    public async Task<IReadOnlyList<AdminDashboardPaymentDto>> GetPaymentsAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var rows = await _context.Payments
            .AsNoTracking()
            .Where(payment => payment.UserId == userId)
            .OrderByDescending(payment => payment.SubmittedAt)
            .Select(payment => new
            {
                payment.Id,
                payment.UserId,
                UserName = ((payment.User.FirstName ?? string.Empty) + " " +
                            (payment.User.SecondName ?? string.Empty)).Trim(),
                payment.Amount,
                payment.Currency,
                MethodName = payment.PaymentMethodNameSnapshot ?? payment.PaymentMethod.Name,
                payment.Status,
                payment.SubmittedAt
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new AdminDashboardPaymentDto
            {
                Id = row.Id,
                UserId = row.UserId,
                UserName = row.UserName,
                Amount = row.Amount,
                Currency = row.Currency,
                PaymentMethodName = row.MethodName,
                Status = row.Status,
                StatusName = PaymentCatalog.GetStatusName(row.Status),
                SubmittedAt = row.SubmittedAt
            })
            .ToList();
    }

    public async Task<decimal> GetTotalPaidAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var payments = await _context.Payments
            .Where(payment => payment.UserId == userId && payment.Status == PaymentStatus.Approved)
            .SumAsync(payment => (decimal?)payment.Amount, cancellationToken) ?? 0m;

        var bookings = await _context.BannerBookings
            .Where(booking => booking.UserId == userId &&
                              booking.PaymentStatus == BannerPaymentStatus.Paid)
            .SumAsync(booking => (decimal?)booking.Price, cancellationToken) ?? 0m;

        return payments + bookings;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
