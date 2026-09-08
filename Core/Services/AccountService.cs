using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Account;
using Shared.Enums;
using Shared.Exceptions;

namespace Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAccountClosureRepository _closure;
    private readonly IUserAccessStateCache _accessState;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        IAccountClosureRepository closure,
        IUserAccessStateCache accessState,
        IUnitOfWork unitOfWork,
        ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _closure = closure;
        _accessState = accessState;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<DeactivatedAccountDto> DeactivateAsync(
        string userId, DeactivateAccountRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UnauthorizedException(UserMessages.Account.UnknownAccount);

        if (user.Status == UserAccountStatus.Deactivated)
            throw new ConflictException(UserMessages.Account.AlreadyDeactivated);

        await EnsureNotAnAdministratorAsync(user);

        await EnsurePasswordConfirmedAsync(user, request.Password);

        AccountClosureResult closure;

        await using (var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
        {
            closure = await _closure.CloseAsync(user.Id, cancellationToken);

            var now = DateTime.UtcNow;

            user.Status = UserAccountStatus.Deactivated;
            user.StatusChangedAt = now;
            user.StatusChangedBy = user.Id;
            user.StatusReason = Trim(request.Reason);
            user.UpdatedAt = now;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            user.PasswordResetOtpHash = null;
            user.PasswordResetOtpExpiry = null;
            user.PasswordResetTokenHash = null;
            user.PasswordResetVerified = false;
            user.PasswordResetVerifiedExpiry = null;

            var update = await _userManager.UpdateAsync(user);

            if (!update.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);

                if (update.Errors.Any(error =>
                        string.Equals(error.Code, "ConcurrencyFailure", StringComparison.Ordinal)))
                    throw new ConflictException(UserMessages.Account.AlreadyDeactivated);

                throw new BadRequestException(
                    UserMessages.Errors.Unexpected,
                    update.Errors.Select(error => error.Description).ToList());
            }

            await transaction.CommitAsync(cancellationToken);
        }

        _accessState.Invalidate(user.Id);

        _logger.LogInformation(
            "Account {UserId} was closed by its owner. {ListingsHidden} listings hidden, " +
            "{InterestsRemoved} notification interests removed.",
            user.Id, closure.ListingsHidden, closure.InterestsRemoved);

        return new DeactivatedAccountDto
        {
            UserId = user.Id,
            Status = (int)user.Status,
            StatusName = UserAccountCatalog.GetStatusName(user.Status),
            DeactivatedAt = user.StatusChangedAt!.Value,
            ListingsHidden = closure.ListingsHidden
        };
    }

    private async Task EnsureNotAnAdministratorAsync(ApplicationUser user)
    {
        if (await _userManager.IsInRoleAsync(user, AppRoles.SuperAdmin))
        {
            var superAdmins = await _userManager.GetUsersInRoleAsync(AppRoles.SuperAdmin);

            var remaining = superAdmins.Count(candidate =>
                candidate.Status == UserAccountStatus.Active &&
                !string.Equals(candidate.Id, user.Id, StringComparison.Ordinal));

            if (remaining == 0)
                throw new ForbiddenException(UserMessages.Account.LastSuperAdminProtected);
        }

        if (await _userManager.IsInRoleAsync(user, AppRoles.Admin) ||
            await _userManager.IsInRoleAsync(user, AppRoles.SuperAdmin))
            throw new ForbiddenException(UserMessages.Account.AdminCannotSelfDeactivate);
    }

    private async Task EnsurePasswordConfirmedAsync(ApplicationUser user, string? password)
    {
        if (!await _userManager.HasPasswordAsync(user))
            return;

        if (string.IsNullOrWhiteSpace(password))
            throw new BadRequestException(UserMessages.Account.PasswordRequired);

        if (!await _userManager.CheckPasswordAsync(user, password))
            throw new UnauthorizedException(UserMessages.Account.PasswordIncorrect);
    }

    private static string? Trim(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
