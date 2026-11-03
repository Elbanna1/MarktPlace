using System.Net;
using System.Text.Json;
using Persistence.Listings;
using Shared.Constants;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

[Collection(LifecycleApiCollection.Name)]
public class ExpiredListingsAdminPageTests
{
    private const ListingModuleType Cars = ListingModuleType.Advertisement;

    private readonly LifecycleApiFixture _api;

    public ExpiredListingsAdminPageTests(LifecycleApiFixture api)
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
    public async Task Expired_page_lists_the_ad_and_republish_restores_it_on_its_original_category_page(
        ListingModuleType module, int? subCategoryId)
    {
        var (ownerId, owner) = await _api.CreateOwnerAsync();
        var title = "منتهي-" + Guid.NewGuid().ToString("N");
        var id = await _api.SeedAsync(module, ownerId, title, subCategoryId);

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}/approve");

        var categoryPage = await CategoryPageAsync(module, subCategoryId, id);
        Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, categoryPage.Url)).Body);

        var now = DateTime.UtcNow;
        await _api.SetWindowAsync(module, id, now.AddDays(-31), now.AddMinutes(-5));
        await _api.RunJobAsync(now);

        Assert.DoesNotContain(id.ToString(), (await GetAsync(_api.Anonymous, categoryPage.Url)).Body);

        var expiredRow = await ExpiredRowAsync($"type={(int)module}&ownerId={ownerId}");
        Assert.NotNull(expiredRow);

        var row = expiredRow!.Value;
        var displayTitle = row.GetProperty("title").GetString() ?? title;
        Assert.Equal(id, row.GetProperty("id").GetGuid());
        Assert.Equal((int)module, row.GetProperty("typeId").GetInt32());
        Assert.Equal("Expired", row.GetProperty("status").GetString());
        Assert.True(row.GetProperty("isExpired").GetBoolean());
        Assert.True(row.GetProperty("canRepublish").GetBoolean());
        Assert.False(string.IsNullOrWhiteSpace(row.GetProperty("categoryName").GetString()));

        var (countStatus, countBody) = await GetAsync(_api.Admin, "/api/v2/admin/ads/expired/count");
        Assert.Equal(HttpStatusCode.OK, countStatus);
        Assert.True(JsonDocument.Parse(countBody).RootElement.GetProperty("data").GetInt32() >= 1);

        var result = await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}/republish");

        Assert.Equal(TimeSpan.FromDays(30),
            result.GetProperty("expireAt").GetDateTime() - result.GetProperty("publishedAt").GetDateTime());

        var stored = await _api.ReadAsync(module, id);
        Assert.Equal(1, stored.Count);
        Assert.Equal(ModerationStatus.Approved, stored.ModerationStatus);
        Assert.Equal(TimeSpan.FromDays(30), stored.ExpireAt - stored.PublishedAt);
        Assert.True(stored.ExpireAt > DateTime.UtcNow.AddDays(29));

        await _api.RunJobAsync(DateTime.UtcNow);

        Assert.Null(await ExpiredRowAsync($"type={(int)module}&ownerId={ownerId}"));

        var page = await GetAsync(_api.Anonymous, categoryPage.Url);
        Assert.Equal(HttpStatusCode.OK, page.Status);
        Assert.Contains(id.ToString(), page.Body);

        if (categoryPage.SearchUrl is { } searchUrl)
        {
            var search = await GetAsync(_api.Anonymous, searchUrl + Uri.EscapeDataString(displayTitle));
            Assert.Equal(HttpStatusCode.OK, search.Status);
            Assert.Contains(id.ToString(), search.Body);
        }

        Assert.Contains(id.ToString(), (await GetAsync(owner, "/api/profile/my-listings?pageSize=50")).Body);
    }

    [LocalDbFact]
    public async Task Expired_page_without_any_filter_spans_every_module_at_once()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var marker = "كل-الأقسام-" + Guid.NewGuid().ToString("N");
        var now = DateTime.UtcNow;

        var modules = new[]
        {
            ListingModuleType.Furniture, ListingModuleType.Bee, ListingModuleType.Apartment, Cars
        };

        var ids = new List<Guid>();

        foreach (var module in modules)
        {
            var id = await _api.SeedAsync(module, ownerId, $"{marker}-{module}", module == Cars ? 1 : null);
            await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}/approve");
            await _api.SetWindowAsync(module, id, now.AddDays(-32), now.AddDays(-2));
            ids.Add(id);
        }

        await _api.RunJobAsync(now);

        var (status, body) = await GetAsync(
            _api.Admin, $"/api/v2/admin/ads/expired?pageSize=50&search={Uri.EscapeDataString(marker)}");

        Assert.Equal(HttpStatusCode.OK, status);

        var items = JsonDocument.Parse(body).RootElement.GetProperty("data").GetProperty("items")
            .EnumerateArray().Select(item => item.GetProperty("id").GetGuid()).ToList();

        Assert.Equal(ids.OrderBy(id => id), items.OrderBy(id => id));

        var all = await GetAsync(_api.Admin, "/api/v2/admin/ads?status=Expired&pageSize=50");
        Assert.Equal(HttpStatusCode.OK, all.Status);
    }

    [LocalDbFact]
    public async Task A_running_listing_is_not_on_the_expired_page_and_cannot_be_republished()
    {
        var (ownerId, _) = await _api.CreateOwnerAsync();
        var title = "ساري-" + Guid.NewGuid().ToString("N");
        var id = await _api.SeedAsync(ListingModuleType.Furniture, ownerId, title);

        await PatchOk(_api.Admin, $"/api/v2/admin/ads/{(int)ListingModuleType.Furniture}/{id}/approve");

        Assert.Null(await ExpiredRowAsync($"search={Uri.EscapeDataString(title)}"));

        var response = await _api.Admin.PatchAsync(
            $"/api/v2/admin/ads/{(int)ListingModuleType.Furniture}/{id}/republish", Json(new { }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [LocalDbFact]
    public async Task The_expired_page_is_admin_only()
    {
        var (_, owner) = await _api.CreateOwnerAsync();

        Assert.Equal(HttpStatusCode.Unauthorized, (await _api.Anonymous.GetAsync("/api/v2/admin/ads/expired")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await owner.GetAsync("/api/v2/admin/ads/expired")).StatusCode);
    }

    private async Task<JsonElement?> ExpiredRowAsync(string query)
    {
        var (status, body) = await GetAsync(_api.Admin, $"/api/v2/admin/ads/expired?pageSize=50&{query}");

        Assert.Equal(HttpStatusCode.OK, status);

        var items = JsonDocument.Parse(body).RootElement.GetProperty("data").GetProperty("items")
            .EnumerateArray().Select(item => item.Clone()).ToList();

        return items.Count switch
        {
            0 => null,
            1 => items[0],
            _ => throw new Xunit.Sdk.XunitException($"Expected one expired row for {query}, got {items.Count}.")
        };
    }

    private async Task<(string Url, string? SearchUrl)> CategoryPageAsync(
        ListingModuleType module, int? subCategoryId, Guid id)
    {
        var sub = subCategoryId ?? (int?)ListingModuleCatalog.SubCategoryOf(module);
        var category = sub is { } value ? (int?)ListingModuleCatalog.CategoryOf((SubCategoryType)value) : null;

        if (category is null && module == Cars)
            category = (int)CategoryType.Cars;

        if (category is not null)
        {
            var route = sub is null
                ? $"/api/lookups/read-config/{category}"
                : $"/api/lookups/read-config/{category}/{sub}";

            var (status, body) = await GetAsync(_api.Anonymous, route);

            if (status == HttpStatusCode.OK &&
                JsonDocument.Parse(body).RootElement.GetProperty("data").TryGetProperty("list", out var list))
            {
                var endpoint = list.GetProperty("endpoint").GetString()!;
                var query = new List<string> { "pageSize=50", $"t={Guid.NewGuid():N}" };

                if (list.TryGetProperty("query", out var pins))
                {
                    foreach (var pin in pins.EnumerateObject())
                        query.Add($"{pin.Name}={Uri.EscapeDataString(pin.Value.ToString())}");
                }

                var url = $"{endpoint}?{string.Join("&", query)}";

                var searchable = list.TryGetProperty("queryParameters", out var parameters) &&
                                 parameters.EnumerateArray().Any(parameter =>
                                     parameter.GetProperty("name").GetString() is "search" or "Search");

                return (url, searchable ? url + "&search=" : null);
            }
        }

        var details = await AdminDetailsEndpointAsync(module, id);
        var feed = details[..details.LastIndexOf('/')];

        var kind = module switch
        {
            ListingModuleType.LostItem => "&postType=Lost",
            ListingModuleType.FoundItem => "&postType=Found",
            _ => string.Empty
        };

        return ($"{feed}?pageSize=50{kind}&t={Guid.NewGuid():N}", null);
    }

    private async Task<string> AdminDetailsEndpointAsync(ListingModuleType module, Guid id)
    {
        var (_, body) = await GetAsync(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}");

        return JsonDocument.Parse(body).RootElement.GetProperty("data").GetProperty("detailsEndpoint").GetString()!;
    }

    private static async Task<(HttpStatusCode Status, string Body)> GetAsync(HttpClient client, string url)
    {
        var response = await client.GetAsync(url);

        return (response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    private static async Task<JsonElement> PatchOk(HttpClient client, string url)
    {
        var response = await client.PatchAsync(url, Json(new { }));
        var text = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, $"{url}: {(int)response.StatusCode} {text}");

        return JsonDocument.Parse(text).RootElement.GetProperty("data").Clone();
    }

    private static StringContent Json(object body) =>
        new(JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json");
}
