using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminReferralFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? ReferrerUserId { get; set; }

    public string? ReferredUserId { get; set; }

    public string? ReferralCode { get; set; }

    public ReferralStatus? Status { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}

public class AdminReferralDto
{
    public Guid Id { get; set; }

    public string ReferrerUserId { get; set; } = default!;

    public string ReferrerName { get; set; } = default!;

    public string? ReferrerUserName { get; set; }

    public string ReferredUserId { get; set; } = default!;

    public string ReferredName { get; set; } = default!;

    public string? ReferredUserName { get; set; }

    public string ReferralCode { get; set; } = default!;

    public string ReferralLink { get; set; } = default!;

    public ReferralStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}

public class AdminTopReferrerDto
{
    public string UserId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? UserName { get; set; }

    public string? ReferralCode { get; set; }

    public string? ReferralLink { get; set; }

    public int TotalReferrals { get; set; }

    public int CompletedReferrals { get; set; }
}

public class AdminReferrerFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? UserId { get; set; }

    public int? MinimumReferrals { get; set; }

    public AdminReferrerSortBy SortBy { get; set; } = AdminReferrerSortBy.MostReferrals;
}

public enum AdminReferrerSortBy
{
    MostReferrals = 1,

    NewestReferrer = 2
}

public class AdminReferrerDto
{
    public string UserId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public UserAccountStatus AccountStatus { get; set; }

    public string AccountStatusName { get; set; } = default!;

    public DateTime RegisteredAt { get; set; }

    public string? ReferralCode { get; set; }

    public string? ReferralLink { get; set; }

    public int TotalReferrals { get; set; }

    public int CompletedReferrals { get; set; }

    public int PendingReferrals { get; set; }

    public int TotalShares { get; set; }

    public int TotalClicks { get; set; }

    public decimal? ConversionRate { get; set; }

    public DateTime? LastReferralAt { get; set; }
}

public class AdminReferralStatisticsDto
{
    public int TotalReferrals { get; set; }

    public int CompletedReferrals { get; set; }

    public int PendingReferrals { get; set; }

    public int ActiveReferrers { get; set; }

    public int Today { get; set; }

    public int ThisWeek { get; set; }

    public int ThisMonth { get; set; }

    public int TotalShares { get; set; }

    public int TotalClicks { get; set; }

    public decimal? ConversionRate { get; set; }

    public IReadOnlyList<AdminTopReferrerDto> TopReferrers { get; set; } =
        Array.Empty<AdminTopReferrerDto>();

    public DateTime GeneratedAt { get; set; }
}
