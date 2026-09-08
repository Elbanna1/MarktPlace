using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Admin;

public class AdminUserService : IAdminUserService
{
    private readonly IAdminUserRepository _users;
    private readonly IAdminAdRepository _ads;
    private readonly IAdminAdService _adService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notifications;
    private readonly IAdminAuditService _audit;
    private readonly IAdminAlertService _alerts;
    private readonly IUserAccessStateCache _accessState;
    private readonly ILogger<AdminUserService> _logger;

    private const int RecentAdsCount = 10;

    public AdminUserService(
        IAdminUserRepository users,
        IAdminAdRepository ads,
        IAdminAdService adService,
        UserManager<ApplicationUser> userManager,
        INotificationService notifications,
        IAdminAuditService audit,
        IAdminAlertService alerts,
        IUserAccessStateCache accessState,
        ILogger<AdminUserService> logger)
    {
        _users = users;
        _ads = ads;
        _adService = adService;
        _userManager = userManager;
        _notifications = notifications;
        _audit = audit;
        _alerts = alerts;
        _accessState = accessState;
        _logger = logger;
    }

    public async Task<PaginatedResult<AdminUserListItemDto>> GetUsersAsync(
        AdminUserFilterParams filter, CancellationToken cancellationToken = default)
    {
        var adminUserIds = await GetAdminUserIdsAsync();

        var (users, totalCount) = await _users.GetPagedAsync(filter, adminUserIds, cancellationToken);

        if (users.Count == 0)
            return new PaginatedResult<AdminUserListItemDto>(
                Array.Empty<AdminUserListItemDto>(), totalCount, filter.PageIndex, filter.PageSize);

        var adCounts = await _ads.CountByOwnersAsync(
            users.Select(user => user.Id).ToList(), DateTime.UtcNow, cancellationToken);

        var items = users
            .Select(user => new AdminUserListItemDto
            {
                Id = user.Id,
                Name = BuildName(user),
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Image = user.ProfileImageUrl,
                AdsCount = adCounts.TryGetValue(user.Id, out var count) ? count : 0,
                Status = user.Status,
                StatusName = UserAccountCatalog.GetStatusName(user.Status),
                IsAdmin = adminUserIds.Contains(user.Id),
                CreatedAt = user.CreatedAt
            })
            .ToList();

        return new PaginatedResult<AdminUserListItemDto>(
            items, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<AdminUserDetailsDto> GetUserAsync(
        string id, CancellationToken cancellationToken = default)
    {
        var user = await _users.FindAsync(id, cancellationToken)
            ?? throw new NotFoundException("المستخدم غير موجود.");

        return await BuildDetailsAsync(user, cancellationToken);
    }

    public async Task<AdminUserDetailsDto> UpdateStatusAsync(
        string id, string adminUserId, UpdateUserStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _users.FindAsync(id, cancellationToken)
            ?? throw new NotFoundException("المستخدم غير موجود.");

        if (string.Equals(user.Id, adminUserId, StringComparison.Ordinal))
            throw new ForbiddenException("لا يمكن تغيير حالة حسابك الخاص.");

        if (await _userManager.IsInRoleAsync(user, AppRoles.Admin))
            throw new ForbiddenException("لا يمكن تغيير حالة حساب مسؤول.");

        var reason = request.Reason?.Trim();

        if (!UserAccountCatalog.IsAdminAssignable(request.Status))
            throw new BadRequestException("حالة المستخدم غير صحيحة.");

        if (request.Status != UserAccountStatus.Active && string.IsNullOrWhiteSpace(reason))
            throw new BadRequestException("سبب الإيقاف أو الحظر مطلوب.");

        var previousStatus = user.Status;

        user.Status = request.Status;
        user.StatusChangedAt = DateTime.UtcNow;
        user.StatusChangedBy = adminUserId;
        user.StatusReason = request.Status == UserAccountStatus.Active ? null : reason;

        if (request.Status != UserAccountStatus.Active)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
        }

        await _users.SaveChangesAsync(cancellationToken);

        _accessState.Invalidate(user.Id);

        _logger.LogInformation(
            "Admin {AdminId} set account {UserId} to {Status}.", adminUserId, user.Id, request.Status);

        await _audit.LogAsync(
            request.Status switch
            {
                UserAccountStatus.Suspended => AdminAuditAction.SuspendUser,
                UserAccountStatus.Blocked => AdminAuditAction.BlockUser,
                _ => AdminAuditAction.ActivateUser
            },
            AdminAuditCatalog.Targets.User, user.Id,
            $"{UserAccountCatalog.GetStatusName(request.Status)}: {BuildName(user)}",
            oldValue: UserAccountCatalog.GetStatusName(previousStatus),
            newValue: UserAccountCatalog.GetStatusName(request.Status),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyAsync(user, request.Status, reason);

        if (request.Status != UserAccountStatus.Active)
        {
            await _alerts.NotifyUserModerationAsync(
                user.Id, BuildName(user), request.Status, reason, cancellationToken);
        }

        return await BuildDetailsAsync(user, cancellationToken);
    }

    private async Task<AdminUserDetailsDto> BuildDetailsAsync(
        ApplicationUser user, CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;

        var breakdown = await _ads.GetStatusBreakdownAsync(utcNow, user.Id, cancellationToken);

        var counts = new AdminUserListingCountsDto
        {
            Total = breakdown.Sum(row => row.Count),
            Active = Sum(breakdown, row => row.Status == ListingStatus.Active),
            Pending = Sum(breakdown, row => row.ModerationStatus == ModerationStatus.Pending),
            Rejected = Sum(breakdown, row => row.ModerationStatus == ModerationStatus.Rejected),
            Suspended = Sum(breakdown, row => row.ModerationStatus == ModerationStatus.Suspended),
            Expired = Sum(breakdown, row => row.Status == ListingStatus.Expired)
        };

        var recentAds = await _adService.GetAdsAsync(
            new AdminAdFilterParams { OwnerId = user.Id, PageIndex = 1, PageSize = RecentAdsCount },
            cancellationToken);

        var bannerRequests = await _users.GetBannerRequestsAsync(user.Id, cancellationToken);
        var activeBanners = await _users.CountActiveBannersAsync(user.Id, utcNow, cancellationToken);
        var payments = await _users.GetPaymentsAsync(user.Id, cancellationToken);
        var totalPaid = await _users.GetTotalPaidAsync(user.Id, cancellationToken);

        return new AdminUserDetailsDto
        {
            Id = user.Id,
            Name = BuildName(user),
            UserName = user.UserName,
            Phone = user.PhoneNumber,

            WhatsApp = user.PhoneNumber,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            Image = user.ProfileImageUrl,
            Governorate = user.Governorate,
            Center = user.Center,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsAdmin = await _userManager.IsInRoleAsync(user, AppRoles.Admin),

            Status = user.Status,
            StatusName = UserAccountCatalog.GetStatusName(user.Status),
            StatusChangedAt = user.StatusChangedAt,
            StatusReason = user.StatusReason,
            IsLockedOut = user.LockoutEnd is { } lockoutEnd && lockoutEnd > DateTimeOffset.UtcNow,

            Listings = counts,
            RecentAds = recentAds.Items,

            BannerRequests = bannerRequests,
            ActiveBannersCount = activeBanners,
            Payments = payments,
            TotalPaid = totalPaid
        };
    }

    private static int Sum(
        IReadOnlyList<AdminAdStatusCount> breakdown, Func<AdminAdStatusCount, bool> predicate) =>
        breakdown.Where(predicate).Sum(row => row.Count);

    private async Task<IReadOnlyCollection<string>> GetAdminUserIdsAsync()
    {
        var admins = await _userManager.GetUsersInRoleAsync(AppRoles.Admin);

        return admins.Select(admin => admin.Id).ToList();
    }

    private async Task NotifyAsync(ApplicationUser user, UserAccountStatus status, string? reason)
    {
        var (title, body, icon) = status switch
        {
            UserAccountStatus.Suspended =>
                ("إيقاف الحساب", $"⏸️ تم إيقاف حسابك مؤقتًا. السبب: {reason}", "⏸️"),
            UserAccountStatus.Blocked =>
                ("حظر الحساب", $"🚫 تم حظر حسابك. السبب: {reason}", "🚫"),
            _ =>
                ("تفعيل الحساب", "✅ تم تفعيل حسابك ويمكنك استخدام المنصة مرة أخرى.", "✅")
        };

        await _notifications.CreateAsync(
            user.Id,
            title,
            body,
            NotificationType.SystemNotification,
            referenceId: null,
            referenceType: NotificationReferenceTypes.Account,
            action: NotificationAction.Updated,
            icon: icon,
            entityName: "الحساب");
    }

    private static string BuildName(ApplicationUser user) =>
        $"{user.FirstName} {user.SecondName}".Trim();
}
