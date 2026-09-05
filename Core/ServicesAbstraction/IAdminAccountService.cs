using Domain.Entities;
using Shared.DTOs.Admin;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IAdminAccountService
{
    Task<PaginatedResult<AdminAccountListItemDto>> GetAdminsAsync(
        AdminAccountFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdminAccountDetailsDto> GetAdminAsync(string id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminCandidateUserDto>> SearchCandidatesAsync(
        AdminCandidateSearchParams filter, CancellationToken cancellationToken = default);

    Task<AdminCandidateUserDto> GetCandidateAsync(
        string userId, CancellationToken cancellationToken = default);

    Task<AdminAccountDetailsDto> ConfirmAdminAsync(
        ConfirmAdminRequest request, string actingSuperAdminId, CancellationToken cancellationToken = default);

    Task<AdminAccountDetailsDto> UpdateAdminAsync(
        string id, UpdateAdminRequest request, string actingSuperAdminId,
        CancellationToken cancellationToken = default);

    Task<AdminAccountDetailsDto> SetStatusAsync(
        string id, UpdateAdminStatusRequest request, string actingSuperAdminId,
        CancellationToken cancellationToken = default);

    Task<AdminAccountDetailsDto> UpdatePermissionsAsync(
        string id, UpdateAdminPermissionsRequest request, string actingSuperAdminId,
        CancellationToken cancellationToken = default);

    Task RevokeAdminAsync(
        string id, string actingSuperAdminId, CancellationToken cancellationToken = default);
}

public interface IAdminAccountRepository
{
    Task<IReadOnlyList<AdminPageGrant>> GetGrantsAsync(
        string adminUserId, CancellationToken cancellationToken = default);

    Task<int> GetPermissionMaskAsync(
        string adminUserId, string pageKey, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, int>> CountPagesAsync(
        IReadOnlyCollection<string> adminUserIds, CancellationToken cancellationToken = default);

    Task ReplaceGrantsAsync(
        string adminUserId, IReadOnlyList<(string PageKey, IReadOnlyList<int> Permissions)> grants,
        string? grantedBy, CancellationToken cancellationToken = default);

    Task RemoveAllGrantsAsync(string adminUserId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ApplicationUser> Items, int TotalCount)> GetPagedAsync(
        AdminAccountFilterParams filter, IReadOnlyCollection<string> adminUserIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApplicationUser>> SearchUsersAsync(
        AdminCandidateSearchParams filter, IReadOnlyCollection<string> excludedUserIds,
        CancellationToken cancellationToken = default);
}
