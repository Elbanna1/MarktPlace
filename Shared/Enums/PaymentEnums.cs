namespace Shared.Enums;

public enum PaymentMethodType
{
    MobileWallet = 1,

    InstaPay = 2,

    BankAccount = 3,

    Other = 99
}

public enum PaymentStatus
{
    Pending = 1,

    Approved = 2,

    Rejected = 3,

    Refunded = 4
}

public enum PaymentPurpose
{
    Unspecified = 0,

    FeaturedAdvertisement = 1,

    PremiumMembership = 2,

    SubscriptionPlan = 3,

    AdvertisementPackage = 4,

    ListingCredits = 5
}
