using Shared.DTOs.Admin;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IAdminReferralService
{
    Task<PaginatedResult<AdminReferralDto>> GetReferralsAsync(
        AdminReferralFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdminReferralDto> GetReferralAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PaginatedResult<AdminReferrerDto>> GetReferrersAsync(
        AdminReferrerFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdminReferralStatisticsDto> GetStatisticsAsync(
        int topCount = 5, CancellationToken cancellationToken = default);

    Task<PaginatedResult<AdminReferralDto>> GetForUserAsync(
        string userId, AdminReferralFilterParams filter, CancellationToken cancellationToken = default);
}
