using Shared.Constants;
using Shared.Enums;

namespace Domain.Entities;

public class BannerPlacementSetting
{
    public int Id { get; set; }

    public BannerLocation Location { get; set; }

    public decimal Price { get; set; }

    public int DurationDays { get; set; } = BannerBookingCatalog.DefaultDurationDays;

    public int MaxSlots { get; set; }

    public int DesktopWidth { get; set; }

    public int DesktopHeight { get; set; }

    public int MobileWidth { get; set; }

    public int MobileHeight { get; set; }

    public long MaxImageSizeBytes { get; set; }

    public string AllowedFormats { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<BannerBooking> Bookings { get; set; } = new List<BannerBooking>();
}
