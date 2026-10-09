using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Enums;

namespace Persistence.Data.Development;

public sealed record DemoSeedImage(string FileName, string RelativePath, string Url);

public static class RestaurantAndCafeDemoSeed
{
    public static readonly Guid ListingId = new("7e5a1c0f-64aa-4c1b-9e15-000000000064");

    public const int RemainingDaysAtBaseline = 1;

    public const string Name = "كافيه ومطعم النخيل";

    public static async Task<bool> SeedAsync(
        AppDbContext db,
        ApplicationUser owner,
        IReadOnlyList<DemoSeedImage> images,
        DateTime baselineUtc,
        CancellationToken cancellationToken = default)
    {
        if (images.Count == 0)
            return false;

        var exists = await db.RestaurantsAndCafes
            .IgnoreQueryFilters()
            .AnyAsync(listing => listing.Id == ListingId, cancellationToken);

        if (exists)
            return false;

        var expireAt = baselineUtc.AddDays(RemainingDaysAtBaseline);
        var publishedAt = expireAt.AddDays(-ListingLifecycle.ActiveDurationDays);

        db.RestaurantsAndCafes.Add(new RestaurantAndCafe
        {
            Id = ListingId,
            UserId = owner.Id,
            Name = Name,
            Title = "فطار وغدا وقهوة مختصة على بحيرة قارون",
            Description =
                "مطعم وكافيه عائلي في قلب الفيوم بنقدّم فطار فلاحي، مشويات، بيتزا ومشروبات ساخنة وباردة. " +
                "مكان مكيّف وجلسات خارجية تطل على البحيرة، ومتاح حجز المناسبات وخدمة توصيل داخل المدينة.",
            Address = "طريق بحيرة قارون، بجوار نادي الصيد، الفيوم",
            GoogleMapsUrl = "https://maps.google.com/?q=Lake+Qarun+Fayoum",
            Phone = owner.PhoneNumber ?? "01012345678",
            WhatsApp = owner.PhoneNumber,
            Email = owner.Email,
            CreatedAt = publishedAt,
            ModerationStatus = ModerationStatus.Approved,
            ModeratedAt = publishedAt,
            PublishedAt = publishedAt,
            FirstPublishedAt = publishedAt,
            ExpireAt = expireAt
        });

        await db.SaveChangesAsync(cancellationToken);

        for (var index = 0; index < images.Count; index++)
        {
            db.RestaurantAndCafeImages.Add(new RestaurantAndCafeImage
            {
                Id = Guid.NewGuid(),
                ListingId = ListingId,
                FileName = images[index].FileName,
                ImagePath = images[index].RelativePath,
                ImageUrl = images[index].Url,
                IsPrimary = index == 0,
                SortOrder = index,
                CreatedAt = publishedAt
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
