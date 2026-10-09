using Shared.DTOs.Listings;
using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminRepublishResultDto
{
    public Guid Id { get; set; }

    public ListingModuleType TypeId { get; set; }

    public string Type { get; set; } = default!;

    public string Title { get; set; } = default!;

    public DateTime PublishedAt { get; set; }

    public DateTime ExpireAt { get; set; }

    public DateTime? PreviousExpireAt { get; set; }

    public int RemainingDays { get; set; }

    public int RepublishCount { get; set; }

    public string Status { get; set; } = default!;

    public ListingModerationDto Moderation { get; set; } = default!;
}
