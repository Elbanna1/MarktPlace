using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Persistence.Data;
using Persistence.Repositories;
using Services;
using Services.Mapping;
using Services.Referrals;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Notifications;
using Shared.DTOs.Referrals;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

internal sealed class GoogleSignInHarness : IAsyncDisposable
{
    private readonly ServiceProvider _provider;

    private GoogleSignInHarness(ServiceProvider provider)
    {
        _provider = provider;

        Context = provider.GetRequiredService<AppDbContext>();
        Users = provider.GetRequiredService<UserManager<ApplicationUser>>();
        Notifications = (RecordingNotificationService)provider.GetRequiredService<INotificationService>();
        Google = (StubGoogleTokenValidator)provider.GetRequiredService<IGoogleTokenValidator>();
        Tokens = (StubTokenService)provider.GetRequiredService<ITokenService>();
        Referrals = provider.GetRequiredService<IReferralService>();
        Auth = provider.GetRequiredService<IAuthService>();
    }

    public AppDbContext Context { get; }

    public UserManager<ApplicationUser> Users { get; }

    public RecordingNotificationService Notifications { get; }

    public StubGoogleTokenValidator Google { get; }

    public StubTokenService Tokens { get; }

    public IReferralService Referrals { get; }

    public IAuthService Auth { get; }

    public static GoogleSignInHarness Create(GoogleAuthSettings? settings = null)
    {
        var services = new ServiceCollection();
        var databaseName = Guid.NewGuid().ToString();

        services.AddLogging();
        services.AddDataProtection();

        services.AddDbContext<AppDbContext>(options => options
            .UseInMemoryDatabase(databaseName)
            .ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId
                    .TransactionIgnoredWarning)));

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = string.Empty;

            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddSingleton<IMapper>(new MapperConfiguration(
            configuration => configuration.AddProfile<MappingProfile>(),
            NullLoggerFactory.Instance).CreateMapper());

        services.Configure<JwtSettings>(options =>
        {
            options.Issuer = "MarkatPlaceAPI";
            options.Audience = "MarkatPlaceClient";
            options.SecretKey = new string('t', 64);
            options.AccessTokenExpirationDays = 40;
            options.RefreshTokenExpirationDays = 60;
        });

        var google = settings ?? new GoogleAuthSettings
        {
            ClientId = "test-client-id.apps.googleusercontent.com"
        };

        services.AddSingleton(Options.Create(google));

        services.AddSingleton<ITokenService, StubTokenService>();
        services.AddSingleton<IEmailService, StubEmailService>();
        services.AddSingleton<INotificationService, RecordingNotificationService>();
        services.AddSingleton<IGoogleTokenValidator, StubGoogleTokenValidator>();
        services.AddSingleton<IUnitOfWork, FakeUnitOfWork>();
        services.AddSingleton<IReferralLinkBuilder, StubReferralLinkBuilder>();

        services.AddScoped<IUserRepository, StubUserRepository>();
        services.AddScoped<IReferralRepository, ReferralRepository>();
        services.AddScoped<IReferralService, ReferralService>();
        services.AddScoped<IAuthService, AuthService>();

        return new GoogleSignInHarness(services.BuildServiceProvider());
    }

    public async Task<ApplicationUser> AddPasswordUserAsync(
        string username, string email, string? password = "Passw0rd!",
        UserAccountStatus status = UserAccountStatus.Active,
        string? referralCode = null)
    {
        var user = new ApplicationUser
        {
            FirstName = "اسم",
            SecondName = "تاني",
            UserName = username,
            Email = email,
            PhoneNumber = null,
            Governorate = LocationConstants.Governorate,
            Center = LocationConstants.Centers[0],
            CreatedAt = DateTime.UtcNow,
            Status = status,
            ReferralCode = referralCode
        };

        var result = password is null
            ? await Users.CreateAsync(user)
            : await Users.CreateAsync(user, password);

        Assert.True(result.Succeeded, string.Join("; ", result.Errors.Select(e => e.Description)));

        return user;
    }

    public Task<int> CountReferralsAsync(string referrerUserId) =>
        Context.Set<Referral>().CountAsync(referral => referral.ReferrerUserId == referrerUserId);

    public ValueTask DisposeAsync() => _provider.DisposeAsync();
}

internal sealed class StubGoogleTokenValidator : IGoogleTokenValidator
{
    private readonly GoogleAuthSettings _settings;

    public StubGoogleTokenValidator(IOptions<GoogleAuthSettings> settings)
    {
        _settings = settings.Value;
    }

    public Dictionary<string, GoogleUserProfile> Credentials { get; } = [];

    public HashSet<string> ExpiredCredentials { get; } = [];

    public int ValidationCount { get; private set; }

    public bool IsConfigured => _settings.IsConfigured;

    public string? ClientId => _settings.ClientId;

    public GoogleUserProfile Register(
        string credential,
        string subject,
        string email,
        bool emailVerified = true,
        string? name = null,
        string? givenName = null,
        string? familyName = null)
    {
        var profile = new GoogleUserProfile(
            subject, email, emailVerified, name, givenName, familyName, null);

        Credentials[credential] = profile;

        return profile;
    }

    public Task<GoogleUserProfile> ValidateAsync(
        Shared.DTOs.Auth.GoogleSignInRequest request, CancellationToken cancellationToken = default)
    {
        ValidationCount++;

        if (!_settings.IsConfigured)
            throw new BadRequestException(UserMessages.Auth.GoogleNotAvailable);

        var credential = request.IdToken ?? request.Code;

        if (string.IsNullOrWhiteSpace(credential))
            throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialRequired);

        if (ExpiredCredentials.Contains(credential))
            throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialInvalid);

        return Credentials.TryGetValue(credential, out var profile)
            ? Task.FromResult(profile)
            : throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialInvalid);
    }
}

internal sealed class StubTokenService : ITokenService
{
    public List<(string UserId, IReadOnlyList<string> Roles)> Issued { get; } = [];

    public Shared.DTOs.Auth.AccessTokenResult GenerateAccessToken(
        ApplicationUser user, IEnumerable<string>? roles = null)
    {
        var claimedRoles = roles?.ToList() ?? [];
        Issued.Add((user.Id, claimedRoles));

        var token = string.Join('.', "header", user.Id, string.Join(',', claimedRoles));

        return new Shared.DTOs.Auth.AccessTokenResult(token, DateTime.UtcNow.AddDays(40));
    }

    public string GenerateRefreshToken() => Guid.NewGuid().ToString("N");

    public System.Security.Claims.ClaimsPrincipal? GetPrincipalFromExpiredToken(string token) => null;
}

internal sealed class StubEmailService : IEmailService
{
    public Task SendAsync(
        string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task SendPasswordResetOtpAsync(
        string toEmail, string userName, string otp, int expiryMinutes,
        CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}

internal sealed class StubReferralLinkBuilder : IReferralLinkBuilder
{
    public string Build(string referralCode) => $"https://shopiklopik.com/register?ref={referralCode}";
}

internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public int Committed { get; private set; }

    public int RolledBack { get; private set; }

    public Task<ITransactionScope> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<ITransactionScope>(new Scope(this));

    public bool IsUniqueConstraintViolation(Exception exception) => false;

    private sealed class Scope : ITransactionScope
    {
        private readonly FakeUnitOfWork _owner;

        public Scope(FakeUnitOfWork owner)
        {
            _owner = owner;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            _owner.Committed++;
            return Task.CompletedTask;
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            _owner.RolledBack++;
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}

internal sealed class StubUserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public StubUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> IsPhoneNumberTakenAsync(
        string phoneNumber, string? excludeUserId = null, CancellationToken cancellationToken = default) =>
        _context.Users.AnyAsync(
            user => user.PhoneNumber == phoneNumber && user.Id != excludeUserId, cancellationToken);

    public Task<ApplicationUser?> FindByPasswordResetTokenHashAsync(string tokenHash) =>
        _context.Users.FirstOrDefaultAsync(user => user.PasswordResetTokenHash == tokenHash);

    public Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken) =>
        _context.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);

    public async Task<IReadOnlyList<string>> GetUserIdsInRoleAsync(
        string roleName, CancellationToken cancellationToken = default) =>
        await (from role in _context.Roles
               join membership in _context.UserRoles on role.Id equals membership.RoleId
               where role.Name == roleName
               select membership.UserId).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<string>> GetRoleNamesAsync(
        string userId, CancellationToken cancellationToken = default) =>
        await (from membership in _context.UserRoles
               join role in _context.Roles on membership.RoleId equals role.Id
               where membership.UserId == userId
               select role.Name!).ToListAsync(cancellationToken);

    public async Task<UserDisplayName?> GetDisplayNameAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            candidate => candidate.Id == userId, cancellationToken);

        return user is null ? null : new UserDisplayName(user.FirstName, user.SecondName, user.UserName);
    }
}

internal sealed class RecordingNotificationService : INotificationService
{
    public List<(string UserId, NotificationType Type, string Title, string Message)> Created { get; } = [];

    public Task NotifyAsync(
        string userId, ListingModuleType module, NotificationAction action, Guid entityId,
        string? entityTitle = null) => Task.CompletedTask;

    public Task NotifyAsync(
        string userId, NotificationSubject subject, NotificationAction action, Guid? entityId = null,
        string? entityTitle = null) => Task.CompletedTask;

    public Task<NotificationDto> CreateAsync(
        string userId, NotificationContent content, Guid? referenceId = null,
        NotificationAction action = NotificationAction.Created)
    {
        Created.Add((userId, content.Type, content.Title, content.Message));

        return Task.FromResult(new NotificationDto
        {
            Id = Guid.NewGuid(), Title = content.Title, Message = content.Message
        });
    }

    public Task<int> CreateManyAsync(
        IReadOnlyCollection<string> userIds, NotificationContent content, Guid? referenceId = null,
        NotificationAction action = NotificationAction.Created,
        CancellationToken cancellationToken = default) => Task.FromResult(0);

    public Task<NotificationDto> CreateAsync(
        string userId, string title, string message, NotificationType type, Guid? referenceId = null,
        string? referenceType = null, NotificationAction action = NotificationAction.Created,
        string? icon = null, string? entityName = null, string? deepLink = null,
        ListingModuleType? listingType = null, int? categoryId = null, int? subCategoryId = null,
        string? imageUrl = null)
    {
        Created.Add((userId, type, title, message));
        return Task.FromResult(new NotificationDto { Id = Guid.NewGuid(), Title = title, Message = message });
    }

    public Task<bool> CreateIfNotExistsAsync(
        string userId, NotificationContent content, Guid? referenceId, DateTime? createdAfterUtc = null,
        NotificationAction action = NotificationAction.Created,
        CancellationToken cancellationToken = default)
    {
        Created.Add((userId, content.Type, content.Title, content.Message));
        return Task.FromResult(true);
    }

    public Task<int> CreateManyIfNotExistAsync(
        IReadOnlyCollection<string> userIds, NotificationContent content, Guid? referenceId,
        DateTime? createdAfterUtc = null, NotificationAction action = NotificationAction.Created,
        CancellationToken cancellationToken = default) => Task.FromResult(0);

    public Task<bool> CreateIfNotExistsAsync(
        string userId, string title, string message, NotificationType type, Guid? referenceId,
        string? referenceType = null, DateTime? createdAfterUtc = null,
        NotificationAction action = NotificationAction.Created, string? icon = null,
        string? entityName = null, string? deepLink = null)
    {
        Created.Add((userId, type, title, message));
        return Task.FromResult(true);
    }

    public Task<PaginatedResult<NotificationDto>> GetMyNotificationsAsync(
        string userId, NotificationFilterParams filter, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PaginatedResult<NotificationDto>([], 0, 1, 10));

    public Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default) =>
        Task.FromResult(0);

    public Task MarkAsReadAsync(string userId, Guid notificationId) => Task.CompletedTask;

    public Task<int> MarkAllAsReadAsync(string userId) => Task.FromResult(0);

    public Task DeleteAsync(string userId, Guid notificationId) => Task.CompletedTask;

    public Task<int> DeleteAllAsync(string userId) => Task.FromResult(0);
}
