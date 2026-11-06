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
    [LocalDbFact]
    public async Task A_general_payment_proof_is_private_and_reaches_only_its_owner_and_admins()
    {
        var (ownerId, owner) = await _api.CreateOwnerAsync();
        var (_, stranger) = await _api.CreateOwnerAsync();
        var methodId = await FirstMethodIdAsync();

        var submitted = await SubmitAsync(owner, "/api/payments", methodId, tamperedAmount: "150");
        var text = await submitted.Content.ReadAsStringAsync();
        Assert.True(submitted.StatusCode == HttpStatusCode.Created, $"{(int)submitted.StatusCode} {text}");

        var payment = JsonDocument.Parse(text).RootElement.GetProperty("data");
        var paymentId = payment.GetProperty("id").GetGuid();
        Assert.Equal($"/api/payments/{paymentId}/screenshot", payment.GetProperty("screenshotUrl").GetString());

        using (var scope = _api.CreateScope())
        {
            var row = await scope.ServiceProvider.GetRequiredService<AppDbContext>().Payments
                .AsNoTracking().SingleAsync(p => p.Id == paymentId);

            Assert.DoesNotContain("/uploads/", row.ScreenshotUrl);
            Assert.Equal(PaymentPurpose.Unspecified, row.Purpose);
            Assert.NotNull(scope.ServiceProvider.GetRequiredService<IPrivateFileStore>().Open(row.ScreenshotPath));
        }

        var own = await owner.GetAsync($"/api/payments/{paymentId}/screenshot");
        Assert.Equal(HttpStatusCode.OK, own.StatusCode);
        Assert.Equal(Png(), await own.Content.ReadAsByteArrayAsync());
        Assert.Contains("no-store", own.Headers.CacheControl!.ToString());

        Assert.Equal(HttpStatusCode.NotFound, (await stranger.GetAsync($"/api/payments/{paymentId}/screenshot")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await _api.Anonymous.GetAsync($"/api/payments/{paymentId}/screenshot")).StatusCode);

        var adminRow = await DataAsync(_api.Admin, $"/api/v2/admin/payments/{paymentId}");
        Assert.Equal($"/api/v2/admin/payments/{paymentId}/screenshot", adminRow.GetProperty("screenshotUrl").GetString());
        Assert.Equal(HttpStatusCode.OK, (await _api.Admin.GetAsync($"/api/v2/admin/payments/{paymentId}/screenshot")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await owner.GetAsync($"/api/v2/admin/payments/{paymentId}/screenshot")).StatusCode);

        string ownerName;
        using (var scope = _api.CreateScope())
        {
            var user = await scope.ServiceProvider.GetRequiredService<AppDbContext>().Users
                .AsNoTracking().SingleAsync(u => u.Id == ownerId);
            ownerName = $"{user.FirstName} {user.SecondName}".Trim();
        }

        var alert = await SingleNotificationAsync(_api.AdminId, NotificationType.AdminNewPayment, paymentId);
        Assert.Contains(ownerName, alert.Message);
        Assert.DoesNotContain(payment.GetProperty("paymentMethod").GetProperty("name").GetString()!, alert.Message);

        await PatchOkAsync(_api.Admin, $"/api/v2/admin/payments/{paymentId}/approve", new { });

        var approved = await SingleNotificationAsync(ownerId, NotificationType.PaymentApproved, paymentId);
        Assert.Equal($"/payments/{paymentId}", approved.DeepLink);
    }

    [LocalDbFact]
    public async Task An_admin_cannot_refund_a_payment_they_made_themselves()
    {
        var submitted = await SubmitAsync(_api.Admin, "/api/payments", await FirstMethodIdAsync(), tamperedAmount: "75");
        var paymentId = JsonDocument.Parse(await submitted.Content.ReadAsStringAsync())
            .RootElement.GetProperty("data").GetProperty("id").GetGuid();

        Assert.Equal(HttpStatusCode.Forbidden, (await _api.Admin.PatchAsync(
            $"/api/v2/admin/payments/{paymentId}/approve", Json(new { }))).StatusCode);

        using (var scope = _api.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<AppDbContext>().Payments
                .Where(p => p.Id == paymentId)
                .ExecuteUpdateAsync(set => set.SetProperty(p => p.Status, PaymentStatus.Approved));
        }

        Assert.Equal(HttpStatusCode.Forbidden, (await _api.Admin.PatchAsync(
            $"/api/v2/admin/payments/{paymentId}/refund", Json(new { reason = "استرداد لنفسي" }))).StatusCode);

        using var check = _api.CreateScope();
        var status = await check.ServiceProvider.GetRequiredService<AppDbContext>().Payments
            .Where(p => p.Id == paymentId).Select(p => p.Status).SingleAsync();
        Assert.Equal(PaymentStatus.Approved, status);
    }
}
