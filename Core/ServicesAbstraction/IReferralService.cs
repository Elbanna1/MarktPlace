using Domain.Entities;
using Shared.DTOs.Referrals;

namespace ServicesAbstraction;

public interface IReferralService
{
    Task<MyReferralDto> GetMineAsync(string userId, CancellationToken cancellationToken = default);

    Task<ReferralStatisticsDto> GetMyStatisticsAsync(
        string userId, CancellationToken cancellationToken = default);

    Task<Shared.Responses.PaginatedResult<ReferredUserDto>> GetMyReferredUsersAsync(
        string userId, MyReferralFilterParams filter, CancellationToken cancellationToken = default);

    Task<MyReferralDto> RecordShareAsync(string userId, CancellationToken cancellationToken = default);

    Task<ReferralResolutionDto> ResolveAsync(
        string? code, string? callerUserId = null, CancellationToken cancellationToken = default);

    Task<ApplicationUser?> ResolveReferrerForRegistrationAsync(
        string? code, CancellationToken cancellationToken = default);

    Task<Referral> RecordAsync(
        ApplicationUser referrer, ApplicationUser referred, string codeUsed,
        CancellationToken cancellationToken = default);

    Task<string> GenerateUniqueCodeAsync(CancellationToken cancellationToken = default);

    Task<bool> NotifyReferrerAsync(Guid referralId, CancellationToken cancellationToken = default);
}
