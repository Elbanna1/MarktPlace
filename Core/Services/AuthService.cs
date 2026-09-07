using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Auth;
using Shared.DTOs.Profile;
using Shared.Exceptions;
using Shared.Settings;

namespace Services;

public class AuthService : IAuthService
{
    private const int OtpExpiryMinutes = 10;
    private const int VerifiedSessionExpiryMinutes = 15;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;
    private readonly JwtSettings _jwtSettings;
    private readonly INotificationService _notificationService;
    private readonly IReferralService _referralService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleTokenValidator _googleTokens;
    private readonly GoogleAuthSettings _googleSettings;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IEmailService emailService,
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<AuthService> logger,
        IOptions<JwtSettings> jwtSettings,
        INotificationService notificationService,
        IReferralService referralService,
        IUnitOfWork unitOfWork,
        IGoogleTokenValidator googleTokens,
        IOptions<GoogleAuthSettings> googleSettings)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _emailService = emailService;
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
        _notificationService = notificationService;
        _referralService = referralService;
        _unitOfWork = unitOfWork;
        _googleTokens = googleTokens;
        _googleSettings = googleSettings.Value;
    }

    private static void EnsureAccountUsable(ApplicationUser user)
    {
        if (user.Status == UserAccountStatus.Active)
            return;

        var reason = string.IsNullOrWhiteSpace(user.StatusReason) ? null : $" السبب: {user.StatusReason}";

        var message = user.Status == UserAccountStatus.Blocked
            ? $"تم حظر هذا الحساب.{reason}"
            : $"تم إيقاف هذا الحساب مؤقتًا.{reason}";

        throw new ForbiddenException(message.Trim());
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var username = AccountNameRules.NormalizeWhitespace(request.Username);

        if (await _userManager.FindByNameAsync(username) is not null)
            throw new ConflictException("اسم المستخدم ده متاخد قبل كده.");

        if (await _userManager.FindByEmailAsync(request.Email) is not null)
            throw new ConflictException("البريد الإلكتروني ده مسجل قبل كده.");

        if (await _userRepository.IsPhoneNumberTakenAsync(request.Phone))
            throw new ConflictException("رقم الموبايل ده مسجل قبل كده.");

        var referrer = await _referralService.ResolveReferrerForRegistrationAsync(request.ReferralCode);

        var user = new ApplicationUser
        {
            FirstName = AccountNameRules.NormalizeWhitespace(request.FirstName),
            SecondName = AccountNameRules.NormalizeWhitespace(request.SecondName),
            UserName = username,
            Email = request.Email.Trim(),
            PhoneNumber = request.Phone.Trim(),
            Governorate = LocationConstants.Governorate,
            Center = request.Center,
            CreatedAt = DateTime.UtcNow,

            ReferralCode = await _referralService.GenerateUniqueCodeAsync()
        };

        Guid? referralId = null;

        await using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            IdentityResult result;

            try
            {
                result = await _userManager.CreateAsync(user, request.Password);
            }
            catch (Exception exception) when (_unitOfWork.IsUniqueConstraintViolation(exception))
            {
                _logger.LogInformation(
                    exception,
                    "A concurrent registration lost a uniqueness race; reporting it as a conflict.");

                throw new ConflictException("رقم الموبايل ده مسجل قبل كده.");
            }

            if (!result.Succeeded)
                throw new BadRequestException("مش قادرين نكمل إنشاء الحساب. حاول تاني.", DescribeErrors(result));

            if (referrer is not null)
            {
                var referral = await _referralService.RecordAsync(referrer, user, referrer.ReferralCode!);
                referralId = referral.Id;
            }

            await transaction.CommitAsync();
        }

        await _notificationService.CreateAsync(
            user.Id,
            "مرحبًا بك",
            "🎊 تم إنشاء حسابك بنجاح. أهلًا بك في سوق الفيوم.",
            NotificationType.AccountCreated,
            referenceId: null,
            referenceType: NotificationReferenceTypes.Account,
            action: NotificationAction.Created,
            icon: "🎊",
            entityName: "الحساب");

        if (referralId is not null)
        {
            try
            {
                await _referralService.NotifyReferrerAsync(referralId.Value);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Referral {ReferralId} was recorded but its notification could not be sent.",
                    referralId);
            }
        }

        return await BuildAuthResponseAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            throw new UnauthorizedException("اسم المستخدم أو كلمة السر غلط.");

        if (await _userManager.IsLockedOutAsync(user))
        {
            _logger.LogWarning("Login attempt on locked-out account {UserId}.", user.Id);
            throw new UnauthorizedException(
                UserMessages.Auth.AccountLocked);
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            await _userManager.AccessFailedAsync(user);
            throw new UnauthorizedException("اسم المستخدم أو كلمة السر غلط.");
        }

        EnsureAccountUsable(user);

        if (await _userManager.GetAccessFailedCountAsync(user) > 0)
            await _userManager.ResetAccessFailedCountAsync(user);

        await _notificationService.CreateAsync(
            user.Id,
            "تسجيل دخول جديد",
            "🛡️ تم تسجيل دخول جديد إلى حسابك.",
            NotificationType.NewLogin,
            referenceId: null,
            referenceType: NotificationReferenceTypes.Account,
            action: NotificationAction.Created,
            icon: "🛡️",
            entityName: "الحساب");

        return await BuildAuthResponseAsync(user);
    }

    public GoogleAuthConfigDto GetGoogleConfig() =>
        new()
        {
            Enabled = _googleTokens.IsConfigured,
            ClientId = _googleTokens.ClientId
        };

    public async Task<GoogleSignInResult> GoogleSignInAsync(
        GoogleSignInRequest request, CancellationToken cancellationToken = default)
    {
        var profile = await _googleTokens.ValidateAsync(request, cancellationToken);

        var linked = await _userManager.FindByLoginAsync(GoogleAuthCatalog.ProviderName, profile.Subject);

        if (linked is not null)
        {
            await EnsureGoogleAccountUsableAsync(linked);
            await NotifyNewLoginAsync(linked);

            return new GoogleSignInResult(await BuildAuthResponseAsync(linked), AccountCreated: false);
        }

        if (_googleSettings.RequireVerifiedEmail && !profile.EmailVerified)
            throw new UnauthorizedException(UserMessages.Auth.GoogleEmailNotVerified);

        if (string.IsNullOrWhiteSpace(profile.Email))
            throw new UnauthorizedException(UserMessages.Auth.GoogleEmailNotVerified);

        var email = profile.Email.Trim();

        var existing = await _userManager.FindByEmailAsync(email);

        if (existing is not null)
        {
            if (!_googleSettings.LinkVerifiedEmailToExistingAccount)
                throw new ConflictException(UserMessages.Auth.GoogleEmailAlreadyRegistered);

            await EnsureGoogleAccountUsableAsync(existing);
            await LinkGoogleLoginAsync(existing, profile);
            await NotifyNewLoginAsync(existing);

            return new GoogleSignInResult(await BuildAuthResponseAsync(existing), AccountCreated: false);
        }

        var created = await CreateGoogleUserAsync(request, profile, email, cancellationToken);

        return new GoogleSignInResult(await BuildAuthResponseAsync(created), AccountCreated: true);
    }

    private async Task<ApplicationUser> CreateGoogleUserAsync(
        GoogleSignInRequest request,
        GoogleUserProfile profile,
        string email,
        CancellationToken cancellationToken)
    {
        var referrer = await _referralService.ResolveReferrerForRegistrationAsync(
            request.ReferralCode, cancellationToken);

        var (firstName, secondName) = GoogleAuthCatalog.SplitDisplayName(
            profile.GivenName, profile.FamilyName, profile.Name, email);

        var user = new ApplicationUser
        {
            FirstName = firstName,
            SecondName = secondName,
            UserName = await ReserveUsernameAsync(profile.Name, email),
            Email = email,
            EmailConfirmed = profile.EmailVerified,
            Governorate = GoogleAuthCatalog.DefaultGovernorate,
            Center = LocationConstants.IsValidCenter(request.Center)
                ? request.Center!
                : GoogleAuthCatalog.DefaultCenter,
            CreatedAt = DateTime.UtcNow,

            ReferralCode = await _referralService.GenerateUniqueCodeAsync(cancellationToken)
        };

        Guid? referralId = null;

        await using (var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
        {
            IdentityResult result;

            try
            {
                result = await _userManager.CreateAsync(user);
            }
            catch (Exception exception) when (_unitOfWork.IsUniqueConstraintViolation(exception))
            {
                _logger.LogInformation(
                    exception,
                    "A concurrent external sign-in lost a uniqueness race; asking the caller to retry.");

                throw new ConflictException(UserMessages.Auth.GoogleSignInRetry);
            }

            if (!result.Succeeded)
                throw new BadRequestException(UserMessages.Auth.GoogleSignInRetry, DescribeErrors(result));

            await LinkGoogleLoginAsync(user, profile);

            if (referrer is not null)
            {
                var referral = await _referralService.RecordAsync(
                    referrer, user, referrer.ReferralCode!, cancellationToken);

                referralId = referral.Id;
            }

            await transaction.CommitAsync(cancellationToken);
        }

        await _notificationService.CreateAsync(
            user.Id,
            "مرحبًا بك",
            "🎊 تم إنشاء حسابك بنجاح. أهلًا بك في سوق الفيوم.",
            NotificationType.AccountCreated,
            referenceId: null,
            referenceType: NotificationReferenceTypes.Account,
            action: NotificationAction.Created,
            icon: "🎊",
            entityName: "الحساب");

        if (referralId is not null)
        {
            try
            {
                await _referralService.NotifyReferrerAsync(referralId.Value, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Referral {ReferralId} was recorded but its notification could not be sent.",
                    referralId);
            }
        }

        return user;
    }

    private async Task LinkGoogleLoginAsync(ApplicationUser user, GoogleUserProfile profile)
    {
        var login = new UserLoginInfo(
            GoogleAuthCatalog.ProviderName, profile.Subject, GoogleAuthCatalog.ProviderDisplayName);

        IdentityResult result;

        try
        {
            result = await _userManager.AddLoginAsync(user, login);
        }
        catch (Exception exception) when (_unitOfWork.IsUniqueConstraintViolation(exception))
        {
            _logger.LogInformation(
                exception,
                "The external login for user {UserId} was already stored; treating it as linked.",
                user.Id);

            return;
        }

        if (result.Succeeded)
            return;

        var alreadyLinked = await _userManager.GetLoginsAsync(user);

        if (alreadyLinked.Any(stored =>
                stored.LoginProvider == GoogleAuthCatalog.ProviderName &&
                stored.ProviderKey == profile.Subject))
        {
            return;
        }

        _logger.LogWarning(
            "Could not link the external login to user {UserId}: {Errors}",
            user.Id, string.Join("; ", DescribeErrors(result)));

        throw new ConflictException(UserMessages.Auth.GoogleSignInRetry);
    }

    private async Task<string> ReserveUsernameAsync(string? fullName, string email)
    {
        var baseName = GoogleAuthCatalog.SuggestUsername(fullName, email);

        for (var attempt = 0; attempt < GoogleAuthCatalog.MaxUsernameAttempts; attempt++)
        {
            var candidate = GoogleAuthCatalog.UsernameCandidate(baseName, attempt);

            if (await _userManager.FindByNameAsync(candidate) is null)
                return candidate;
        }

        throw new ConflictException(UserMessages.Auth.GoogleSignInRetry);
    }

    private async Task EnsureGoogleAccountUsableAsync(ApplicationUser user)
    {
        if (await _userManager.IsLockedOutAsync(user))
        {
            _logger.LogWarning("External sign-in attempt on locked-out account {UserId}.", user.Id);
            throw new UnauthorizedException(UserMessages.Auth.AccountLocked);
        }

        EnsureAccountUsable(user);

        if (await _userManager.GetAccessFailedCountAsync(user) > 0)
            await _userManager.ResetAccessFailedCountAsync(user);
    }

    private Task NotifyNewLoginAsync(ApplicationUser user) =>
        _notificationService.CreateAsync(
            user.Id,
            "تسجيل دخول جديد",
            "🛡️ تم تسجيل دخول جديد إلى حسابك.",
            NotificationType.NewLogin,
            referenceId: null,
            referenceType: NotificationReferenceTypes.Account,
            action: NotificationAction.Created,
            icon: "🛡️",
            entityName: "الحساب");

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = await _userRepository.FindByRefreshTokenAsync(request.RefreshToken)
            ?? throw new UnauthorizedException("رمز التحديث مش صحيح أو انتهت صلاحيته.");

        if (user.RefreshTokenExpiryTime is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new UnauthorizedException("رمز التحديث مش صحيح أو انتهت صلاحيته.");

        EnsureAccountUsable(user);

        if (!string.IsNullOrWhiteSpace(request.Token))
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token)
                ?? throw new UnauthorizedException("رمز الدخول مش صحيح.");

            var tokenUserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(tokenUserId) ||
                !string.Equals(tokenUserId, user.Id, StringComparison.Ordinal))
            {
                throw new UnauthorizedException("رمز الدخول مش صحيح.");
            }
        }

        return await BuildAuthResponseAsync(user);
    }

    public async Task<PasswordResetSessionDto> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var sessionToken = GenerateSessionToken();

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new PasswordResetSessionDto { ResetToken = sessionToken };

        var otp = GenerateNumericOtp();

        user.PasswordResetOtpHash = Sha256Hex(otp);
        user.PasswordResetOtpExpiry = DateTime.UtcNow.AddMinutes(OtpExpiryMinutes);
        user.PasswordResetTokenHash = Sha256Hex(sessionToken);
        user.PasswordResetVerified = false;
        user.PasswordResetVerifiedExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            throw new BadRequestException("مش قادرين ننفذ الطلب. حاول تاني.", DescribeErrors(updateResult));

        try
        {
            await _emailService.SendPasswordResetOtpAsync(user.Email!, user.UserName!, otp, OtpExpiryMinutes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset OTP email to user {UserId}.", user.Id);
        }

        return new PasswordResetSessionDto { ResetToken = sessionToken };
    }

    public async Task VerifyResetCodeAsync(string? resetToken, VerifyResetCodeRequest request)
    {
        var user = await GetResetSessionUserAsync(resetToken);

        EnsureOtpValid(user, request.Otp);

        user.PasswordResetOtpHash = null;
        user.PasswordResetOtpExpiry = null;
        user.PasswordResetVerified = true;
        user.PasswordResetVerifiedExpiry = DateTime.UtcNow.AddMinutes(VerifiedSessionExpiryMinutes);
        user.UpdatedAt = DateTime.UtcNow;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            throw new BadRequestException("مش قادرين نتأكد من كود التحقق. حاول تاني.", DescribeErrors(updateResult));
    }

    public async Task ResetPasswordAsync(string? resetToken, ResetPasswordRequest request)
    {
        var user = await GetResetSessionUserAsync(resetToken);

        if (!user.PasswordResetVerified ||
            user.PasswordResetVerifiedExpiry is null ||
            user.PasswordResetVerifiedExpiry <= DateTime.UtcNow)
        {
            throw new UnauthorizedException(
                UserMessages.Auth.ResetSessionExpired);
        }

        var identityResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, identityResetToken, request.NewPassword);
        if (!result.Succeeded)
            throw new BadRequestException("مش قادرين نغير كلمة السر. حاول تاني.", DescribeErrors(result));

        user.PasswordResetOtpHash = null;
        user.PasswordResetOtpExpiry = null;
        user.PasswordResetTokenHash = null;
        user.PasswordResetVerified = false;
        user.PasswordResetVerifiedExpiry = null;
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _userManager.UpdateSecurityStampAsync(user);
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            throw new BadRequestException("مش قادرين نغير كلمة السر. حاول تاني.", DescribeErrors(updateResult));

        await NotifyPasswordChangedAsync(_notificationService, user.Id);
    }

    internal static Task NotifyPasswordChangedAsync(INotificationService notifications, string userId) =>
        notifications.CreateAsync(
            userId,
            "تغيير كلمة المرور",
            "🔒 تم تغيير كلمة المرور الخاصة بحسابك.",
            NotificationType.PasswordChanged,
            referenceId: null,
            referenceType: NotificationReferenceTypes.Account,
            action: NotificationAction.Updated,
            icon: "🔒",
            entityName: "الحساب");

    private async Task<ApplicationUser> GetResetSessionUserAsync(string? resetToken)
    {
        if (string.IsNullOrWhiteSpace(resetToken))
            throw new UnauthorizedException("رمز جلسة إعادة تعيين كلمة السر مطلوب.");

        var user = await _userRepository.FindByPasswordResetTokenHashAsync(Sha256Hex(resetToken));
        return user ?? throw new UnauthorizedException("جلسة إعادة تعيين كلمة السر مش صحيحة أو انتهت صلاحيتها.");
    }

    public async Task LogoutAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("المستخدم مش موجود.");

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new BadRequestException("مش قادرين نسجل خروجك. حاول تاني.", DescribeErrors(result));
    }

    private async Task<AuthResponse> BuildAuthResponseAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiry;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new BadRequestException("مش قادرين نصدر رموز الدخول. حاول تاني.", DescribeErrors(result));

        return new AuthResponse
        {
            Token = accessToken.Token,
            RefreshToken = refreshToken,
            Expiration = accessToken.ExpiresAt,
            RefreshTokenExpiration = refreshTokenExpiry,
            User = _mapper.Map<UserDto>(user)
        };
    }

    private void EnsureOtpValid(ApplicationUser user, string otp)
    {
        if (string.IsNullOrEmpty(user.PasswordResetOtpHash) || user.PasswordResetOtpExpiry is null)
            throw new BadRequestException("كود التحقق مش صحيح أو انتهت صلاحيته.");

        if (user.PasswordResetOtpExpiry <= DateTime.UtcNow)
            throw new BadRequestException("كود التحقق انتهت صلاحيته.");

        var providedHash = Sha256Hex(otp);
        var storedHash = user.PasswordResetOtpHash;

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(providedHash),
                Encoding.UTF8.GetBytes(storedHash)))
        {
            throw new BadRequestException("كود التحقق مش صحيح.");
        }
    }

    private static string GenerateNumericOtp()
    {
        var value = RandomNumberGenerator.GetInt32(0, 1_000_000);
        return value.ToString("D6");
    }

    private static string GenerateSessionToken() =>
        Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

    private static string Sha256Hex(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }

    private static IReadOnlyList<string> DescribeErrors(IdentityResult result) =>
        result.Errors.Select(e => e.Description).ToList();
}
