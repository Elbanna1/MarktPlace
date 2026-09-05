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

public class AdminAccountService : IAdminAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAdminAccountRepository _repository;
    private readonly IAdminAuditService _audit;
    private readonly INotificationService _notifications;
    private readonly IUserRoleCache _roleCache;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AdminAccountService> _logger;

    public AdminAccountService(
        UserManager<ApplicationUser> userManager,
        IAdminAccountRepository repository,
        IAdminAuditService audit,
        INotificationService notifications,
        IUserRoleCache roleCache,
        IUnitOfWork unitOfWork,
        ILogger<AdminAccountService> logger)
    {
        _userManager = userManager;
        _repository = repository;
        _audit = audit;
        _notifications = notifications;
        _roleCache = roleCache;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResult<AdminAccountListItemDto>> GetAdminsAsync(
        AdminAccountFilterParams filter, CancellationToken cancellationToken = default)
    {
        var adminIds = await GetRoleMemberIdsAsync(AppRoles.Admin);
        var superAdminIds = await GetRoleMemberIdsAsync(AppRoles.SuperAdmin);

        var (users, totalCount) = await _repository.GetPagedAsync(filter, adminIds, cancellationToken);

        if (users.Count == 0)
            return new PaginatedResult<AdminAccountListItemDto>(
                Array.Empty<AdminAccountListItemDto>(), totalCount, filter.PageIndex, filter.PageSize);

        var pageCounts = await _repository.CountPagesAsync(
            users.Select(user => user.Id).ToList(), cancellationToken);

        var items = users
            .Select(user => ToListItem(
                user,
                superAdminIds.Contains(user.Id),
                pageCounts.TryGetValue(user.Id, out var count) ? count : 0))
            .ToList();

        return new PaginatedResult<AdminAccountListItemDto>(
            items, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<AdminAccountDetailsDto> GetAdminAsync(
        string id, CancellationToken cancellationToken = default)
    {
        var user = await FindAdminAsync(id);

        return await BuildDetailsAsync(user, cancellationToken);
    }

    public async Task<IReadOnlyList<AdminCandidateUserDto>> SearchCandidatesAsync(
        AdminCandidateSearchParams filter, CancellationToken cancellationToken = default)
    {
        var term = filter.Q?.Trim() ?? string.Empty;

        if (term.Length < AdminCandidateSearchParams.MinimumTermLength)
            throw new BadRequestException(
                $"اكتب {AdminCandidateSearchParams.MinimumTermLength} حروف على الأقل عشان تدور.");

        var adminIds = await GetRoleMemberIdsAsync(AppRoles.Admin);
        var superAdminIds = await GetRoleMemberIdsAsync(AppRoles.SuperAdmin);

        var excluded = filter.ExcludeAdmins ? adminIds : (IReadOnlyCollection<string>)Array.Empty<string>();

        var users = await _repository.SearchUsersAsync(
            new AdminCandidateSearchParams
            {
                Q = term,
                ExcludeAdmins = filter.ExcludeAdmins,
                Limit = filter.Limit
            },
            excluded,
            cancellationToken);

        return users
            .Select(user => ToCandidate(user, adminIds.Contains(user.Id), superAdminIds.Contains(user.Id)))
            .ToList();
    }

    public async Task<AdminCandidateUserDto> GetCandidateAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("المستخدم غير موجود.");

        return ToCandidate(
            user,
            await _userManager.IsInRoleAsync(user, AppRoles.Admin),
            await _userManager.IsInRoleAsync(user, AppRoles.SuperAdmin));
    }

    public async Task<AdminAccountDetailsDto> ConfirmAdminAsync(
        ConfirmAdminRequest request, string actingSuperAdminId, CancellationToken cancellationToken = default)
    {
        var grants = NormalizeGrants(request.Pages);

        var userId = request.UserId?.Trim();

        if (string.IsNullOrWhiteSpace(userId))
            throw new BadRequestException("معرف المستخدم مطلوب.");

        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("المستخدم غير موجود.");

        if (await _userManager.IsInRoleAsync(user, AppRoles.Admin))
            throw new ConflictException("المستخدم ده مسؤول بالفعل.");

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        IdentityResult roleAssignment;

        try
        {
            roleAssignment = await _userManager.AddToRoleAsync(user, AppRoles.Admin);
        }
        catch (Exception exception) when (_unitOfWork.IsUniqueConstraintViolation(exception))
        {
            throw new ConflictException("المستخدم ده مسؤول بالفعل.");
        }

        if (!roleAssignment.Succeeded)
            throw new BadRequestException("مش قادرين نضيف صلاحية المسؤول.", Describe(roleAssignment));

        if (grants.Count > 0)
            await _repository.ReplaceGrantsAsync(user.Id, grants, actingSuperAdminId, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        _roleCache.Invalidate(user.Id);

        await _audit.LogAsync(
            AdminAuditAction.CreateAdmin,
            AdminAuditCatalog.Targets.Admin,
            user.Id,
            $"اعتماد مستخدم قائم كمسؤول: {user.UserName}",
            newValue: DescribeGrants(grants),
            adminUserId: actingSuperAdminId,
            cancellationToken: cancellationToken);

        await _notifications.CreateAsync(
            user.Id,
            NotificationCatalog.AdminAccounts.Confirmed(
                grants
                    .Select(grant => (
                        grant.PageKey,
                        grant.Permissions.Aggregate(
                            AdminPermission.None,
                            (mask, permission) => mask | (AdminPermission)permission)))
                    .ToList()),
            referenceId: null,
            action: NotificationAction.Created);

        _logger.LogInformation(
            "Super Admin {SuperAdminId} confirmed existing user {AdminId} as administrator with {PageCount} page grants.",
            actingSuperAdminId, user.Id, grants.Count);

        return await BuildDetailsAsync(user, cancellationToken);
    }

    public async Task<AdminAccountDetailsDto> UpdateAdminAsync(
        string id, UpdateAdminRequest request, string actingSuperAdminId,
        CancellationToken cancellationToken = default)
    {
        var admin = await FindAdminAsync(id);

        await EnsureNotAnotherSuperAdminAsync(admin, actingSuperAdminId);

        var email = request.Email.Trim();
        var phone = request.Phone.Trim();

        if (!string.Equals(admin.Email, email, StringComparison.OrdinalIgnoreCase) &&
            await _userManager.FindByEmailAsync(email) is not null)
            throw new ConflictException("البريد الإلكتروني ده مستخدم قبل كده.");

        if (!string.Equals(admin.PhoneNumber, phone, StringComparison.Ordinal) &&
            _userManager.Users.Any(user => user.PhoneNumber == phone))
            throw new ConflictException("رقم الموبايل ده مستخدم قبل كده.");

        admin.FirstName = AccountNameRules.NormalizeWhitespace(request.FirstName);
        admin.SecondName = AccountNameRules.NormalizeWhitespace(request.SecondName);
        admin.Email = email;
        admin.PhoneNumber = phone;
        admin.UpdatedAt = DateTime.UtcNow;

        var update = await _userManager.UpdateAsync(admin);

        if (!update.Succeeded)
            throw new BadRequestException("مش قادرين نحفظ التعديلات.", Describe(update));

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(admin);
            var reset = await _userManager.ResetPasswordAsync(admin, token, request.Password);

            if (!reset.Succeeded)
                throw new BadRequestException("مش قادرين نغير كلمة السر.", Describe(reset));
        }

        await _audit.LogAsync(
            AdminAuditAction.UpdateAdmin,
            AdminAuditCatalog.Targets.Admin,
            admin.Id,
            $"تعديل بيانات المسؤول: {admin.UserName}",

            newValue: $"{admin.FirstName} {admin.SecondName} / {admin.Email} / {admin.PhoneNumber}",
            adminUserId: actingSuperAdminId,
            cancellationToken: cancellationToken);

        return await BuildDetailsAsync(admin, cancellationToken);
    }

    public async Task<AdminAccountDetailsDto> SetStatusAsync(
        string id, UpdateAdminStatusRequest request, string actingSuperAdminId,
        CancellationToken cancellationToken = default)
    {
        var admin = await FindAdminAsync(id);

        if (string.Equals(admin.Id, actingSuperAdminId, StringComparison.Ordinal))
            throw new ForbiddenException("مينفعش تغير حالة حسابك الخاص.");

        await EnsureNotAnotherSuperAdminAsync(admin, actingSuperAdminId);

        var reason = request.Reason?.Trim();

        if (!request.IsActive && string.IsNullOrWhiteSpace(reason))
            throw new BadRequestException("سبب الإيقاف مطلوب.");

        var previous = admin.Status;

        admin.Status = request.IsActive ? UserAccountStatus.Active : UserAccountStatus.Suspended;
        admin.StatusChangedAt = DateTime.UtcNow;
        admin.StatusChangedBy = actingSuperAdminId;
        admin.StatusReason = request.IsActive ? null : reason;
        admin.UpdatedAt = DateTime.UtcNow;

        if (!request.IsActive)
        {
            admin.RefreshToken = null;
            admin.RefreshTokenExpiryTime = null;
        }

        var update = await _userManager.UpdateAsync(admin);

        if (!update.Succeeded)
            throw new BadRequestException("مش قادرين نغير حالة الحساب.", Describe(update));

        await _audit.LogAsync(
            request.IsActive ? AdminAuditAction.ActivateAdmin : AdminAuditAction.DeactivateAdmin,
            AdminAuditCatalog.Targets.Admin,
            admin.Id,
            request.IsActive
                ? $"تفعيل المسؤول: {admin.UserName}"
                : $"إيقاف المسؤول: {admin.UserName}. السبب: {reason}",
            oldValue: UserAccountCatalog.GetStatusName(previous),
            newValue: UserAccountCatalog.GetStatusName(admin.Status),
            adminUserId: actingSuperAdminId,
            cancellationToken: cancellationToken);

        return await BuildDetailsAsync(admin, cancellationToken);
    }

    public async Task<AdminAccountDetailsDto> UpdatePermissionsAsync(
        string id, UpdateAdminPermissionsRequest request, string actingSuperAdminId,
        CancellationToken cancellationToken = default)
    {
        var admin = await FindAdminAsync(id);

        if (string.Equals(admin.Id, actingSuperAdminId, StringComparison.Ordinal))
            throw new ForbiddenException("مينفعش تعدل صلاحيات حسابك الخاص.");

        await EnsureNotAnotherSuperAdminAsync(admin, actingSuperAdminId);

        var grants = NormalizeGrants(request.Pages);

        var before = await BuildPagesAsync(admin.Id, cancellationToken);

        await _repository.ReplaceGrantsAsync(admin.Id, grants, actingSuperAdminId, cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdateAdminPermissions,
            AdminAuditCatalog.Targets.Admin,
            admin.Id,
            $"تعديل صلاحيات المسؤول: {admin.UserName}",
            oldValue: DescribePages(before),
            newValue: DescribeGrants(grants),
            adminUserId: actingSuperAdminId,
            cancellationToken: cancellationToken);

        return await BuildDetailsAsync(admin, cancellationToken);
    }

    public async Task RevokeAdminAsync(
        string id, string actingSuperAdminId, CancellationToken cancellationToken = default)
    {
        var admin = await FindAdminAsync(id);

        if (string.Equals(admin.Id, actingSuperAdminId, StringComparison.Ordinal))
            throw new ForbiddenException("مينفعش تسحب صلاحياتك الخاصة.");

        await EnsureNotAnotherSuperAdminAsync(admin, actingSuperAdminId);

        await _repository.RemoveAllGrantsAsync(admin.Id, cancellationToken);

        var removal = await _userManager.RemoveFromRoleAsync(admin, AppRoles.Admin);

        if (!removal.Succeeded)
            throw new BadRequestException("مش قادرين نسحب صلاحية المسؤول.", Describe(removal));

        _roleCache.Invalidate(admin.Id);

        await _audit.LogAsync(
            AdminAuditAction.RevokeAdmin,
            AdminAuditCatalog.Targets.Admin,
            admin.Id,
            $"سحب صلاحية المسؤول من: {admin.UserName}",
            adminUserId: actingSuperAdminId,
            cancellationToken: cancellationToken);
    }

    private async Task<ApplicationUser> FindAdminAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new NotFoundException("المسؤول غير موجود.");

        if (!await _userManager.IsInRoleAsync(user, AppRoles.Admin))
            throw new NotFoundException("المسؤول غير موجود.");

        return user;
    }

    private async Task EnsureNotAnotherSuperAdminAsync(ApplicationUser target, string actingSuperAdminId)
    {
        if (string.Equals(target.Id, actingSuperAdminId, StringComparison.Ordinal))
            return;

        if (await _userManager.IsInRoleAsync(target, AppRoles.SuperAdmin))
            throw new ForbiddenException("مينفعش تعدل حساب مسؤول أعلى تاني.");
    }

    private static List<(string PageKey, IReadOnlyList<int> Permissions)> NormalizeGrants(
        IReadOnlyList<AdminPageAssignmentRequest>? pages)
    {
        var result = new List<(string PageKey, IReadOnlyList<int> Permissions)>();

        if (pages is null || pages.Count == 0)
            return result;

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var assignment in pages)
        {
            var page = AdminPageCatalog.Find(assignment.PageKey)
                ?? throw new BadRequestException($"الصفحة \"{assignment.PageKey}\" غير موجودة.");

            if (!seen.Add(page.Key))
                throw new BadRequestException($"الصفحة \"{page.Key}\" متكررة في الطلب.");

            var permissions = new List<int>();

            foreach (var name in assignment.Permissions ?? Array.Empty<string>())
            {
                if (!Enum.TryParse<AdminPermission>(name?.Trim(), ignoreCase: true, out var permission) ||
                    permission == AdminPermission.None ||
                    !AdminPageCatalog.AllPermissions.Contains(permission))
                    throw new BadRequestException($"الصلاحية \"{name}\" غير معروفة.");

                if (!AdminPageCatalog.Allows(page.Key, permission))
                    throw new BadRequestException(
                        $"الصلاحية \"{permission}\" مش متاحة على صفحة \"{page.NameAr}\".");

                if (!permissions.Contains((int)permission))
                    permissions.Add((int)permission);
            }

            if (permissions.Count == 0)
                continue;

            result.Add((page.Key, permissions));
        }

        return result;
    }

    private async Task<AdminAccountDetailsDto> BuildDetailsAsync(
        ApplicationUser user, CancellationToken cancellationToken)
    {
        var isSuperAdmin = await _userManager.IsInRoleAsync(user, AppRoles.SuperAdmin);

        var pages = isSuperAdmin
            ? AdminPageCatalog.All.Select(page => Describe(page, page.Allowed)).ToList()
            : await BuildPagesAsync(user.Id, cancellationToken);

        var listItem = ToListItem(user, isSuperAdmin, pages.Count);

        return new AdminAccountDetailsDto
        {
            Id = listItem.Id,
            Name = listItem.Name,
            UserName = listItem.UserName,
            Email = listItem.Email,
            Phone = listItem.Phone,
            Image = listItem.Image,
            IsActive = listItem.IsActive,
            IsSuperAdmin = listItem.IsSuperAdmin,
            Status = listItem.Status,
            StatusName = listItem.StatusName,
            PagesCount = pages.Count,
            CreatedAt = listItem.CreatedAt,
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            StatusReason = user.StatusReason,
            StatusChangedAt = user.StatusChangedAt,
            Pages = pages
        };
    }

    private async Task<List<AdminGrantedPageDto>> BuildPagesAsync(
        string userId, CancellationToken cancellationToken)
    {
        var grants = await _repository.GetGrantsAsync(userId, cancellationToken);

        var byKey = grants.ToDictionary(
            grant => grant.PageKey,
            grant => grant.Permissions.Aggregate(
                AdminPermission.None, (mask, permission) => mask | (AdminPermission)permission.Permission),
            StringComparer.OrdinalIgnoreCase);

        return AdminPageCatalog.All
            .Where(page => byKey.ContainsKey(page.Key))
            .Select(page => Describe(page, byKey[page.Key] & page.Allowed))
            .Where(page => page.Permissions.Count > 0)
            .ToList();
    }

    private static AdminGrantedPageDto Describe(
        AdminPageCatalog.AdminPageDefinition page, AdminPermission mask) => new()
    {
        Key = page.Key,
        Name = page.Name,
        NameAr = page.NameAr,
        Route = page.Route,
        Icon = page.Icon,
        Group = page.Group,
        Permissions = AdminPageCatalog.Explode(mask).Select(permission => permission.ToString()).ToList()
    };

    private static AdminAccountListItemDto ToListItem(
        ApplicationUser user, bool isSuperAdmin, int pagesCount) => new()
    {
        Id = user.Id,
        Name = $"{user.FirstName} {user.SecondName}".Trim(),
        UserName = user.UserName ?? string.Empty,
        Email = user.Email,
        Phone = user.PhoneNumber,
        Image = user.ProfileImageUrl,
        IsActive = user.Status == UserAccountStatus.Active,
        IsSuperAdmin = isSuperAdmin,
        Status = user.Status,
        StatusName = UserAccountCatalog.GetStatusName(user.Status),
        PagesCount = pagesCount,
        CreatedAt = user.CreatedAt
    };

    private static AdminCandidateUserDto ToCandidate(
        ApplicationUser user, bool isAdmin, bool isSuperAdmin) => new()
    {
        UserId = user.Id,
        UserName = user.UserName ?? string.Empty,
        Name = $"{user.FirstName} {user.SecondName}".Trim(),
        Email = user.Email,
        Phone = user.PhoneNumber,
        Image = user.ProfileImageUrl,
        IsAdmin = isAdmin,
        IsSuperAdmin = isSuperAdmin,
        Status = user.Status,
        StatusName = UserAccountCatalog.GetStatusName(user.Status)
    };

    private async Task<HashSet<string>> GetRoleMemberIdsAsync(string role)
    {
        var members = await _userManager.GetUsersInRoleAsync(role);

        return members.Select(user => user.Id).ToHashSet(StringComparer.Ordinal);
    }

    private static string DescribeGrants(
        IReadOnlyList<(string PageKey, IReadOnlyList<int> Permissions)> grants) =>
        grants.Count == 0
            ? "لا توجد صلاحيات"
            : string.Join(" | ", grants.Select(grant =>
                $"{grant.PageKey}: {string.Join(",", grant.Permissions.Select(value => ((AdminPermission)value).ToString()))}"));

    private static string DescribePages(IReadOnlyList<AdminGrantedPageDto> pages) =>
        pages.Count == 0
            ? "لا توجد صلاحيات"
            : string.Join(" | ", pages.Select(page => $"{page.Key}: {string.Join(",", page.Permissions)}"));

    private static IReadOnlyList<string> Describe(IdentityResult result) =>
        result.Errors.Select(error => error.Description).ToList();
}
