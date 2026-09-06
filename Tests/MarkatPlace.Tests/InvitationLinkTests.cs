using Domain.Entities;
using MarkatPlace.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Persistence.Services;
using Services.Referrals;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Notifications;
using Shared.DTOs.Referrals;
using Shared.Enums;
using Shared.Responses;
using Shared.Settings;
using ServicesAbstraction;
using Xunit;

namespace MarkatPlace.Tests;

public class InvitationLinkTests
{
    private const string FrontendUrl = "https://shopiklopik.com";

    private static ReferralLinkBuilder BuildLinks(AppSettings settings) =>
        new(Options.Create(settings));

    private static AppSettings Configured() => new()
    {
        BaseUrl = "https://api.shopiklopik.com",
        FrontendUrl = FrontendUrl
    };

    [Fact]
    public void An_invitation_link_points_at_the_frontend_domain()
    {
        var link = BuildLinks(Configured()).Build("ABCD2345");

        Assert.StartsWith("https://shopiklopik.com/", link, StringComparison.Ordinal);
        Assert.Equal("https://shopiklopik.com/register?ref=ABCD2345", link);
    }

    [Fact]
    public void An_invitation_link_never_points_at_the_API_host()
    {
        var link = BuildLinks(Configured()).Build("ABCD2345");

        Assert.DoesNotContain("api.shopiklopik.com", link, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void An_invitation_link_never_points_at_localhost()
    {
        var settings = Configured();
        settings.BaseUrl = "https://localhost:7139";

        var link = BuildLinks(settings).Build("ABCD2345");

        Assert.DoesNotContain("localhost", link, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("127.0.0.1", link, StringComparison.Ordinal);
    }

    [Fact]
    public void A_trailing_slash_on_the_configured_frontend_url_does_not_double_up()
    {
        var settings = Configured();
        settings.FrontendUrl = "https://shopiklopik.com/";

        Assert.Equal("https://shopiklopik.com/register?ref=ABCD2345", BuildLinks(settings).Build("ABCD2345"));
    }

    [Fact]
    public void The_register_path_and_query_parameter_stay_configurable()
    {
        var settings = Configured();
        settings.RegisterPath = "auth/signup";
        settings.ReferralQueryParameter = "invite";

        Assert.Equal("https://shopiklopik.com/auth/signup?invite=ABCD2345",
            BuildLinks(settings).Build("ABCD2345"));
    }

    [Fact]
    public void An_existing_query_string_on_the_register_path_is_preserved()
    {
        var settings = Configured();
        settings.RegisterPath = "/register?source=invite";

        Assert.Equal("https://shopiklopik.com/register?source=invite&ref=ABCD2345",
            BuildLinks(settings).Build("ABCD2345"));
    }

    [Fact]
    public void A_referral_code_is_escaped_into_the_query_string()
    {
        var settings = Configured();

        Assert.Equal("https://shopiklopik.com/register?ref=A%20B%26C", BuildLinks(settings).Build("A B&C"));
    }

    [Fact]
    public void Building_a_link_without_any_configured_site_address_fails_loudly()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => BuildLinks(new AppSettings()).Build("ABCD2345"));

        Assert.Contains("FrontendUrl", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_link_is_not_derived_from_the_incoming_request()
    {
        var source = RepositoryRoot.ReadFile("Infastrucre/Presitance/Services/ReferralLinkBuilder.cs");

        Assert.DoesNotContain("HttpContext", source, StringComparison.Ordinal);
        Assert.DoesNotContain("request.Host", source, StringComparison.Ordinal);
    }

    [Fact]
    public void The_committed_configuration_sends_invitations_to_the_frontend()
    {
        foreach (var file in new[] { "MarkatPlace/appsettings.json", "MarkatPlace/appsettings.Production.json" })
        {
            var frontendUrl = ReadJson(file)["App:FrontendUrl"];

            Assert.Equal(FrontendUrl, frontendUrl);
        }
    }

    [Fact]
    public void Production_start_up_refuses_a_missing_or_unusable_frontend_url()
    {
        Assert.Throws<InvalidOperationException>(() => Validate("Production", frontendUrl: null));
        Assert.Throws<InvalidOperationException>(() => Validate("Production", "shopiklopik.com"));
        Assert.Throws<InvalidOperationException>(() => Validate("Production", "http://localhost:3000"));
        Assert.Throws<InvalidOperationException>(() => Validate("Production", "ftp://shopiklopik.com"));

        Validate("Production", FrontendUrl);
    }

    [Fact]
    public void The_production_configuration_passes_start_up_validation()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(RepositoryRoot.Path)
            .AddJsonFile("MarkatPlace/appsettings.json", optional: false)
            .AddJsonFile("MarkatPlace/appsettings.Production.json", optional: false)
            .Build();

        new ServiceCollection().AddAppUrlSettings(configuration, new StubEnvironment("Production"));
    }

    [Fact]
    public async Task Reading_my_invitation_creates_a_code_and_returns_a_frontend_link()
    {
        var user = ActiveUser("user-1");
        var repository = new FakeReferralRepository(user);
        var service = BuildService(repository);

        var result = await service.GetMineAsync(user.Id);

        Assert.False(string.IsNullOrWhiteSpace(result.ReferralCode));
        Assert.Equal(ReferralCatalog.CodeLength, result.ReferralCode.Length);
        Assert.Equal($"{FrontendUrl}/register?ref={result.ReferralCode}", result.ReferralLink);
        Assert.StartsWith("https://shopiklopik.com/", result.ReferralLink, StringComparison.Ordinal);
        Assert.DoesNotContain("api.shopiklopik.com", result.ReferralLink, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("localhost", result.ReferralLink, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task An_existing_invitation_code_keeps_working_and_keeps_its_link()
    {
        var user = ActiveUser("user-1");
        user.ReferralCode = "LEGACY12";

        var service = BuildService(new FakeReferralRepository(user));

        var result = await service.GetMineAsync(user.Id);

        Assert.Equal("LEGACY12", result.ReferralCode);
        Assert.Equal($"{FrontendUrl}/register?ref=LEGACY12", result.ReferralLink);
    }

    [Fact]
    public async Task An_existing_invitation_code_still_validates()
    {
        var referrer = ActiveUser("user-1");
        referrer.ReferralCode = "LEGACY12";

        var service = BuildService(new FakeReferralRepository(referrer));

        var resolution = await service.ResolveAsync("legacy12");

        Assert.True(resolution.Valid);
        Assert.Equal("LEGACY12", resolution.ReferralCode);
    }

    [Fact]
    public async Task An_unknown_invitation_code_is_rejected()
    {
        var service = BuildService(new FakeReferralRepository(ActiveUser("user-1")));

        Assert.False((await service.ResolveAsync("NOTACODE")).Valid);
        Assert.False((await service.ResolveAsync("")).Valid);
        Assert.False((await service.ResolveAsync(new string('A', ReferralCatalog.MaxCodeLength + 1))).Valid);
    }

    [Fact]
    public async Task An_invitation_from_a_suspended_account_is_rejected()
    {
        var referrer = ActiveUser("user-1");
        referrer.ReferralCode = "LEGACY12";
        referrer.Status = UserAccountStatus.Suspended;

        var service = BuildService(new FakeReferralRepository(referrer));

        Assert.False((await service.ResolveAsync("LEGACY12")).Valid);
    }

    [Fact]
    public async Task Nobody_can_use_their_own_invitation_link()
    {
        var referrer = ActiveUser("user-1");
        referrer.ReferralCode = "LEGACY12";

        var service = BuildService(new FakeReferralRepository(referrer));

        Assert.False((await service.ResolveAsync("LEGACY12", referrer.Id)).Valid);
    }

    [Fact]
    public void An_invitation_link_is_never_stored_as_an_absolute_url()
    {
        var referral = typeof(Referral).GetProperties().Select(property => property.Name);
        var linkEvent = typeof(ReferralLinkEvent).GetProperties().Select(property => property.Name);

        foreach (var name in referral.Concat(linkEvent))
        {
            Assert.False(name.EndsWith("Url", StringComparison.Ordinal) ||
                         name.EndsWith("Link", StringComparison.Ordinal),
                $"{name} looks like a persisted invitation URL. Links are built from the code on " +
                "every read so that changing App:FrontendUrl fixes every existing invitation.");
        }
    }

    private static void Validate(string environmentName, string? frontendUrl)
    {
        var values = new Dictionary<string, string?> { ["App:FrontendUrl"] = frontendUrl };

        new ServiceCollection().AddAppUrlSettings(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build(),
            new StubEnvironment(environmentName));
    }

    private static IConfigurationRoot ReadJson(string relativePath) =>
        new ConfigurationBuilder()
            .SetBasePath(RepositoryRoot.Path)
            .AddJsonFile(relativePath, optional: false)
            .Build();

    private static ReferralService BuildService(FakeReferralRepository repository) =>
        new(repository, BuildLinks(Configured()), new UnusedNotificationService(),
            NullLogger<ReferralService>.Instance);

    private static ApplicationUser ActiveUser(string id) => new()
    {
        Id = id,
        UserName = id,
        FirstName = "منى",
        SecondName = "سيد",
        Status = UserAccountStatus.Active
    };

    private sealed class StubEnvironment : IHostEnvironment
    {
        public StubEnvironment(string environmentName) => EnvironmentName = environmentName;

        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; } = "MarkatPlace.Tests";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }

    private sealed class FakeReferralRepository : IReferralRepository
    {
        private readonly List<ApplicationUser> _users;

        public FakeReferralRepository(params ApplicationUser[] users) => _users = [.. users];

        public List<ReferralLinkEvent> LinkEvents { get; } = [];

        public Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken = default) =>
            Task.FromResult(_users.Any(user =>
                string.Equals(user.ReferralCode, code, StringComparison.Ordinal)));

        public Task<ApplicationUser?> FindUserByCodeAsync(
            string code, CancellationToken cancellationToken = default) =>
            Task.FromResult(_users.FirstOrDefault(user =>
                string.Equals(user.ReferralCode, code, StringComparison.Ordinal)));

        public Task<ApplicationUser?> FindUserAsync(
            string userId, CancellationToken cancellationToken = default) =>
            Task.FromResult(_users.FirstOrDefault(user =>
                string.Equals(user.Id, userId, StringComparison.Ordinal)));

        public Task AddLinkEventAsync(
            ReferralLinkEvent linkEvent, CancellationToken cancellationToken = default)
        {
            LinkEvents.Add(linkEvent);
            return Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(1);

        public Task<IReadOnlyDictionary<ReferralStatus, int>> CountByStatusAsync(
            string referrerUserId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyDictionary<ReferralStatus, int>>(
                new Dictionary<ReferralStatus, int>());

        public Task<IReadOnlyDictionary<ReferralLinkEventType, int>> CountLinkEventsAsync(
            string referrerUserId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyDictionary<ReferralLinkEventType, int>>(
                new Dictionary<ReferralLinkEventType, int>());

        public Task AddAsync(Referral referral, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<Referral?> FindByReferredUserAsync(
            string referredUserId, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<Referral?> FindAsync(Guid id, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<IReadOnlyDictionary<ReferralStatus, int>> CountAllByStatusAsync(
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<int> CountSinceAsync(
            string referrerUserId, DateTime fromUtc, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<int> CountAllSinceAsync(
            DateTime fromUtc, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<int> CountDistinctReferrersAsync(CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<(IReadOnlyList<ReferredUserDto> Items, int Total)> GetReferredUsersAsync(
            string referrerUserId, MyReferralFilterParams filter,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<(IReadOnlyList<AdminReferralDto> Items, int Total)> GetForAdminAsync(
            AdminReferralFilterParams filter, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<AdminReferralDto?> GetAdminReferralAsync(
            Guid id, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<IReadOnlyList<AdminTopReferrerDto>> GetTopReferrersAsync(
            int count, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<(IReadOnlyList<AdminReferrerDto> Items, int Total)> GetReferrersForAdminAsync(
            AdminReferrerFilterParams filter, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<IReadOnlyDictionary<ReferralLinkEventType, int>> CountAllLinkEventsAsync(
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }

    private sealed class UnusedNotificationService : INotificationService
    {
        public Task NotifyAsync(
            string userId, ListingModuleType module, NotificationAction action,
            Guid entityId, string? entityTitle = null) => throw new NotSupportedException();

        public Task NotifyAsync(
            string userId, NotificationSubject subject, NotificationAction action,
            Guid? entityId = null, string? entityTitle = null) => throw new NotSupportedException();

        public Task<NotificationDto> CreateAsync(
            string userId, NotificationContent content, Guid? referenceId = null,
            NotificationAction action = NotificationAction.Created) => throw new NotSupportedException();

        public Task<int> CreateManyAsync(
            IReadOnlyCollection<string> userIds, NotificationContent content, Guid? referenceId = null,
            NotificationAction action = NotificationAction.Created,
            CancellationToken cancellationToken = default) => throw new NotSupportedException();

        public Task<NotificationDto> CreateAsync(
            string userId, string title, string message, NotificationType type, Guid? referenceId = null,
            string? referenceType = null, NotificationAction action = NotificationAction.Created,
            string? icon = null, string? entityName = null, string? deepLink = null,
            ListingModuleType? listingType = null, int? categoryId = null, int? subCategoryId = null,
            string? imageUrl = null) => throw new NotSupportedException();

        public Task<bool> CreateIfNotExistsAsync(
            string userId, NotificationContent content, Guid? referenceId,
            DateTime? createdAfterUtc = null, NotificationAction action = NotificationAction.Created,
            CancellationToken cancellationToken = default) => throw new NotSupportedException();

        public Task<int> CreateManyIfNotExistAsync(
            IReadOnlyCollection<string> userIds, NotificationContent content, Guid? referenceId,
            DateTime? createdAfterUtc = null, NotificationAction action = NotificationAction.Created,
            CancellationToken cancellationToken = default) => throw new NotSupportedException();

        public Task<bool> CreateIfNotExistsAsync(
            string userId, string title, string message, NotificationType type, Guid? referenceId,
            string? referenceType = null, DateTime? createdAfterUtc = null,
            NotificationAction action = NotificationAction.Created, string? icon = null,
            string? entityName = null, string? deepLink = null) => throw new NotSupportedException();

        public Task<PaginatedResult<NotificationDto>> GetMyNotificationsAsync(
            string userId, NotificationFilterParams filter,
            CancellationToken cancellationToken = default) => throw new NotSupportedException();

        public Task<int> GetUnreadCountAsync(
            string userId, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task MarkAsReadAsync(string userId, Guid notificationId) =>
            throw new NotSupportedException();

        public Task<int> MarkAllAsReadAsync(string userId) => throw new NotSupportedException();

        public Task DeleteAsync(string userId, Guid notificationId) => throw new NotSupportedException();

        public Task<int> DeleteAllAsync(string userId) => throw new NotSupportedException();
    }
}
