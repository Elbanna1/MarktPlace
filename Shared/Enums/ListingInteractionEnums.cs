namespace Shared.Enums;

public enum ListingReportReason
{
    Violating = 1,

    Fraud = 2,

    IncorrectData = 3,

    InappropriateContent = 4,

    Duplicate = 5,

    Other = 6
}

public enum ListingReportStatus
{
    Pending = 0,

    UnderReview = 1,

    ActionTaken = 2,

    Dismissed = 3
}
