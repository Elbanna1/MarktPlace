using Shared.Enums;

namespace Shared.DTOs.Advertisements;

public sealed record AdvertisementOwnership(
    string OwnerId, AdvertisementStatus Status, DateTime? ExpireAt,
    ModerationStatus ModerationStatus, DateTime? DeletedAt);
