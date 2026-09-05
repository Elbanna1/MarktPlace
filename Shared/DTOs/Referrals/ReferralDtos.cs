using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Referrals;

public class MyReferralDto
{
    public string ReferralCode { get; set; } = default!;

    public string ReferralLink { get; set; } = default!;

    public int TotalReferrals { get; set; }

    public int CompletedReferrals { get; set; }

    public int PendingReferrals { get; set; }

    public int TotalShares { get; set; }

    public int TotalClicks { get; set; }
}

public class ReferralResolutionDto
{
    public bool Valid { get; set; }

    public string? ReferralCode { get; set; }

    public string? ReferrerName { get; set; }

    public string? Message { get; set; }
}

public class ReferredUserDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public DateTime RegisteredAt { get; set; }

    public ReferralStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime? CompletedAt { get; set; }
}

public class MyReferralFilterParams : PaginationParams
{
    public ReferralStatus? Status { get; set; }
}

public class ReferralStatisticsDto
{
    public int Total { get; set; }

    public int Completed { get; set; }

    public int Pending { get; set; }

    public int Today { get; set; }

    public int ThisWeek { get; set; }

    public int ThisMonth { get; set; }

    public DateTime GeneratedAt { get; set; }
}
