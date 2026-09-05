using Shared.Enums;

namespace Shared.DTOs.BannerBookings;

public class BannerSlotAvailabilityDto
{
    public int SlotNumber { get; set; }

    public string SlotName { get; set; } = default!;

    public bool IsAvailable { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime? AvailableFrom { get; set; }

    public DateTime? ReservedUntil { get; set; }
}

public class BannerAvailabilityDto
{
    public BannerLocation Location { get; set; }

    public string LocationName { get; set; } = default!;

    public BannerPlacementDto Placement { get; set; } = default!;

    public IReadOnlyList<BannerSlotAvailabilityDto> Slots { get; set; } = Array.Empty<BannerSlotAvailabilityDto>();

    public IReadOnlyList<int> AvailableSlots { get; set; } = Array.Empty<int>();

    public bool IsAvailable { get; set; }

    public string Message { get; set; } = default!;

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? SubCategoryId { get; set; }

    public string? SubCategoryName { get; set; }

    public DateTime? NextStartDate { get; set; }

    public DateTime? NextEndDate { get; set; }
}

public class BannerAvailabilityQuery
{
    public BannerLocation Location { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }
}
