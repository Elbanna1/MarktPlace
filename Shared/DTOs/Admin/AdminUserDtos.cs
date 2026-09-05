using System.ComponentModel.DataAnnotations;
using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminUserFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public UserAccountStatus? Status { get; set; }

    public bool? IsAdmin { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}

public class AdminUserListItemDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? UserName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Image { get; set; }

    public int AdsCount { get; set; }

    public UserAccountStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public bool IsAdmin { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class AdminUserDetailsDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? UserName { get; set; }

    public string? Phone { get; set; }

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? Image { get; set; }

    public string? Governorate { get; set; }

    public string? Center { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsAdmin { get; set; }

    public UserAccountStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime? StatusChangedAt { get; set; }

    public string? StatusReason { get; set; }

    public bool IsLockedOut { get; set; }

    public AdminUserListingCountsDto Listings { get; set; } = new();

    public IReadOnlyList<AdminAdListItemDto> RecentAds { get; set; } = Array.Empty<AdminAdListItemDto>();

    public IReadOnlyList<AdminDashboardBannerRequestDto> BannerRequests { get; set; } =
        Array.Empty<AdminDashboardBannerRequestDto>();

    public int ActiveBannersCount { get; set; }

    public IReadOnlyList<AdminDashboardPaymentDto> Payments { get; set; } =
        Array.Empty<AdminDashboardPaymentDto>();

    public decimal TotalPaid { get; set; }
}

public class AdminUserListingCountsDto
{
    public int Total { get; set; }

    public int Active { get; set; }

    public int Pending { get; set; }

    public int Rejected { get; set; }

    public int Suspended { get; set; }

    public int Expired { get; set; }
}

public class UpdateUserStatusRequest
{
    [Required(ErrorMessage = "حالة المستخدم مطلوبة.")]
    [EnumDataType(typeof(UserAccountStatus), ErrorMessage = "حالة المستخدم غير صحيحة.")]
    public UserAccountStatus Status { get; set; }

    [MaxLength(500, ErrorMessage = "لا يمكن أن يتجاوز السبب 500 حرف.")]
    public string? Reason { get; set; }
}
