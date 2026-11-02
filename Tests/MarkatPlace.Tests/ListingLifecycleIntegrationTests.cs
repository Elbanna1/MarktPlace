using System.Net;
using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Listings;
using Shared.Constants;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

[Collection(LifecycleApiCollection.Name)]
public class ListingLifecycleIntegrationTests
{
    private const ListingModuleType Cars = ListingModuleType.Advertisement;

    private const ListingModuleType Generic = ListingModuleType.Furniture;

    private static readonly TimeSpan Tolerance = TimeSpan.FromMinutes(2);

    private readonly LifecycleApiFixture _api;

    public ListingLifecycleIntegrationTests(LifecycleApiFixture api)
    {
        _api = api;
    }

    public static IEnumerable<object?[]> EveryModule()
    {
        foreach (var module in ModerationModuleRegistry.EntityTypes.Keys)
        {
            if (module != Cars)
                yield return new object?[] { module, null };
        }

        foreach (var subCategory in new[] { 1, 2, 3, 4 })
            yield return new object?[] { Cars, subCategory };
    }

    [LocalDbTheory]
    [MemberData(nameof(EveryModule))]
    public async Task Every_module_runs_the_whole_lifecycle_over_real_http(
        ListingModuleType module, int? subCategoryId)
    {
        var (ownerId, owner) = await _api.CreateOwnerAsync();
        var title = "دورة-" + Guid.NewGuid().ToString("N");
        var id = await _api.SeedAsync(module, ownerId, title, subCategoryId);

        var pendingWindow = await _api.ReadAsync(module, id);
        Assert.Null(pendingWindow.ExpireAt);

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}/approve", new { });

        var published = await _api.ReadAsync(module, id);
        Assert.Equal(TimeSpan.FromDays(30), published.ExpireAt - published.PublishedAt);

        var endpoint = await DetailsEndpointAsync(module, ownerId);
        var feed = endpoint[..endpoint.LastIndexOf('/')];

        Assert.Equal(HttpStatusCode.OK, (await GetAsync(_api.Anonymous, endpoint)).Status);
        Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, FeedUrl(feed, module))).Body);

        var now = DateTime.UtcNow;

        await _api.SetWindowAsync(module, id, now.AddDays(-25).AddHours(-1), now.AddDays(5).AddHours(1));
        await _api.RunJobAsync(now);
        Assert.Equal(0, await _api.CountAdminAlertsAsync(id));

        await _api.SetWindowAsync(module, id, now.AddDays(-25).AddHours(1), now.AddDays(5).AddHours(-1));
        await _api.RunJobAsync(now);
        await _api.RunJobAsync(now.AddHours(1));
        await _api.RunJobAsync(now.AddHours(2));
        Assert.Equal(1, await _api.CountAdminAlertsAsync(id));

        await AssertAlertContentAsync(module, id);

        now = DateTime.UtcNow;

        var expiredAt = now.AddMinutes(-1);
        await _api.SetWindowAsync(module, id, expiredAt.AddDays(-30), expiredAt);
        await _api.RunJobAsync(now);
        await _api.RunJobAsync(now);

        Assert.Equal(1, await CountOwnerExpiredNoticesAsync(id, ownerId));

        Assert.Equal(HttpStatusCode.NotFound, (await GetAsync(_api.Anonymous, endpoint)).Status);
        Assert.DoesNotContain(id.ToString(), (await GetAsync(_api.Anonymous, FeedUrl(feed, module))).Body);

        var expired = await AdminRowAsync(module, ownerId, "&status=Expired");
        Assert.True(expired.GetProperty("isExpired").GetBoolean());
        Assert.True(expired.GetProperty("canRepublish").GetBoolean());
        Assert.Equal(0, expired.GetProperty("remainingDays").GetInt32());

        Assert.Equal(1, (await AdminItemsAsync(module, ownerId, "&republishDue=true")).Count);

        Assert.Equal(HttpStatusCode.OK, (await GetAsync(owner, endpoint)).Status);
        Assert.Contains(id.ToString(), (await GetAsync(owner, "/api/profile/expired-advertisements?pageSize=50")).Body);

        var before = await _api.ReadAsync(module, id);
        Assert.Equal(1, before.Count);

        var result = await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}/republish", new { });

        var renewedAt = result.GetProperty("publishedAt").GetDateTime();
        var renewedUntil = result.GetProperty("expireAt").GetDateTime();

        Assert.Equal("Active", result.GetProperty("status").GetString());
        Assert.Equal(TimeSpan.FromDays(30), renewedUntil - renewedAt);
        Assert.Equal(30, result.GetProperty("remainingDays").GetInt32());
        Assert.Equal(1, result.GetProperty("republishCount").GetInt32());
        Assert.InRange(renewedAt, DateTime.UtcNow - Tolerance, DateTime.UtcNow + Tolerance);

        var renewed = await _api.ReadAsync(module, id);

        Assert.Equal(1, renewed.Count);
        Assert.Equal(1, renewed.RepublishCount);
        Assert.Equal(ModerationStatus.Approved, renewed.ModerationStatus);
        Assert.Equal(published.FirstPublishedAt, renewed.FirstPublishedAt);
        Assert.True(renewed.CreatedAt > published.CreatedAt);

        await _api.RunJobAsync(DateTime.UtcNow);

        Assert.Equal(HttpStatusCode.OK, (await GetAsync(_api.Anonymous, endpoint)).Status);
        Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, FeedUrl(feed, module))).Body);

        Assert.Contains(id.ToString(), (await GetAsync(owner, "/api/profile/my-listings?pageSize=50")).Body);
        Assert.DoesNotContain(id.ToString(), (await GetAsync(owner, "/api/profile/expired-advertisements?pageSize=50")).Body);

        var active = await AdminRowAsync(module, ownerId, string.Empty);
        Assert.Equal("Active", active.GetProperty("status").GetString());
        Assert.False(active.GetProperty("isExpired").GetBoolean());
        Assert.False(active.GetProperty("canRepublish").GetBoolean());
        Assert.Equal(1, active.GetProperty("republishCount").GetInt32());
        Assert.Equal(30, active.GetProperty("remainingDays").GetInt32());

        await AssertOwnerStaysAsync(module, id, ownerId);
    }

    [LocalDbFact]
    public async Task A_new_cycle_is_alerted_again_but_only_once()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var id = await ApprovedAsync(Generic, ownerId, "دورة ثانية");

        var now = DateTime.UtcNow;
        await _api.SetWindowAsync(Generic, id, now.AddDays(-26), now.AddDays(4));
        await _api.RunJobAsync(now);
        Assert.Equal(1, await _api.CountAdminAlertsAsync(id));

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)Generic}/{id}/republish", new { });

        var cycle = await _api.ReadAsync(Generic, id);

        now = DateTime.UtcNow;
        await _api.SetWindowAsync(Generic, id, cycle.PublishedAt!.Value, now.AddDays(4));
        await _api.RunJobAsync(now);
        await _api.RunJobAsync(now);

        Assert.Equal(2, await _api.CountAdminAlertsAsync(id));
    }

    [LocalDbFact]
    public async Task The_alert_fires_at_exactly_five_days_and_not_a_moment_before()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var id = await ApprovedAsync(Cars, ownerId, "حد الخمسة أيام");

        var now = DateTime.UtcNow;
        var now2 = new DateTime(now.Ticks - now.Ticks % TimeSpan.TicksPerSecond, DateTimeKind.Utc);
        var expireAt = now2.AddDays(5);

        await _api.SetWindowAsync(Cars, id, now2.AddDays(-25), expireAt);

        await _api.RunJobAsync(now2.AddSeconds(-1));
        Assert.Equal(0, await _api.CountAdminAlertsAsync(id));

        await _api.RunJobAsync(now2);
        Assert.Equal(1, await _api.CountAdminAlertsAsync(id));
    }

    [LocalDbFact]
    public async Task A_missed_job_run_still_alerts_late_rather_than_never()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var id = await ApprovedAsync(Generic, ownerId, "الوظيفة وقفت");

        var now = DateTime.UtcNow;
        await _api.SetWindowAsync(Generic, id, now.AddDays(-29), now.AddDays(1));
        await _api.RunJobAsync(now);

        Assert.Equal(1, await _api.CountAdminAlertsAsync(id));
    }

    [LocalDbFact]
    public async Task An_expired_car_is_never_deleted_however_old_it_gets()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var id = await ApprovedAsync(Cars, ownerId, "سيارة قديمة");

        var longAgo = DateTime.UtcNow.AddDays(-400);
        await _api.SetWindowAsync(Cars, id, longAgo.AddDays(-30), longAgo);
        await _api.RunJobAsync(DateTime.UtcNow);

        using (var scope = _api.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Advertisements
                .IgnoreQueryFilters()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.ExpiredAt, longAgo));
        }

        await _api.RunJobAsync(DateTime.UtcNow);

        Assert.Equal(1, (await _api.ReadAsync(Cars, id)).Count);

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)Cars}/{id}/republish", new { });
    }

    [LocalDbFact]
    public async Task Approving_an_expired_car_makes_it_public_again()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var title = "موافقة بعد الانتهاء-" + Guid.NewGuid().ToString("N");
        var id = await ApprovedAsync(Cars, ownerId, title);

        var now = DateTime.UtcNow;
        await _api.SetWindowAsync(Cars, id, now.AddDays(-31), now.AddDays(-1));
        await _api.RunJobAsync(now);

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)Cars}/{id}/approve", new { });

        var endpoint = await DetailsEndpointAsync(Cars, ownerId);
        var feed = endpoint[..endpoint.LastIndexOf('/')];

        Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, FeedUrl(feed, Cars))).Body);
        Assert.Equal(HttpStatusCode.OK, (await GetAsync(_api.Anonymous, endpoint)).Status);
    }

    [LocalDbFact]
    public async Task A_car_stranded_by_the_old_approve_bug_can_be_republished()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var title = "عالقة-" + Guid.NewGuid().ToString("N");
        var id = await ApprovedAsync(Cars, ownerId, title);

        using (var scope = _api.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Advertisements
                .IgnoreQueryFilters()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.Status, AdvertisementStatus.Expired));
        }

        var row = await AdminRowAsync(Cars, ownerId, "&republishDue=true");
        Assert.True(row.GetProperty("canRepublish").GetBoolean());

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)Cars}/{id}/republish", new { });

        var endpoint = await DetailsEndpointAsync(Cars, ownerId);
        var feed = endpoint[..endpoint.LastIndexOf('/')];

        Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, FeedUrl(feed, Cars))).Body);
    }

    [LocalDbTheory]
    [InlineData(Cars)]
    [InlineData(Generic)]
    public async Task Concurrent_admin_republishes_renew_exactly_once(ListingModuleType module)
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var id = await ApprovedAsync(module, ownerId, "ضغطات متزامنة");

        var now = DateTime.UtcNow;
        await _api.SetWindowAsync(module, id, now.AddDays(-31), now.AddDays(-1));

        var responses = await Task.WhenAll(Enumerable.Range(0, 8).Select(_ =>
            _api.Admin.PatchAsync(
                $"/api/v2/admin/ads/{(int)module}/{id}/republish",
                JsonContent(new { }))));

        var outcomes = await Task.WhenAll(responses.Select(async response =>
            $"{(int)response.StatusCode}: {await response.Content.ReadAsStringAsync()}"));
        var report = string.Join(" || ", outcomes);

        Assert.True(responses.Count(response => response.StatusCode == HttpStatusCode.OK) == 1, report);
        Assert.DoesNotContain(responses, response => (int)response.StatusCode >= 500);

        var stored = await _api.ReadAsync(module, id);

        Assert.Equal(1, stored.Count);
        Assert.True(stored.RepublishCount == 1, $"count {stored.RepublishCount}: {report}");

        using var scope = _api.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Assert.Equal(1, await context.Notifications.CountAsync(n =>
            n.ReferenceId == id && n.Type == NotificationType.AdvertisementRepublished));
    }

    [LocalDbTheory]
    [InlineData(Cars)]
    [InlineData(Generic)]
    public async Task Concurrent_owner_republishes_queue_the_listing_exactly_once(ListingModuleType module)
    {
        var (ownerId, owner) = await _api.CreateOwnerAsync();
        var id = await ApprovedAsync(module, ownerId, "المالك ضغط مرتين");

        var now = DateTime.UtcNow;
        await _api.SetWindowAsync(module, id, now.AddDays(-31), now.AddDays(-1));

        var responses = await Task.WhenAll(Enumerable.Range(0, 6).Select(_ =>
            owner.PostAsync(
                $"/api/advertisements/{id}/republish?type={(int)module}",
                JsonContent(new { }))));

        Assert.DoesNotContain(responses, response => (int)response.StatusCode >= 500);

        var stored = await _api.ReadAsync(module, id);

        Assert.Equal(1, stored.RepublishCount);
        Assert.Equal(ModerationStatus.Pending, stored.ModerationStatus);
        Assert.Null(stored.ExpireAt);
    }

    [LocalDbTheory]
    [InlineData(Cars)]
    [InlineData(Generic)]
    public async Task An_owner_republish_returns_to_review_then_approval_restores_it_publicly(ListingModuleType module)
    {
        var (ownerId, owner) = await _api.CreateOwnerAsync();
        var title = "إعادة من المالك-" + Guid.NewGuid().ToString("N");
        var id = await ApprovedAsync(module, ownerId, title);

        var endpoint = await DetailsEndpointAsync(module, ownerId);
        var feed = endpoint[..endpoint.LastIndexOf('/')];

        var now = DateTime.UtcNow;
        await _api.SetWindowAsync(module, id, now.AddDays(-31), now.AddDays(-1));
        await _api.RunJobAsync(now);

        var response = await owner.PostAsync(
            $"/api/advertisements/{id}/republish?type={(int)module}", JsonContent(new { }));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(ModerationStatus.Pending, (await _api.ReadAsync(module, id)).ModerationStatus);
        Assert.Equal(HttpStatusCode.NotFound, (await GetAsync(_api.Anonymous, endpoint)).Status);

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}/approve", new { });

        var stored = await _api.ReadAsync(module, id);

        Assert.Equal(TimeSpan.FromDays(30), stored.ExpireAt - stored.PublishedAt);
        Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, FeedUrl(feed, module))).Body);
    }

    [LocalDbFact]
    public async Task A_republished_listing_sorts_ahead_of_newer_ones()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var old = await ApprovedAsync(Generic, ownerId, "قديم");

        var now = DateTime.UtcNow;
        await _api.SetWindowAsync(Generic, old, now.AddDays(-31), now.AddDays(-1));

        var newer = await ApprovedAsync(Generic, ownerId, "أحدث");

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)Generic}/{old}/republish", new { });

        var body = (await GetAsync(_api.Anonymous, $"/api/{ListingModuleCatalog.RouteOf(Generic).Split('/')[0]}?pageSize=50&t={Guid.NewGuid():N}")).Body;

        Assert.True(body.IndexOf(old.ToString(), StringComparison.Ordinal) >= 0);
        Assert.True(
            body.IndexOf(old.ToString(), StringComparison.Ordinal) < body.IndexOf(newer.ToString(), StringComparison.Ordinal));
    }

    [LocalDbFact]
    public async Task Pending_rejected_and_running_listings_cannot_be_republished()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();

        var pending = await _api.SeedAsync(Generic, ownerId, "قيد المراجعة");
        var running = await ApprovedAsync(Generic, ownerId, "لسه شغال");

        var first = await _api.Admin.PatchAsync(
            $"/api/v2/admin/ads/{(int)Generic}/{pending}/republish", JsonContent(new { }));
        var second = await _api.Admin.PatchAsync(
            $"/api/v2/admin/ads/{(int)Generic}/{running}/republish", JsonContent(new { }));
        var missing = await _api.Admin.PatchAsync(
            $"/api/v2/admin/ads/{(int)Generic}/{Guid.NewGuid()}/republish", JsonContent(new { }));

        Assert.Equal(HttpStatusCode.BadRequest, first.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
        Assert.Equal(0, (await _api.ReadAsync(Generic, running)).RepublishCount);
    }

    [LocalDbFact]
    public async Task Republish_is_not_open_to_ordinary_users()
    {
        var (ownerId, owner) = await _api.CreateOwnerAsync();
        var id = await ApprovedAsync(Generic, ownerId, "مش أدمن");

        var response = await owner.PatchAsync(
            $"/api/v2/admin/ads/{(int)Generic}/{id}/republish", JsonContent(new { }));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [LocalDbFact]
    public async Task The_admin_reads_the_alert_through_the_notifications_api()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var id = await ApprovedAsync(Cars, ownerId, "إشعار عبر الـ API");

        var now = DateTime.UtcNow;
        await _api.SetWindowAsync(Cars, id, now.AddDays(-26), now.AddDays(4));
        await _api.RunJobAsync(now);

        var body = (await GetAsync(_api.Admin, "/api/notifications?pageSize=50")).Body;

        Assert.Contains($"/admin/ads/Advertisement/{id}", body);
    }

    private async Task<Guid> ApprovedAsync(ListingModuleType module, string ownerId, string title)
    {
        var id = await _api.SeedAsync(module, ownerId, title);

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}/approve", new { });

        return id;
    }

    private async Task AssertAlertContentAsync(ListingModuleType module, Guid id)
    {
        using var scope = _api.CreateScope();

        var notice = await scope.ServiceProvider.GetRequiredService<AppDbContext>().Notifications
            .SingleAsync(n => n.ReferenceId == id &&
                              n.UserId == _api.AdminId &&
                              n.Type == NotificationType.AdminListingExpiring);

        Assert.False(string.IsNullOrWhiteSpace(notice.ListingTitle));
        Assert.Contains(notice.ListingTitle!, notice.Message);
        Assert.Contains(ListingLifecycle.ToDisplayTime(DateTime.UtcNow.AddDays(5)).Year.ToString(), notice.Message);
        Assert.Equal($"/admin/ads/{module}/{id}", notice.DeepLink);
        Assert.Equal(module, notice.ListingType);
        Assert.NotNull(notice.CategoryId);
        Assert.NotEqual(0, notice.CategoryId);
    }

    private async Task<int> CountOwnerExpiredNoticesAsync(Guid id, string ownerId)
    {
        using var scope = _api.CreateScope();

        return await scope.ServiceProvider.GetRequiredService<AppDbContext>().Notifications
            .CountAsync(n => n.ReferenceId == id &&
                             n.UserId == ownerId &&
                             n.Type == NotificationType.AdvertisementExpired);
    }

    private async Task AssertOwnerStaysAsync(ListingModuleType module, Guid id, string ownerId)
    {
        using var scope = _api.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var entityType = ModerationModuleRegistry.EntityTypes[module];

        var listing = (IModeratedListing)(await context.FindAsync(entityType, id))!;

        Assert.Equal(ownerId, listing.OwnerUserId);
    }

    private async Task<string> DetailsEndpointAsync(ListingModuleType module, string ownerId)
    {
        var row = await AdminRowAsync(module, ownerId, string.Empty);

        return row.GetProperty("detailsEndpoint").GetString()!;
    }

    private async Task<JsonElement> AdminRowAsync(ListingModuleType module, string ownerId, string query)
    {
        var items = await AdminItemsAsync(module, ownerId, query);

        return Assert.Single(items);
    }

    private async Task<IReadOnlyList<JsonElement>> AdminItemsAsync(
        ListingModuleType module, string ownerId, string query)
    {
        var (status, body) = await GetAsync(
            _api.Admin,
            $"/api/v2/admin/ads?type={(int)module}&ownerId={ownerId}&pageSize=50{query}");

        Assert.Equal(HttpStatusCode.OK, status);

        return JsonDocument.Parse(body).RootElement
            .GetProperty("data").GetProperty("items").EnumerateArray().Select(item => item.Clone()).ToList();
    }

    private static string FeedUrl(string feed, ListingModuleType module)
    {
        var kind = module switch
        {
            ListingModuleType.LostItem => "&postType=Lost",
            ListingModuleType.FoundItem => "&postType=Found",
            _ => string.Empty
        };

        return $"{feed}?pageSize=50{kind}&t={Guid.NewGuid():N}";
    }

    private static async Task<(HttpStatusCode Status, string Body)> GetAsync(HttpClient client, string url)
    {
        var response = await client.GetAsync(url);

        return (response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    private static async Task<JsonElement> PatchOk(HttpClient client, string url, object body)
    {
        var response = await client.PatchAsync(url, JsonContent(body));
        var text = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, $"{url}: {(int)response.StatusCode} {text}");

        return JsonDocument.Parse(text).RootElement.GetProperty("data").Clone();
    }

    private static StringContent JsonContent(object body) =>
        new(JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json");
}
