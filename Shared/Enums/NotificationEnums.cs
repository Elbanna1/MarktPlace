namespace Shared.Enums;

public enum NotificationType
{
    AdvertisementExpired = 1,
    AdvertisementDeleted = 2,
    General = 3,

    AdvertisementPublished = 4,
    AdvertisementRepublished = 5,
    AdvertisementApproved = 6,
    AdvertisementRejected = 7,

    LostItemPublished = 8,
    FoundItemPublished = 9,
    LostItemReturned = 10,

    NewComment = 11,
    NewLike = 12,

    AdminMessage = 13,
    SystemNotification = 14,

    AdvertisementExpiringSoon = 15,

    AdvertisementExpiringTomorrow = 16,

    PaymentSubmitted = 17,

    PaymentApproved = 18,

    PaymentRejected = 19,

    ListingPublished = 20,

    ListingUpdated = 21,

    ListingDeleted = 22,

    ProfileUpdated = 23,

    PasswordChanged = 24,

    NewLogin = 25,

    AccountCreated = 26,

    AdminUpdatedListing = 27,

    AdminDeletedListing = 28,

    ListingReported = 29,

    ListingReportReviewed = 30,

    FeedbackSubmitted = 31,

    FeedbackUpdated = 32,

    BannerBookingSubmitted = 33,

    BannerPaymentApproved = 34,

    BannerPaymentRejected = 35,

    BannerBookingApproved = 36,

    BannerBookingRejected = 37,

    BannerBookingPublished = 38,

    BannerBookingExpired = 39,

    ListingSuspended = 40,

    ListingRated = 55,

    ReferralCompleted = 56,

    NewListingForInterest = 41,

    AdminPendingListing = 50,

    AdminNewReport = 51,

    AdminNewBannerRequest = 52,

    AdminNewPayment = 53,

    AdminUserModeration = 54,

    AdminBannerExpiring = 55,

    AdminRoleGranted = 57
}

public enum NotificationAction
{
    Created = 1,

    Updated = 2,

    Deleted = 3,

    Approved = 4,

    Rejected = 5,

    Republished = 6,

    Expired = 7,

    AdminUpdated = 8,

    AdminDeleted = 9,

    Reported = 10,

    ReportUnderReview = 11,

    ReportActionTaken = 12,

    ReportDismissed = 13,

    Suspended = 14,

    Rated = 15,

    Liked = 16,

    Commented = 17,

    EditedPendingReview = 18
}
