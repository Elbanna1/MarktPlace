using Shared.Enums;

namespace Shared.DTOs.BannerBookings;

public class BannerImageSpecDto
{
    public BannerImageKind Kind { get; set; }

    public string KindName { get; set; } = default!;

    public int Width { get; set; }

    public int Height { get; set; }

    public string Resolution { get; set; } = default!;

    public string AspectRatio { get; set; } = default!;

    public IReadOnlyList<string> AllowedFormats { get; set; } = Array.Empty<string>();

    public string AllowedFormatNames { get; set; } = default!;

    public long MaxSizeBytes { get; set; }

    public int MaxSizeMegabytes { get; set; }

    public string? Note { get; set; }
}

public class BannerPlacementDto
{
    public int Id { get; set; }

    public BannerLocation Location { get; set; }

    public string LocationName { get; set; } = default!;

    public decimal Price { get; set; }

    public string Currency { get; set; } = default!;

    public string PriceDisplay { get; set; } = default!;

    public int MaxSlots { get; set; }

    public int DurationDays { get; set; }

    public string DurationDisplay { get; set; } = default!;

    public bool RequiresSubCategory { get; set; }

    public bool IsActive { get; set; }

    public int DisplayOrder { get; set; }

    public BannerImageSpecDto Desktop { get; set; } = default!;

    public BannerImageSpecDto Mobile { get; set; } = default!;

    public string ImageUsageNote { get; set; } = default!;
}

public class UpdateBannerPlacementRequest
{
    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public int MaxSlots { get; set; }

    public int DesktopWidth { get; set; }
    public int DesktopHeight { get; set; }
    public int MobileWidth { get; set; }
    public int MobileHeight { get; set; }

    public int MaxImageSizeMegabytes { get; set; }

    public IReadOnlyList<string>? AllowedFormats { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }
}
