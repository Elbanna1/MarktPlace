using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public partial class AdPaymentIntegrationTests
{
    private const string CatalogUrl = "/api/v2/admin/catalog-configuration";

    private const int HomeFurnishingCategory = (int)CategoryType.HomeFurnishing;

    private const int RealEstateCategory = (int)CategoryType.RealEstate;

    private const int CharityCategory = (int)CategoryType.Charity;

    [LocalDbFact]
    public async Task The_super_admin_approves_and_rejects_ad_payments_and_unauthorized_callers_cannot()
    {
        await WithCatalogResetAsync(async () =>
        {
            var me = await DataAsync(_api.Admin, "/api/v2/admin/me/permissions");
            Assert.True(me.GetProperty("admin").GetProperty("isSuperAdmin").GetBoolean());

            await EnableAsync(35m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var methodId = await FirstMethodIdAsync();
            var first = await _api.SeedAsync(Generic, ownerId, "سوبر-" + Guid.NewGuid().ToString("N"));
            var second = await _api.SeedAsync(Generic, ownerId, "سوبر-" + Guid.NewGuid().ToString("N"));

            var firstPayment = (await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{first}/payment", methodId))
                .GetProperty("paymentSubmissionId").GetGuid();
            var secondPayment = (await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{second}/payment", methodId))
                .GetProperty("paymentSubmissionId").GetGuid();

            var firstReview = $"/api/v2/admin/ad-payment/submissions/{firstPayment}/review";
            var secondReview = $"/api/v2/admin/ad-payment/submissions/{secondPayment}/review";

            var (_, stranger) = await _api.CreateOwnerAsync();
            var categoriesOnly = await CreateAdminAsync("categories", "View", "Edit");

            Assert.Equal(HttpStatusCode.Unauthorized,
                (await _api.Anonymous.PatchAsync(firstReview, Json(new { decision = "Approved" }))).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden,
                (await stranger.PatchAsync(firstReview, Json(new { decision = "Approved" }))).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden,
                (await categoriesOnly.PatchAsync(firstReview, Json(new { decision = "Approved" }))).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden,
                (await owner.PatchAsync(secondReview, Json(new { decision = "Rejected" }))).StatusCode);

            var approved = await PatchOkAsync(_api.Admin, firstReview, new { decision = "Approved" });
            Assert.Equal((int)PaymentStatus.Approved, approved.GetProperty("payment").GetProperty("paymentStatus").GetInt32());

            var rejected = await PatchOkAsync(_api.Admin, secondReview,
                new { decision = "Rejected", rejectionReason = "الإيصال مش واضح" });
            Assert.Equal((int)PaymentStatus.Rejected, rejected.GetProperty("payment").GetProperty("paymentStatus").GetInt32());

            var generic = (await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{second}/payment/resubmit", methodId))
                .GetProperty("paymentSubmissionId").GetGuid();
            await PatchOkAsync(_api.Admin, $"/api/v2/admin/payments/{generic}/approve", new { });

            var actions = await DataAsync(_api.Admin, "/api/v2/admin/dashboard");
            var paymentsAction = actions.GetProperty("quickActions").EnumerateArray()
                .Single(action => action.GetProperty("key").GetString() == "review-payments");
            Assert.Equal("payments", paymentsAction.GetProperty("pageKey").GetString());
        });
    }

    [LocalDbFact]
    public async Task The_hierarchy_lists_every_category_sub_category_and_module_from_the_catalog()
    {
        var hierarchy = await DataAsync(_api.Admin, CatalogUrl);
        var categories = hierarchy.GetProperty("categories").EnumerateArray().ToList();

        foreach (var entry in CategoryCatalog.Categories)
            Assert.Contains(categories, category => category.GetProperty("nodeId").GetInt32() == (int)entry.Id);

        foreach (var entry in CategoryCatalog.SubCategories)
        {
            var parent = categories.Single(category => category.GetProperty("nodeId").GetInt32() == (int)entry.Category);
            var sub = parent.GetProperty("children").EnumerateArray()
                .SingleOrDefault(child => child.GetProperty("nodeId").GetInt32() == (int)entry.Id);

            Assert.True(sub.ValueKind == JsonValueKind.Object, $"sub-category {entry.Id} missing under {entry.Category}");
            Assert.Equal($"Category:{(int)entry.Category}", sub.GetProperty("parentKey").GetString());

            if (ListingModuleCatalog.ModuleOf(entry.Id) is { } module)
            {
                var child = Assert.Single(sub.GetProperty("children").EnumerateArray());
                Assert.Equal(module.ToString(), child.GetProperty("moduleKey").GetString());
                Assert.Equal((int)module, child.GetProperty("nodeId").GetInt32());
            }
        }

        var charity = categories.Single(category => category.GetProperty("nodeId").GetInt32() == CharityCategory);
        Assert.True(charity.GetProperty("isProtectedFree").GetBoolean());

        var options = await DataAsync(_api.Admin, $"{CatalogUrl}/options");
        Assert.Contains(options.GetProperty("areas").EnumerateArray(), area => area.GetProperty("key").GetString() == "AdCreation");
        Assert.Contains(CharityCategory, options.GetProperty("protectedFreeCategoryIds").EnumerateArray().Select(id => id.GetInt32()));
    }

    [LocalDbFact]
    public async Task A_category_rule_reaches_its_modules_a_module_override_wins_and_reset_restores_inheritance()
    {
        await WithCatalogResetAsync(async () =>
        {
            await PutCatalogOkAsync("payment", new
            {
                targets = new[] { Target("Category", HomeFurnishingCategory) },
                paymentEnabled = true,
                paymentAmount = 70m
            });

            var (ownerId, owner) = await _api.CreateOwnerAsync();

            var furniture = await _api.SeedAsync(Generic, ownerId, "أثاث-" + Guid.NewGuid().ToString("N"));
            Assert.Equal(HttpStatusCode.PaymentRequired, await ApproveStatusAsync(Generic, furniture));

            var config = await DataAsync(owner, $"/api/v2/ad-payment/config?type={Generic}");
            Assert.True(config.GetProperty("isEnabled").GetBoolean());
            Assert.Equal(70m, config.GetProperty("price").GetDecimal());
            Assert.Equal("Module", config.GetProperty("scope").GetString());

            var global = await DataAsync(owner, "/api/v2/ad-payment/config");
            Assert.False(global.GetProperty("isEnabled").GetBoolean());
            Assert.True(global.GetProperty("anyCategoryRequiresPayment").GetBoolean());

            var paid = await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{furniture}/payment", await FirstMethodIdAsync());
            Assert.Equal(70m, paid.GetProperty("amount").GetDecimal());

            var land = await _api.SeedAsync(ListingModuleType.Land, ownerId, "أرض-" + Guid.NewGuid().ToString("N"));
            Assert.Equal(HttpStatusCode.OK, await ApproveStatusAsync(ListingModuleType.Land, land));

            await PutCatalogOkAsync("payment", new
            {
                targets = new[] { Target("Module", (int)Generic) },
                paymentEnabled = false
            });

            var freeFurniture = await _api.SeedAsync(Generic, ownerId, "أثاث-" + Guid.NewGuid().ToString("N"));
            Assert.Equal(HttpStatusCode.OK, await ApproveStatusAsync(Generic, freeFurniture));

            var kitchen = await _api.SeedAsync(ListingModuleType.KitchenTool, ownerId, "مطبخ-" + Guid.NewGuid().ToString("N"));
            Assert.Equal(HttpStatusCode.PaymentRequired, await ApproveStatusAsync(ListingModuleType.KitchenTool, kitchen));

            var node = await DataAsync(_api.Admin, $"{CatalogUrl}/nodes/Module/{(int)Generic}");
            Assert.Equal("Explicit", node.GetProperty("payment").GetProperty("effective").GetProperty("inheritanceMode").GetString());

            var reset = await PostCatalogOkAsync("reset", new { targets = new[] { Target("Module", (int)Generic) }, kind = "Payment" });
            Assert.Equal(1, reset.GetProperty("removed").GetInt32());

            var inheriting = await DataAsync(_api.Admin, $"{CatalogUrl}/nodes/Module/{(int)Generic}");
            var effective = inheriting.GetProperty("payment").GetProperty("effective");
            Assert.Equal("Inherited", effective.GetProperty("inheritanceMode").GetString());
            Assert.Equal($"Category:{HomeFurnishingCategory}", effective.GetProperty("sourceKey").GetString());
            Assert.False(inheriting.GetProperty("payment").TryGetProperty("explicit", out var removed) &&
                         removed.ValueKind != JsonValueKind.Null);

            var again = await _api.SeedAsync(Generic, ownerId, "أثاث-" + Guid.NewGuid().ToString("N"));
            Assert.Equal(HttpStatusCode.PaymentRequired, await ApproveStatusAsync(Generic, again));
        });
    }

    [LocalDbFact]
    public async Task Bulk_requests_enable_and_disable_several_categories_at_once_and_are_audited()
    {
        await WithCatalogResetAsync(async () =>
        {
            var targets = new[] { Target("Category", HomeFurnishingCategory), Target("Category", RealEstateCategory) };

            var enabled = await PutCatalogOkAsync("payment", new { targets, paymentEnabled = true, paymentAmount = 30m });
            Assert.Equal(2, enabled.GetProperty("created").GetInt32());

            var (ownerId, _) = await _api.CreateOwnerAsync();
            var land = await _api.SeedAsync(ListingModuleType.Land, ownerId, "أرض-" + Guid.NewGuid().ToString("N"));
            var furniture = await _api.SeedAsync(Generic, ownerId, "أثاث-" + Guid.NewGuid().ToString("N"));

            Assert.Equal(HttpStatusCode.PaymentRequired, await ApproveStatusAsync(ListingModuleType.Land, land));
            Assert.Equal(HttpStatusCode.PaymentRequired, await ApproveStatusAsync(Generic, furniture));

            var disabled = await PutCatalogOkAsync("payment", new { targets, paymentEnabled = false });
            Assert.Equal(2, disabled.GetProperty("updated").GetInt32());

            Assert.Equal(HttpStatusCode.OK, await ApproveStatusAsync(ListingModuleType.Land, land));
            Assert.Equal(HttpStatusCode.OK, await ApproveStatusAsync(Generic, furniture));

            var history = await DataAsync(_api.Admin, $"{CatalogUrl}/history?pageSize=50");
            Assert.Contains(history.GetProperty("items").EnumerateArray(),
                item => item.GetProperty("targetId").GetString() == $"Category:{RealEstateCategory}");

            var tooMany = Enumerable.Range(1, 101).Select(_ => Target("Category", HomeFurnishingCategory)).ToArray();
            Assert.Equal(HttpStatusCode.BadRequest,
                (await PutCatalogAsync(_api.Admin, "payment", new { targets = tooMany, paymentEnabled = false })).StatusCode);

            Assert.Equal(HttpStatusCode.BadRequest, (await PutCatalogAsync(_api.Admin, "payment",
                new { targets = new[] { Target("Category", 987) }, paymentEnabled = false })).StatusCode);
        });
    }

    [LocalDbFact]
    public async Task The_charity_category_and_its_descendants_stay_free_and_keep_normal_moderation()
    {
        await WithCatalogResetAsync(async () =>
        {
            foreach (var target in new[]
                     {
                         Target("Category", CharityCategory),
                         Target("SubCategory", (int)SubCategoryType.Rescues),
                         Target("Module", (int)ListingModuleType.BloodRequest)
                     })
            {
                var refused = await PutCatalogAsync(_api.Admin, "payment",
                    new { targets = new[] { target }, paymentEnabled = true, paymentAmount = 10m });
                Assert.Equal(HttpStatusCode.BadRequest, refused.StatusCode);
            }

            await EnableAsync(40m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var rescue = await _api.SeedAsync(ListingModuleType.Rescue, ownerId, "استغاثة-" + Guid.NewGuid().ToString("N"));

            Assert.False(await InQueueAsync(ListingModuleType.Rescue, string.Empty, rescue, "AwaitingPayment"));
            Assert.True(await InQueueAsync(ListingModuleType.Rescue, string.Empty, rescue));

            Assert.Equal(HttpStatusCode.BadRequest,
                (await SubmitAsync(owner, $"/api/v2/ads/{ListingModuleType.Rescue}/{rescue}/payment", await FirstMethodIdAsync())).StatusCode);

            var config = await DataAsync(owner, $"/api/v2/ad-payment/config?type={ListingModuleType.Rescue}");
            Assert.False(config.GetProperty("isEnabled").GetBoolean());
            Assert.True(config.GetProperty("isProtected").GetBoolean());

            Assert.Equal(HttpStatusCode.OK, await ApproveStatusAsync(ListingModuleType.Rescue, rescue));

            var other = await _api.SeedAsync(ListingModuleType.AskConsult, ownerId, "سؤال-" + Guid.NewGuid().ToString("N"));
            var rejected = await _api.Admin.PatchAsync($"/api/v2/admin/ads/{(int)ListingModuleType.AskConsult}/{other}/reject",
                Json(new { reason = (int)ListingRejectionReason.IncompleteData }));
            Assert.Equal(HttpStatusCode.OK, rejected.StatusCode);

            var publicConfig = await DataAsync(_api.Anonymous, "/api/v2/public/catalog-configuration");
            var charity = publicConfig.GetProperty("categories").EnumerateArray()
                .Single(category => category.GetProperty("nodeId").GetInt32() == CharityCategory);
            Assert.True(charity.GetProperty("payment").GetProperty("isFree").GetBoolean());
            Assert.False(charity.GetProperty("payment").GetProperty("paymentRequired").GetBoolean());
        });
    }

    [LocalDbFact]
    public async Task Scheduled_payment_rules_wait_for_their_date_and_bad_values_are_refused()
    {
        await WithCatalogResetAsync(async () =>
        {
            var tomorrow = DateTime.UtcNow.AddDays(1);

            await PutCatalogOkAsync("payment", new
            {
                targets = new[] { Target("Category", HomeFurnishingCategory) },
                paymentEnabled = true,
                paymentAmount = 90m,
                paymentActivationDate = tomorrow
            });

            var (ownerId, _) = await _api.CreateOwnerAsync();
            var furniture = await _api.SeedAsync(Generic, ownerId, "مجدول-" + Guid.NewGuid().ToString("N"));
            Assert.Equal(HttpStatusCode.OK, await ApproveStatusAsync(Generic, furniture));

            var scheduled = await DataAsync(_api.Admin, $"{CatalogUrl}/scheduled");
            Assert.Contains(scheduled.EnumerateArray(), row =>
                row.GetProperty("key").GetString() == $"Category:{HomeFurnishingCategory}" &&
                row.GetProperty("configurationStatus").GetString() == "Scheduled");

            var target = new[] { Target("Category", HomeFurnishingCategory) };

            Assert.Equal(HttpStatusCode.BadRequest, (await PutCatalogAsync(_api.Admin, "payment",
                new { targets = target, paymentEnabled = true, paymentAmount = 0 })).StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, (await PutCatalogAsync(_api.Admin, "payment",
                new { targets = target, paymentEnabled = true, paymentAmount = 10.555m })).StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, (await PutCatalogAsync(_api.Admin, "payment",
                new { targets = target, paymentEnabled = true, paymentAmount = 10m, currency = "EGYP" })).StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, (await PutCatalogAsync(_api.Admin, "payment",
                new { targets = target, paymentEnabled = true, paymentAmount = 10m, paymentActivationDate = "2027-01-01T00:00:00" })).StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, (await PutCatalogAsync(_api.Admin, "payment",
                new { targets = target, paymentEnabled = true, paymentAmount = 10m, paymentExpirationDate = DateTime.UtcNow.AddDays(-1) })).StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, (await PutCatalogAsync(_api.Admin, "payment",
                new { targets = target, paymentEnabled = true })).StatusCode);
        });
    }

    [LocalDbFact]
    public async Task Visibility_hides_nodes_from_the_selected_areas_only_and_blocks_creation_when_disabled()
    {
        await WithCatalogResetAsync(async () =>
        {
            await PutCatalogOkAsync("visibility", new
            {
                targets = new[] { Target("Category", HomeFurnishingCategory) },
                areas = new[] { "CategoryListing", "HomePage" },
                isVisible = false
            });

            Assert.DoesNotContain(HomeFurnishingCategory, await LookupIdsAsync("/api/lookups/categories"));
            Assert.DoesNotContain(HomeFurnishingCategory, await LookupIdsAsync("/api/lookups/categories-tree"));
            Assert.Contains(RealEstateCategory, await LookupIdsAsync("/api/lookups/categories"));

            var subCategories = await LookupIdsAsync($"/api/lookups/subcategories/{HomeFurnishingCategory}");
            Assert.Contains((int)SubCategoryType.Furniture, subCategories);

            Assert.Equal(HttpStatusCode.OK, (await NoCacheGetAsync(
                $"/api/lookups/create-ad-form/{HomeFurnishingCategory}/{(int)SubCategoryType.Furniture}")).StatusCode);

            await PutCatalogOkAsync("visibility", new
            {
                targets = new[] { Target("Module", (int)Generic) },
                areas = new[] { "AdCreation" },
                isVisible = true,
                isEnabled = false
            });

            Assert.Equal(HttpStatusCode.NotFound, (await NoCacheGetAsync(
                $"/api/lookups/create-ad-form/{HomeFurnishingCategory}/{(int)SubCategoryType.Furniture}")).StatusCode);

            var (_, owner) = await _api.CreateOwnerAsync();
            using (var form = new MultipartFormDataContent { { new StringContent("عنوان"), "Title" } })
            {
                var create = await owner.PostAsync("/api/" + ListingModuleCatalog.RouteOf(Generic), form);
                Assert.Equal(HttpStatusCode.Forbidden, create.StatusCode);
            }

            var effective = await DataAsync(_api.Admin, $"{CatalogUrl}/effective?type={Generic}");
            var creation = effective.GetProperty("visibility").EnumerateArray()
                .Single(area => area.GetProperty("areaKey").GetString() == "AdCreation");
            Assert.False(creation.GetProperty("isEnabled").GetBoolean());
            Assert.Equal("Explicit", creation.GetProperty("inheritanceMode").GetString());

            var home = effective.GetProperty("visibility").EnumerateArray()
                .Single(area => area.GetProperty("areaKey").GetString() == "HomePage");
            Assert.False(home.GetProperty("isVisible").GetBoolean());
            Assert.Equal("Inherited", home.GetProperty("inheritanceMode").GetString());

            var publicBody = (await GetAsync(_api.Anonymous, "/api/v2/public/catalog-configuration")).Body;
            Assert.DoesNotContain("updatedBy", publicBody, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("configurationId", publicBody, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("createdBy", publicBody, StringComparison.OrdinalIgnoreCase);

            await PostCatalogOkAsync("reset", new
            {
                targets = new[] { Target("Category", HomeFurnishingCategory), Target("Module", (int)Generic) },
                kind = "Visibility"
            });

            Assert.Contains(HomeFurnishingCategory, await LookupIdsAsync("/api/lookups/categories"));
            Assert.Equal(HttpStatusCode.OK, (await NoCacheGetAsync(
                $"/api/lookups/create-ad-form/{HomeFurnishingCategory}/{(int)SubCategoryType.Furniture}")).StatusCode);
        });
    }

    [LocalDbFact]
    public async Task Configuration_writes_need_the_matching_admin_permission()
    {
        await WithCatalogResetAsync(async () =>
        {
            var body = new { targets = new[] { Target("Category", HomeFurnishingCategory) }, paymentEnabled = false };
            var visibility = new { targets = new[] { Target("Category", HomeFurnishingCategory) }, areas = new[] { "Search" }, isVisible = true };

            var (_, user) = await _api.CreateOwnerAsync();
            Assert.Equal(HttpStatusCode.Unauthorized, (await PutCatalogAsync(_api.Anonymous, "payment", body)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await PutCatalogAsync(user, "payment", body)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await user.GetAsync(CatalogUrl)).StatusCode);

            var categoriesEditor = await CreateAdminAsync("categories", "View", "Edit");
            Assert.Equal(HttpStatusCode.Forbidden, (await PutCatalogAsync(categoriesEditor, "payment", body)).StatusCode);
            Assert.Equal(HttpStatusCode.OK, (await PutCatalogAsync(categoriesEditor, "visibility", visibility)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await categoriesEditor.PostAsync($"{CatalogUrl}/reset",
                Json(new { targets = body.targets, kind = "Payment" }))).StatusCode);

            var paymentsEditor = await CreateAdminAsync("payments", "View", "Edit");
            Assert.Equal(HttpStatusCode.OK, (await PutCatalogAsync(paymentsEditor, "payment", body)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await PutCatalogAsync(paymentsEditor, "visibility", visibility)).StatusCode);
        });
    }

    [LocalDbFact]
    public async Task Turning_payments_on_later_never_unpublishes_a_live_ad_or_charges_an_ad_created_before()
    {
        await WithCatalogResetAsync(async () =>
        {
            var (ownerId, owner) = await _api.CreateOwnerAsync();

            var live = await _api.SeedAsync(Generic, ownerId, "منشور-" + Guid.NewGuid().ToString("N"));
            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ads/{(int)Generic}/{live}/approve", new { });

            var older = await _api.SeedAsync(Generic, ownerId, "قديم-" + Guid.NewGuid().ToString("N"));

            await PutCatalogOkAsync("payment", new
            {
                targets = new[] { Target("Category", HomeFurnishingCategory) },
                paymentEnabled = true,
                paymentAmount = 25m
            });

            var feed = await FeedAsync(Generic, live);
            Assert.Contains(live.ToString(), (await GetAsync(_api.Anonymous, feed)).Body);

            Assert.True(await InQueueAsync(Generic, string.Empty, older));
            Assert.Equal(HttpStatusCode.OK, await ApproveStatusAsync(Generic, older));

            var status = await DataAsync(owner, $"/api/v2/ads/{Generic}/{live}/payment");
            Assert.False(status.GetProperty("isPaymentRequired").GetBoolean());
        });
    }

    private static object Target(string nodeType, int nodeId) => new { nodeType, nodeId };

    private async Task<HttpStatusCode> ApproveStatusAsync(ListingModuleType module, Guid id) =>
        (await _api.Admin.PatchAsync($"/api/v2/admin/ads/{(int)module}/{id}/approve", Json(new { }))).StatusCode;

    private static Task<HttpResponseMessage> PutCatalogAsync(HttpClient client, string path, object body) =>
        client.PutAsync($"{CatalogUrl}/{path}", Json(body));

    private async Task<JsonElement> PutCatalogOkAsync(string path, object body)
    {
        var response = await PutCatalogAsync(_api.Admin, path, body);
        var text = await response.Content.ReadAsStringAsync();

        Assert.True(response.StatusCode == HttpStatusCode.OK, $"{(int)response.StatusCode} {text}");

        return JsonDocument.Parse(text).RootElement.GetProperty("data").Clone();
    }

    private async Task<JsonElement> PostCatalogOkAsync(string path, object body)
    {
        var response = await _api.Admin.PostAsync($"{CatalogUrl}/{path}", Json(body));
        var text = await response.Content.ReadAsStringAsync();

        Assert.True(response.StatusCode == HttpStatusCode.OK, $"{(int)response.StatusCode} {text}");

        return JsonDocument.Parse(text).RootElement.GetProperty("data").Clone();
    }

    private async Task<HttpClient> CreateAdminAsync(string pageKey, params string[] permissions)
    {
        var (userId, client) = await _api.CreateOwnerAsync();

        var response = await _api.Admin.PostAsync("/api/v2/admin/admins", Json(new
        {
            userId,
            pages = new[] { new { pageKey, permissions } }
        }));

        Assert.True(response.IsSuccessStatusCode,
            $"promote: {(int)response.StatusCode} {await response.Content.ReadAsStringAsync()}");

        return client;
    }

    private Task<HttpResponseMessage> NoCacheGetAsync(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };

        return _api.Anonymous.SendAsync(request);
    }

    private async Task<IReadOnlyList<int>> LookupIdsAsync(string url)
    {
        var response = await NoCacheGetAsync(url);
        var text = await response.Content.ReadAsStringAsync();

        Assert.True(response.StatusCode == HttpStatusCode.OK, $"{(int)response.StatusCode} {text}");

        return JsonDocument.Parse(text).RootElement.GetProperty("data").EnumerateArray()
            .Select(item => item.GetProperty("id").GetInt32())
            .ToList();
    }

    private async Task WithCatalogResetAsync(Func<Task> body)
    {
        await ClearCatalogAsync();

        try
        {
            await WithPaymentsDisabledAfterAsync(body);
        }
        finally
        {
            await ClearCatalogAsync();
        }
    }

    private async Task ClearCatalogAsync()
    {
        using var scope = _api.CreateScope();

        await scope.ServiceProvider.GetRequiredService<AppDbContext>().CatalogConfigurations.ExecuteDeleteAsync();
        scope.ServiceProvider.GetRequiredService<ICatalogConfigurationProvider>().Invalidate();
    }
}
