namespace Shared.Enums;

public enum ModerationStatus
{
    Pending = 0,

    Approved = 1,

    Rejected = 2,

    Suspended = 3
}

public enum ListingRejectionReason
{
    IncompleteData = 1,

    ProhibitedImages = 2,

    ProhibitedContent = 3,

    Duplicate = 4,

    IncorrectInformation = 5,

    Other = 99
}
