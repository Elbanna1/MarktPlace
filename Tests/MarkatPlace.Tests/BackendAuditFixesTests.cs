using System.Reflection;
using Domain.Entities;
using Services.BannerBookings;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.BannerBookings;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class BackendAuditFixesTests
{
    [Fact]
    public async Task Banner_availability_reads_every_slot_with_one_overlap_query()
    {
        var end = DateTime.UtcNow.Date.AddDays(10);
        var repository = BannerRepositoryFake.Create(
            new BannerPlacementSetting
            {
                Id = 1, Location = BannerLocation.HomeSlider1, MaxSlots = 3, IsActive = true,
                DurationDays = BannerBookingCatalog.DefaultDurationDays
            },
            [
                new BannerBooking { SlotNumber = 1, EndDate = end, Location = BannerLocation.HomeSlider1 },
                new BannerBooking { SlotNumber = 3, EndDate = end.AddDays(2), Location = BannerLocation.HomeSlider1 }
            ]);

        var availability = await Service(repository).GetAvailabilityAsync(
            new BannerAvailabilityQuery { Location = BannerLocation.HomeSlider1 });

        var fake = BannerRepositoryFake.Of(repository);
        var overlapCall = Assert.Single(fake.OverlapCalls);
        Assert.Null(overlapCall);

        Assert.Equal(new[] { 1, 2, 3 }, availability.Slots.Select(slot => slot.SlotNumber));
        Assert.Equal(new[] { false, true, false }, availability.Slots.Select(slot => slot.IsAvailable));
        Assert.Equal(end, availability.Slots[0].ReservedUntil);
        Assert.Null(availability.Slots[1].ReservedUntil);
        Assert.Equal(end.AddDays(2), availability.Slots[2].AvailableFrom);
        Assert.Equal(new[] { 2 }, availability.AvailableSlots);
        Assert.True(availability.IsAvailable);
    }

    [Fact]
    public async Task Banner_availability_reports_a_fully_booked_placement_as_unavailable()
    {
        var end = DateTime.UtcNow.Date.AddDays(5);
        var repository = BannerRepositoryFake.Create(
            new BannerPlacementSetting
            {
                Id = 1, Location = BannerLocation.HomeSlider2, MaxSlots = 2, IsActive = true,
                DurationDays = BannerBookingCatalog.DefaultDurationDays
            },
            [
                new BannerBooking { SlotNumber = 1, EndDate = end, Location = BannerLocation.HomeSlider2 },
                new BannerBooking { SlotNumber = 2, EndDate = end, Location = BannerLocation.HomeSlider2 }
            ]);

        var availability = await Service(repository).GetAvailabilityAsync(
            new BannerAvailabilityQuery { Location = BannerLocation.HomeSlider2 });

        Assert.Single(BannerRepositoryFake.Of(repository).OverlapCalls);
        Assert.All(availability.Slots, slot => Assert.False(slot.IsAvailable));
        Assert.False(availability.IsAvailable);
        Assert.Null(availability.NextStartDate);
    }

    [Fact]
    public void An_admin_deleted_listing_notification_says_deleted_not_suspended()
    {
        var id = Guid.NewGuid();

        var content = NotificationCatalog.Moderation.DeletedByAdmin(ListingModuleType.Workshop, id, "ورشة الأمانة");

        Assert.Equal("تم حذف الإعلان", content.Title);
        Assert.Contains("بواسطة الإدارة", content.Message);
        Assert.Contains("ورشة الأمانة", content.Message);
        Assert.DoesNotContain("إيقاف", content.Message);
        Assert.DoesNotContain("مؤقت", content.Message);
        Assert.Equal(NotificationType.ListingSuspended, content.Type);
        Assert.Equal(
            NotificationCatalog.Moderation.Suspended(ListingModuleType.Workshop, id, null, null).DeepLink,
            content.DeepLink);
    }

    [Fact]
    public void The_admin_delete_path_uses_the_deletion_wording()
    {
        var source = File.ReadAllText(Path.Combine(
            RepositoryRoot.Path, "Core", "Services", "Admin", "AdminAdService.cs"));

        Assert.Contains("NotificationCatalog.Moderation.DeletedByAdmin(type, id, title)", source);
        Assert.DoesNotContain("تم حذف الإعلان بواسطة الإدارة", source);
    }

    private static BannerBookingService Service(IBannerBookingRepository repository) =>
        new(repository,
            SilentProxy<IPaymentService>.Create(),
            SilentProxy<IFileService>.Create(),
            SilentProxy<IPrivateFileStore>.Create(),
            SilentProxy<INotificationService>.Create(),
            null!,
            SilentProxy<IAdminAuditService>.Create(),
            SilentProxy<IAdminAlertService>.Create(),
            SilentProxy<IPublicResponseCache>.Create(),
            SilentProxy<ILookupCache>.Create());
}

public class BannerRepositoryFake : DispatchProxy
{
    private BannerPlacementSetting _placement = default!;
    private IReadOnlyList<BannerBooking> _bookings = [];

    public List<int?> OverlapCalls { get; } = new();

    public static IBannerBookingRepository Create(BannerPlacementSetting placement, IReadOnlyList<BannerBooking> bookings)
    {
        var proxy = DispatchProxy.Create<IBannerBookingRepository, BannerRepositoryFake>();
        var fake = Of(proxy);
        fake._placement = placement;
        fake._bookings = bookings;
        return proxy;
    }

    public static BannerRepositoryFake Of(IBannerBookingRepository proxy) => (BannerRepositoryFake)(object)proxy;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        switch (targetMethod!.Name)
        {
            case nameof(IBannerBookingRepository.GetPlacementAsync):
                return Task.FromResult<BannerPlacementSetting?>(_placement);

            case nameof(IBannerBookingRepository.GetOverlappingAsync):
                OverlapCalls.Add((int?)args![1]);
                var slot = (int?)args[1];
                IReadOnlyList<BannerBooking> matching = slot is { } number
                    ? _bookings.Where(booking => booking.SlotNumber == number).ToList()
                    : _bookings;
                return Task.FromResult(matching);

            default:
                throw new NotSupportedException(targetMethod.Name);
        }
    }
}
