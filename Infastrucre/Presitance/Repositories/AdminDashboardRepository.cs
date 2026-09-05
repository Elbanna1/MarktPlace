using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;

namespace Persistence.Repositories;

public class AdminDashboardRepository : IAdminDashboardRepository
{
    private readonly AppDbContext _context;

    public AdminDashboardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(int Total, int Suspended)> GetUserCountsAsync(
        CancellationToken cancellationToken = default)
    {
        var total = await _context.Users.CountAsync(cancellationToken);

        var suspended = await _context.Users
            .CountAsync(user => user.Status != UserAccountStatus.Active, cancellationToken);

        return (total, suspended);
    }

    public async Task<(int Active, int Pending)> GetBannerCountsAsync(
        DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var active = await _context.BannerBookings
            .CountAsync(
                booking => booking.Status == BannerBookingStatus.Published &&
                           booking.StartDate <= utcNow &&
                           booking.EndDate > utcNow,
                cancellationToken);

        var pending = await _context.BannerBookings
            .CountAsync(
                booking => booking.Status == BannerBookingStatus.PendingReview ||
                           booking.Status == BannerBookingStatus.PaymentApproved,
                cancellationToken);

        return (active, pending);
    }

    public async Task<int> GetPendingPaymentsCountAsync(CancellationToken cancellationToken = default)
    {
        var payments = await _context.Payments
            .CountAsync(payment => payment.Status == PaymentStatus.Pending, cancellationToken);

        var bannerTransfers = await _context.BannerBookings
            .CountAsync(
                booking => booking.PaymentStatus == BannerPaymentStatus.Pending &&
                           booking.Status != BannerBookingStatus.Cancelled &&
                           booking.Status != BannerBookingStatus.Rejected,
                cancellationToken);

        return payments + bannerTransfers;
    }

    public Task<int> GetPendingReportsCountAsync(CancellationToken cancellationToken = default) =>
        _context.ListingReports
            .CountAsync(report => report.Status == ListingReportStatus.Pending, cancellationToken);

    public async Task<(decimal Total, decimal Monthly)> GetRevenueAsync(
        DateTime monthStartUtc, CancellationToken cancellationToken = default)
    {
        var approvedPayments = _context.Payments
            .Where(payment => payment.Status == PaymentStatus.Approved);

        var paidBookings = _context.BannerBookings
            .Where(booking => booking.PaymentStatus == BannerPaymentStatus.Paid);

        var paymentsTotal = await approvedPayments
            .SumAsync(payment => (decimal?)payment.Amount, cancellationToken) ?? 0m;

        var bookingsTotal = await paidBookings
            .SumAsync(booking => (decimal?)booking.Price, cancellationToken) ?? 0m;

        var paymentsMonthly = await approvedPayments
            .Where(payment => payment.ApprovedAt >= monthStartUtc)
            .SumAsync(payment => (decimal?)payment.Amount, cancellationToken) ?? 0m;

        var bookingsMonthly = await paidBookings
            .Where(booking => booking.PaymentApprovedAt >= monthStartUtc)
            .SumAsync(booking => (decimal?)booking.Price, cancellationToken) ?? 0m;

        return (paymentsTotal + bookingsTotal, paymentsMonthly + bookingsMonthly);
    }

    public async Task<IReadOnlyList<AdminDashboardBannerRequestDto>> GetLatestBannerRequestsAsync(
        int count, CancellationToken cancellationToken = default)
    {
        var rows = await _context.BannerBookings
            .AsNoTracking()
            .OrderByDescending(booking => booking.SubmittedAt)
            .Take(count)
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

        return rows.Select(NameBannerRequest).ToList();
    }

    public async Task<IReadOnlyList<AdminDashboardPaymentDto>> GetLatestPaymentsAsync(
        int count, CancellationToken cancellationToken = default)
    {
        var payments = await _context.Payments
            .AsNoTracking()
            .OrderByDescending(payment => payment.SubmittedAt)
            .Take(count)
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

        return payments
            .Select(payment => new AdminDashboardPaymentDto
            {
                Id = payment.Id,
                UserId = payment.UserId,
                UserName = payment.UserName,
                Amount = payment.Amount,
                Currency = payment.Currency,
                PaymentMethodName = payment.MethodName,
                Status = payment.Status,
                StatusName = PaymentCatalog.GetStatusName(payment.Status),
                SubmittedAt = payment.SubmittedAt
            })
            .ToList();
    }

    public async Task<IReadOnlyList<AdminTimeSeriesPointDto>> GetUsersOverTimeAsync(
        DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        var rows = await _context.Users
            .AsNoTracking()
            .Where(user => user.CreatedAt >= from && user.CreatedAt <= to)
            .GroupBy(user => user.CreatedAt.Date)
            .Select(group => new AdminTimeSeriesPointDto { Date = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        return rows.OrderBy(point => point.Date).ToList();
    }

    public async Task<IReadOnlyList<AdminRevenuePointDto>> GetRevenueOverTimeAsync(
        DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        var payments = await _context.Payments
            .AsNoTracking()
            .Where(payment => payment.Status == PaymentStatus.Approved &&
                              payment.ApprovedAt != null &&
                              payment.ApprovedAt >= from && payment.ApprovedAt <= to)
            .GroupBy(payment => payment.ApprovedAt!.Value.Date)
            .Select(group => new { Date = group.Key, Amount = group.Sum(payment => payment.Amount) })
            .ToListAsync(cancellationToken);

        var bookings = await _context.BannerBookings
            .AsNoTracking()
            .Where(booking => booking.PaymentStatus == BannerPaymentStatus.Paid &&
                              booking.PaymentApprovedAt != null &&
                              booking.PaymentApprovedAt >= from && booking.PaymentApprovedAt <= to)
            .GroupBy(booking => booking.PaymentApprovedAt!.Value.Date)
            .Select(group => new { Date = group.Key, Amount = group.Sum(booking => booking.Price) })
            .ToListAsync(cancellationToken);

        return payments
            .Select(row => row.Date)
            .Union(bookings.Select(row => row.Date))
            .OrderBy(date => date)
            .Select(date => new AdminRevenuePointDto
            {
                Date = date,
                Amount = payments.Where(row => row.Date == date).Sum(row => row.Amount)
                         + bookings.Where(row => row.Date == date).Sum(row => row.Amount),
                BannerAmount = bookings.Where(row => row.Date == date).Sum(row => row.Amount)
            })
            .ToList();
    }

    public async Task<AdminViewsAnalyticsDto> GetViewsAsync(
        int topCount, CancellationToken cancellationToken = default)
    {
        var total = await _context.ListingViewCounters
            .AsNoTracking()
            .SumAsync(counter => (long?)counter.TotalViews, cancellationToken) ?? 0L;

        var top = await _context.ListingViewCounters
            .AsNoTracking()
            .OrderByDescending(counter => counter.TotalViews)
            .Take(topCount)
            .Select(counter => new AdminTopListingDto
            {
                ListingId = counter.ListingId,
                ListingType = counter.ListingType,
                Views = counter.TotalViews
            })
            .ToListAsync(cancellationToken);

        foreach (var listing in top)
            listing.ListingTypeName = ListingModuleCatalog.NameOf(listing.ListingType);

        return new AdminViewsAnalyticsDto { TotalViews = total, TopListings = top };
    }

    public async Task<(decimal Total, decimal Monthly)> GetBannerRevenueAsync(
        DateTime monthStartUtc, CancellationToken cancellationToken = default)
    {
        var paid = _context.BannerBookings
            .AsNoTracking()
            .Where(booking => booking.PaymentStatus == BannerPaymentStatus.Paid);

        var total = await paid.SumAsync(booking => (decimal?)booking.Price, cancellationToken) ?? 0m;

        var monthly = await paid
            .Where(booking => booking.PaymentApprovedAt >= monthStartUtc)
            .SumAsync(booking => (decimal?)booking.Price, cancellationToken) ?? 0m;

        return (total, monthly);
    }

    public async Task<IReadOnlyDictionary<BannerBookingStatus, int>> GetBannerStatusCountsAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _context.BannerBookings
            .AsNoTracking()
            .GroupBy(booking => booking.Status)
            .Select(group => new { Status = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(row => row.Status, row => row.Count);
    }

    public Task<int> GetBannerPaymentPendingCountAsync(CancellationToken cancellationToken = default) =>
        _context.BannerBookings
            .AsNoTracking()
            .CountAsync(
                booking => booking.PaymentStatus == BannerPaymentStatus.Pending &&
                           booking.Status != BannerBookingStatus.Cancelled &&
                           booking.Status != BannerBookingStatus.Rejected,
                cancellationToken);

    public async Task<IReadOnlyList<AdminDashboardBannerRequestDto>> GetExpiringBannersAsync(
        DateTime utcNow, DateTime until, int count, CancellationToken cancellationToken = default)
    {
        var rows = await _context.BannerBookings
            .AsNoTracking()
            .Where(booking => booking.Status == BannerBookingStatus.Published &&
                              booking.EndDate > utcNow && booking.EndDate <= until)
            .OrderBy(booking => booking.EndDate)
            .Take(count)
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

        return rows.Select(NameBannerRequest).ToList();
    }

    public async Task<IReadOnlyList<AdminBannerSlotOccupancy>> GetSlotOccupancyAsync(
        DateTime utcNow, CancellationToken cancellationToken = default) =>
        await _context.BannerBookings
            .AsNoTracking()
            .Where(booking =>

                (booking.Status == BannerBookingStatus.PendingReview ||
                 booking.Status == BannerBookingStatus.PaymentApproved ||
                 booking.Status == BannerBookingStatus.Approved ||
                 booking.Status == BannerBookingStatus.Published) &&
                booking.EndDate > utcNow)
            .OrderBy(booking => booking.Location)
            .ThenBy(booking => booking.SlotNumber)
            .Select(booking => new AdminBannerSlotOccupancy(
                booking.Location,
                booking.SlotNumber,
                booking.CategoryId,
                booking.Category != null ? booking.Category.NameAr : null,
                booking.SubCategoryId,
                booking.SubCategory != null ? booking.SubCategory.NameAr : null,
                booking.Id,
                booking.Title,
                booking.AdvertiserName,
                booking.StartDate,
                booking.EndDate,
                booking.Status,
                booking.Status == BannerBookingStatus.Published && booking.StartDate <= utcNow))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Shared.DTOs.Listings.ListingReportDto>> GetRecentReportsAsync(
        int count, CancellationToken cancellationToken = default)
    {
        var rows = await _context.ListingReports
            .AsNoTracking()
            .Where(report => report.Status == ListingReportStatus.Pending)
            .OrderByDescending(report => report.CreatedAt)
            .Take(count)
            .Select(report => new Shared.DTOs.Listings.ListingReportDto
            {
                Id = report.Id,
                ListingType = report.ListingType,
                ListingId = report.ListingId,
                ListingTitle = report.ListingTitle,
                Reason = report.Reason,
                Details = report.Details,
                Status = report.Status,
                CreatedAt = report.CreatedAt,
                ListingOwnerId = report.ListingOwnerId,
                ReporterUserId = report.ReporterUserId
            })
            .ToListAsync(cancellationToken);

        foreach (var report in rows)
        {
            report.ListingTypeName = ListingModuleCatalog.NameOf(report.ListingType);
            report.ReasonName = ListingInteractionCatalog.NameOf(report.Reason);
            report.StatusName = ListingInteractionCatalog.NameOf(report.Status);
        }

        return rows;
    }

    public async Task<IReadOnlyList<AdminDashboardBannerRequestDto>> GetPendingBannerRequestsAsync(
        int count, CancellationToken cancellationToken = default)
    {
        var rows = await _context.BannerBookings
            .AsNoTracking()
            .Where(booking => booking.Status == BannerBookingStatus.PendingReview ||
                              booking.Status == BannerBookingStatus.PaymentApproved)
            .OrderByDescending(booking => booking.SubmittedAt)
            .Take(count)
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

        return rows.Select(NameBannerRequest).ToList();
    }

    public async Task<IReadOnlyList<AdminDashboardPaymentDto>> GetPendingPaymentsAsync(
        int count, CancellationToken cancellationToken = default)
    {
        var rows = await _context.Payments
            .AsNoTracking()
            .Where(payment => payment.Status == PaymentStatus.Pending)
            .OrderByDescending(payment => payment.SubmittedAt)
            .Take(count)
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

    internal static AdminDashboardBannerRequestDto NameBannerRequest(AdminDashboardBannerRequestDto row)
    {
        row.LocationName = BannerBookingCatalog.GetLocationName(row.Location);
        row.PaymentStatusName = BannerBookingCatalog.GetPaymentStatusName(row.PaymentStatus);
        row.StatusName = BannerBookingCatalog.GetStatusName(row.Status);

        return row;
    }
}
