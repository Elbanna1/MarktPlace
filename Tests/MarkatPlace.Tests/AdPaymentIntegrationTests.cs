using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Listings;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

[Collection(LifecycleApiCollection.Name)]
public class AdPaymentIntegrationTests
{
    private const ListingModuleType Cars = ListingModuleType.Advertisement;

    private const ListingModuleType Generic = ListingModuleType.Furniture;

    private const string SettingsUrl = "/api/v2/admin/ad-payment/settings";

    private const string ConfigUrl = "/api/v2/public/ad-payment/config";

    private readonly LifecycleApiFixture _api;

    public AdPaymentIntegrationTests(LifecycleApiFixture api)
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

    [LocalDbFact]
    public async Task The_admin_enables_disables_and_reprices_and_users_see_it_immediately()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            var initial = await DataAsync(_api.Anonymous, ConfigUrl);
            Assert.False(initial.GetProperty("isEnabled").GetBoolean());
            Assert.Equal(0, initial.GetProperty("paymentMethods").GetArrayLength());

            var refused = await PutAsync(_api.Admin, SettingsUrl, new { isEnabled = true, price = 0 });
            Assert.Equal(HttpStatusCode.BadRequest, refused.StatusCode);

            var negative = await PutAsync(_api.Admin, SettingsUrl, new { isEnabled = false, price = -5 });
            Assert.Equal(HttpStatusCode.BadRequest, negative.StatusCode);

            await EnableAsync(150m, "حوّل المبلغ وارفع صورة الإيصال.");

            var config = await DataAsync(_api.Anonymous, ConfigUrl);
            Assert.True(config.GetProperty("isEnabled").GetBoolean());
            Assert.Equal(150m, config.GetProperty("price").GetDecimal());
            Assert.Equal("EGP", config.GetProperty("currency").GetString());
            Assert.Equal("حوّل المبلغ وارفع صورة الإيصال.", config.GetProperty("instructions").GetString());
            Assert.True(config.GetProperty("paymentMethods").GetArrayLength() > 0);

            await EnableAsync(275.5m);
            Assert.Equal(275.5m, (await DataAsync(_api.Anonymous, ConfigUrl)).GetProperty("price").GetDecimal());

            var methodId = (await DataAsync(_api.Anonymous, ConfigUrl))
                .GetProperty("paymentMethods")[0].GetProperty("id").GetInt32();

            await PutOkAsync(_api.Admin, SettingsUrl,
                new { isEnabled = true, price = 99, paymentMethodIds = new[] { methodId } });

            var narrowed = await DataAsync(_api.Anonymous, ConfigUrl);
            Assert.Equal(1, narrowed.GetProperty("paymentMethods").GetArrayLength());
            Assert.Equal(methodId, narrowed.GetProperty("paymentMethods")[0].GetProperty("id").GetInt32());

            var unknown = await PutAsync(_api.Admin, SettingsUrl,
                new { isEnabled = true, price = 99, paymentMethodIds = new[] { 987654 } });
            Assert.Equal(HttpStatusCode.BadRequest, unknown.StatusCode);

            var settings = await DataAsync(_api.Admin, SettingsUrl);
            Assert.Equal(_api.AdminId, settings.GetProperty("updatedBy").GetString());
            Assert.NotEqual(JsonValueKind.Null, settings.GetProperty("updatedAt").ValueKind);

            await PutOkAsync(_api.Admin, SettingsUrl, new { isEnabled = false, price = 99 });
            Assert.False((await DataAsync(_api.Anonymous, ConfigUrl)).GetProperty("isEnabled").GetBoolean());

            var (_, user) = await _api.CreateOwnerAsync();
            Assert.Equal(HttpStatusCode.Forbidden,
                (await PutAsync(user, SettingsUrl, new { isEnabled = true, price = 1 })).StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized,
                (await PutAsync(_api.Anonymous, SettingsUrl, new { isEnabled = true, price = 1 })).StatusCode);
        });
    }

    [LocalDbFact]
    public async Task A_new_ad_is_paid_reviewed_and_still_moderated_before_it_goes_public()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(150m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var (_, stranger) = await _api.CreateOwnerAsync();
            var title = "مدفوع-" + Guid.NewGuid().ToString("N");
            var id = await _api.SeedAsync(Generic, ownerId, title);
            var paymentUrl = $"/api/v2/ads/{Generic}/{id}/payment";
            var methodId = await FirstMethodIdAsync();

            var blocked = await _api.Admin.PatchAsync(
                $"/api/v2/admin/ads/{(int)Generic}/{id}/approve", Json(new { }));
            Assert.Equal(HttpStatusCode.PaymentRequired, blocked.StatusCode);

            var status = await DataAsync(owner, paymentUrl);
            Assert.True(status.GetProperty("isPaymentRequired").GetBoolean());
            Assert.True(status.GetProperty("canSubmit").GetBoolean());
            Assert.False(status.GetProperty("hasPayment").GetBoolean());
            Assert.Equal(150m, status.GetProperty("currentPrice").GetDecimal());

            Assert.Equal(HttpStatusCode.NotFound, (await stranger.GetAsync(paymentUrl)).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await SubmitAsync(stranger, paymentUrl, methodId)).StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, (await SubmitAsync(_api.Anonymous, paymentUrl, methodId)).StatusCode);

            Assert.Equal(HttpStatusCode.BadRequest,
                (await SubmitAsync(owner, paymentUrl, methodId, withScreenshot: false)).StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest,
                (await SubmitAsync(owner, paymentUrl, methodId, screenshot: "not an image"u8.ToArray())).StatusCode);

            var submitted = await SubmitOkAsync(owner, paymentUrl, methodId, reference: "TX-123", tamperedAmount: "1");
            Assert.Equal(150m, submitted.GetProperty("amount").GetDecimal());
            Assert.Equal("EGP", submitted.GetProperty("currency").GetString());
            Assert.Equal((int)PaymentStatus.Pending, submitted.GetProperty("paymentStatus").GetInt32());
            Assert.Equal(id, submitted.GetProperty("advertisementId").GetGuid());

            var firstPayment = submitted.GetProperty("paymentSubmissionId").GetGuid();

            Assert.Equal(HttpStatusCode.Conflict, (await SubmitAsync(owner, paymentUrl, methodId)).StatusCode);

            var pending = await DataAsync(_api.Admin, $"/api/v2/admin/ad-payment/submissions?advertisementId={id}");
            var row = Assert.Single(pending.GetProperty("items").EnumerateArray());
            Assert.Equal(firstPayment, row.GetProperty("paymentSubmissionId").GetGuid());
            Assert.Equal(title, row.GetProperty("advertisementTitle").GetString());
            Assert.Equal(ownerId, row.GetProperty("ownerId").GetString());
            Assert.Equal("TX-123", row.GetProperty("transactionReference").GetString());
            Assert.False(string.IsNullOrWhiteSpace(row.GetProperty("categoryName").GetString()));

            var screenshotUrl = row.GetProperty("screenshotUrl").GetString()!;
            var screenshot = await _api.Admin.GetAsync(screenshotUrl);
            Assert.Equal(HttpStatusCode.OK, screenshot.StatusCode);
            Assert.Equal("image/png", screenshot.Content.Headers.ContentType?.MediaType);
            Assert.Equal(Png(), await screenshot.Content.ReadAsByteArrayAsync());

            Assert.Equal(HttpStatusCode.Unauthorized, (await _api.Anonymous.GetAsync(screenshotUrl)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await owner.GetAsync(screenshotUrl)).StatusCode);

            var storedPath = await ScreenshotPathAsync(firstPayment);
            Assert.DoesNotContain("uploads", storedPath, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(HttpStatusCode.NotFound, (await _api.Anonymous.GetAsync("/uploads/" + storedPath)).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await _api.Anonymous.GetAsync("/" + storedPath)).StatusCode);

            var reviewUrl = $"/api/v2/admin/ad-payment/submissions/{firstPayment}/review";

            Assert.Equal(HttpStatusCode.Forbidden,
                (await owner.PatchAsync(reviewUrl, Json(new { decision = "Approved" }))).StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest,
                (await _api.Admin.PatchAsync(reviewUrl, Json(new { decision = "Rejected" }))).StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest,
                (await _api.Admin.PatchAsync(reviewUrl, Json(new { decision = "Maybe" }))).StatusCode);

            var rejected = await PatchOkAsync(_api.Admin, reviewUrl,
                new { decision = "Rejected", rejectionReason = "الإيصال مش واضح" });
            Assert.Equal((int)PaymentStatus.Rejected, rejected.GetProperty("payment").GetProperty("paymentStatus").GetInt32());

            Assert.Equal(HttpStatusCode.Conflict,
                (await _api.Admin.PatchAsync(reviewUrl, Json(new { decision = "Approved" }))).StatusCode);

            var afterReject = await DataAsync(owner, paymentUrl);
            Assert.Equal((int)PaymentStatus.Rejected, afterReject.GetProperty("paymentStatus").GetInt32());
            Assert.Equal("الإيصال مش واضح", afterReject.GetProperty("rejectionReason").GetString());
            Assert.True(afterReject.GetProperty("canSubmit").GetBoolean());

            var second = await SubmitOkAsync(owner, paymentUrl, methodId);
            var secondPayment = second.GetProperty("paymentSubmissionId").GetGuid();

            var approved = await PatchOkAsync(_api.Admin,
                $"/api/v2/admin/ad-payment/submissions/{secondPayment}/review", new { decision = "Approved" });

            Assert.Equal("AwaitingModeration", approved.GetProperty("listingAction").GetString());
            Assert.Equal((await _api.ReadAsync(Generic, id)).ModerationStatus, ModerationStatus.Pending);
            Assert.Null((await _api.ReadAsync(Generic, id)).ExpireAt);

            var feed = await FeedAsync(Generic, id);
            Assert.DoesNotContain(id.ToString(), (await GetAsync(_api.Anonymous, feed)).Body);

            var afterApprove = await DataAsync(owner, paymentUrl);
            Assert.Equal((int)PaymentStatus.Approved, afterApprove.GetProperty("paymentStatus").GetInt32());
            Assert.False(afterApprove.GetProperty("isPaymentRequired").GetBoolean());
            Assert.Equal(HttpStatusCode.Conflict, (await SubmitAsync(owner, paymentUrl, methodId)).StatusCode);

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ads/{(int)Generic}/{id}/approve", new { });

            Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, feed)).Body);
            Assert.NotNull(await ConsumedAtAsync(secondPayment));

            var history = await DataAsync(_api.Admin, $"/api/v2/admin/ad-payment/history?advertisementId={id}");
            Assert.Equal(2, history.GetProperty("totalCount").GetInt32());

            var rejectedOnly = await DataAsync(_api.Admin,
                $"/api/v2/admin/ad-payment/history?status=Rejected&advertisementId={id}");
            Assert.Equal(firstPayment,
                Assert.Single(rejectedOnly.GetProperty("items").EnumerateArray()).GetProperty("paymentSubmissionId").GetGuid());

            var notified = await CountNotificationsAsync(ownerId,
                NotificationType.PaymentSubmitted, NotificationType.PaymentApproved, NotificationType.PaymentRejected);
            Assert.True(notified >= 4, $"expected submit/reject/submit/approve notices, got {notified}");
        });
    }

    [LocalDbFact]
    public async Task Paying_for_an_expired_ad_republishes_it_on_approval()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var title = "تجديد-" + Guid.NewGuid().ToString("N");
            var id = await _api.SeedAsync(Generic, ownerId, title);

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ads/{(int)Generic}/{id}/approve", new { });

            await EnableAsync(80m);

            var paymentUrl = $"/api/v2/ads/{Generic}/{id}/payment";
            var methodId = await FirstMethodIdAsync();

            Assert.Equal(HttpStatusCode.BadRequest, (await SubmitAsync(owner, paymentUrl, methodId)).StatusCode);

            var now = DateTime.UtcNow;
            await _api.SetWindowAsync(Generic, id, now.AddDays(-30).AddHours(-2), now.AddHours(-2));
            await _api.RunJobAsync(now);

            var feed = await FeedAsync(Generic, id);
            Assert.DoesNotContain(id.ToString(), (await GetAsync(_api.Anonymous, feed)).Body);

            Assert.True((await DataAsync(owner, paymentUrl)).GetProperty("isPaymentRequired").GetBoolean());

            var paymentId = (await SubmitOkAsync(owner, paymentUrl, methodId))
                .GetProperty("paymentSubmissionId").GetGuid();

            var reviewed = await PatchOkAsync(_api.Admin,
                $"/api/v2/admin/ad-payment/submissions/{paymentId}/review", new { decision = "approve" });

            Assert.Equal("Republished", reviewed.GetProperty("listingAction").GetString());
            Assert.Equal("Approved", reviewed.GetProperty("listingModerationStatus").GetString());

            var renewed = await _api.ReadAsync(Generic, id);
            Assert.Equal(TimeSpan.FromDays(30), renewed.ExpireAt - renewed.PublishedAt);
            Assert.Equal(1, renewed.RepublishCount);

            Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, feed)).Body);
            Assert.NotNull(await ConsumedAtAsync(paymentId));

            var expired = await GetAsync(_api.Admin,
                $"/api/v2/admin/ads/expired?pageSize=50&search={Uri.EscapeDataString(title)}");
            Assert.DoesNotContain(id.ToString(), expired.Body);
        });
    }

    [LocalDbFact]
    public async Task Disabled_payments_change_nothing_and_old_pending_ads_are_grandfathered()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            var (ownerId, owner) = await _api.CreateOwnerAsync();

            var free = await _api.SeedAsync(Generic, ownerId, "مجاني-" + Guid.NewGuid().ToString("N"));
            var methodId = await FirstMethodIdAsync();

            Assert.Equal(HttpStatusCode.BadRequest,
                (await SubmitAsync(owner, $"/api/v2/ads/{Generic}/{free}/payment", methodId)).StatusCode);
            Assert.False((await DataAsync(owner, $"/api/v2/ads/{Generic}/{free}/payment"))
                .GetProperty("isPaymentRequired").GetBoolean());

            var legacy = await _api.SeedAsync(Generic, ownerId, "قديم-" + Guid.NewGuid().ToString("N"));

            await EnableAsync(60m);

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ads/{(int)Generic}/{legacy}/approve", new { });
            Assert.Equal(ModerationStatus.Approved, (await _api.ReadAsync(Generic, legacy)).ModerationStatus);
        });
    }

    [LocalDbFact]
    public async Task An_admin_cannot_review_a_payment_they_submitted_themselves()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(40m);

            var id = await _api.SeedAsync(Generic, _api.AdminId, "إعلان-المسؤول-" + Guid.NewGuid().ToString("N"));

            var paymentId = (await SubmitOkAsync(_api.Admin, $"/api/v2/ads/{Generic}/{id}/payment", await FirstMethodIdAsync()))
                .GetProperty("paymentSubmissionId").GetGuid();

            var response = await _api.Admin.PatchAsync(
                $"/api/v2/admin/ad-payment/submissions/{paymentId}/review", Json(new { decision = "Approved" }));

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        });
    }

    [LocalDbFact]
    public async Task The_existing_payments_screen_approves_an_ad_payment_through_the_same_workflow()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(70m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var id = await _api.SeedAsync(Generic, ownerId, "شاشة-المدفوعات-" + Guid.NewGuid().ToString("N"));

            var paymentId = (await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{id}/payment", await FirstMethodIdAsync()))
                .GetProperty("paymentSubmissionId").GetGuid();

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/payments/{paymentId}/approve", new { });

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ads/{(int)Generic}/{id}/approve", new { });

            Assert.NotNull(await ConsumedAtAsync(paymentId));
            Assert.Equal(ModerationStatus.Approved, (await _api.ReadAsync(Generic, id)).ModerationStatus);
        });
    }

    [LocalDbTheory]
    [MemberData(nameof(EveryModule))]
    public async Task Every_module_supports_the_payment_workflow(ListingModuleType module, int? subCategoryId)
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(25m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var id = await _api.SeedAsync(module, ownerId, "وحدة-" + Guid.NewGuid().ToString("N"), subCategoryId);
            var paymentUrl = $"/api/v2/ads/{module}/{id}/payment";

            Assert.Equal(HttpStatusCode.PaymentRequired,
                (await _api.Admin.PatchAsync($"/api/v2/admin/ads/{(int)module}/{id}/approve", Json(new { }))).StatusCode);

            var paymentId = (await SubmitOkAsync(owner, paymentUrl, await FirstMethodIdAsync()))
                .GetProperty("paymentSubmissionId").GetGuid();

            await PatchOkAsync(_api.Admin,
                $"/api/v2/admin/ad-payment/submissions/{paymentId}/review", new { decision = "Approved" });

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}/approve", new { });

            var feed = await FeedAsync(module, id);
            Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, feed)).Body);
            Assert.NotNull(await ConsumedAtAsync(paymentId));
        });
    }

    private async Task WithPaymentsDisabledAfterAsync(Func<Task> body)
    {
        try
        {
            await body();
        }
        finally
        {
            await PutAsync(_api.Admin, SettingsUrl, new { isEnabled = false, price = 0 });
        }
    }

    private Task EnableAsync(decimal price, string? instructions = null) =>
        PutOkAsync(_api.Admin, SettingsUrl, new { isEnabled = true, price, instructions });

    private async Task<int> FirstMethodIdAsync() =>
        (await DataAsync(_api.Admin, SettingsUrl)).GetProperty("paymentMethods")[0].GetProperty("id").GetInt32();

    private async Task<string> FeedAsync(ListingModuleType module, Guid id)
    {
        var (_, body) = await GetAsync(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}");
        var details = JsonDocument.Parse(body).RootElement.GetProperty("data").GetProperty("detailsEndpoint").GetString()!;
        var feed = details[..details.LastIndexOf('/')];

        var kind = module switch
        {
            ListingModuleType.LostItem => "&postType=Lost",
            ListingModuleType.FoundItem => "&postType=Found",
            _ => string.Empty
        };

        return $"{feed}?pageSize=50{kind}&t={Guid.NewGuid():N}";
    }

    private async Task<string> ScreenshotPathAsync(Guid paymentId)
    {
        using var scope = _api.CreateScope();

        return await scope.ServiceProvider.GetRequiredService<AppDbContext>().Payments
            .Where(payment => payment.Id == paymentId)
            .Select(payment => payment.ScreenshotPath)
            .SingleAsync();
    }

    private async Task<DateTime?> ConsumedAtAsync(Guid paymentId)
    {
        using var scope = _api.CreateScope();

        return await scope.ServiceProvider.GetRequiredService<AppDbContext>().Payments
            .Where(payment => payment.Id == paymentId)
            .Select(payment => payment.ActivatedAt)
            .SingleAsync();
    }

    private async Task<int> CountNotificationsAsync(string userId, params NotificationType[] types)
    {
        using var scope = _api.CreateScope();

        return await scope.ServiceProvider.GetRequiredService<AppDbContext>().Notifications
            .CountAsync(notice => notice.UserId == userId && types.Contains(notice.Type));
    }

    private static byte[] Png() =>
    [
        0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
        0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
        0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
        0x08, 0x06, 0x00, 0x00, 0x00, 0x1F, 0x15, 0xC4, 0x89
    ];

    private static async Task<HttpResponseMessage> SubmitAsync(
        HttpClient client, string url, int methodId, bool withScreenshot = true, byte[]? screenshot = null,
        string? reference = null, string? tamperedAmount = null)
    {
        using var form = new MultipartFormDataContent();

        form.Add(new StringContent(methodId.ToString()), "PaymentMethodId");

        if (reference is not null)
            form.Add(new StringContent(reference), "TransactionReference");

        if (tamperedAmount is not null)
            form.Add(new StringContent(tamperedAmount), "Amount");

        if (withScreenshot)
        {
            var file = new ByteArrayContent(screenshot ?? Png());
            file.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            form.Add(file, "Screenshot", "receipt.png");
        }

        return await client.PostAsync(url, form);
    }

    private static async Task<JsonElement> SubmitOkAsync(
        HttpClient client, string url, int methodId, string? reference = null, string? tamperedAmount = null)
    {
        var response = await SubmitAsync(client, url, methodId, reference: reference, tamperedAmount: tamperedAmount);
        var text = await response.Content.ReadAsStringAsync();

        Assert.True(response.StatusCode == HttpStatusCode.Created, $"{url}: {(int)response.StatusCode} {text}");

        return JsonDocument.Parse(text).RootElement.GetProperty("data").Clone();
    }

    private static async Task<JsonElement> DataAsync(HttpClient client, string url)
    {
        var (status, body) = await GetAsync(client, url);

        Assert.True(status == HttpStatusCode.OK, $"{url}: {(int)status} {body}");

        return JsonDocument.Parse(body).RootElement.GetProperty("data").Clone();
    }

    private static async Task<(HttpStatusCode Status, string Body)> GetAsync(HttpClient client, string url)
    {
        var response = await client.GetAsync(url);

        return (response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    private static Task<HttpResponseMessage> PutAsync(HttpClient client, string url, object body) =>
        client.PutAsync(url, Json(body));

    private static async Task PutOkAsync(HttpClient client, string url, object body)
    {
        var response = await PutAsync(client, url, body);

        Assert.True(response.IsSuccessStatusCode,
            $"{url}: {(int)response.StatusCode} {await response.Content.ReadAsStringAsync()}");
    }

    private static async Task<JsonElement> PatchOkAsync(HttpClient client, string url, object body)
    {
        var response = await client.PatchAsync(url, Json(body));
        var text = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, $"{url}: {(int)response.StatusCode} {text}");

        return JsonDocument.Parse(text).RootElement.GetProperty("data").Clone();
    }

    private static StringContent Json(object body) =>
        new(JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json");
}
