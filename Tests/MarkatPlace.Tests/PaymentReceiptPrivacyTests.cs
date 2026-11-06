using System.Buffers.Binary;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public partial class AdPaymentIntegrationTests
{
    [LocalDbFact]
    public async Task A_banner_payment_receipt_is_private_and_reaches_only_its_owner_and_banner_admins()
    {
        var (_, owner) = await _api.CreateOwnerAsync();
        var (_, stranger) = await _api.CreateOwnerAsync();
        var (_, paymentsOnlyAdmin) = await CreatePaymentsAdminWithIdAsync("View", "Approve");

        var booking = await CreateBannerBookingAsync(owner);
        var bookingId = booking.GetProperty("id").GetGuid();
        var proofRoute = $"/api/banner-bookings/{bookingId}/payment-proof";

        try
        {
            Assert.Equal(proofRoute, booking.GetProperty("paymentProofUrl").GetString());

            string proofPath;
            using (var scope = _api.CreateScope())
            {
                var row = await scope.ServiceProvider.GetRequiredService<AppDbContext>().BannerBookings
                    .AsNoTracking().SingleAsync(b => b.Id == bookingId);

                Assert.Equal(proofRoute, row.PaymentProofUrl);
                Assert.DoesNotContain("/uploads/", row.PaymentProofUrl);
                proofPath = row.PaymentProofPath;

                var stored = scope.ServiceProvider.GetRequiredService<IPrivateFileStore>().Open(proofPath);
                Assert.NotNull(stored);
                await stored!.Content.DisposeAsync();
                Assert.Null(scope.ServiceProvider.GetRequiredService<IFileService>().Open(proofPath));
            }

            var own = await owner.GetAsync(proofRoute);
            Assert.Equal(HttpStatusCode.OK, own.StatusCode);
            Assert.Equal(Receipt(), await own.Content.ReadAsByteArrayAsync());
            Assert.Contains("no-store", own.Headers.CacheControl!.ToString());
            Assert.Equal("nosniff", own.Headers.GetValues("X-Content-Type-Options").Single());

            Assert.Equal(HttpStatusCode.NotFound, (await stranger.GetAsync(proofRoute)).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await paymentsOnlyAdmin.GetAsync(proofRoute)).StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, (await _api.Anonymous.GetAsync(proofRoute)).StatusCode);
            Assert.Equal(HttpStatusCode.OK, (await _api.Admin.GetAsync(proofRoute)).StatusCode);

            var adminView = await DataAsync(_api.Admin, $"/api/v2/admin/banner-requests/{bookingId}");
            Assert.Equal(proofRoute, adminView.GetProperty("paymentProofUrl").GetString());

            await DeleteBannerBookingAsync(bookingId);

            bookingId = Guid.Empty;

            using var after = _api.CreateScope();
            Assert.Null(after.ServiceProvider.GetRequiredService<IPrivateFileStore>().Open(proofPath));
        }
        finally
        {
            if (bookingId != Guid.Empty)
                await DeleteBannerBookingAsync(bookingId);
        }
    }

    [LocalDbFact]
    public async Task Legacy_public_receipts_are_refused_on_the_uploads_path_but_still_served_to_their_owner()
    {
        var (ownerId, owner) = await _api.CreateOwnerAsync();
        var (_, stranger) = await _api.CreateOwnerAsync();

        var booking = await CreateBannerBookingAsync(owner);
        var bookingId = booking.GetProperty("id").GetGuid();

        var paymentResponse = await SubmitAsync(owner, "/api/payments", await FirstMethodIdAsync(), tamperedAmount: "40");
        var paymentId = JsonDocument.Parse(await paymentResponse.Content.ReadAsStringAsync())
            .RootElement.GetProperty("data").GetProperty("id").GetGuid();

        var legacyName = $"legacy-{Guid.NewGuid():N}.png";
        string uploadsRoot;
        string bannerFile, paymentFile;

        using (var scope = _api.CreateScope())
        {
            uploadsRoot = scope.ServiceProvider.GetRequiredService<IOptions<FileStorageSettings>>().Value.UploadsRootPath!;
        }

        bannerFile = Path.Combine(uploadsRoot, ImageConstants.BannerBookingProofFolder, legacyName);
        paymentFile = Path.Combine(uploadsRoot, ImageConstants.PaymentsFolder, legacyName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(bannerFile)!);
            Directory.CreateDirectory(Path.GetDirectoryName(paymentFile)!);
            await File.WriteAllBytesAsync(bannerFile, Png());
            await File.WriteAllBytesAsync(paymentFile, Png());

            using (var scope = _api.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await db.BannerBookings.Where(b => b.Id == bookingId).ExecuteUpdateAsync(set => set
                    .SetProperty(b => b.PaymentProofUrl, $"/uploads/{ImageConstants.BannerBookingProofFolder}/{legacyName}")
                    .SetProperty(b => b.PaymentProofPath, $"{ImageConstants.BannerBookingProofFolder}/{legacyName}"));

                await db.Payments.Where(p => p.Id == paymentId).ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.ScreenshotUrl, $"/uploads/{ImageConstants.PaymentsFolder}/{legacyName}")
                    .SetProperty(p => p.ScreenshotPath, $"{ImageConstants.PaymentsFolder}/{legacyName}"));
            }

            Assert.Equal(HttpStatusCode.NotFound,
                (await _api.Anonymous.GetAsync($"/uploads/{ImageConstants.BannerBookingProofFolder}/{legacyName}")).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound,
                (await _api.Anonymous.GetAsync($"/uploads/{ImageConstants.PaymentsFolder}/{legacyName}")).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound,
                (await owner.GetAsync($"/UPLOADS/{ImageConstants.PaymentsFolder.ToUpperInvariant()}/{legacyName}")).StatusCode);

            var bannerProof = await owner.GetAsync($"/api/banner-bookings/{bookingId}/payment-proof");
            Assert.Equal(HttpStatusCode.OK, bannerProof.StatusCode);
            Assert.Equal(Png(), await bannerProof.Content.ReadAsByteArrayAsync());
            Assert.Equal(HttpStatusCode.NotFound,
                (await stranger.GetAsync($"/api/banner-bookings/{bookingId}/payment-proof")).StatusCode);

            var details = await DataAsync(owner, $"/api/banner-bookings/{bookingId}");
            Assert.Equal($"/api/banner-bookings/{bookingId}/payment-proof", details.GetProperty("paymentProofUrl").GetString());

            var paymentProof = await owner.GetAsync($"/api/payments/{paymentId}/screenshot");
            Assert.Equal(HttpStatusCode.OK, paymentProof.StatusCode);
            Assert.Equal(Png(), await paymentProof.Content.ReadAsByteArrayAsync());

            var paymentDetails = await DataAsync(owner, $"/api/payments/{paymentId}");
            Assert.Equal($"/api/payments/{paymentId}/screenshot", paymentDetails.GetProperty("screenshotUrl").GetString());

            var adminRow = await DataAsync(_api.Admin, $"/api/v2/admin/payments/{paymentId}");
            Assert.Equal($"/api/v2/admin/payments/{paymentId}/screenshot", adminRow.GetProperty("screenshotUrl").GetString());
            Assert.Equal(HttpStatusCode.OK,
                (await _api.Admin.GetAsync($"/api/v2/admin/payments/{paymentId}/screenshot")).StatusCode);

            await DeleteBannerBookingAsync(bookingId);
            bookingId = Guid.Empty;
            Assert.False(File.Exists(bannerFile));

            Assert.Equal(ownerId, (await DataAsync(owner, $"/api/payments/{paymentId}")).GetProperty("userId").GetString());
        }
        finally
        {
            if (bookingId != Guid.Empty)
                await DeleteBannerBookingAsync(bookingId);

            File.Delete(bannerFile);
            File.Delete(paymentFile);
        }
    }

    [Fact]
    public void The_nginx_site_refuses_every_receipt_folder_before_serving_uploads_from_disk()
    {
        var site = RepositoryRoot.ReadFile("deploy/nginx/api.shopiklopik.com.conf");

        var refusal = System.Text.RegularExpressions.Regex.Match(
            site, @"location ~\* \^/uploads/\(([^)]+)\)/ \{\s*return 404;\s*\}");
        Assert.True(refusal.Success, "The Nginx site has no 404 rule for the receipt folders.");

        Assert.Equal(
            ImageConstants.NonPublicUploadFolders.OrderBy(folder => folder),
            refusal.Groups[1].Value.Split('|').OrderBy(folder => folder));

        var media = site.IndexOf("location ~* ^/uploads/.+", StringComparison.Ordinal);
        Assert.True(media > refusal.Index, "The receipt rule must come before the media regex location.");
    }

    private async Task<JsonElement> CreateBannerBookingAsync(HttpClient owner)
    {
        var methodId = await FirstMethodIdAsync();

        (BannerLocation Location, int Slots, int DesktopWidth, int DesktopHeight, int MobileWidth, int MobileHeight)[] placements;

        using (var scope = _api.CreateScope())
        {
            placements = (await scope.ServiceProvider.GetRequiredService<AppDbContext>().BannerPlacementSettings
                    .AsNoTracking()
                    .Where(p => p.IsActive && p.Location != BannerLocation.SubCategoryBanner)
                    .OrderBy(p => p.DisplayOrder)
                    .Select(p => new { p.Location, p.MaxSlots, p.DesktopWidth, p.DesktopHeight, p.MobileWidth, p.MobileHeight })
                    .ToListAsync())
                .Select(p => (p.Location, Math.Max(1, p.MaxSlots), p.DesktopWidth, p.DesktopHeight, p.MobileWidth, p.MobileHeight))
                .ToArray();
        }

        var failures = new List<string>();

        foreach (var placement in placements)
        {
            for (var slot = 1; slot <= placement.Slots; slot++)
            {
                using var form = new MultipartFormDataContent
                {
                    { Image(PngOfSize(placement.DesktopWidth, placement.DesktopHeight)), "DesktopImage", "desktop.png" },
                    { Image(PngOfSize(placement.MobileWidth, placement.MobileHeight)), "MobileImage", "mobile.png" },
                    { Image(Receipt()), "PaymentProof", "receipt.png" },
                    { new StringContent("بانر اختبار " + Guid.NewGuid().ToString("N")[..8]), "Title" },
                    { new StringContent("اعرف أكتر"), "ButtonText" },
                    { new StringContent("/ads"), "TargetUrl" },
                    { new StringContent(((int)placement.Location).ToString()), "Location" },
                    { new StringContent(slot.ToString()), "SlotNumber" },
                    { new StringContent("معلن تجريبي"), "AdvertiserName" },
                    { new StringContent("01000000000"), "PhoneNumber" },
                    { new StringContent(methodId.ToString()), "PaymentMethodId" },
                    { new StringContent("true"), "ConfirmationAccepted" }
                };

                var response = await owner.PostAsync("/api/banner-bookings", form);
                var text = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    return JsonDocument.Parse(text).RootElement.GetProperty("data").Clone();

                failures.Add($"{placement.Location}/{slot}: {(int)response.StatusCode} {text}");
            }
        }

        throw new Xunit.Sdk.XunitException("No banner slot accepted a booking: " + string.Join(" | ", failures));
    }

    private async Task DeleteBannerBookingAsync(Guid bookingId)
    {
        var response = await _api.Admin.DeleteAsync($"/api/v2/admin/banner-requests/{bookingId}");

        Assert.True(response.IsSuccessStatusCode,
            $"delete {bookingId}: {(int)response.StatusCode} {await response.Content.ReadAsStringAsync()}");
    }

    private static ByteArrayContent Image(byte[] bytes)
    {
        var content = new ByteArrayContent(bytes);
        content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        return content;
    }

    private static byte[] Receipt() => PngOfSize(7, 5);

    private static byte[] PngOfSize(int width, int height)
    {
        var png = Png();
        BinaryPrimitives.WriteInt32BigEndian(png.AsSpan(16, 4), width);
        BinaryPrimitives.WriteInt32BigEndian(png.AsSpan(20, 4), height);
        return png;
    }
}
