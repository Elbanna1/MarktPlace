using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Admin;

public class AdminReferralService : IAdminReferralService
{
    private const int DefaultTopCount = 5;
    private const int MaxTopCount = 50;

    private readonly IReferralRepository _repository;
    private readonly IReferralLinkBuilder _links;

    public AdminReferralService(IReferralRepository repository, IReferralLinkBuilder links)
    {
        _repository = repository;
        _links = links;
    }

    public async Task<PaginatedResult<AdminReferralDto>> GetReferralsAsync(
        AdminReferralFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetForAdminAsync(filter, cancellationToken);

        foreach (var item in items)
            item.ReferralLink = _links.Build(item.ReferralCode);

        return new PaginatedResult<AdminReferralDto>(items, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<AdminReferralDto> GetReferralAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var referral = await _repository.GetAdminReferralAsync(id, cancellationToken)
            ?? throw new NotFoundException("الدعوة غير موجودة.");

        referral.ReferralLink = _links.Build(referral.ReferralCode);

        return referral;
    }

    public async Task<PaginatedResult<AdminReferrerDto>> GetReferrersAsync(
        AdminReferrerFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetReferrersForAdminAsync(filter, cancellationToken);

        foreach (var item in items)
        {
            item.ReferralLink = item.ReferralCode is null ? null : _links.Build(item.ReferralCode);
            item.AccountStatusName = UserAccountCatalog.GetStatusName(item.AccountStatus);
            item.ConversionRate = Conversion(item.TotalReferrals, item.TotalClicks);
        }

        return new PaginatedResult<AdminReferrerDto>(items, total, filter.PageIndex, filter.PageSize);
    }

    private static decimal? Conversion(int referrals, int clicks) =>
        clicks <= 0 ? null : Math.Round(referrals * 100m / clicks, 1);

    public async Task<AdminReferralStatisticsDto> GetStatisticsAsync(
        int topCount = DefaultTopCount, CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        topCount = Math.Clamp(topCount, 1, MaxTopCount);

        var counts = await _repository.CountAllByStatusAsync(cancellationToken);

        var linkEvents = await _repository.CountAllLinkEventsAsync(cancellationToken);

        return new AdminReferralStatisticsDto
        {
            TotalReferrals = counts.Values.Sum(),
            CompletedReferrals = counts.GetValueOrDefault(ReferralStatus.Completed),
            PendingReferrals = counts.GetValueOrDefault(ReferralStatus.Pending),
            ActiveReferrers = await _repository.CountDistinctReferrersAsync(cancellationToken),
            Today = await _repository.CountAllSinceAsync(utcNow.Date, cancellationToken),
            ThisWeek = await _repository.CountAllSinceAsync(utcNow.Date.AddDays(-7), cancellationToken),
            ThisMonth = await _repository.CountAllSinceAsync(MonthStart(utcNow), cancellationToken),
            TopReferrers = await WithLinksAsync(topCount, cancellationToken),
            TotalShares = linkEvents.GetValueOrDefault(ReferralLinkEventType.Share),
            TotalClicks = linkEvents.GetValueOrDefault(ReferralLinkEventType.Click),
            ConversionRate = Conversion(
                counts.Values.Sum(), linkEvents.GetValueOrDefault(ReferralLinkEventType.Click)),
            GeneratedAt = utcNow
        };
    }

    private async Task<IReadOnlyList<AdminTopReferrerDto>> WithLinksAsync(
        int topCount, CancellationToken cancellationToken)
    {
        var top = await _repository.GetTopReferrersAsync(topCount, cancellationToken);

        foreach (var row in top)
            row.ReferralLink = row.ReferralCode is null ? null : _links.Build(row.ReferralCode);

        return top;
    }

    public Task<PaginatedResult<AdminReferralDto>> GetForUserAsync(
        string userId, AdminReferralFilterParams filter, CancellationToken cancellationToken = default)
    {
        filter.ReferrerUserId = userId;

        return GetReferralsAsync(filter, cancellationToken);
    }

    private static DateTime MonthStart(DateTime utcNow) =>
        new(utcNow.Year, utcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
}
