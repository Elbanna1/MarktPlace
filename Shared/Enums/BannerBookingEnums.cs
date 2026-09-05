namespace Shared.Enums;

public enum BannerLocation
{
    HomeSlider1 = 1,

    HomeSlider2 = 2,

    SubCategoryBanner = 3
}

public enum BannerBookingStatus
{
    PendingReview = 1,

    PaymentApproved = 2,

    Approved = 3,

    Published = 4,

    Rejected = 5,

    Expired = 6,

    Cancelled = 7
}

public enum BannerPaymentStatus
{
    Pending = 1,

    Paid = 2,

    Rejected = 3
}

public enum BannerRejectionReason
{
    UnsuitableImage = 1,

    UnclearPaymentProof = 2,

    InvalidUrl = 3,

    MissingInformation = 4,

    PolicyViolation = 5,

    Other = 99
}

public enum BannerImageKind
{
    Desktop = 1,

    Mobile = 2
}
