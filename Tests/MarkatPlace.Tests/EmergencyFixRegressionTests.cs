using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Persistence.Services;
using Services;
using Services.Admin;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Notifications;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class EmergencyFixRegressionTests
{
    [Fact]
    public void Governorate_directory_category_keeps_its_id_and_takes_the_new_name()
    {
        var category = CategoryCatalog.FindCategory(CategoryType.WorkshopsAndCraftsmen)!;

        Assert.Equal(2, (int)category.Id);
        Assert.Equal("دليل محافظتي", category.NameAr);
        Assert.DoesNotContain(CategoryCatalog.Categories, entry => entry.NameAr == "الورش والحرفيين");
    }

    [Fact]
    public void Farms_sub_category_keeps_its_id_and_parent_and_takes_the_new_name()
    {
        var farms = CategoryCatalog.FindSubCategory(SubCategoryType.Farms)!;

        Assert.Equal(10, (int)farms.Id);
        Assert.Equal(CategoryType.Business, farms.Category);
        Assert.Equal("مزارع ومحاصيل", farms.NameAr);
    }

    [Fact]
    public void Rename_migration_updates_the_existing_rows_in_place()
    {
        var file = Directory.GetFiles(
                Path.Combine(RepositoryRoot.Path, "Infastrucre", "Presitance", "Migrations"),
                "*_RenameWorkshopsCategoryAndFarmsSubCategory.cs")
            .Single(path => !path.EndsWith(".Designer.cs", StringComparison.Ordinal));
        var up = File.ReadAllText(file).Split("protected override void Down")[0];

        Assert.Contains("table: \"Categories\"", up);
        Assert.Contains("value: \"دليل محافظتي\"", up);
        Assert.Contains("table: \"SubCategories\"", up);
        Assert.Contains("value: \"مزارع ومحاصيل\"", up);
        Assert.DoesNotContain("DropTable", up);
        Assert.DoesNotContain("DeleteData", up);
        Assert.DoesNotContain("InsertData", up);
    }

    [Fact]
    public void Uploads_check_finds_existing_files_and_reports_missing_ones()
    {
        var root = Path.Combine(Path.GetTempPath(), "uploads-check-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "hospitals"));
        File.WriteAllBytes(Path.Combine(root, "hospitals", "present.png"), [1]);

        try
        {
            Assert.True(UploadsDiagnostics.Exists(root, "/uploads/hospitals/present.png"));
            Assert.True(UploadsDiagnostics.Exists(root, "uploads\\hospitals\\present.png"));
            Assert.False(UploadsDiagnostics.Exists(root, "/uploads/hospitals/missing.png"));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void Uploads_check_never_looks_outside_the_uploads_root()
    {
        var parent = Path.Combine(Path.GetTempPath(), "uploads-check-" + Guid.NewGuid().ToString("N"));
        var root = Path.Combine(parent, "uploads");
        Directory.CreateDirectory(root);
        File.WriteAllBytes(Path.Combine(parent, "secret.txt"), [1]);

        try
        {
            Assert.False(UploadsDiagnostics.Exists(root, "/uploads/../secret.txt"));
            Assert.False(UploadsDiagnostics.Exists(root, "../secret.txt"));
        }
        finally
        {
            Directory.Delete(parent, recursive: true);
        }
    }

    [Fact]
    public async Task Cached_access_state_without_the_admin_role_is_denied_without_a_database_call()
    {
        var service = new AdminPermissionService(
            null!, null!, null!, Cache("u1", UserAccountStatus.Active, "Customer"));

        var result = await service.AuthorizeAdminAsync("u1");

        Assert.False(result.Allowed);
    }

    [Fact]
    public async Task Cached_active_super_admin_is_allowed_without_a_database_call()
    {
        var service = new AdminPermissionService(
            null!, null!, null!, Cache("u1", UserAccountStatus.Active, AppRoles.Admin, AppRoles.SuperAdmin));

        Assert.True((await service.AuthorizeSuperAdminAsync("u1")).Allowed);
    }

    [Fact]
    public async Task Cached_plain_admin_is_refused_super_admin_operations()
    {
        var service = new AdminPermissionService(
            null!, null!, null!, Cache("u1", UserAccountStatus.Active, AppRoles.Admin));

        Assert.False((await service.AuthorizeSuperAdminAsync("u1")).Allowed);
    }

    [Fact]
    public async Task One_failed_realtime_push_does_not_stop_the_rest_of_a_batch()
    {
        var notifier = new FlakyNotifier(failFor: "u1");
        var service = new NotificationService(
            new CountingRepository(), notifier, NotificationMapper(), NullLogger<NotificationService>.Instance);

        var created = await service.CreateManyAsync(
            ["u1", "u2", "u3"],
            new NotificationContent("عنوان", "رسالة", "🔔", default, "Account", "الحساب", null));

        Assert.Equal(3, created);
        Assert.Equal(["u2", "u3"], notifier.Delivered);
        Assert.Equal(["u2", "u3"], notifier.CountsSentTo);
    }

    private static IUserAccessStateCache Cache(string userId, UserAccountStatus status, params string[] roles)
    {
        var cache = new UserAccessStateCache();
        cache.Set(userId, new UserAccessState(true, status, roles));
        return cache;
    }

    private static IMapper NotificationMapper() =>
        new MapperConfiguration(
            configuration => configuration.CreateMap<Notification, NotificationDto>(),
            NullLoggerFactory.Instance).CreateMapper();

    private sealed class FlakyNotifier(string failFor) : IRealtimeNotifier
    {
        public List<string> Delivered { get; } = new();

        public List<string> CountsSentTo { get; } = new();

        public Task NotifyAsync(string userId, NotificationDto notification, CancellationToken cancellationToken = default)
        {
            if (userId == failFor)
                throw new InvalidOperationException("connection lost");

            Delivered.Add(userId);
            return Task.CompletedTask;
        }

        public Task NotifyUnreadCountAsync(string userId, int unreadCount, CancellationToken cancellationToken = default)
        {
            CountsSentTo.Add(userId);
            return Task.CompletedTask;
        }
    }

    private sealed class CountingRepository : INotificationRepository
    {
        public Task AddAsync(Notification notification) => Task.CompletedTask;

        public Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;

        public Task<IReadOnlyDictionary<string, int>> CountUnreadAsync(
            IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyDictionary<string, int>>(userIds.ToDictionary(id => id, _ => 1));

        public Task<int> SaveChangesAsync() => Task.FromResult(1);

        public Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetPagedForUserAsync(
            string userId, NotificationFilterParams filter, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<Notification?> GetAsync(Guid id, string userId) => throw new NotSupportedException();

        public Task<int> CountUnreadAsync(string userId, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<int> MarkAllAsReadAsync(string userId, DateTime readAt) => throw new NotSupportedException();

        public void Remove(Notification notification) => throw new NotSupportedException();

        public Task<int> DeleteAllAsync(string userId) => throw new NotSupportedException();

        public Task<IReadOnlyList<NotificationDelivery>> FindDeliveriesAsync(
            IReadOnlyCollection<string> userIds, NotificationType type,
            IReadOnlyCollection<Guid> referenceIds, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<bool> ExistsAsync(
            string userId, NotificationType type, Guid? referenceId, DateTime? createdAfterUtc = null) =>
            throw new NotSupportedException();

        public Task<IReadOnlySet<string>> FindRecipientsWithAsync(
            IReadOnlyCollection<string> userIds, NotificationType type, Guid? referenceId,
            DateTime? createdAfterUtc = null, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }
}
