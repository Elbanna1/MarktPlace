using Domain.Entities;
using Shared.Constants;
using Shared.Enums;

namespace Services.AdPayments;

public static class AdPaymentRules
{
    public static DateTime? WindowEndOf(IModeratedListing listing) => listing switch
    {
        IExpiringListing expiring => expiring.ExpireAt,
        Advertisement advertisement => advertisement.ExpireAt,
        _ => null
    };

    public static bool IsStampedExpired(IModeratedListing listing) =>
        listing is Advertisement { Status: AdvertisementStatus.Expired };

    public static bool IsDeleted(IModeratedListing listing) =>
        listing is Advertisement { Status: AdvertisementStatus.Deleted } or Advertisement { DeletedAt: not null };

    public static bool OpensNewWindow(IModeratedListing listing, DateTime utcNow) =>
        !(WindowEndOf(listing) is { } current && current > utcNow && !IsStampedExpired(listing));

    public static bool NeedsNewWindow(IModeratedListing listing, DateTime utcNow)
    {
        if (IsDeleted(listing))
            return false;

        return listing.ModerationStatus switch
        {
            ModerationStatus.Approved =>
                IsStampedExpired(listing) || AdminRepublishPolicy.IsWindowOpen(WindowEndOf(listing), utcNow),

            ModerationStatus.Pending or ModerationStatus.Rejected => OpensNewWindow(listing, utcNow),

            _ => false
        };
    }

    public static bool IsGrandfathered(DateTime? enabledAt, ModerationStatus status, DateTime? listingCreatedAt) =>
        status != ModerationStatus.Approved &&
        enabledAt is { } enabled && listingCreatedAt is { } created && created < enabled;

    public static string NotNeededReason(IModeratedListing listing) => listing.ModerationStatus switch
    {
        ModerationStatus.Suspended => "الإعلان ده موقوف من الإدارة، ومينفعش تدفع له دلوقتي.",
        ModerationStatus.Approved =>
            $"الإعلان منشور ولسه فاضل على انتهائه أكتر من {AdminRepublishPolicy.WarningDays} أيام، فمش محتاج دفع دلوقتي.",
        _ => "الإعلان ده منشور بالفعل ومدته لسه سارية، فمش محتاج دفع دلوقتي."
    };
}
