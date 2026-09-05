using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IAdminAdService
{
    Task<PaginatedResult<AdminAdListItemDto>> GetAdsAsync(
        AdminAdFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminAdListItemDto>> GetLatestAdsAsync(
        int count, CancellationToken cancellationToken = default);

    Task<int> GetPendingCountAsync(CancellationToken cancellationToken = default);

    Task<AdminAdDetailsDto> GetAdDetailsAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default);

    Task DeleteAsync(
        ListingModuleType type, Guid id, string adminUserId, CancellationToken cancellationToken = default);

    Task<AdminModerationResultDto> ApproveAsync(
        ListingModuleType type, Guid id, string adminUserId,
        ApproveListingRequest request, CancellationToken cancellationToken = default);

    Task<AdminModerationResultDto> RejectAsync(
        ListingModuleType type, Guid id, string adminUserId,
        RejectListingRequest request, CancellationToken cancellationToken = default);

    Task<AdminModerationResultDto> SuspendAsync(
        ListingModuleType type, Guid id, string adminUserId,
        SuspendListingRequest request, CancellationToken cancellationToken = default);

    Task<AdminAdMetadataDto> GetMetadataAsync(CancellationToken cancellationToken = default);
}
