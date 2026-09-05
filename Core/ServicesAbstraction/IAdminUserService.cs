using Domain.Entities;
using Shared.DTOs.Admin;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IAdminUserService
{
    Task<PaginatedResult<AdminUserListItemDto>> GetUsersAsync(
        AdminUserFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdminUserDetailsDto> GetUserAsync(string id, CancellationToken cancellationToken = default);

    Task<AdminUserDetailsDto> UpdateStatusAsync(
        string id, string adminUserId, UpdateUserStatusRequest request,
        CancellationToken cancellationToken = default);
}

public interface IAdminUserRepository
{
    Task<(IReadOnlyList<ApplicationUser> Items, int TotalCount)> GetPagedAsync(
        AdminUserFilterParams filter, IReadOnlyCollection<string> adminUserIds,
        CancellationToken cancellationToken = default);

    Task<ApplicationUser?> FindAsync(string id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminDashboardBannerRequestDto>> GetBannerRequestsAsync(
        string userId, CancellationToken cancellationToken = default);

    Task<int> CountActiveBannersAsync(
        string userId, DateTime utcNow, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminDashboardPaymentDto>> GetPaymentsAsync(
        string userId, CancellationToken cancellationToken = default);

    Task<decimal> GetTotalPaidAsync(string userId, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
