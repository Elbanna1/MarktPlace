using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public partial class AdPaymentIntegrationTests
{
    [LocalDbFact]
    public async Task A_rejection_reason_is_optional_and_review_feedback_is_persisted_and_sent_to_the_owner()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(45m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var title = "ملاحظات-" + Guid.NewGuid().ToString("N");
            var id = await _api.SeedAsync(Generic, ownerId, title);
            var paymentUrl = $"/api/v2/ads/{Generic}/{id}/payment";
            var methodId = await FirstMethodIdAsync();

            var firstPayment = (await SubmitOkAsync(owner, paymentUrl, methodId)).GetProperty("paymentSubmissionId").GetGuid();
            const string rejectFeedback = "صوّر الإيصال كامل وواضح.";

            var rejected = await PatchOkAsync(_api.Admin, $"/api/v2/admin/ad-payment/submissions/{firstPayment}/review",
                new { decision = "Rejected", feedback = rejectFeedback });

            var rejectedRow = rejected.GetProperty("payment");
            Assert.Equal((int)PaymentStatus.Rejected, rejectedRow.GetProperty("paymentStatus").GetInt32());
            AssertAbsent(rejectedRow, "rejectionReason");
            Assert.Equal(rejectFeedback, rejectedRow.GetProperty("reviewFeedback").GetString());
            Assert.Equal(_api.AdminId, rejectedRow.GetProperty("reviewedBy").GetString());
            Assert.NotEqual(JsonValueKind.Null, rejectedRow.GetProperty("reviewedAt").ValueKind);
            Assert.Equal("None", rejected.GetProperty("listingAction").GetString());

            var status = await DataAsync(owner, paymentUrl);
            Assert.Equal("PaymentRejected", status.GetProperty("stage").GetString());
            Assert.Equal(rejectFeedback, status.GetProperty("reviewFeedback").GetString());
            AssertAbsent(status, "rejectionReason");
            Assert.True(status.GetProperty("canResubmit").GetBoolean());
            Assert.Equal(ModerationStatus.Pending, (await _api.ReadAsync(Generic, id)).ModerationStatus);

            var rejectedNotice = await SingleNotificationAsync(ownerId, NotificationType.PaymentRejected, firstPayment);
            Assert.Contains(title, rejectedNotice.Message);
            Assert.Contains(rejectFeedback, rejectedNotice.Message);
            Assert.DoesNotContain("السبب", rejectedNotice.Message);
            Assert.Equal(Generic, rejectedNotice.ListingType);
            Assert.False(rejectedNotice.IsRead);
            Assert.Contains(firstPayment.ToString(), rejectedNotice.DeepLink);

            var secondPayment = (await SubmitOkAsync(owner, paymentUrl + "/resubmit", methodId))
                .GetProperty("paymentSubmissionId").GetGuid();

            const string approveFeedback = "تمام، شكراً.";
            await PatchOkAsync(_api.Admin, $"/api/v2/admin/payments/{secondPayment}/approve", new { feedback = approveFeedback });

            var approvedRow = await DataAsync(_api.Admin, $"/api/v2/admin/ad-payment/submissions/{secondPayment}");
            Assert.Equal((int)PaymentStatus.Approved, approvedRow.GetProperty("paymentStatus").GetInt32());
            Assert.Equal(approveFeedback, approvedRow.GetProperty("reviewFeedback").GetString());

            var generic = await DataAsync(_api.Admin, $"/api/v2/admin/payments/{secondPayment}");
            Assert.Equal(approveFeedback, generic.GetProperty("reviewFeedback").GetString());

            var approvedNotice = await SingleNotificationAsync(ownerId, NotificationType.PaymentApproved, secondPayment);
            Assert.Contains(approveFeedback, approvedNotice.Message);

            var other = await _api.SeedAsync(Generic, ownerId, "رفض-بدون-سبب-" + Guid.NewGuid().ToString("N"));
            var thirdPayment = (await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{other}/payment", methodId))
                .GetProperty("paymentSubmissionId").GetGuid();

            var genericReject = await PatchOkAsync(_api.Admin, $"/api/v2/admin/payments/{thirdPayment}/reject", new { });
            Assert.Equal((int)PaymentStatus.Rejected, genericReject.GetProperty("status").GetInt32());
            AssertAbsent(genericReject, "rejectReason");

            const string reason = "المبلغ ناقص";
            var withReason = await _api.SeedAsync(Generic, ownerId, "رفض-بسبب-" + Guid.NewGuid().ToString("N"));
            var fourthPayment = (await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{withReason}/payment", methodId))
                .GetProperty("paymentSubmissionId").GetGuid();

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ad-payment/submissions/{fourthPayment}/review",
                new { decision = "Rejected", rejectionReason = reason });

            Assert.Contains(reason,
                (await SingleNotificationAsync(ownerId, NotificationType.PaymentRejected, fourthPayment)).Message);
            Assert.Equal(reason, (await DataAsync(owner, $"/api/v2/ads/{Generic}/{withReason}/payment"))
                .GetProperty("rejectionReason").GetString());
        });
    }

    [LocalDbFact]
    public async Task Admins_are_alerted_with_the_listing_and_payer_and_a_resubmission_is_flagged()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(35m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var title = "تنبيه-" + Guid.NewGuid().ToString("N");
            var id = await _api.SeedAsync(Generic, ownerId, title);
            var paymentUrl = $"/api/v2/ads/{Generic}/{id}/payment";

            var config = await DataAsync(_api.Admin, SettingsUrl);
            var method = config.GetProperty("paymentMethods")[0];
            var methodName = method.GetProperty("arabicName").GetString() ?? method.GetProperty("name").GetString()!;

            var firstPayment = (await SubmitOkAsync(owner, paymentUrl, method.GetProperty("id").GetInt32()))
                .GetProperty("paymentSubmissionId").GetGuid();

            var alert = await SingleNotificationAsync(_api.AdminId, NotificationType.AdminNewPayment, firstPayment);
            Assert.Equal("إثبات دفع إعلان جديد", alert.Title);
            Assert.Contains(title, alert.Message);
            Assert.Contains("35", alert.Message);
            Assert.DoesNotContain($"({methodName})", alert.Message);
            Assert.Equal(Generic, alert.ListingType);

            Assert.Equal(0, await CountNotificationsAsync(ownerId, NotificationType.AdminNewPayment));
            Assert.Equal(1, await CountNotificationsAsync(ownerId, NotificationType.PaymentSubmitted));

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ad-payment/submissions/{firstPayment}/review",
                new { decision = "Rejected" });

            var secondPayment = (await SubmitOkAsync(owner, paymentUrl + "/resubmit", method.GetProperty("id").GetInt32()))
                .GetProperty("paymentSubmissionId").GetGuid();

            var resubmitted = await SingleNotificationAsync(_api.AdminId, NotificationType.AdminNewPayment, secondPayment);
            Assert.Equal("إعادة رفع إثبات دفع إعلان", resubmitted.Title);
            Assert.Contains(title, resubmitted.Message);
        });
    }

    [LocalDbFact]
    public async Task Concurrent_submissions_and_concurrent_reviews_leave_one_pending_payment_and_one_decision()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(30m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var id = await _api.SeedAsync(Generic, ownerId, "تزامن-" + Guid.NewGuid().ToString("N"));
            var paymentUrl = $"/api/v2/ads/{Generic}/{id}/payment";
            var methodId = await FirstMethodIdAsync();

            var submissions = await Task.WhenAll(
                Enumerable.Range(0, 5).Select(_ => SubmitAsync(owner, paymentUrl, methodId)));

            Assert.Equal(1, submissions.Count(response => response.StatusCode == HttpStatusCode.Created));
            Assert.All(submissions.Where(response => response.StatusCode != HttpStatusCode.Created),
                response => Assert.Equal(HttpStatusCode.Conflict, response.StatusCode));

            using (var scope = _api.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                Assert.Equal(1, await context.Payments.CountAsync(payment => payment.TargetId == id));
            }

            var paymentId = (await ReadCreatedAsync(submissions.Single(r => r.StatusCode == HttpStatusCode.Created)))
                .GetProperty("paymentSubmissionId").GetGuid();
            var reviewUrl = $"/api/v2/admin/ad-payment/submissions/{paymentId}/review";

            var reviews = await Task.WhenAll(
                _api.Admin.PatchAsync(reviewUrl, Json(new { decision = "Approved" })),
                _api.Admin.PatchAsync(reviewUrl, Json(new { decision = "Rejected", rejectionReason = "مكرر" })),
                _api.Admin.PatchAsync(reviewUrl, Json(new { decision = "Approved" })));

            Assert.Equal(1, reviews.Count(response => response.IsSuccessStatusCode));
            Assert.All(reviews.Where(response => !response.IsSuccessStatusCode),
                response => Assert.Equal(HttpStatusCode.Conflict, response.StatusCode));

            using var check = _api.CreateScope();
            var db = check.ServiceProvider.GetRequiredService<AppDbContext>();

            Assert.Equal(1, await db.Notifications.CountAsync(notice =>
                notice.UserId == ownerId && notice.ReferenceId == paymentId &&
                (notice.Type == NotificationType.PaymentApproved || notice.Type == NotificationType.PaymentRejected)));
        });
    }

    [LocalDbFact]
    public async Task A_refund_keeps_the_reviewer_and_notifies_the_owner_with_its_own_type()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(20m);

            var (approverId, approver) = await CreatePaymentsAdminWithIdAsync("View", "Approve");
            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var id = await _api.SeedAsync(Generic, ownerId, "استرداد-" + Guid.NewGuid().ToString("N"));

            var paymentId = (await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{id}/payment", await FirstMethodIdAsync()))
                .GetProperty("paymentSubmissionId").GetGuid();

            await PatchOkAsync(approver, $"/api/v2/admin/ad-payment/submissions/{paymentId}/review",
                new { decision = "Approved" });

            Assert.Equal(HttpStatusCode.Forbidden, (await approver.PatchAsync(
                $"/api/v2/admin/payments/{paymentId}/refund", Json(new { reason = "خطأ" }))).StatusCode);

            var refunded = await PatchOkAsync(_api.Admin, $"/api/v2/admin/payments/{paymentId}/refund",
                new { reason = "الإعلان اتلغى" });
            Assert.Equal((int)PaymentStatus.Refunded, refunded.GetProperty("status").GetInt32());
            Assert.Equal(approverId, refunded.GetProperty("reviewedBy").GetString());

            Assert.Equal(HttpStatusCode.BadRequest, (await _api.Admin.PatchAsync(
                $"/api/v2/admin/payments/{paymentId}/refund", Json(new { reason = "تاني" }))).StatusCode);

            var notice = await SingleNotificationAsync(ownerId, NotificationType.PaymentRefunded, paymentId);
            Assert.Contains("الإعلان اتلغى", notice.Message);

            var status = await DataAsync(owner, $"/api/v2/ads/{Generic}/{id}/payment");
            Assert.True(status.GetProperty("isPaymentRequired").GetBoolean());
            Assert.Equal(HttpStatusCode.PaymentRequired,
                (await _api.Admin.PatchAsync($"/api/v2/admin/ads/{(int)Generic}/{id}/approve", Json(new { }))).StatusCode);
        });
    }

    [LocalDbFact]
    public async Task Payment_notifications_reach_only_their_owner_in_real_time_and_stay_readable_later()
    {
        await WithPaymentsDisabledAfterAsync(async () =>
        {
            await EnableAsync(15m);

            var (ownerId, owner) = await _api.CreateOwnerAsync();
            var (_, stranger) = await _api.CreateOwnerAsync();
            var id = await _api.SeedAsync(Generic, ownerId, "لحظي-" + Guid.NewGuid().ToString("N"));

            var ownerInbox = new ConcurrentQueue<JsonElement>();
            var strangerInbox = new ConcurrentQueue<JsonElement>();
            var adminInbox = new ConcurrentQueue<JsonElement>();

            await using var ownerHub = _api.ConnectNotifications(owner);
            await using var strangerHub = _api.ConnectNotifications(stranger);
            await using var adminHub = _api.ConnectNotifications(_api.Admin);

            ownerHub.On<JsonElement>("ReceiveNotification", notice => ownerInbox.Enqueue(notice));
            strangerHub.On<JsonElement>("ReceiveNotification", notice => strangerInbox.Enqueue(notice));
            adminHub.On<JsonElement>("ReceiveNotification", notice => adminInbox.Enqueue(notice));

            await Task.WhenAll(ownerHub.StartAsync(), strangerHub.StartAsync(), adminHub.StartAsync());

            var paymentId = (await SubmitOkAsync(owner, $"/api/v2/ads/{Generic}/{id}/payment", await FirstMethodIdAsync()))
                .GetProperty("paymentSubmissionId").GetGuid();

            var submitted = await WaitForAsync(ownerInbox, paymentId, NotificationType.PaymentSubmitted);
            Assert.Equal((int)Generic, submitted.GetProperty("listingType").GetInt32());

            await WaitForAsync(adminInbox, paymentId, NotificationType.AdminNewPayment);

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ad-payment/submissions/{paymentId}/review",
                new { decision = "Rejected", rejectionReason = "مش واضح" });

            var rejected = await WaitForAsync(ownerInbox, paymentId, NotificationType.PaymentRejected);
            Assert.Contains("مش واضح", rejected.GetProperty("message").GetString());

            Assert.DoesNotContain(strangerInbox, notice => ReferenceOf(notice) == paymentId);
            Assert.Single(ownerInbox, notice =>
                ReferenceOf(notice) == paymentId && TypeOf(notice) == NotificationType.PaymentRejected);

            var stored = await SingleNotificationAsync(ownerId, NotificationType.PaymentRejected, paymentId);
            Assert.Equal(stored.Id, rejected.GetProperty("id").GetGuid());

            var unreadBefore = (await DataAsync(owner, "/api/notifications/unread-count")).GetInt32();
            Assert.True(unreadBefore >= 2);

            Assert.Equal(HttpStatusCode.NotFound,
                (await stranger.PatchAsync($"/api/notifications/{stored.Id}/read", null)).StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized,
                (await _api.Anonymous.GetAsync("/api/notifications/unread-count")).StatusCode);

            var read = await owner.PatchAsync($"/api/notifications/{stored.Id}/read", null);
            Assert.Equal(HttpStatusCode.OK, read.StatusCode);
            Assert.Equal(unreadBefore - 1, (await DataAsync(owner, "/api/notifications/unread-count")).GetInt32());
            Assert.True((await SingleNotificationAsync(ownerId, NotificationType.PaymentRejected, paymentId)).IsRead);

            var (offlineId, offline) = await _api.CreateOwnerAsync();
            var offlineListing = await _api.SeedAsync(Generic, offlineId, "غير-متصل-" + Guid.NewGuid().ToString("N"));
            var offlinePayment = (await SubmitOkAsync(offline, $"/api/v2/ads/{Generic}/{offlineListing}/payment",
                await FirstMethodIdAsync())).GetProperty("paymentSubmissionId").GetGuid();

            await PatchOkAsync(_api.Admin, $"/api/v2/admin/ad-payment/submissions/{offlinePayment}/review",
                new { decision = "Approved" });

            var (_, inbox) = await GetAsync(offline, "/api/notifications?pageSize=50");
            Assert.Contains(offlinePayment.ToString(), inbox);
            Assert.DoesNotContain(strangerInbox, notice => ReferenceOf(notice) == offlinePayment);
        });
    }

    private static void AssertAbsent(JsonElement element, string name) =>
        Assert.True(!element.TryGetProperty(name, out var value) || value.ValueKind == JsonValueKind.Null,
            $"expected '{name}' to be null or omitted");

    private static Guid? ReferenceOf(JsonElement notice) =>
        notice.TryGetProperty("referenceId", out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetGuid()
            : null;

    private static NotificationType TypeOf(JsonElement notice) =>
        (NotificationType)notice.GetProperty("type").GetInt32();

    private static async Task<JsonElement> WaitForAsync(
        ConcurrentQueue<JsonElement> inbox, Guid referenceId, NotificationType type)
    {
        var deadline = DateTime.UtcNow.AddSeconds(15);

        while (DateTime.UtcNow < deadline)
        {
            foreach (var notice in inbox)
            {
                if (ReferenceOf(notice) == referenceId && TypeOf(notice) == type)
                    return notice;
            }

            await Task.Delay(50);
        }

        throw new Xunit.Sdk.XunitException($"No real-time {type} notification for {referenceId} arrived.");
    }

    private async Task<Domain.Entities.Notification> SingleNotificationAsync(
        string userId, NotificationType type, Guid referenceId)
    {
        using var scope = _api.CreateScope();

        var rows = await scope.ServiceProvider.GetRequiredService<AppDbContext>().Notifications
            .AsNoTracking()
            .Where(notice => notice.UserId == userId && notice.Type == type && notice.ReferenceId == referenceId)
            .ToListAsync();

        return Assert.Single(rows);
    }
}
