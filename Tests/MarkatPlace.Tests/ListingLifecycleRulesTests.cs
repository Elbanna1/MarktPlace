using System.Text.RegularExpressions;
using Domain.Entities;
using Persistence.Listings;
using Shared.Constants;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class ListingLifecycleRulesTests
{
    private static readonly DateTime Now = new(2026, 11, 2, 12, 0, 0, DateTimeKind.Utc);

    public static IEnumerable<object[]> EveryModule() =>
        ModerationModuleRegistry.EntityTypes.Keys.Select(module => new object[] { module });

    [Theory]
    [MemberData(nameof(EveryModule))]
    public void Every_module_is_open_to_admin_republish(ListingModuleType module)
    {
        Assert.True(AdminRepublishPolicy.Supports(module));
        Assert.True(AdminRepublishPolicy.CanRepublish(module, ModerationStatus.Approved, Now.AddDays(-1), Now));
        Assert.False(AdminRepublishPolicy.CanRepublish(module, ModerationStatus.Pending, Now.AddDays(-1), Now));
    }

    [Theory]
    [MemberData(nameof(EveryModule))]
    public void Every_module_has_a_notification_subject_the_job_can_use(ListingModuleType module)
    {
        var subject = NotificationCatalog.For(module);

        Assert.False(string.IsNullOrWhiteSpace(subject.EntityName));
        Assert.False(string.IsNullOrWhiteSpace(subject.Route));
    }

    [Fact]
    public void Every_module_except_cars_carries_the_expiring_contract()
    {
        var missing = ModerationModuleRegistry.EntityTypes
            .Where(entry => entry.Value != typeof(Advertisement) &&
                            !typeof(IExpiringListing).IsAssignableFrom(entry.Value))
            .Select(entry => entry.Key)
            .ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void The_window_is_thirty_days_across_a_daylight_saving_change()
    {
        var start = new DateTime(2026, 3, 20, 22, 0, 0, DateTimeKind.Utc);

        Assert.Equal(TimeSpan.FromDays(30), ListingLifecycle.EndOf(start) - start);
    }

    [Fact]
    public void A_listing_expires_at_its_end_instant_not_a_tick_later()
    {
        var end = Now;

        Assert.False(ListingLifecycle.HasExpired(end, end.AddTicks(-1)));
        Assert.True(ListingLifecycle.HasExpired(end, end));
        Assert.False(ListingLifecycle.HasExpired(null, end));
    }

    [Theory]
    [InlineData(5, true)]
    [InlineData(5.001, false)]
    [InlineData(0.001, true)]
    [InlineData(0, false)]
    public void Expiring_soon_is_inclusive_of_five_days_and_exclusive_of_now(double daysLeft, bool expected)
    {
        Assert.Equal(expected, AdminRepublishPolicy.IsExpiringSoon(Now.AddDays(daysLeft), Now));
    }

    [Fact]
    public void The_alert_shows_the_date_as_it_reads_in_egypt()
    {
        var lateEvening = new DateTime(2026, 11, 2, 23, 30, 0, DateTimeKind.Utc);

        var content = NotificationCatalog.AdminAlerts.ListingExpiring(
            ListingModuleType.Furniture, Guid.NewGuid(), "كنبة", lateEvening, 5);

        Assert.Equal(new DateTime(2026, 11, 3).Date, ListingLifecycle.ToDisplayTime(lateEvening).Date);
        Assert.Contains("2026/11/03", content.Message);
        Assert.Contains("كنبة", content.Message);
        Assert.Equal(NotificationType.AdminListingExpiring, content.Type);
    }

    [Fact]
    public void The_alert_type_does_not_collide_with_another_notification_type()
    {
        var sameValue = Enum.GetValues<NotificationType>()
            .Count(type => (int)type == (int)NotificationType.AdminListingExpiring);

        Assert.Equal(1, sameValue);
    }

    [Fact]
    public void Nothing_in_the_expiry_path_can_remove_a_listing()
    {
        var root = FindRepositoryRoot();

        var processor = File.ReadAllText(Path.Combine(root, "Core", "Services", "Listings", "ListingLifecycleProcessor.cs"));
        var job = File.ReadAllText(Path.Combine(root, "Infastrucre", "Presitance", "BackgroundServices", "ListingLifecycleService.cs"));

        foreach (var source in new[] { processor, job })
        {
            Assert.DoesNotMatch(new Regex(@"\.Remove\(|\.Delete\(|ExecuteDeleteAsync|ToDeleteAsync"), source);
        }
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "CLAUDE.md")) &&
               !Directory.Exists(Path.Combine(directory.FullName, "Core")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new InvalidOperationException("Repository root not found.");
    }
}
