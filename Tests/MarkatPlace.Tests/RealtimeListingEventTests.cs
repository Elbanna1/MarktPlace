using AutoMapper;
using Domain.Entities;
using MarkatPlace.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Persistence.Data;
using Persistence.Interceptors;
using Persistence.Listings;
using Persistence.Repositories;
using Services;
using Services.Admin;
using Services.Lookups;
using Services.Mapping;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Realtime;
using Shared.Enums;
using Shared.Exceptions;
using Xunit;

namespace MarkatPlace.Tests;

public class RealtimeListingEventTests
{
    private sealed class RecordingPublisher : IListingEventPublisher
    {
        public List<ListingChange> Changes { get; } = new();

        public Task PublishAsync(IReadOnlyList<ListingChange> changes, CancellationToken cancellationToken = default)
        {
            Changes.AddRange(changes);
            return Task.CompletedTask;
        }
    }

    private static (AppDbContext Context, RecordingPublisher Publisher) NewContext(string? name = null)
    {
        var publisher = new RecordingPublisher();
        var interceptor = new ListingRealtimeInterceptor([publisher], NullLogger<ListingRealtimeInterceptor>.Instance);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(name ?? Guid.NewGuid().ToString())
            .AddInterceptors(interceptor)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        publisher.Changes.Clear();
        return (context, publisher);
    }

    public static TheoryData<ListingModuleType> NewModules() => new()
    {
        ListingModuleType.RestaurantAndCafe,
        ListingModuleType.LawyerOffice,
        ListingModuleType.EngineeringConsultancyOffice,
        ListingModuleType.Contractor,
        ListingModuleType.IndustrialConsultancyOffice,
        ListingModuleType.AccountantOffice
    };

    private static DirectoryListing NewListing(ListingModuleType module, string owner)
    {
        var listing = (DirectoryListing)Activator.CreateInstance(ModerationModuleRegistry.EntityTypes[module])!;
        listing.Id = Guid.NewGuid();
        listing.UserId = owner;
        listing.Name = "اسم";
        listing.Title = "عنوان";
        listing.Description = "وصف";
        listing.Address = "عنوان";
        listing.Phone = "01012345678";
        return listing;
    }

    [Theory]
    [MemberData(nameof(NewModules))]
    public async Task Every_new_module_publishes_its_lifecycle_after_each_successful_save(ListingModuleType module)
    {
        var (context, publisher) = NewContext();
        var listing = NewListing(module, "owner-1");

        context.Add(listing);
        await context.SaveChangesAsync();

        var created = Assert.Single(publisher.Changes);
        Assert.Equal("owner-1", created.OwnerUserId);
        Assert.Equal(ListingChangeKinds.Created, created.Event.Change);
        Assert.Equal(module, created.Event.Type);
        Assert.Equal(listing.Id, created.Event.Id);
        Assert.Equal((int?)ListingModuleCatalog.CategoryOf(module), created.Event.CategoryId);
        Assert.Equal((int?)ListingModuleCatalog.SubCategoryOf(module), created.Event.SubCategoryId);
        Assert.Equal(ListingModuleCatalog.RouteOf(module), created.Event.Route);
        Assert.False(created.Event.IsPublic);
        Assert.Equal(ListingStatus.Pending, created.Event.Status);

        publisher.Changes.Clear();
        listing.ModerationStatus = ModerationStatus.Approved;
        listing.PublishedAt = DateTime.UtcNow;
        listing.ExpireAt = DateTime.UtcNow.AddDays(30);
        await context.SaveChangesAsync();

        var approved = Assert.Single(publisher.Changes).Event;
        Assert.Equal(ListingChangeKinds.Approved, approved.Change);
        Assert.True(approved.IsPublic);
        Assert.False(approved.WasPublic);
        Assert.Equal(ListingStatus.Active, approved.Status);
        Assert.Equal(30, approved.RemainingDays);

        publisher.Changes.Clear();
        listing.IsDeleted = true;
        await context.SaveChangesAsync();

        var deleted = Assert.Single(publisher.Changes).Event;
        Assert.Equal(ListingChangeKinds.Deleted, deleted.Change);
        Assert.True(deleted.WasPublic);
        Assert.False(deleted.IsPublic);
    }

    [Fact]
    public async Task Suspension_rejection_and_republish_are_named_and_carry_visibility()
    {
        var (context, publisher) = NewContext();
        var listing = NewListing(ListingModuleType.RestaurantAndCafe, "owner");
        listing.ModerationStatus = ModerationStatus.Approved;
        listing.PublishedAt = DateTime.UtcNow.AddDays(-31);
        listing.ExpireAt = DateTime.UtcNow.AddDays(-1);
        context.Add(listing);
        await context.SaveChangesAsync();
        publisher.Changes.Clear();

        listing.PublishedAt = DateTime.UtcNow;
        listing.ExpireAt = DateTime.UtcNow.AddDays(30);
        listing.RepublishCount++;
        await context.SaveChangesAsync();

        var republished = Assert.Single(publisher.Changes).Event;
        Assert.Equal(ListingChangeKinds.Republished, republished.Change);
        Assert.False(republished.WasPublic);
        Assert.True(republished.IsPublic);
        Assert.Equal(1, republished.RepublishCount);

        publisher.Changes.Clear();
        listing.ModerationStatus = ModerationStatus.Suspended;
        await context.SaveChangesAsync();

        var suspended = Assert.Single(publisher.Changes).Event;
        Assert.Equal(ListingChangeKinds.Suspended, suspended.Change);
        Assert.True(suspended.WasPublic);
        Assert.False(suspended.IsPublic);

        publisher.Changes.Clear();
        listing.ModerationStatus = ModerationStatus.Rejected;
        await context.SaveChangesAsync();
        Assert.Equal(ListingChangeKinds.Rejected, Assert.Single(publisher.Changes).Event.Change);

        publisher.Changes.Clear();
        listing.ModerationStatus = ModerationStatus.Pending;
        await context.SaveChangesAsync();
        Assert.Equal(ListingChangeKinds.ReturnedToReview, Assert.Single(publisher.Changes).Event.Change);
    }

    [Fact]
    public async Task Lost_and_found_posts_resolve_to_their_own_module()
    {
        var (context, publisher) = NewContext();

        context.LostFoundPosts.Add(new LostFoundPost
        {
            Id = Guid.NewGuid(), UserId = "owner", PostType = PostType.Found, Name = "اسم",
            ItemName = "محفظة", Description = "وصف", Governorate = "الفيوم", Center = "الفيوم",
            PhoneNumber = "01012345678"
        });
        await context.SaveChangesAsync();

        Assert.Equal(ListingModuleType.FoundItem, Assert.Single(publisher.Changes).Event.Type);
    }

    [Fact]
    public async Task A_failed_save_publishes_nothing_and_leaves_nothing_behind()
    {
        var name = Guid.NewGuid().ToString();
        var (context, publisher) = NewContext(name);
        var listing = NewListing(ListingModuleType.AccountantOffice, "owner");
        context.Add(listing);
        await context.SaveChangesAsync();
        publisher.Changes.Clear();

        var (second, secondPublisher) = NewContext(name);
        var duplicate = NewListing(ListingModuleType.AccountantOffice, "owner");
        duplicate.Id = listing.Id;
        second.Add(duplicate);

        await Assert.ThrowsAnyAsync<Exception>(() => second.SaveChangesAsync());
        Assert.Empty(secondPublisher.Changes);

        second.ChangeTracker.Clear();
        var other = NewListing(ListingModuleType.AccountantOffice, "owner");
        second.Add(other);
        await second.SaveChangesAsync();

        Assert.Equal(other.Id, Assert.Single(secondPublisher.Changes).Event.Id);
    }

    [Fact]
    public async Task Saves_that_touch_no_listing_publish_nothing()
    {
        var (context, publisher) = NewContext();

        context.Users.Add(new ApplicationUser
        {
            Id = "u", UserName = "u", FirstName = "أ", SecondName = "ب", Governorate = "الفيوم", Center = "الفيوم"
        });
        await context.SaveChangesAsync();

        Assert.Empty(publisher.Changes);
    }

    private static (AdminAdService Service, SilentProxy<INotificationService> Notifications) Admin(AppDbContext context)
    {
        var mapper = new MapperConfiguration(
            c => c.AddMaps(typeof(RestaurantsAndCafesMappingProfile).Assembly), NullLoggerFactory.Instance).CreateMapper();

        var repository = new AdminAdRepository(
            context,
            [new RestaurantAndCafeListingSource(context)],
            [new ListingModerationSource<RestaurantAndCafe>(context, ListingModuleType.RestaurantAndCafe)]);

        var notifications = SilentProxy<INotificationService>.Create();

        var service = new AdminAdService(
            repository,
            SilentProxy<IListingInteractionService>.Create(),
            new LookupService(new LookupRepository(context), new MemoryCache(new MemoryCacheOptions()), new LookupCache(), mapper),
            notifications,
            SilentProxy<IListingInterestNotifier>.Create(),
            SilentProxy<IAdminAuditService>.Create(),
            NullLogger<AdminAdService>.Instance);

        return (service, SilentProxy<INotificationService>.Of(notifications));
    }

    [Fact]
    public async Task Approval_notifies_the_owner_exactly_once_even_when_repeated_and_publishes_one_event()
    {
        var (context, publisher) = NewContext();
        var listing = NewListing(ListingModuleType.RestaurantAndCafe, "owner-7");
        context.Add(listing);
        await context.SaveChangesAsync();
        publisher.Changes.Clear();

        var (admin, notifications) = Admin(context);

        await admin.ApproveAsync(ListingModuleType.RestaurantAndCafe, listing.Id, "admin", new ApproveListingRequest());
        await admin.ApproveAsync(ListingModuleType.RestaurantAndCafe, listing.Id, "admin", new ApproveListingRequest());

        var sent = notifications.Calls.Where(call => call.Method == nameof(INotificationService.CreateAsync)).ToList();
        var only = Assert.Single(sent);
        Assert.Equal("owner-7", only.Args[0]);
        Assert.Equal(NotificationAction.Approved, only.Args[3]);

        var approvals = publisher.Changes.Where(change => change.Event.Change == ListingChangeKinds.Approved).ToList();
        Assert.Single(approvals);
        Assert.True(approvals[0].Event.IsPublic);
        Assert.Equal("owner-7", approvals[0].OwnerUserId);
    }

    [Fact]
    public async Task Approving_an_expired_listing_again_reopens_it_and_does_notify()
    {
        var (context, _) = NewContext();
        var listing = NewListing(ListingModuleType.RestaurantAndCafe, "owner");
        listing.ModerationStatus = ModerationStatus.Approved;
        listing.PublishedAt = DateTime.UtcNow.AddDays(-40);
        listing.ExpireAt = DateTime.UtcNow.AddDays(-10);
        context.Add(listing);
        await context.SaveChangesAsync();

        var (admin, notifications) = Admin(context);
        await admin.ApproveAsync(ListingModuleType.RestaurantAndCafe, listing.Id, "admin", new ApproveListingRequest());

        Assert.Single(notifications.Calls, call => call.Method == nameof(INotificationService.CreateAsync));
    }

    [Fact]
    public async Task A_failed_approval_creates_no_notification_and_no_event()
    {
        var (context, publisher) = NewContext();
        var (admin, notifications) = Admin(context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            admin.ApproveAsync(ListingModuleType.RestaurantAndCafe, Guid.NewGuid(), "admin", new ApproveListingRequest()));

        Assert.Empty(notifications.Calls);
        Assert.Empty(publisher.Changes);
    }

    [Fact]
    public async Task An_admin_republish_publishes_a_republished_event_that_is_public_again()
    {
        var (context, publisher) = NewContext();
        var listing = NewListing(ListingModuleType.RestaurantAndCafe, "owner");
        listing.ModerationStatus = ModerationStatus.Approved;
        listing.PublishedAt = DateTime.UtcNow.AddDays(-35);
        listing.ExpireAt = DateTime.UtcNow.AddDays(-5);
        context.Add(listing);
        await context.SaveChangesAsync();
        publisher.Changes.Clear();

        var (admin, _) = Admin(context);
        await admin.RepublishAsync(ListingModuleType.RestaurantAndCafe, listing.Id, "admin");

        var change = Assert.Single(publisher.Changes).Event;
        Assert.Equal(ListingChangeKinds.Republished, change.Change);
        Assert.True(change.IsPublic);
        Assert.False(change.WasPublic);
        Assert.Equal(30, change.RemainingDays);
    }

    [Fact]
    public void Private_events_never_reach_the_public_broadcast_shape()
    {
        var properties = typeof(PublicListingChangedEventDto).GetProperties().Select(p => p.Name).OrderBy(n => n);

        Assert.Equal(
            new[] { "CategoryId", "Change", "Id", "IsPublic", "OccurredAt", "Route", "SubCategoryId", "Type" },
            properties);
    }

    [Fact]
    public void The_hub_routes_private_events_to_the_owner_and_admins_and_only_public_ones_to_everyone()
    {
        var publisher = RepositoryRoot.ReadFile("MarkatPlace/RealTime/SignalRListingEventPublisher.cs");

        Assert.Contains("Clients.User(ownerUserId)", publisher);
        Assert.Contains("Clients.Group(NotificationHub.AdminsGroup)", publisher);
        Assert.Contains("Clients.All.SendAsync(\n                        NotificationHub.PublicListingChanged,\n                        PublicListingChangedEventDto.From(change)",
            publisher.Replace("\r\n", "\n"));
        Assert.DoesNotContain("Clients.All.SendAsync(NotificationHub.ListingChanged", publisher);
    }

    [Fact]
    public void The_realtime_interceptor_runs_after_the_edit_review_interceptor()
    {
        var di = RepositoryRoot.ReadFile("Infastrucre/Presitance/DependencyInjection.cs");

        var edit = di.IndexOf("AddInterceptors(provider.GetRequiredService<ListingEditModerationInterceptor>())", StringComparison.Ordinal);
        var realtime = di.IndexOf("AddInterceptors(provider.GetRequiredService<ListingRealtimeInterceptor>())", StringComparison.Ordinal);

        Assert.True(edit > 0 && realtime > edit);
    }

    [Fact]
    public void The_uploads_root_defaults_to_wwwroot_and_can_be_moved_outside_the_application()
    {
        var content = Path.Combine(Path.GetTempPath(), "app");

        var defaults = new ConfigurationBuilder().Build();
        Assert.Equal(Path.GetFullPath(Path.Combine(content, "wwwroot", "uploads")),
            UploadsStorage.ResolveRoot(defaults, content));

        var outside = Path.Combine(Path.GetTempPath(), "shopiklopik-uploads");
        var configured = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { [UploadsStorage.ConfigurationKey] = outside })
            .Build();

        Assert.Equal(Path.GetFullPath(outside), UploadsStorage.ResolveRoot(configured, content));
        Assert.True(UploadsStorage.IsInside(UploadsStorage.ResolveRoot(defaults, content), content));
        Assert.False(UploadsStorage.IsInside(outside, content));
    }

    [Fact]
    public async Task The_uploads_health_check_reports_a_missing_or_writable_folder()
    {
        var missing = new UploadsStorageHealthCheck(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        Assert.Equal(Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
            (await missing.CheckHealthAsync(new())).Status);

        var folder = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"))).FullName;
        try
        {
            var ok = new UploadsStorageHealthCheck(folder);
            Assert.Equal(Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy,
                (await ok.CheckHealthAsync(new())).Status);
            Assert.Empty(Directory.GetFiles(folder));
        }
        finally
        {
            Directory.Delete(folder, recursive: true);
        }
    }
}
