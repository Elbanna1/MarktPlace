namespace Shared.Enums;

public enum FeedbackType
{
    Suggestion = 1,

    Complaint = 2,

    ProblemReport = 3,

    FeatureRequest = 4,

    Other = 5
}

public enum FeedbackStatus
{
    New = 0,

    UnderReview = 1,

    Replied = 2,

    Resolved = 3,

    Closed = 4
}
