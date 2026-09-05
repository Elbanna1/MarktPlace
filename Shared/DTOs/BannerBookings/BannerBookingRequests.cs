using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.BannerBookings;

public class CreateBannerBookingRequest
{
    public IFormFile? DesktopImage { get; set; }

    public IFormFile? MobileImage { get; set; }

    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public string ButtonText { get; set; } = default!;

    public string TargetUrl { get; set; } = default!;

    public BannerLocation Location { get; set; }

    public int? SlotNumber { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public string AdvertiserName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string? WhatsAppNumber { get; set; }

    public string? Email { get; set; }

    public int PaymentMethodId { get; set; }

    public IFormFile? PaymentProof { get; set; }

    public bool ConfirmationAccepted { get; set; }
}

public class BannerImagePreviewRequest
{
    public BannerLocation Location { get; set; } = BannerLocation.HomeSlider1;

    public IFormFile? DesktopImage { get; set; }

    public IFormFile? MobileImage { get; set; }
}

public class BannerBookingQuoteQuery
{
    public BannerLocation Location { get; set; }

    public int? SlotNumber { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public int? PaymentMethodId { get; set; }
}

public class RejectBannerBookingRequest
{
    public BannerRejectionReason Reason { get; set; }

    public string? Notes { get; set; }
}

public class RejectBannerPaymentRequest
{
    public string? Notes { get; set; }
}
