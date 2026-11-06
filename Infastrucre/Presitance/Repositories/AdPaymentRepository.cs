using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Listings;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.AdPayments;
using Shared.Enums;

namespace Persistence.Repositories;

public class AdPaymentRepository : IAdPaymentRepository
{
    private const PaymentPurpose Purpose = PaymentPurpose.AdvertisementPublication;

    private readonly AppDbContext _context;
    private readonly IReadOnlyDictionary<ListingModuleType, IUserListingSource> _sources;

    public AdPaymentRepository(AppDbContext context, IEnumerable<IUserListingSource> sources)
    {
        _context = context;
        _sources = sources
            .GroupBy(source => source.Type)
            .ToDictionary(group => group.Key, group => group.First());
    }

    public async Task<AdPaymentSetting?> GetSettingsAsync(
        bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = _context.AdPaymentSettings.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(s => s.Id == 1, cancellationToken);
    }

    public void AddSettings(AdPaymentSetting settings) => _context.AdPaymentSettings.Add(settings);

    public async Task<IReadOnlyList<PaymentMethod>> GetActiveMethodsAsync(CancellationToken cancellationToken = default) =>
        await _context.PaymentMethods
            .AsNoTracking()
            .Where(method => method.IsActive)
            .OrderBy(method => method.DisplayOrder)
            .ThenBy(method => method.Id)
            .ToListAsync(cancellationToken);

    public async Task<Payment?> GetLatestForListingAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default) =>
        await ForListing(type, listingId)
            .AsNoTracking()
            .Include(payment => payment.PaymentMethod)
            .OrderByDescending(payment => payment.SubmittedAt)
            .ThenByDescending(payment => payment.Id)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<bool> HasPendingAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default) =>
        ForListing(type, listingId).AnyAsync(payment => payment.Status == PaymentStatus.Pending, cancellationToken);

    public Task<bool> HasCreditAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default) =>
        ForListing(type, listingId).AnyAsync(
            payment => payment.Status == PaymentStatus.Approved && payment.ActivatedAt == null, cancellationToken);

    public async Task<int> ConsumeCreditAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow, DateTime? windowEnd,
        CancellationToken cancellationToken = default)
    {
        var creditId = await ForListing(type, listingId)
            .Where(payment => payment.Status == PaymentStatus.Approved && payment.ActivatedAt == null)
            .OrderBy(payment => payment.ApprovedAt)
            .Select(payment => (Guid?)payment.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (creditId is not { } id)
            return 0;

        return await _context.Payments
            .Where(payment => payment.Id == id &&
                              payment.Status == PaymentStatus.Approved &&
                              payment.ActivatedAt == null)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(payment => payment.ActivatedAt, utcNow)
                .SetProperty(payment => payment.ActivationStartAt, utcNow)
                .SetProperty(payment => payment.ActivationEndAt, windowEnd), cancellationToken);
    }

    public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await WithDetails()
            .AsNoTracking()
            .FirstOrDefaultAsync(payment => payment.Id == id, cancellationToken);

    public async Task<(IReadOnlyList<Payment> Items, int TotalCount)> GetPagedAsync(
        AdminAdPaymentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = WithDetails().AsNoTracking().Where(payment => payment.Purpose == Purpose);

        if (filter.Status is { } status)
            query = query.Where(payment => payment.Status == status);

        var types = TypesOf(filter);

        if (types is not null)
            query = query.Where(payment => payment.TargetType != null && types.Contains(payment.TargetType));

        if (filter.AdvertisementId is { } advertisementId)
            query = query.Where(payment => payment.TargetId == advertisementId);

        if (!string.IsNullOrWhiteSpace(filter.UserId))
            query = query.Where(payment => payment.UserId == filter.UserId);

        if (filter.FromDate is { } from)
            query = query.Where(payment => payment.SubmittedAt >= from);

        if (filter.ToDate is { } to)
            query = query.Where(payment => payment.SubmittedAt <= to);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Payment>(), 0);

        var items = await query
            .OrderByDescending(payment => payment.SubmittedAt)
            .ThenByDescending(payment => payment.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<int> DecideAsync(
        Guid id, PaymentStatus decision, string reviewerId, string? rejectionReason, string? feedback,
        DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var pending = _context.Payments.Where(payment =>
            payment.Id == id && payment.Purpose == Purpose && payment.Status == PaymentStatus.Pending);

        return decision == PaymentStatus.Approved
            ? pending.ExecuteUpdateAsync(setters => setters
                .SetProperty(payment => payment.Status, PaymentStatus.Approved)
                .SetProperty(payment => payment.ApprovedAt, utcNow)
                .SetProperty(payment => payment.ApprovedBy, reviewerId)
                .SetProperty(payment => payment.RejectedAt, (DateTime?)null)
                .SetProperty(payment => payment.RejectReason, (string?)null)
                .SetProperty(payment => payment.ReviewFeedback, feedback), cancellationToken)
            : pending.ExecuteUpdateAsync(setters => setters
                .SetProperty(payment => payment.Status, PaymentStatus.Rejected)
                .SetProperty(payment => payment.RejectedAt, utcNow)
                .SetProperty(payment => payment.ApprovedBy, reviewerId)
                .SetProperty(payment => payment.RejectReason, rejectionReason)
                .SetProperty(payment => payment.ReviewFeedback, feedback), cancellationToken);
    }

    public async Task<IReadOnlyDictionary<(ListingModuleType Type, Guid Id), AdPaymentListingSummary>> GetListingSummariesAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid Id)> listings, CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<(ListingModuleType Type, Guid Id), AdPaymentListingSummary>();
        var now = DateTime.UtcNow;

        foreach (var group in listings.GroupBy(listing => listing.Type))
        {
            if (!_sources.TryGetValue(group.Key, out var source))
                continue;

            var ids = group.Select(listing => listing.Id).Distinct().ToList();

            var rows = await source
                .Query(ownerId: null, now, includeUnmoderated: true)
                .Where(row => ids.Contains(row.Id))
                .Select(row => new { row.Id, row.Title, row.CategoryId, row.SubCategoryId })
                .ToListAsync(cancellationToken);

            foreach (var row in rows)
                result[(group.Key, row.Id)] = new AdPaymentListingSummary(row.Title, row.CategoryId, row.SubCategoryId);
        }

        return result;
    }

    public async Task<IReadOnlyDictionary<string, string>> GetUserNamesAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
            return new Dictionary<string, string>();

        var users = await _context.Users
            .AsNoTracking()
            .Where(user => userIds.Contains(user.Id))
            .Select(user => new { user.Id, user.FirstName, user.SecondName })
            .ToListAsync(cancellationToken);

        return users.ToDictionary(
            user => user.Id,
            user => $"{user.FirstName} {user.SecondName}".Trim());
    }

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default) =>
        await _context.Payments.AddAsync(payment, cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public async Task<IReadOnlyDictionary<(ListingModuleType Type, Guid Id), AdPaymentListingFacts>> GetListingFactsAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid Id)> listings, CancellationToken cancellationToken = default)
    {
        if (listings.Count == 0)
            return new Dictionary<(ListingModuleType, Guid), AdPaymentListingFacts>();

        var ids = listings.Select(listing => listing.Id).Distinct().ToList();

        var rows = await _context.Payments
            .AsNoTracking()
            .Where(payment => payment.Purpose == Purpose && payment.TargetId != null && ids.Contains(payment.TargetId.Value))
            .Select(payment => new
            {
                payment.Id,
                payment.TargetType,
                TargetId = payment.TargetId!.Value,
                payment.Status,
                payment.ActivatedAt,
                payment.SubmittedAt
            })
            .ToListAsync(cancellationToken);

        var wanted = listings.ToHashSet();
        var facts = new Dictionary<(ListingModuleType, Guid), AdPaymentListingFacts>();

        foreach (var group in rows.GroupBy(row => (row.TargetType, row.TargetId)))
        {
            if (!Enum.TryParse<ListingModuleType>(group.Key.TargetType, out var type) ||
                !wanted.Contains((type, group.Key.TargetId)))
                continue;

            var latest = group.OrderByDescending(row => row.SubmittedAt).ThenByDescending(row => row.Id).First();

            facts[(type, group.Key.TargetId)] = new AdPaymentListingFacts(
                latest.Id,
                latest.Status,
                group.Any(row => row.Status == PaymentStatus.Approved && row.ActivatedAt == null));
        }

        return facts;
    }

    private IQueryable<Payment> ForListing(ListingModuleType type, Guid listingId)
    {
        var target = type.ToString();

        return _context.Payments.Where(payment =>
            payment.Purpose == Purpose && payment.TargetType == target && payment.TargetId == listingId);
    }

    private IQueryable<Payment> WithDetails() =>
        _context.Payments
            .Include(payment => payment.User)
            .Include(payment => payment.Reviewer)
            .Include(payment => payment.PaymentMethod);

    private static List<string>? TypesOf(AdminAdPaymentFilterParams filter)
    {
        if (filter.Type is null && filter.CategoryId is null)
            return null;

        return ListingModuleCatalog.All
            .Append(ListingModuleType.Advertisement)
            .Distinct()
            .Where(type => filter.Type is null || type == filter.Type)
            .Where(type => filter.CategoryId is null ||
                           (int?)ListingModuleCatalog.CategoryOf(type) == filter.CategoryId ||
                           (type == ListingModuleType.Advertisement && filter.CategoryId == (int)CategoryType.Cars))
            .Select(type => type.ToString())
            .ToList();
    }
}
