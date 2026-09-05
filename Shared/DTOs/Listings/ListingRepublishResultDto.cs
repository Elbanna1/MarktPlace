using Shared.Enums;

namespace Shared.DTOs.Listings;

public class ListingRepublishResultDto
{
    public Guid Id { get; set; }

    public ListingModuleType TypeId { get; set; }

    public string Type { get; set; } = default!;

    public string Route { get; set; } = default!;

    public string Title { get; set; } = default!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? RemainingDays { get; set; }

    public string Status { get; set; } = default!;

    public bool RequiresReview { get; set; }

    public int RepublishCount { get; set; }

    public string Message { get; set; } = default!;
}
