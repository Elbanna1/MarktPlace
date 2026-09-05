using Domain.Entities;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Referrals;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Referrals;

public class ReferralService : IReferralService
{
    private readonly IReferralRepository _repository;
    private readonly IReferralLinkBuilder _links;
    private readonly INotificationService _notifications;
    private readonly ILogger<ReferralService> _logger;

    public ReferralService(
        IReferralRepository repository,
        IReferralLinkBuilder links,
        INotificationService notifications,
        ILogger<ReferralService> logger)
    {
        _repository = repository;
        _links = links;
        _notifications = notifications;
        _logger = logger;
    }

    public async Task<MyReferralDto> GetMineAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var user = await _repository.FindUserAsync(userId, cancellationToken)
            ?? throw new NotFoundException("المستخدم مش موجود.");

        var code = await EnsureCodeAsync(user, cancellationToken);

        return await BuildMineAsync(userId, code, cancellationToken);
    }

    public async Task<MyReferralDto> RecordShareAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var user = await _repository.FindUserAsync(userId, cancellationToken)
            ?? throw new NotFoundException("المستخدم مش موجود.");

        var code = await EnsureCodeAsync(user, cancellationToken);

        await _repository.AddLinkEventAsync(
            new ReferralLinkEvent
            {
                Id = Guid.NewGuid(),
                ReferrerUserId = userId,
                ReferralCode = code,
                EventType = ReferralLinkEventType.Share,
                CreatedAt = DateTime.UtcNow
            },
            cancellationToken);

        return await BuildMineAsync(userId, code, cancellationToken);
    }

    private async Task<MyReferralDto> BuildMineAsync(
        string userId, string code, CancellationToken cancellationToken)
    {
        var counts = await _repository.CountByStatusAsync(userId, cancellationToken);
        var linkEvents = await _repository.CountLinkEventsAsync(userId, cancellationToken);

        return new MyReferralDto
        {
            ReferralCode = code,
            ReferralLink = _links.Build(code),
            TotalReferrals = counts.Values.Sum(),
            CompletedReferrals = counts.GetValueOrDefault(ReferralStatus.Completed),
            PendingReferrals = counts.GetValueOrDefault(ReferralStatus.Pending),
            TotalShares = linkEvents.GetValueOrDefault(ReferralLinkEventType.Share),
            TotalClicks = linkEvents.GetValueOrDefault(ReferralLinkEventType.Click)
        };
    }

    public async Task<ReferralStatisticsDto> GetMyStatisticsAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        var counts = await _repository.CountByStatusAsync(userId, cancellationToken);

        return new ReferralStatisticsDto
        {
            Total = counts.Values.Sum(),
            Completed = counts.GetValueOrDefault(ReferralStatus.Completed),
            Pending = counts.GetValueOrDefault(ReferralStatus.Pending),
            Today = await _repository.CountSinceAsync(userId, utcNow.Date, cancellationToken),
            ThisWeek = await _repository.CountSinceAsync(userId, utcNow.Date.AddDays(-7), cancellationToken),
            ThisMonth = await _repository.CountSinceAsync(userId, MonthStart(utcNow), cancellationToken),
            GeneratedAt = utcNow
        };
    }

    public async Task<PaginatedResult<ReferredUserDto>> GetMyReferredUsersAsync(
        string userId, MyReferralFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetReferredUsersAsync(userId, filter, cancellationToken);

        return new PaginatedResult<ReferredUserDto>(items, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<ReferralResolutionDto> ResolveAsync(
        string? code, string? callerUserId = null, CancellationToken cancellationToken = default)
    {
        var normalized = ReferralCatalog.Normalize(code);

        if (normalized is null)
            return Invalid("رابط الدعوة غير صالح.");

        var referrer = await _repository.FindUserByCodeAsync(normalized, cancellationToken);

        if (referrer is null)
            return Invalid("رابط الدعوة غير صالح أو منتهي.");

        if (callerUserId is not null &&
            string.Equals(callerUserId, referrer.Id, StringComparison.Ordinal))
        {
            return Invalid("لا يمكنك استخدام رابط الدعوة الخاص بك.");
        }

        if (referrer.Status != UserAccountStatus.Active)
            return Invalid("رابط الدعوة غير صالح أو منتهي.");

        try
        {
            await _repository.AddLinkEventAsync(
                new ReferralLinkEvent
                {
                    Id = Guid.NewGuid(),
                    ReferrerUserId = referrer.Id,
                    ReferralCode = normalized,
                    EventType = ReferralLinkEventType.Click,
                    CreatedAt = DateTime.UtcNow
                },
                cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Could not record a referral link click; continuing.");
        }

        return new ReferralResolutionDto
        {
            Valid = true,
            ReferralCode = normalized,

            ReferrerName = DisplayName(referrer)
        };
    }

    public async Task<ApplicationUser?> ResolveReferrerForRegistrationAsync(
        string? code, CancellationToken cancellationToken = default)
    {
        var normalized = ReferralCatalog.Normalize(code);

        if (normalized is null)
            return null;

        var referrer = await _repository.FindUserByCodeAsync(normalized, cancellationToken);

        if (referrer is null)
        {
            _logger.LogInformation("Registration carried an unknown referral code; continuing without a referral.");
            return null;
        }

        if (referrer.Status != UserAccountStatus.Active)
        {
            _logger.LogInformation(
                "Referral code belongs to account {UserId}, which is not active; continuing without a referral.",
                referrer.Id);
            return null;
        }

        return referrer;
    }

    public async Task<Referral> RecordAsync(
        ApplicationUser referrer, ApplicationUser referred, string codeUsed,
        CancellationToken cancellationToken = default)
    {
        if (string.Equals(referrer.Id, referred.Id, StringComparison.Ordinal))
            throw new BadRequestException("مش ممكن تدعو نفسك.");

        var utcNow = DateTime.UtcNow;

        var referral = new Referral
        {
            Id = Guid.NewGuid(),
            ReferrerUserId = referrer.Id,
            ReferredUserId = referred.Id,
            ReferralCode = codeUsed,

            Status = ReferralStatus.Completed,
            CreatedAt = utcNow,
            CompletedAt = utcNow
        };

        await _repository.AddAsync(referral, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        return referral;
    }

    public async Task<string> GenerateUniqueCodeAsync(CancellationToken cancellationToken = default)
    {
        for (var attempt = 0; attempt < ReferralCatalog.MaxGenerationAttempts; attempt++)
        {
            var candidate = ReferralCatalog.GenerateCode();

            if (!await _repository.CodeExistsAsync(candidate, cancellationToken))
                return candidate;
        }

        throw new InvalidOperationException(
            $"Could not generate a unique referral code after {ReferralCatalog.MaxGenerationAttempts} attempts.");
    }

    public async Task<bool> NotifyReferrerAsync(Guid referralId, CancellationToken cancellationToken = default)
    {
        var referral = await _repository.FindAsync(referralId, cancellationToken);

        if (referral is null || referral.ReferrerNotified)
            return false;

        if (referral.Status != ReferralStatus.Completed)
            return false;

        var counts = await _repository.CountByStatusAsync(referral.ReferrerUserId, cancellationToken);
        var completed = counts.GetValueOrDefault(ReferralStatus.Completed);

        var created = await _notifications.CreateIfNotExistsAsync(
            referral.ReferrerUserId,
            NotificationCatalog.Referrals.Joined(referral.Id, completed),
            referenceId: referral.Id,
            cancellationToken: cancellationToken);

        referral.ReferrerNotified = true;
        await _repository.SaveChangesAsync(cancellationToken);

        return created;
    }

    private async Task<string> EnsureCodeAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var existing = ReferralCatalog.Normalize(user.ReferralCode);

        if (existing is not null)
            return existing;

        user.ReferralCode = await GenerateUniqueCodeAsync(cancellationToken);
        user.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync(cancellationToken);

        return user.ReferralCode;
    }

    private static ReferralResolutionDto Invalid(string message) =>
        new() { Valid = false, Message = message };

    private static string DisplayName(ApplicationUser user) =>
        $"{user.FirstName} {user.SecondName}".Trim();

    private static DateTime MonthStart(DateTime utcNow) =>
        new(utcNow.Year, utcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
}
