using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public partial class AdPaymentIntegrationTests
{
    private const string PendingUrl = "/api/v2/admin/ads/pending";

    [LocalDbTheory]
    [MemberData(nameof(EveryModule))]
    public async Task An_unpaid_ad_enters_the_moderation_queue_only_after_its_payment_is_approved(
        ListingModuleType module, int? subCategoryId)
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(30m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var title = "طابور-" + Guid.NewGuid().ToString("N");
            var id = await _api.SeedAsync(module, ownerId, title, subCategoryId);
            var paymentUrl = $"/api/v2/ads/{module}/{id}/payment";

            Assert.False(await InQueueAsync(module, title, id));
            Assert.True(await InQueueAsync(module, title, id, "AwaitingPayment"));
            Assert.True(await InQueueAsync(module, title, id, "All"));

            var paymentId = (await SubmitOkAsync(owner, paymentUrl, await FirstMethodIdAsync()))
                .GetProperty("paymentSubmissionId").GetGuid();

            Assert.False(await InQueueAsync(module, title, id));
            Assert.True(await InQueueAsync(module, title, id, "AwaitingPayment"));

            var feed = await FeedAsync(module, id);
            Assert.DoesNotContain(id.ToString(), (await GetAsync(_api.Anonymous, feed)).Body);

            var review = await PatchOkAsync(_api.Admin,
                $"/api/v2/admin/ad-payment/submissions/{paymentId}/review", new { decision = "Approved" });
            Assert.Equal("AwaitingModeration", review.GetProperty("listingAction").GetString());

            Assert.True(await InQueueAsync(module, title, id));
            Assert.False(await InQueueAsync(module, title, id, "AwaitingPayment"));
            Assert.DoesNotContain(id.ToString(), (await GetAsync(_api.Anonymous, feed)).Body);

            var ready = await SingleNotificationAsync(_api.AdminId, NotificationType.AdminPendingListing, id);
            Assert.Equal(module, ready.ListingType);
            Assert.Contains("بعد تأكيد الدفع", ready.Title);

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ads/{(int)module}/{id}/approve", new { });

            Assert.Contains(id.ToString(), (await GetAsync(_api.Anonymous, feed)).Body);
            Assert.False(await InQueueAsync(module, title, id));
        });
    }

    [LocalDbFact]
    public async Task The_pending_count_ignores_unpaid_ads_and_counts_them_once_paid()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(40m);

            var before = await PendingCountAsync();

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var id = await _api.SeedAsync(Generic, ownerId, "عدد-" + Guid.NewGuid().ToString("N"));

            Assert.Equal(before, await PendingCountAsync());

            var paymentId = (await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{id}/payment", await FirstMethodIdAsync()))
                .GetProperty("paymentSubmissionId").GetGuid();

            Assert.Equal(before, await PendingCountAsync());

            await PatchOkAsync(_api.Admin,
                $"/api/v2/admin/ad-payment/submissions/{paymentId}/review", new { decision = "Approved" });

            Assert.Equal(before + 1, await PendingCountAsync());
        });
    }

    [LocalDbFact]
    public async Task A_rejected_payment_keeps_the_ad_out_of_the_queue_until_a_new_proof_is_approved()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(45m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var title = "مرفوض-" + Guid.NewGuid().ToString("N");
            var id = await _api.SeedAsync(Generic, ownerId, title);
            var paymentUrl = $"/api/v2/ads/{Generic}/{id}/payment";
            var methodId = await FirstMethodIdAsync();

            var first = (await SubmitOkAsync(owner, paymentUrl, methodId)).GetProperty("paymentSubmissionId").GetGuid();

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ad-payment/submissions/{first}/review",
                new { decision = "Rejected", rejectionReason = "الإيصال ناقص" });

            Assert.False(await InQueueAsync(Generic, title, id));
            Assert.Equal("PaymentRejected", (await DataAsync(owner, paymentUrl)).GetProperty("stage").GetString());
            Assert.Equal(0, await CountReadyAlertsAsync(id));

            var second = (await SubmitOkAsync(owner, paymentUrl + "/resubmit", methodId))
                .GetProperty("paymentSubmissionId").GetGuid();

            Assert.False(await InQueueAsync(Generic, title, id));

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ad-payment/submissions/{second}/review",
                new { decision = "Approved" });

            Assert.True(await InQueueAsync(Generic, title, id));
            Assert.Equal(1, await CountReadyAlertsAsync(id));

            Assert.Equal(HttpStatusCode.Conflict, (await _api.Admin.PatchAsync(
                $"/api/v2/admin/ad-payment/submissions/{second}/review", Json(new { decision = "Approved" }))).StatusCode);
            Assert.Equal(1, await CountReadyAlertsAsync(id));
        });
    }

    [LocalDbFact]
    public async Task Without_payments_the_queue_is_unchanged_and_grandfathered_ads_stay_in_it()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await PutOkAsync(_api.Admin, SettingsUrl, new { isEnabled = false, price = 0 });

            var (ownerId, _) = await _api.CreateOwnerAsync();
            var title = "مجاني-" + Guid.NewGuid().ToString("N");
            var id = await _api.SeedAsync(Generic, ownerId, title);

            Assert.True(await InQueueAsync(Generic, title, id));
            Assert.False(await InQueueAsync(Generic, title, id, "AwaitingPayment"));

            var invalid = await _api.Admin.GetAsync($"{PendingUrl}?paymentStage=Sometimes");
            Assert.Equal(HttpStatusCode.BadRequest, invalid.StatusCode);

            await EnableAsync(50m);

            Assert.True(await InQueueAsync(Generic, title, id));
            Assert.False(await InQueueAsync(Generic, title, id, "AwaitingPayment"));
        });
    }

    [LocalDbFact]
    public async Task Creation_notices_follow_the_payment_switch_for_the_owner_and_the_admins()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(60m);

            var (ownerId, _) = await _api.CreateOwnerAsync();
            var unpaid = await _api.SeedAsync(Generic, ownerId, "إشعار-" + Guid.NewGuid().ToString("N"));

            await RaiseCreationNoticesAsync(ownerId, Generic, unpaid);

            var ownerNotice = await SingleNotificationAsync(ownerId, NotificationType.ListingPublished, unpaid);
            Assert.Contains("في انتظار الدفع", ownerNotice.Title);
            Assert.Equal(0, await CountAdminPendingAlertsAsync(unpaid));

            await PutOkAsync(_api.Admin, SettingsUrl, new { isEnabled = false, price = 0 });

            var free = await _api.SeedAsync(Generic, ownerId, "مجاني-" + Guid.NewGuid().ToString("N"));

            await RaiseCreationNoticesAsync(ownerId, Generic, free);

            var freeNotice = await SingleNotificationAsync(ownerId, NotificationType.ListingPublished, free);
            Assert.DoesNotContain("الدفع", freeNotice.Title + freeNotice.Message);
            Assert.Contains("قيد المراجعة", freeNotice.Message);
            Assert.Equal(1, await CountAdminPendingAlertsAsync(free));
        });
    }

    private async Task RaiseCreationNoticesAsync(string ownerId, ListingModuleType module, Guid id)
    {
        using var scope = _api.CreateScope();

        await scope.ServiceProvider.GetRequiredService<INotificationService>()
            .NotifyAsync(ownerId, module, NotificationAction.Created, id, "إعلان");

        await scope.ServiceProvider.GetRequiredService<IAdminAlertService>()
            .NotifyPendingListingAsync(module, id, "إعلان", cycleStartedUtc: null);
    }

    private async Task<bool> InQueueAsync(ListingModuleType module, string title, Guid id, string? stage = null)
    {
        var url = $"{PendingUrl}?type={(int)module}&pageSize=100" +
                  (stage is null ? string.Empty : $"&paymentStage={stage}");

        var data = await DataAsync(_api.Admin, url);

        return data.GetProperty("items").EnumerateArray().Any(item => item.GetProperty("id").GetGuid() == id);
    }

    private async Task<int> PendingCountAsync() =>
        (await DataAsync(_api.Admin, $"{PendingUrl}/count")).GetInt32();

    private Task<int> CountReadyAlertsAsync(Guid id) => CountAdminPendingAlertsAsync(id, readyOnly: true);

    private async Task<int> CountAdminPendingAlertsAsync(Guid id, bool readyOnly = false)
    {
        using var scope = _api.CreateScope();

        var alerts = scope.ServiceProvider.GetRequiredService<AppDbContext>().Notifications
            .Where(notice => notice.ReferenceId == id &&
                             notice.UserId == _api.AdminId &&
                             notice.Type == NotificationType.AdminPendingListing);

        if (readyOnly)
            alerts = alerts.Where(notice => notice.Title.Contains("بعد تأكيد الدفع"));

        return await alerts.CountAsync();
    }
}
